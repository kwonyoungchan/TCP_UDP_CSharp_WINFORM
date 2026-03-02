using System;
using System.Windows.Forms;
using Common.Network;

namespace UdpCommunicationExample
{
    public partial class SubMonitorForm : Form
    {
        public SubMonitorForm()
        {
            InitializeComponent();
        }

        // 💡 메인 폼의 타이머(UI 스레드)가 33ms마다 이 함수를 직접 호출해 줍니다.
        // 덕분에 서브 폼은 골치 아픈 Invoke 없이 바로 라벨 글자를 바꿀 수 있습니다!
        public void UpdateData(FlightDataPacket data)
        {
            // 언리얼 엔진으로 넘어가기 전, 현재 수신된 물리 상태를 직관적으로 모니터링합니다.
            lblPitch.Text = $"Pitch: {data.Pitch:F2}°";
            lblRoll.Text = $"Roll: {data.Roll:F2}°";
            lblYaw.Text = $"Yaw: {data.Yaw:F2}°";

            lblMainRotor.Text = $"Main Rotor: {data.MainRotorRpm} RPM";
            lblTailRotor.Text = $"Tail Rotor: {data.TailRotorRpm} RPM";
        }
    }
}