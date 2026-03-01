# 📡 C# Winform Network Communication Study

## 📖 프로젝트 개요
C# Winform 환경에서 비동기 네트워크 통신(TCP/UDP)을 구현하는 방법을 학습하고 정리한 저장소입니다. 
향후 시뮬레이션 물리 데이터의 실시간 연동이나, 자동화 테스트 도구의 제어 명령 및 결과 로그를 주고받는 강력한 통신 모듈로 확장하기 위한 기초 뼈대를 담고 있습니다.

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
   - 메인 스레드에서 통신 대기(`AcceptTcpClient`, `Read` 등)를 수행하면 UI가 멈추는 "응답 없음" 상태가 발생함.
   - `async/await` 및 `Task`를 활용하여 백그라운드에서 비동기적으로 연결 및 데이터 수신을 처리함.
2. **크로스 스레드(Cross-Thread) 해결**
   - 백그라운드 통신 스레드에서 메인 UI 컨트롤(`TextBox` 등)에 직접 접근할 수 없음.
   - `Control.InvokeRequired`와 `Invoke`를 사용하여 UI 업데이트를 메인 스레드에 안전하게 위임함.
3. **우아한 종료 (Graceful Shutdown)와 자원 관리**
   - 폼 종료 시 활성화된 통신 스레드가 백그라운드에 남아 메모리 누수(좀비 스레드)를 일으키는 것을 방지.
   - 접속자 명부(`List<TcpClient>`)를 관리하고, 폼의 `FormClosing` 이벤트에서 활성화된 모든 소켓 리소스(`Close()`, `Dispose()`)를 명시적으로 해제함.

---

## 💻 실전 구현 1: TCP 서버 & 클라이언트



### 1. TCP 서버 (Server)
- `TcpListener`를 활용하여 지정된 포트에서 대기.
- `await _listener.AcceptTcpClientAsync()`로 UI 멈춤 없이 클라이언트 접속을 수락.
- `_ = HandleClientAsync(client)` (Fire-and-Forget 패턴)를 통해 여러 클라이언트의 다중 접속을 병렬로 처리.
- `lock` 키워드를 활용한 스레드 안전한 접속자 명부 관리.

### 2. TCP 클라이언트 (Client)
- `TcpClient`를 생성하고 `ConnectAsync`를 통해 서버에 비동기 접속.
- `NetworkStream`을 활용하여 바이트 배열(`byte[]`) 형태로 데이터를 송수신.
- `ReadAsync`가 `0`을 반환할 때 정상적인 연결 종료로 판단하고 리소스를 정리.

### 3. 단일 솔루션 멀티 폼 실행 (`Program.cs`)
서버와 클라이언트 폼을 동시에 띄워 로컬 루프백(`127.0.0.1`) 환경에서 원활하게 테스트할 수 있도록 진입점을 수정함.
```csharp
// 서버는 띄워만 두고, 클라이언트를 메인 루프에 바인딩
ServerForm serverForm = new ServerForm();
serverForm.Show();
Application.Run(new ClientForm());