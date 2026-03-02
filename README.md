# 📡 C# Winform Network Communication & Simulation Architecture

## 📖 프로젝트 개요
C# Winform 환경에서 비동기 네트워크 통신(TCP/UDP)을 구현하고, 외부 시스템(C++ 백엔드, JSBSim 비행 시뮬레이터 등)과의 대규모 실시간 데이터 연동을 위한 최적의 아키텍처를 학습하고 정리한 저장소입니다.

단순한 데이터 송수신을 넘어, **이기종 간 메모리 패킹, 패킷 라우팅, 싱글톤 기반의 스레드 안전성 확보, 그리고 렌더링 병목 현상을 제거한 다중 UI 동기화 기법**을 다룹니다.

---

## 🔄 주요 변경 및 아키텍처 개선 사항 (Changelog)

기존의 단순 1:1 통신 구조에서, 대용량 실시간 물리 데이터 스트리밍에 적합한 구조로 대폭 업그레이드되었습니다.

| 구분 | 기존 설계 (AS-IS) | 개선된 설계 (TO-BE) | 개선 효과 및 목적 |
| :--- | :--- | :--- | :--- |
| **UI 갱신** | 수신 스레드에서 `Control.Invoke` 사용 | **Singleton 데이터 센터 + 메인 UI 타이머(Game Loop)** | 초당 수백 번의 잦은 `Invoke`로 인한 UI 병목 현상(Freezing) 완벽 제거. |
| **다중 창 동기화**| 각 폼이 개별적으로 업데이트 처리 | **메인 폼이 33ms(30FPS) 주기로 모든 서브 폼 동시 갱신** | 여러 모니터링 패널 간의 데이터 표시 타이밍(Sync) 불일치 및 화면 끊김 방지. |
| **패킷 식별** | 단일 구조체 전송 | **`PacketHeader` 도입 (MsgID 기반 라우팅)** | 수신된 바이트의 맨 앞 6바이트만 읽어 패킷 종류를 식별하고 알맞은 구조체로 분기 처리하는 멀티플렉싱 구현. |

---

## 📚 핵심 개념: TCP vs UDP


| 특징 | TCP (Transmission Control Protocol) | UDP (User Datagram Protocol) |
| :--- | :--- | :--- |
| **연결 방식** | 연결 지향형 (3-way Handshake) | 비연결형 |
| **신뢰성** | 높음 (패킷 누락/순서 보장) | 낮음 (패킷 유실 가능성 있음) |
| **속도** | 상대적으로 느림 | 매우 빠름 |
| **주요 용도** | 중요한 제어 명령, 파일 전송, 1:1 통신 | **실시간 물리 데이터 스트리밍, 위치 동기화** |

---

## 📦 실전 구현 1: Common 라이브러리 (이기종 통신 규격화)

네트워크를 타고 넘어가는 데이터는 날것(Unmanaged)의 상태이므로, C#의 관리되는(Managed) 환경과 C++ 기반 시스템 간의 완벽한 통역을 위해 공통 DLL 모듈을 구축했습니다.



### 1. 구조체 메모리 제어 및 패킷 라우팅 헤더
* **`Pack = 1` (Sequential):** C++ 시스템과 통신할 때 컴파일러가 임의로 집어넣는 빈 공간(Padding)을 없애고 1바이트 단위로 꽉꽉 눌러 담습니다.
* **`PacketHeader` 도입:** 모든 구조체 맨 앞에 공통 헤더(SrcID, DstID, MsgID)를 배치하여, 수신 시 헤더만 먼저 파싱해 메시지 종류를 판별합니다.
* **`MarshalAs(UnmanagedType.ByValArray)`:** C# 구조체 내부에 고정 크기 배열의 실제 메모리 공간을 직접 할당합니다.

### 2. 엔디안 변환 (Endianness) 및 직렬화 (`PacketSerializer`)
* **네트워크 바이트 오더 변환:** `IPAddress.HostToNetworkOrder`와 비트 시프트를 활용하여 구조체 내부의 모든 숫자(short, int, float)를 Big-Endian <-> Little-Endian으로 안전하게 변환합니다.
* **`Marshal` 기반 직렬화/역직렬화:** `Marshal.AllocHGlobal`, `StructureToPtr`, `PtrToStructure`를 활용하여 외부 메모리 작업대를 빌려 구조체 전체를 한 번에 바이트 배열로 변환합니다.

---

## 🚀 실전 구현 2: 고성능 UDP 스트리밍 & 다중 UI 렌더링 아키텍처

비행 시뮬레이터와 같이 쉴 새 없이 쏟아지는 UDP 데이터를 UI 버벅임 없이 여러 화면에 동시에 뿌려주기 위해 **Game Loop 패턴**을 적용했습니다.



### 1. 중앙 데이터 저장소 (`DataManager` Singleton)
네트워크 스레드와 UI 스레드 사이의 충돌을 막기 위해 단 하나의 전역 데이터 매니저를 둡니다.
- **Producer (네트워크 스레드):** 데이터가 들어오는 즉시 `lock`을 걸고 싱글톤 변수(`LatestFlight`, `LatestFragment`)의 값을 덮어씁니다. (`Invoke` 미사용으로 초고속 처리)

### 2. 패킷 라우팅 (헤더 분기 처리)
수신 루프에서는 바이트 배열 전체를 다루기 전, 최소 크기(6바이트)의 헤더만 먼저 역직렬화하여 `MsgID`를 확인합니다.
```csharp
switch (header.MsgID)
{
    case 100: // 일반 데이터
        DataManager.Instance.LatestFragment = 파싱된_데이터; break;
    case 200: // 비행 물리 데이터
        DataManager.Instance.LatestFlight = 파싱된_데이터; break;
}