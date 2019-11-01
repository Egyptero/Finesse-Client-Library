namespace SampleFinesseDE
{
    partial class AgentWinForm
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
            this.components = new System.ComponentModel.Container();
            this.AgentID = new System.Windows.Forms.TextBox();
            this.AgentPassword = new System.Windows.Forms.TextBox();
            this.AgentExt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Login = new System.Windows.Forms.Button();
            this.AgentStatus = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.queueGrid = new System.Windows.Forms.DataGridView();
            this.HostName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Status = new System.Windows.Forms.Label();
            this.Loading = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ChangeState = new System.Windows.Forms.Button();
            this.agentWinFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.queueGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.agentWinFormBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // AgentID
            // 
            this.AgentID.Location = new System.Drawing.Point(578, 19);
            this.AgentID.Name = "AgentID";
            this.AgentID.Size = new System.Drawing.Size(169, 20);
            this.AgentID.TabIndex = 0;
            // 
            // AgentPassword
            // 
            this.AgentPassword.Location = new System.Drawing.Point(578, 45);
            this.AgentPassword.Name = "AgentPassword";
            this.AgentPassword.Size = new System.Drawing.Size(169, 20);
            this.AgentPassword.TabIndex = 1;
            this.AgentPassword.UseSystemPasswordChar = true;
            // 
            // AgentExt
            // 
            this.AgentExt.Location = new System.Drawing.Point(578, 71);
            this.AgentExt.Name = "AgentExt";
            this.AgentExt.Size = new System.Drawing.Size(169, 20);
            this.AgentExt.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(449, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Agent ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(449, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(449, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Extension";
            // 
            // Login
            // 
            this.Login.Location = new System.Drawing.Point(609, 145);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(75, 23);
            this.Login.TabIndex = 9;
            this.Login.Text = "Login";
            this.Login.UseVisualStyleBackColor = true;
            this.Login.Click += new System.EventHandler(this.login_Click);
            // 
            // AgentStatus
            // 
            this.AgentStatus.FormattingEnabled = true;
            this.AgentStatus.Location = new System.Drawing.Point(51, 21);
            this.AgentStatus.Name = "AgentStatus";
            this.AgentStatus.Size = new System.Drawing.Size(258, 21);
            this.AgentStatus.TabIndex = 7;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(51, 226);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(240, 150);
            this.dataGridView1.TabIndex = 8;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // queueGrid
            // 
            this.queueGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.queueGrid.Location = new System.Drawing.Point(101, 70);
            this.queueGrid.Name = "queueGrid";
            this.queueGrid.Size = new System.Drawing.Size(240, 150);
            this.queueGrid.TabIndex = 8;
            // 
            // HostName
            // 
            this.HostName.Location = new System.Drawing.Point(578, 97);
            this.HostName.Name = "HostName";
            this.HostName.Size = new System.Drawing.Size(169, 20);
            this.HostName.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(449, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Host name";
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(585, 197);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(123, 13);
            this.Status.TabIndex = 11;
            this.Status.Text = "Status : Disconnected !!";
            // 
            // Loading
            // 
            this.Loading.AutoSize = true;
            this.Loading.Location = new System.Drawing.Point(242, 312);
            this.Loading.Name = "Loading";
            this.Loading.Size = new System.Drawing.Size(19, 13);
            this.Loading.TabIndex = 12;
            this.Loading.Text = "...";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(97, 312);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Loading Step";
            // 
            // ChangeState
            // 
            this.ChangeState.Location = new System.Drawing.Point(323, 19);
            this.ChangeState.Name = "ChangeState";
            this.ChangeState.Size = new System.Drawing.Size(75, 23);
            this.ChangeState.TabIndex = 7;
            this.ChangeState.Text = "ChangeState";
            this.ChangeState.UseVisualStyleBackColor = true;
            this.ChangeState.Click += new System.EventHandler(this.change_State_Click);
            // 
            // agentWinFormBindingSource
            // 
            this.agentWinFormBindingSource.DataSource = typeof(SampleFinesseDE.AgentWinForm);
            this.agentWinFormBindingSource.CurrentChanged += new System.EventHandler(this.agentWinFormBindingSource_CurrentChanged);
            // 
            // AgentWinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 314);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Loading);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.HostName);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.queueGrid);
            this.Controls.Add(this.AgentStatus);
            this.Controls.Add(this.Login);
            this.Controls.Add(this.ChangeState);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AgentExt);
            this.Controls.Add(this.AgentPassword);
            this.Controls.Add(this.AgentID);
            this.Name = "AgentWinForm";
            this.Text = "Finesse Sample WinForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.queueGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.agentWinFormBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox AgentID;
        private System.Windows.Forms.TextBox AgentPassword;
        private System.Windows.Forms.TextBox AgentExt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Login;
        private System.Windows.Forms.Button ChangeState;
        private System.Windows.Forms.ComboBox AgentStatus;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView queueGrid;
        private System.Windows.Forms.TextBox HostName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.Label Loading;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.BindingSource agentWinFormBindingSource;
    }
}

