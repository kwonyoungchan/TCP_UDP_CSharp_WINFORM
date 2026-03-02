# 📡 C# Winform Network Communication Study

## 📖 프로젝트 개요
C# Winform 환경에서 비동기 네트워크 통신(TCP/UDP)을 구현하는 방법을 학습하고 정리한 저장소입니다. 
이기종 시스템(C++ 기반 서버, 비행 시뮬레이터 등)과의 원활한 통신을 위해 메모리 패킹, 엔디안 변환, 구조체 직렬화 기술을 포함하는 `Common` 라이브러리를 구축하여 통신 안정성과 재사용성을 극대화했습니다.

## 📚 핵심 개념: TCP vs UDP
| 특징 | TCP (Transmission Control Protocol) | UDP (User Datagram Protocol) |
| :--- | :--- | :--- |
| **연결 방식** | 연결 지향형 (3-way Handshake) | 비연결형 |
| **신뢰성** | 높음 (패킷 누락/순서 뒤바뀜 보장) | 낮음 (패킷 유실 가능성 있음) |
| **속도** | 상대적으로 느림 | 매우 빠름 |
| **주요 용도** | 중요한 제어 명령, 파일 전송, 1:1 통신 | 실시간 물리 데이터 스트리밍, 위치 동기화 |

---

## 💡 Winform 네트워크 프로그래밍 핵심 포인트
1. **비동기 처리 (UI Freezing 방지)**
   - `async/await` 및 `Task`를 활용하여 백그라운드에서 비동기적으로 연결 및 데이터 수신 처리.
2. **크로스 스레드(Cross-Thread) 해결**
   - 백그라운드 통신 스레드에서 UI 접근 시 `Control.Invoke`를 사용하여 메인 스레드에 안전하게 위임.
3. **우아한 종료 (Graceful Shutdown)와 자원 관리**
   - 폼 종료(`FormClosing`) 시 활성화된 모든 소켓 리소스(`Close()`, `Dispose()`)를 명시적으로 해제하여 메모리 누수 방지.

---

## 📦 실전 구현 1: Common 라이브러리 (네트워크 패킷 규격화)
네트워크를 타고 넘어가는 데이터는 날것(Unmanaged)의 상태이므로, C#의 관리되는(Managed) 메모리 환경과 이기종 시스템 간의 완벽한 통역을 위해 공통 DLL 모듈을 구축했습니다.



### 1. 구조체 메모리 제어 (`StructLayout`)
* **`Pack = 1` (Sequential):** C++ 시스템 등 외부와 통신할 때 컴파일러가 임의로 집어넣는 빈 공간(Padding)을 없애고 1바이트 단위로 꽉꽉 눌러 담습니다. x64 아키텍처라도 네트워크 패킷은 플랫폼 독립성을 위해 패딩 없이 전송하는 것이 원칙입니다.
* **`Explicit` & `FieldOffset`:** 상대방 시스템이 `Pack = 8` 등으로 고정되어 있어 패딩 바이트까지 완벽하게 맞춰야 할 경우, 메모리 주소(오프셋)를 1바이트 단위로 직접 지정하여 대참사를 방지합니다.
* **`MarshalAs(UnmanagedType.ByValArray)`:** C# 구조체 내부에 고정 크기 배열을 선언할 때, 참조(포인터 주소)가 아닌 실제 메모리 공간을 직접 파서 할당하기 위한 '거푸집' 역할을 합니다.

### 2. 엔디안 변환 (Endianness)
* **네트워크 바이트 오더(Big-Endian) vs 호스트 바이트 오더(Little-Endian):**
  데이터를 송수신할 때는 반드시 네트워크 표준인 Big-Endian으로 변환해야 합니다.
* **`IPAddress.HostToNetworkOrder` 활용:** 직접 비트 시프트를 구현하는 것보다, 현재 실행 중인 CPU의 아키텍처를 자동 판별하여 안전하게 변환해 주는 .NET 내장 함수를 사용합니다.



### 3. 패킷 직렬화/역직렬화 (`PacketSerializer`)
* **`Marshal.AllocHGlobal` / `FreeHGlobal`:** C# 가비지 컬렉터(GC)의 관리를 받지 않는 날것의 외부 메모리(Unmanaged Memory) 작업대를 임대하고 확실하게 반납합니다.
* **직렬화 (구조체 ➔ byte[]):** `Marshal.StructureToPtr`을 통해 C# 구조체를 메모리에 도장 찍듯 복사한 뒤, `Marshal.Copy`로 바이트 배열로 쓸어 담습니다.
* **역직렬화 (byte[] ➔ 구조체):** 제네릭 `<T>`와 `typeof(T)`를 활용하여, 전달받은 설계도(타입)에 맞게 바이트 파편들을 `Marshal.PtrToStructure`로 정확하게 조립해 냅니다.

---

## 💻 실전 구현 2: TCP 서버 & 클라이언트

### 1. TCP 서버 (Server)
- `TcpListener`를 활용하여 지정된 포트에서 대기 및 `await _listener.AcceptTcpClientAsync()`로 비동기 접속 수락.
- `_ = HandleClientAsync(client)` (Fire-and-Forget 패턴)를 통해 다중 접속 병렬 처리.

### 2. TCP 클라이언트 (Client)
- `TcpClient`와 `ConnectAsync`를 통해 비동기 접속.
- `NetworkStream` 및 `Common` 라이브러리의 `PacketSerializer`를 활용하여 규격화된 구조체 송수신.

---

## 🚀 실전 구현 3: UDP 브로드캐스팅 & 데이터 스트리밍 (🚧 개발 예정)
*(UDP 세션 학습 후, 비연결성 통신의 특징과 `UdpClient`를 활용한 빠르고 가벼운 데이터 전송 구현 내용을 이곳에 추가할 예정입니다.)*