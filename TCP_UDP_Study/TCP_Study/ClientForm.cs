using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpClientExample
{
    public partial class ClientForm : Form
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private bool _isConnected = false;

        public ClientForm()
        {
            InitializeComponent();
        }

        // '서버 연결 / 끊기' 토글 버튼
        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (_isConnected)
            {
                Disconnect();
                return;
            }

            string ip = txtIp.Text;
            int port = int.Parse(txtPort.Text);

            try
            {
                _client = new TcpClient();
                Log($"⏳ {ip}:{port} 서버에 연결을 시도합니다...");

                // 비동기로 서버 접속 (UI 멈춤 방지)
                await _client.ConnectAsync(ip, port);

                _isConnected = true;
                _stream = _client.GetStream();
                btnConnect.Text = "연결 끊기";
                Log("🟢 서버에 성공적으로 연결되었습니다!");

                // 서버가 보내는 메시지(혹은 종료 신호)를 받기 위해 백그라운드 수신 대기 시작
                _ = ReceiveMessagesAsync();
            }
            catch (Exception ex)
            {
                Log($"❌ 연결 실패: {ex.Message}");
            }
        }

        // '메시지 전송' 버튼
        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (!_isConnected || _stream == null)
            {
                MessageBox.Show("먼저 서버에 연결해주세요.");
                return;
            }

            string message = txtInput.Text;
            if (string.IsNullOrWhiteSpace(message)) return;

            try
            {
                // 문자열을 Byte 배열로 변환 후 전송
                byte[] data = Encoding.UTF8.GetBytes(message);
                await _stream.WriteAsync(data, 0, data.Length);
                Log($"📤 전송: {message}");
                txtInput.Clear();
            }
            catch (Exception ex)
            {
                Log($"⚠️ 전송 오류: {ex.Message}");
                Disconnect();
            }
        }

        // 서버로부터 데이터를 비동기로 계속 읽어들이는 메서드
        private async Task ReceiveMessagesAsync()
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (_isConnected && _stream != null)
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);

                    // 서버가 먼저 연결을 끊은 경우
                    if (bytesRead == 0)
                    {
                        Log("👋 서버에서 연결을 종료했습니다.");
                        Disconnect();
                        break;
                    }

                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Log($"📩 서버 응답: {receivedData}");
                }
            }
            catch (Exception)
            {
                if (_isConnected)
                {
                    Log("⚠️ 서버와의 연결이 비정상적으로 끊어졌습니다.");
                    Disconnect();
                }
            }
        }

        // 연결 해제 및 자원 정리 (우아한 종료)
        private void Disconnect()
        {
            if (!_isConnected) return;

            _isConnected = false;
            _stream?.Close();
            _client?.Close();

            // 다른 스레드에서 UI를 변경할 수 있으므로 Invoke 처리
            if (btnConnect.InvokeRequired)
            {
                btnConnect.Invoke(new Action(() => btnConnect.Text = "서버 연결"));
            }
            else
            {
                btnConnect.Text = "서버 연결";
            }

            Log("🔴 연결이 해제되었습니다.");
        }

        // 스레드 안전한 로그 출력
        private void Log(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => Log(message)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        // 폼이 닫힐 때 강제 종료 방지
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
        }
    }
}