namespace UdpCommunicationExample
{
    partial class MainMonitorForm
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnOpenSub = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSeq = new System.Windows.Forms.Label();
            this.tmrSync = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();

            // btnStart
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 40);
            this.btnStart.Text = "수신 엔진 켜기";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);

            // btnOpenSub
            this.btnOpenSub.Location = new System.Drawing.Point(138, 12);
            this.btnOpenSub.Name = "btnOpenSub";
            this.btnOpenSub.Size = new System.Drawing.Size(134, 40);
            this.btnOpenSub.Text = "비행 데이터 모니터";
            this.btnOpenSub.Click += new System.EventHandler(this.btnOpenSub_Click);

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 70);
            this.label1.Text = "Fragment 수신 Seq:";

            // lblSeq
            this.lblSeq.AutoSize = true;
            this.lblSeq.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblSeq.ForeColor = System.Drawing.Color.Blue;
            this.lblSeq.Location = new System.Drawing.Point(135, 65);
            this.lblSeq.Text = "0";

            // tmrSync
            this.tmrSync.Tick += new System.EventHandler(this.tmrSync_Tick);

            // MainMonitorForm
            this.ClientSize = new System.Drawing.Size(284, 111);
            this.Controls.Add(this.lblSeq);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOpenSub);
            this.Controls.Add(this.btnStart);
            this.Name = "MainMonitorForm";
            this.Text = "메인 관제소 (라우터)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainMonitorForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnOpenSub;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSeq;
        private System.Windows.Forms.Timer tmrSync;
    }
}