using FinesseClient;
using FinesseClient.Common;
using FinesseClient.Model;
using FinesseClient.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SampleFinesseDE
{
    public partial class AgentWinForm : Form,FinView
    {
        public FinAgent FinAgent { get; set; }
        public AgentWinForm()
        {
            if (FinAgent == null)
            {
                FinAgent = new FinAgent();
                FinAgent.SaveLog = true;
                FinAgent.TraceStatus = true;               
                FinAgent.AgentInformation.SSL = true;
                FinAgent.AgentInformation.XMPPConnectionType = "Tls12";
                FinAgent.AgentInformation.HTTPConnectionType = "Tls12";
                FinAgent.AgentInformation.HTTPPort = "8445";
                FinAgent.AgentInformation.XMPPPort = "7443";
                FinAgent.AgentInformation.HTTPURL = "/finesse";
                FinAgent.AgentInformation.XMPPURL = "/http-bind/";
                FinAgent.LogLocation = "C:\\Users\\Mamdouh\\source\\repos\\CXConnect\\WpfApp1";
                
            }

            InitializeComponent();
            AgentStatus.DisplayMember = "StatusLabel";
        }

        

        public void FireCallEvent(Dialog dialog)
        {
        }

        public void FireDisconnectEvent()
        {
            Status.Text = "Status : Disconnected";
        }

        public void FireErrorMessage(string msg)
        {
            MessageBox.Show("Error : " + msg);
        }

        public void FireLoadingMessage(string msg)
        {
            Loading.Text = msg;
        }

        public void FireLoadLoginScreen()
        {
            AgentID.Text = "";
            AgentPassword.Text = "";
            AgentExt.Text = "";
        }

        public void FireLogMessage(string msg)
        {
        }

       public void FireQueueEvent(Queue queue)
        {
            //MessageBox.Show(queue.ToString());
        }

        public void FireNewEvent()
        {
           
            if (FinAgent.AgentInformation.MessageEvent != null)
            {
                if (FinAgent.AgentInformation.MessageEvent.MessageType.Equals("user"))
                {
                    // MessageBox.Show(finAgent.AgentInformation.MessageEvent.ToString());
                    Status.Text = "Agent Status : " + FinAgent.AgentInformation.SelectedVoiceStatus.StatusLabel;
                    LoadStatusList();
                    // Mamdouh Comment Code
                    //if (FinAgent.AgentInformation.SelectedVoiceStatus.StatusLabel == "Logout")
                    //{
                    //    FinAgent.FireDisconnectEvent();
                    //}

                    if (FinAgent.AgentInformation.SelectedVoiceStatus.StatusLabel != "Logout")
                    {
                        LoadStatusList();
                    }

                }
                else if (FinAgent.AgentInformation.MessageEvent.MessageType.Equals("call"))
                {
                    dataGridView1.DataSource = FinAgent.AgentInformation.Dialogs;

                }
                else if (FinAgent.AgentInformation.MessageEvent.MessageType.Equals("error"))
                {
                    MessageBox.Show(FinAgent.AgentInformation.MessageEvent.ErrorMsg);

                }
                //throw new NotImplementedException();
            }
        }

        public void FireReLoginEvent()
        {
            Status.Text = "Status : Connected";
            FinAgent.LoadAgentInformation();
            FinAgent.LoadCallInformation();
          
            LoadStatusList();
            Status.Text = "Agent Status : " + FinAgent.AgentInformation.SelectedVoiceStatus.StatusLabel;
        }

        public FinesseClient.Common.Screen GetLocation()
        {
            return FinesseClient.Common.Screen.MainGrid;
        }

        public void SetContext(IModel model, FinView finView)
        {
        }

        private void login_Click(object sender, EventArgs e)
        {
            //Login Click
            
            FinAgent.AgentInformation.AgentID = AgentID.Text;
            FinAgent.AgentInformation.Password = AgentPassword.Text;
            FinAgent.AgentInformation.Extension = AgentExt.Text;
            FinAgent.AgentInformation.DomainA = HostName.Text;
            FinAgent.FinView = this;
            FinAgent.SignIn();
            

        }

        private void change_State_Click(object sender, EventArgs e)
        {
            //Login Click
            if (AgentStatus.SelectedItem != null)
            {
                if (FinAgent.AgentInformation.VoiceStatusList != null)
                {
                    foreach (VoiceStatus voiceStatus in FinAgent.AgentInformation.VoiceStatusList)
                    {

                        //int index = AgentStatus.Items.Add(voiceStatus);
                        if (voiceStatus.Selected && voiceStatus != (AgentStatus.SelectedItem as VoiceStatus))
                        {
                            FinAgent.ChangeAgentVoiceStatus(AgentStatus.SelectedItem as VoiceStatus);
                            break;
                        }

                    }
                }

            }


        }
        private void LoadStatusList()
        {
            if(AgentStatus != null)
            {
                if(AgentStatus.Items.Count > 0)
                {
                    AgentStatus.Items.Clear();
                }

                //AgentStatus.DataSource = FinAgent.AgentInformation.VoiceStatusList;
                
                //AgentStatus.Refresh();

                if (FinAgent.AgentInformation.VoiceStatusList != null)
                {
                    foreach(VoiceStatus voiceStatus in FinAgent.AgentInformation.VoiceStatusList)
                    {

                        int index = AgentStatus.Items.Add(voiceStatus);
                        if (voiceStatus.Selected)
                        {
                            AgentStatus.SelectedIndex = index;
                           
                        }
                           
                    }
                }
            }


            queueGrid.DataSource = FinAgent.AgentInformation.Queues;
        }

        private void AgentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
           // MessageBox.Show("Sender : "+sender.ToString());
            if (sender == null)
                return;
            if (AgentStatus.SelectedItem == null)
                return;

            //MessageBox.Show("AgentStatus_SelectedIndexChanged : " + (AgentStatus.SelectedItem as VoiceStatus).StatusLabel);
           
               
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

		private void agentWinFormBindingSource_CurrentChanged(object sender, EventArgs e)
		{

		}
	}
}
