using FinesseClient;
using FinesseClient.Common;
using FinesseClient.Model;
using FinesseClient.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FinesseDE
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window,FinView
    {
        private FinAgent _finAgent = new FinAgent();
        public CXProperties properties = new CXProperties("CXConnectConfig.properties");
        private Loading _loadingWindow;
        public Login()
        {
            if (DateTime.Now > new DateTime(2019, 4, 1))
            {
                MessageBox.Show("License is expired .. please contact Novelvox System Admin", "Error", MessageBoxButton.OK);
                throw new Exception("Error License is expired .. please contact istnetworks");
            }
            _finAgent.TraceStatus = true;
            _finAgent.SaveLog = true;
            _finAgent.FinView = this;
            DataContext = _finAgent;
            InitializeComponent();
            LoadProperties();

        }
        public void SaveProperties()
        {
            properties.set("FinesseA", _finAgent.AgentInformation.DomainA);
            properties.set("FinesseB", _finAgent.AgentInformation.DomainB);
            properties.set("XMPPPort", _finAgent.AgentInformation.XMPPPort);
            properties.set("XMPPURL", _finAgent.AgentInformation.XMPPURL);
            properties.set("HTTPPort", _finAgent.AgentInformation.HTTPPort);
            properties.set("HTTPURL", _finAgent.AgentInformation.HTTPURL);
            properties.set("SSL", _finAgent.AgentInformation.SSL);
            properties.set("AgentID", _finAgent.AgentInformation.AgentID);
            properties.set("Ext", _finAgent.AgentInformation.Extension);
            properties.set("XmppConnectionType", _finAgent.AgentInformation.XMPPConnectionType);
            properties.set("HttpConnectionType", _finAgent.AgentInformation.HTTPConnectionType);

            if (SavePassword.IsChecked == true)
                properties.set("Pass", _finAgent.AgentInformation.Password);
            else
                properties.set("Pass", "");

            properties.set("ShowServerDetails", "true");
            properties.Save();
        }
        public void LoadProperties()
        {
            if (properties.get("AgentID") != null)
                _finAgent.AgentInformation.AgentID = properties.get("AgentID", "");

            if (properties.get("Ext") != null)
                _finAgent.AgentInformation.Extension = properties.get("Ext", "");

            if (properties.get("XMPPPort") != null)
                _finAgent.AgentInformation.XMPPPort = properties.get("XMPPPort", "");

            if (properties.get("XMPPURL") != null)
                _finAgent.AgentInformation.XMPPURL = properties.get("XMPPURL", "");

            if (properties.get("HTTPPort") != null)
                _finAgent.AgentInformation.HTTPPort = properties.get("HTTPPort", "");

            if (properties.get("HTTPURL") != null)
                _finAgent.AgentInformation.HTTPURL = properties.get("HTTPURL", "");

            if (properties.get("SSL") != null)
                _finAgent.AgentInformation.SSL = Convert.ToBoolean(properties.get("SSL", "false"));

            if (properties.get("XmppConnectionType") != null)
                _finAgent.AgentInformation.XMPPConnectionType = properties.get("XmppConnectionType", "Tls11");

            if (properties.get("HttpConnectionType") != null)
                _finAgent.AgentInformation.HTTPConnectionType = properties.get("HttpConnectionType", "Tls12");

            if (properties.get("Pass") != null) { 
                _finAgent.AgentInformation.Password = properties.get("Pass", "");
                if (_finAgent.AgentInformation.Password != ""){
                    Password.Password = _finAgent.AgentInformation.Password;
                    SavePassword.IsChecked = true;
                }
            }

            //Check Finesse Side A Parameter
            if (properties.get("FinesseA") != null)
                _finAgent.AgentInformation.DomainA = properties.get("FinesseA", "");

            //Check Finesse Side B Parameter
            if (properties.get("FinesseB") != null)
                _finAgent.AgentInformation.DomainB = properties.get("FinesseB", "");

            //Check the show server details
            if (properties.get("ShowServerDetails") == null)
                properties.set("ShowServerDetails", "true");
            properties.Save();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            _finAgent.AgentInformation.Password = Password.Password;
            _finAgent.LogMessages.Clear();
            if (_loadingWindow == null)
                _loadingWindow = new Loading(_finAgent, this);
            else
                _finAgent.FinView = _loadingWindow as FinView;
            Hide();
            _loadingWindow.Show();
            SaveProperties();
            _finAgent.SignIn();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        public void FireErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void FireNewEvent()
        {
        }
        public void FireLoadingMessage(string msg)
        {
        }
        public void FireReLoginEvent()
        {
        }
        public void FireLoadLoginScreen()
        {
        }
        public void FireCallEvent(Dialog dialog)
        {
        }

        public void SetContext(IModel model, FinView finView)
        {
        }

        public Screen GetLocation()
        {
            return Screen.Login;
        }

        public void FireLogMessage(string msg)
        {
        }

        public void FireDisconnectEvent()
        {
        }
    }
}
