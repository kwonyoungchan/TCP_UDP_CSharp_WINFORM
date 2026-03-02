using System;
using System.Net.Sockets;
using System.Windows.Forms;
using Common.Network;

namespace UdpCommunicationExample
{
    public partial class UdpReceiverForm : Form
    {
        private UdpClient _udpReceiver;
        private bool _isListening = false;

        public UdpReceiverForm()
        {
            InitializeComponent();
        }

        private async void btnStartListen_Click(object sender, EventArgs e)
        {
            if (_isListening) return;

            int port = 8081;
            _udpReceiver = new UdpClient(port);
            _isListening = true;
            btnStartListen.Enabled = false;

            Log("🟢 UDP 실시간 모니터링을 시작합니다...");

            try
            {
                while (_isListening)
                {
                    UdpReceiveResult result = await _udpReceiver.ReceiveAsync();

                    // 직렬화 & 엔디안 변환
                    var packet = PacketSerializer.ByteArrayToStructure<FragmentPacket>(result.Buffer);
                    var finalPacket = EndianConverter.SwapStruct(packet);

                    // 💡 실시간 모니터링 UI 업데이트!
                    UpdateMonitorUI(finalPacket);
                }
            }
            catch (Exception ex)
            {
                if (_isListening) Log($"⚠️ 수신 오류: {ex.Message}");
            }
        }

        // 스레드 안전하게 모니터 패널을 갱신하는 메서드
        private void UpdateMonitorUI(FragmentPacket packet)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateMonitorUI(packet)));
                return;
            }

            lblMonitorId.Text = packet.FragmentId.ToString();
            lblMonitorSeq.Text = packet.SequenceNumber.ToString();
            lblMonitorSize.Text = packet.PayloadSize.ToString();
            lblMonitorTime.Text = packet.Timestamp.ToString("F3"); // 소수점 3자리까지

            // 16바이트 배열을 16진수 문자열로 예쁘게 표시
            lblMonitorData.Text = BitConverter.ToString(packet.Data).Replace("-", " ");
        }

        private void Log(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => Log(message)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        private void UdpReceiverForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isListening = false;
            _udpReceiver?.Close();
        }
    }
}