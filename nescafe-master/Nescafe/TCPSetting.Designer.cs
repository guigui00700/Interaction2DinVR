namespace Nescafe
{
    partial class TCPSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.masterId = new System.Windows.Forms.TextBox();
            this.SlaveId = new System.Windows.Forms.TextBox();
            this.IpAddress = new System.Windows.Forms.TextBox();
            this.PortNumber = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Master Id";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Slave Id";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "IP";
            // 
            // masterId
            // 
            this.masterId.Location = new System.Drawing.Point(81, 13);
            this.masterId.Name = "masterId";
            this.masterId.Size = new System.Drawing.Size(146, 20);
            this.masterId.TabIndex = 3;
            // 
            // SlaveId
            // 
            this.SlaveId.Location = new System.Drawing.Point(81, 39);
            this.SlaveId.Name = "SlaveId";
            this.SlaveId.Size = new System.Drawing.Size(146, 20);
            this.SlaveId.TabIndex = 4;
            this.SlaveId.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // IpAddress
            // 
            this.IpAddress.Location = new System.Drawing.Point(81, 66);
            this.IpAddress.Name = "IpAddress";
            this.IpAddress.Size = new System.Drawing.Size(146, 20);
            this.IpAddress.TabIndex = 5;
            this.IpAddress.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // PortNumber
            // 
            this.PortNumber.Location = new System.Drawing.Point(81, 92);
            this.PortNumber.Name = "PortNumber";
            this.PortNumber.Size = new System.Drawing.Size(146, 20);
            this.PortNumber.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "port";
            // 
            // SetButton
            // 
            this.SetButton.Location = new System.Drawing.Point(27, 135);
            this.SetButton.Name = "SetButton";
            this.SetButton.Size = new System.Drawing.Size(200, 23);
            this.SetButton.TabIndex = 8;
            this.SetButton.Text = "ok";
            this.SetButton.UseVisualStyleBackColor = true;
            this.SetButton.Click += new System.EventHandler(this.SetButton_Click);
            // 
            // TCPSetting
            // 
            this.ClientSize = new System.Drawing.Size(261, 170);
            this.Controls.Add(this.SetButton);
            this.Controls.Add(this.PortNumber);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.IpAddress);
            this.Controls.Add(this.SlaveId);
            this.Controls.Add(this.masterId);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Name = "TCPSetting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox masterId;
        private System.Windows.Forms.TextBox SlaveId;
        private System.Windows.Forms.TextBox IpAddress;
        private System.Windows.Forms.TextBox PortNumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button SetButton;
    }
}