using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpServerExample
{
    public partial class ServerForm : Form
    {
        private TcpListener _listener;
        private bool _isRunning = false;
        private List<TcpClient> _connectedClients = new List<TcpClient>();
        private readonly object _lock = new object();

        public ServerForm()
        {
            InitializeComponent();
        }

        // '서버 시작' 버튼 클릭 이벤트
        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (_isRunning) return; // 이미 실행 중이면 무시

            int port = 8080; // 열어둘 포트 번호
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            _isRunning = true;
            btnStart.Enabled = false; // 중복 실행 방지

            Log($"🟢 서버가 포트 {port}에서 시작되었습니다. 클라이언트를 기다립니다...");

            try
            {
                // 서버가 실행 중인 동안 계속해서 클라이언트의 접속을 받습니다.
                while (_isRunning)
                {
                    // AcceptTcpClientAsync: 클라이언트가 올 때까지 기다립니다. (UI 멈춤 없음!)
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    Log("🤝 새로운 클라이언트가 접속했습니다!");

                    // 접속한 클라이언트와의 통신은 별도의 Task로 넘겨서, 
                    // 서버가 다른 클라이언트의 접속을 계속 받을 수 있게 합니다.
                    // 해당 방식을 C#에서 버리기(Discard) 또는 Fire-and-Forget(쏘고 잊어버리기) 패턴이라고 부른다. 
                    _ =HandleClientAsync(client);
                }
            }
            catch (Exception ex)
            {
                if (_isRunning) Log($"❌ 서버 오류: {ex.Message}");
            }
        }

        // 개별 클라이언트와 데이터를 주고받는 메서드
        private async Task HandleClientAsync(TcpClient client)
        {
            // 1. 손님이 들어오면 명부에 추가
            lock (_lock) { _connectedClients.Add(client); }

            // 'using' 블록은 괄호 안의 작업이 끝나면(정상 종료든 에러든) 네트워크 연결과 스트림을
            // 자동으로 깔끔하게 닫아주는(Dispose) 역할을 한다.
            using (client)
            using (NetworkStream stream = client.GetStream()) // <- 여기에서 수신받은 데이터를 가져온다. 
            {
                byte[] buffer = new byte[1024]; // 버퍼 초기화 
                try
                {
                    while (true)
                    {
                        // 클라이언트가 보낸 데이터를 비동기로 읽어옵니다.
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                        // 읽은 바이트가 0이면 클라이언트가 연결을 정상적으로 종료한 것입니다.
                        if (bytesRead == 0)
                        {
                            Log("👋 클라이언트와 연결이 끊어졌습니다.");
                            break;
                        }

                        // Byte 배열을 문자열로 변환하여 화면에 출력
                        string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Log($"📩 수신: {receivedData}");
                    }
                }
                catch (Exception ex)
                {
                    Log($"⚠️ 클라이언트 통신 오류: {ex.Message}");
                }
                finally
                {
                    // 2. 통신이 끝나거나 에러가 나서 나갈 때는 명부에서 제거 (가장 중요!)
                    lock (_lock) { _connectedClients.Remove(client); }
                    Log("👋 클라이언트가 명부에서 제거되었습니다.");
                }
            }
        }

        // 스레드 안전하게 텍스트 박스에 로그를 남기는 메서드
        private void Log(string message)
        {
            // 다른 스레드에서 UI 컨트롤에 접근하려고 하면 Invoke를 통해 메인 스레드에 위임합니다.
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => Log(message)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        // 폼이 닫힐 때 자원 해제 (창 파괴 처리)
        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isRunning = false;
            _listener?.Stop(); // 1. 입구 막기

            // 2. 명부에 있는 모든 클라이언트의 연결을 끊음
            lock (_lock)
            {
                foreach (TcpClient client in _connectedClients)
                {
                    client.Close(); // 연결을 강제로 끊음!
                }
            }
        }
    }
}