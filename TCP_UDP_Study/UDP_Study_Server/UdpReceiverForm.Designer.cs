namespace UdpCommunicationExample
{
    partial class UdpReceiverForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnStartListen = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblMonitorId = new System.Windows.Forms.Label();
            this.lblMonitorSeq = new System.Windows.Forms.Label();
            this.lblMonitorSize = new System.Windows.Forms.Label();
            this.lblMonitorTime = new System.Windows.Forms.Label();
            this.lblMonitorData = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStartListen
            // 
            this.btnStartListen.Location = new System.Drawing.Point(12, 12);
            this.btnStartListen.Name = "btnStartListen";
            this.btnStartListen.Size = new System.Drawing.Size(360, 40);
            this.btnStartListen.TabIndex = 0;
            this.btnStartListen.Text = "수신 시작 (실시간 모니터링)";
            this.btnStartListen.UseVisualStyleBackColor = true;
            this.btnStartListen.Click += new System.EventHandler(this.btnStartListen_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(12, 190);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(360, 160);
            this.txtLog.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Fragment ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sequence:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Payload Size:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Timestamp:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "Data Array:";
            // 
            // lblMonitorId
            // 
            this.lblMonitorId.AutoSize = true;
            this.lblMonitorId.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMonitorId.ForeColor = System.Drawing.Color.Blue;
            this.lblMonitorId.Location = new System.Drawing.Point(100, 65);
            this.lblMonitorId.Name = "lblMonitorId";
            this.lblMonitorId.Size = new System.Drawing.Size(14, 15);
            this.lblMonitorId.TabIndex = 7;
            this.lblMonitorId.Text = "0";
            // 
            // lblMonitorSeq
            // 
            this.lblMonitorSeq.AutoSize = true;
            this.lblMonitorSeq.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMonitorSeq.ForeColor = System.Drawing.Color.Blue;
            this.lblMonitorSeq.Location = new System.Drawing.Point(100, 90);
            this.lblMonitorSeq.Name = "lblMonitorSeq";
            this.lblMonitorSeq.Size = new System.Drawing.Size(14, 15);
            this.lblMonitorSeq.TabIndex = 8;
            this.lblMonitorSeq.Text = "0";
            // 
            // lblMonitorSize
            // 
            this.lblMonitorSize.AutoSize = true;
            this.lblMonitorSize.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMonitorSize.ForeColor = System.Drawing.Color.Blue;
            this.lblMonitorSize.Location = new System.Drawing.Point(100, 115);
            this.lblMonitorSize.Name = "lblMonitorSize";
            this.lblMonitorSize.Size = new System.Drawing.Size(14, 15);
            this.lblMonitorSize.TabIndex = 9;
            this.lblMonitorSize.Text = "0";
            // 
            // lblMonitorTime
            // 
            this.lblMonitorTime.AutoSize = true;
            this.lblMonitorTime.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMonitorTime.ForeColor = System.Drawing.Color.Blue;
            this.lblMonitorTime.Location = new System.Drawing.Point(100, 140);
            this.lblMonitorTime.Name = "lblMonitorTime";
            this.lblMonitorTime.Size = new System.Drawing.Size(38, 15);
            this.lblMonitorTime.TabIndex = 10;
            this.lblMonitorTime.Text = "0.000";
            // 
            // lblMonitorData
            // 
            this.lblMonitorData.AutoSize = true;
            this.lblMonitorData.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMonitorData.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblMonitorData.Location = new System.Drawing.Point(100, 165);
            this.lblMonitorData.Name = "lblMonitorData";
            this.lblMonitorData.Size = new System.Drawing.Size(59, 15);
            this.lblMonitorData.TabIndex = 11;
            this.lblMonitorData.Text = "00 00 ...";
            // 
            // UdpReceiverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.lblMonitorData);
            this.Controls.Add(this.lblMonitorTime);
            this.Controls.Add(this.lblMonitorSize);
            this.Controls.Add(this.lblMonitorSeq);
            this.Controls.Add(this.lblMonitorId);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnStartListen);
            this.Name = "UdpReceiverForm";
            this.Text = "UDP 실시간 모니터 (수신)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UdpReceiverForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnStartListen;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblMonitorId;
        private System.Windows.Forms.Label lblMonitorSeq;
        private System.Windows.Forms.Label lblMonitorSize;
        private System.Windows.Forms.Label lblMonitorTime;
        private System.Windows.Forms.Label lblMonitorData;
    }
}