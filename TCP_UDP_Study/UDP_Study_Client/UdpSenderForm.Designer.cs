namespace UdpCommunicationExample
{
    partial class UdpSenderForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnStream = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tmrStream = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();

            // txtIp
            this.txtIp.Location = new System.Drawing.Point(50, 15);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(100, 23);
            this.txtIp.Text = "127.0.0.1";

            // txtPort
            this.txtPort.Location = new System.Drawing.Point(210, 15);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(50, 23);
            this.txtPort.Text = "8081";

            // btnStream
            this.btnStream.Location = new System.Drawing.Point(12, 50);
            this.btnStream.Name = "btnStream";
            this.btnStream.Size = new System.Drawing.Size(248, 40);
            this.btnStream.Text = "테스트 데이터 스트리밍 시작";
            this.btnStream.Click += new System.EventHandler(this.btnStream_Click);

            // txtLog
            this.txtLog.Location = new System.Drawing.Point(12, 100);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(248, 150);

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Text = "IP:";

            // label2
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(165, 18);
            this.label2.Text = "Port:";

            // tmrStream
            this.tmrStream.Tick += new System.EventHandler(this.tmrStream_Tick);

            // UdpSenderForm
            this.ClientSize = new System.Drawing.Size(274, 261);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnStream);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIp);
            this.Name = "UdpSenderForm";
            this.Text = "시뮬레이션 송신 테스트기";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UdpSenderForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnStream;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer tmrStream;
    }
}