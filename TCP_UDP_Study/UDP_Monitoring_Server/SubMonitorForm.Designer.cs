namespace UdpCommunicationExample
{
    partial class SubMonitorForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblPitch = new System.Windows.Forms.Label();
            this.lblRoll = new System.Windows.Forms.Label();
            this.lblYaw = new System.Windows.Forms.Label();
            this.lblMainRotor = new System.Windows.Forms.Label();
            this.lblTailRotor = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblPitch
            this.lblPitch.AutoSize = true;
            this.lblPitch.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblPitch.Location = new System.Drawing.Point(20, 20);
            this.lblPitch.Text = "Pitch: 0.00°";

            // lblRoll
            this.lblRoll.AutoSize = true;
            this.lblRoll.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblRoll.Location = new System.Drawing.Point(20, 50);
            this.lblRoll.Text = "Roll: 0.00°";

            // lblYaw
            this.lblYaw.AutoSize = true;
            this.lblYaw.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblYaw.Location = new System.Drawing.Point(20, 80);
            this.lblYaw.Text = "Yaw: 0.00°";

            // lblMainRotor
            this.lblMainRotor.AutoSize = true;
            this.lblMainRotor.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblMainRotor.ForeColor = System.Drawing.Color.DarkRed;
            this.lblMainRotor.Location = new System.Drawing.Point(20, 120);
            this.lblMainRotor.Text = "Main Rotor: 0 RPM";

            // lblTailRotor
            this.lblTailRotor.AutoSize = true;
            this.lblTailRotor.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblTailRotor.ForeColor = System.Drawing.Color.DarkRed;
            this.lblTailRotor.Location = new System.Drawing.Point(20, 150);
            this.lblTailRotor.Text = "Tail Rotor: 0 RPM";

            // SubMonitorForm
            this.ClientSize = new System.Drawing.Size(264, 200);
            this.Controls.Add(this.lblTailRotor);
            this.Controls.Add(this.lblMainRotor);
            this.Controls.Add(this.lblYaw);
            this.Controls.Add(this.lblRoll);
            this.Controls.Add(this.lblPitch);
            this.Name = "SubMonitorForm";
            this.Text = "JSBSim 비행 물리 패널";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblPitch;
        private System.Windows.Forms.Label lblRoll;
        private System.Windows.Forms.Label lblYaw;
        private System.Windows.Forms.Label lblMainRotor;
        private System.Windows.Forms.Label lblTailRotor;
    }
}