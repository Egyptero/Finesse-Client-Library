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
                FinAgent = new FinAgent();
            InitializeComponent();
            
        }

        public void FireCallEvent(Dialog dialog)
        {
        }

        public void FireDisconnectEvent()
        {
        }

        public void FireErrorMessage(string msg)
        {
        }

        public void FireLoadingMessage(string msg)
        {
            Loading.Text = msg;
        }

        public void FireLoadLoginScreen()
        {
        }

        public void FireLogMessage(string msg)
        {
        }

        public void FireNewEvent()
        {
            LoadStatusList();
        }

        public void FireReLoginEvent()
        {
            Status.Text = "Status : Connected";
            //FinAgent.LoadAgentInformation();
            //FinAgent.LoadCallInformation();
            LoadStatusList();
            
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
        private void LoadStatusList()
        {
            if(AgentStatus != null)
            {
                if (AgentStatus.Items.Count > 1)
                    AgentStatus.Items.Clear();

                if(FinAgent.AgentInformation.VoiceStatusList != null)
                {
                    foreach(VoiceStatus voiceStatus in FinAgent.AgentInformation.VoiceStatusList)
                    {
                        int index = AgentStatus.Items.Add(voiceStatus.StatusLabel);
                        if (voiceStatus.Selected)
                            AgentStatus.SelectedIndex = index;
                    }
                }
            }
        }

        private void AgentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender == null)
                return;
            if (AgentStatus.SelectedItem == null)
                return;

            VoiceStatus _SelectedVoiceStatus = null;
            if (FinAgent.AgentInformation.VoiceStatusList != null)
            {
                foreach (VoiceStatus voiceStatus in FinAgent.AgentInformation.VoiceStatusList)
                {
                    if (voiceStatus.StatusLabel.Equals(AgentStatus.SelectedText))
                    {
                        _SelectedVoiceStatus = voiceStatus;
                    }
                }
            }
            if (_SelectedVoiceStatus != null)
                FinAgent.ChangeAgentVoiceStatus(_SelectedVoiceStatus);
        }

        public void FireQueueEvent(Queue queue)
        {
        }
    }
}
