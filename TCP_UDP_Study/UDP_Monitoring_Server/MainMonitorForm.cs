using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Network; // 우리가 만든 공통 라이브러리 참조

namespace UdpCommunicationExample
{
    public partial class MainMonitorForm : Form
    {
        private UdpClient _udpReceiver;
        private bool _isListening = false;
        private SubMonitorForm _subForm; // 상세 비행 데이터를 보여줄 서브 폼

        public MainMonitorForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_isListening) return;
            _isListening = true;
            btnStart.Enabled = false;

            // 1. 메인 UI가 멈추지 않도록 수신 전용 백그라운드 스레드(Task) 가동
            Task.Run(() => ReceiveUdpLoop());

            // 2. 화면 갱신을 위한 동기화 타이머 시작 (33ms = 약 30FPS)
            // 네트워크 수신 속도와 상관없이 사람 눈에 편안한 주기로만 화면을 다시 그립니다.
            tmrSync.Interval = 33;
            tmrSync.Start();
        }

        private void btnOpenSub_Click(object sender, EventArgs e)
        {
            // 서브 폼이 없거나 닫혀있으면 새로 열어줍니다.
            if (_subForm == null || _subForm.IsDisposed)
            {
                _subForm = new SubMonitorForm();
                _subForm.Show();
            }
        }

        // 💡 핵심: 헤더를 읽고 알맞은 구조체로 파싱하는 수신 무한 루프
        private async Task ReceiveUdpLoop()
        {
            _udpReceiver = new UdpClient(8081); // 8081 포트 개방
            int headerSize = Marshal.SizeOf(typeof(PacketHeader));

            try
            {
                while (_isListening)
                {
                    // 데이터가 들어올 때까지 대기하다가 뭉텅이(Buffer)로 받음
                    UdpReceiveResult result = await _udpReceiver.ReceiveAsync();
                    byte[] buffer = result.Buffer;

                    // 방어 코드: 들어온 데이터가 최소한 헤더(6바이트)보다는 커야 함
                    if (buffer.Length < headerSize) continue;

                    // [1단계 파싱] 맨 앞 6바이트만 읽어서 헤더 구조체로 추출
                    PacketHeader rawHeader = PacketSerializer.ByteArrayToStructure<PacketHeader>(buffer);
                    PacketHeader header = EndianConverter.SwapHeader(rawHeader);

                    // [2단계 파싱] MsgID에 따라 알맞은 전체 구조체 틀을 씌움
                    switch (header.MsgID)
                    {
                        case 100: // FragmentPacket (일반 분할 데이터)
                            var rawFrag = PacketSerializer.ByteArrayToStructure<FragmentPacket>(buffer);
                            // C++14 백엔드에서 보낸 Big-Endian 데이터를 Little-Endian으로 스왑 후 싱글톤에 저장
                            DataManager.Instance.LatestFragment = EndianConverter.SwapStruct(rawFrag);
                            break;

                        case 200: // FlightDataPacket (비행 물리 데이터)
                            var rawFlight = PacketSerializer.ByteArrayToStructure<FlightDataPacket>(buffer);
                            // JSBSim 비행 역학 데이터가 들어오는 즉시 최신 상태로 덮어쓰기 (Invoke 없음!)
                            DataManager.Instance.LatestFlight = EndianConverter.SwapStruct(rawFlight);
                            break;
                    }
                }
            }
            catch (Exception) { /* 폼 종료 시 소켓이 닫히며 발생하는 예외는 자연스럽게 무시 */ }
        }

        // 💡 33ms마다 메인 폼과 서브 폼의 화면을 동시에 갱신하는 타이머
        private void tmrSync_Tick(object sender, EventArgs e)
        {
            // 1. 메인 폼은 MsgID=100 인 일반 패킷의 시퀀스 번호를 모니터링
            FragmentPacket fragData = DataManager.Instance.LatestFragment;
            lblSeq.Text = fragData.SequenceNumber.ToString();

            // 2. 서브 폼이 열려있다면, MsgID=200 인 비행 데이터를 서브 폼으로 밀어넣어줌
            if (_subForm != null && !_subForm.IsDisposed)
            {
                FlightDataPacket flightData = DataManager.Instance.LatestFlight;
                _subForm.UpdateData(flightData); // 서브 폼 화면 갱신!
            }
        }

        private void MainMonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 자원 낭비 및 좀비 스레드 방지를 위한 깔끔한 뒷정리
            _isListening = false;
            tmrSync.Stop();
            _udpReceiver?.Close();
        }
    }
}