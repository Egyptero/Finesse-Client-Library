namespace CXChatEngine
{
    partial class XMPPManager
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.XMPPStatus = new System.Windows.Forms.Label();
            this.Status = new System.Windows.Forms.Label();
            this.ManageEngineBtn = new System.Windows.Forms.Button();
            this.UsersList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.XMPPLog = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.XMPPStatus);
            this.groupBox1.Controls.Add(this.Status);
            this.groupBox1.Controls.Add(this.ManageEngineBtn);
            this.groupBox1.Location = new System.Drawing.Point(594, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(182, 91);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Engine Admin";
            // 
            // XMPPStatus
            // 
            this.XMPPStatus.AutoSize = true;
            this.XMPPStatus.Location = new System.Drawing.Point(79, 65);
            this.XMPPStatus.Name = "XMPPStatus";
            this.XMPPStatus.Size = new System.Drawing.Size(71, 13);
            this.XMPPStatus.TabIndex = 2;
            this.XMPPStatus.Text = "Disconnected";
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(35, 65);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(38, 13);
            this.Status.TabIndex = 1;
            this.Status.Text = "Status";
            // 
            // ManageEngineBtn
            // 
            this.ManageEngineBtn.Location = new System.Drawing.Point(28, 25);
            this.ManageEngineBtn.Name = "ManageEngineBtn";
            this.ManageEngineBtn.Size = new System.Drawing.Size(131, 29);
            this.ManageEngineBtn.TabIndex = 0;
            this.ManageEngineBtn.Text = "Start";
            this.ManageEngineBtn.UseVisualStyleBackColor = true;
            // 
            // UsersList
            // 
            this.UsersList.FormattingEnabled = true;
            this.UsersList.Location = new System.Drawing.Point(594, 128);
            this.UsersList.Name = "UsersList";
            this.UsersList.Size = new System.Drawing.Size(182, 225);
            this.UsersList.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(591, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Active users";
            // 
            // XMPPLog
            // 
            this.XMPPLog.FormattingEnabled = true;
            this.XMPPLog.Location = new System.Drawing.Point(12, 11);
            this.XMPPLog.Name = "XMPPLog";
            this.XMPPLog.Size = new System.Drawing.Size(533, 342);
            this.XMPPLog.TabIndex = 4;
            // 
            // XMPPManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 367);
            this.Controls.Add(this.XMPPLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UsersList);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "XMPPManager";
            this.Text = "CX Chat Engine";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label XMPPStatus;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.Button ManageEngineBtn;
        private System.Windows.Forms.ListBox UsersList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox XMPPLog;
    }
}

