using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Common.Network; // 공통 통신 라이브러리 참조

namespace UdpCommunicationExample
{
    public partial class UdpSenderForm : Form
    {
        private UdpClient _udpSender;
        private IPEndPoint _targetEndPoint;

        // 다중 패킷 테스트를 위한 상태 변수들
        private bool _sendToggle = false;  // true면 분할 패킷, false면 비행 패킷 전송
        private short _currentSeq = 0;     // 시퀀스 번호 (점점 증가함)
        private float _dummyAngle = 0.0f;  // 가짜 비행 각도 데이터 (점점 증가함)

        public UdpSenderForm()
        {
            InitializeComponent();
            _udpSender = new UdpClient(); // 전송 전용이므로 포트 바인딩 생략
        }

        // 💡 실시간 전송 시작/중지 토글 버튼
        private void btnStream_Click(object sender, EventArgs e)
        {
            if (tmrStream.Enabled)
            {
                // 스트리밍 중지
                tmrStream.Stop();
                btnStream.Text = "테스트 데이터 스트리밍 시작";
                Log("🔴 전송을 중지했습니다.");
            }
            else
            {
                // 스트리밍 시작 (목적지 IP/Port 1회 세팅)
                _targetEndPoint = new IPEndPoint(IPAddress.Parse(txtIp.Text), int.Parse(txtPort.Text));
                tmrStream.Interval = 16; // 16ms
                tmrStream.Start();
                btnStream.Text = "전송 중지";
                Log("🟢 실시간 멀티플렉싱 전송을 시작합니다! (16ms 간격)");
            }
        }

        // 💡 100ms마다 실행되며 두 종류의 패킷을 번갈아 쏘는 핵심 엔진
        private async void tmrStream_Tick(object sender, EventArgs e)
        {
            try
            {
                _currentSeq++;
                // 360도를 넘어가면 다시 0으로 초기화되는 가짜 물리 데이터
                _dummyAngle = (_dummyAngle + 1.5f) % 360f;

                byte[] dataToSend;

                if (_sendToggle)
                {
                    // [1] 일반 분할 데이터 (MsgID = 100) 생성
                    var fragPacket = new FragmentPacket
                    {
                        Header = new PacketHeader { SrcID = 1, DstID = 2, MsgID = 100 },
                        SequenceNumber = _currentSeq,
                        PayloadSize = 16,
                        Data = new byte[16] // 내용은 빈 배열로 전송
                    };

                    // 네트워크 오더(Big-Endian) 변환 후 바이트 배열로 직렬화
                    var swapped = EndianConverter.SwapStruct(fragPacket);
                    dataToSend = PacketSerializer.StructureToByteArray(swapped);
                }
                else
                {
                    // [2] 헬리콥터 비행 물리 데이터 (MsgID = 200) 생성
                    var flightPacket = new FlightDataPacket
                    {
                        Header = new PacketHeader { SrcID = 1, DstID = 2, MsgID = 200 },
                        Pitch = _dummyAngle,
                        Roll = _dummyAngle * 0.5f,
                        Yaw = 0f,
                        MainRotorRpm = 300 + (_currentSeq % 50), // 300~350 사이에서 변하는 가짜 RPM
                        TailRotorRpm = 1500
                    };

                    // 네트워크 오더 변환 후 직렬화
                    var swapped = EndianConverter.SwapStruct(flightPacket);
                    dataToSend = PacketSerializer.StructureToByteArray(swapped);
                }

                // 다음 틱(Tick)에서는 다른 종류의 패킷을 보내도록 상태 뒤집기
                _sendToggle = !_sendToggle;

                // 목적지로 냅다 쏘기 (비연결형 통신)
                await _udpSender.SendAsync(dataToSend, dataToSend.Length, _targetEndPoint);
            }
            catch (Exception ex)
            {
                // 에러 발생 시 폭주를 막기 위해 타이머를 즉시 끔
                tmrStream.Stop();
                btnStream.Text = "테스트 데이터 스트리밍 시작";
                Log($"❌ 전송 에러: {ex.Message}");
            }
        }

        private void Log(string message)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }

        private void UdpSenderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmrStream.Stop();
            _udpSender?.Close();
        }
    }
}