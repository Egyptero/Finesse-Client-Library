using Matrix.Xmpp.Client;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace FinesseClient.Core.XMPP
{
    public class FinMatrixClient
    {
        private XmppClient xmppClient;
        private FinAgent _FinAgent;
        private bool onLogin;
        private Timer connectTimer;
        private TimerCallback connectTimerCallback;
        private bool forceDisconnect;
        public bool IsConnected;
        private int connectionTrials = 0;
        string uriPrefix = "http://";
        private string lic = @"eJxkkVFTozAUhf8K42tHQ13bXXZiRgdoSysVRC32LcAFQknSJkFKf/121l19
8O3M+c7cc2YufmA5CA3WkbdC317Q6lLL0vRUwe/2A10QHClZdLkJChJSo9gR
oy8Hxx0VhpmBjDH61NjttJEcFMFryoEkNShWWvcPVsjqAaO/JnYl31MxkEAY
qBQ1TAorAfV+LtYWFYX1DHktZCsrBhqj/3Hsc8paoqHl52N3TBsBppdqp69y
yTH6wOf854aXfUEN+Mc9U+CdFbm2x479w55g9A3hhFWCmk4BaWrHHaWKM2lH
/rjn3ltED9vDcxneLOr5yu2anaueUOxGByeac1N6aPyuMi9d7+KNE25eHkVw
HTfLeebsQiPSCMI+ODXbYVXAermc/fSz2XJa9kmXB2+jZhOnQ7sK/FlLm0U9
jW5yZGfpr9PTpLNfs+RR3ufea7jq6UALQK0zOs0mZVFlJ7PXtxh97cbo31vJ
HwE=";

        public FinMatrixClient(FinAgent finAgent)
        {

            _FinAgent = finAgent;
            if (finAgent.AgentInformation.DomainB == null)
                finAgent.AgentInformation.DomainB = finAgent.AgentInformation.DomainA;
            if (finAgent.AgentInformation.DomainA == null)
                finAgent.AgentInformation.DomainA = finAgent.AgentInformation.DomainB;
            _FinAgent.FireLogMessage("Finesse XMPP Client Object Initiation");
            
            if (finAgent.AgentInformation.SSL)
                uriPrefix = "https://";
            xmppClient = new XmppClient
            {
                XmppDomain = finAgent.AgentInformation.DomainB,
                Uri = new System.Uri(uriPrefix + finAgent.AgentInformation.DomainB + ":" + finAgent.AgentInformation.XMPPPort + finAgent.AgentInformation.XMPPURL),
                Username = finAgent.AgentInformation.AgentID,
                Password = finAgent.AgentInformation.Password,
                Transport = Matrix.Net.Transport.BOSH,
                Port = (finAgent.AgentInformation.SSL) ? 5223 : 5222,
                Resource = "cisco",
//                StartTls = finAgent.AgentInformation.SSL,
//                KeepAliveInterval = 10,
                ResolveSrvRecords = true
            };

            Matrix.License.LicenseManager.SetLicense(lic);

            xmppClient.OnMessage += OnMessage;
            xmppClient.OnClose += OnClose;
            xmppClient.OnBind += OnBind;
            xmppClient.OnBindError += OnBindError;
            xmppClient.OnAuthError += OnAuthError;
            xmppClient.OnError += OnError;
            xmppClient.OnStreamError += OnStreamError;
            xmppClient.OnXmlError += OnXmlError;
            xmppClient.OnLogin += OnLogin;
            xmppClient.OnValidateCertificate += OnValidateCertificate;
            xmppClient.OnReceiveXml += OnReceiveXml;
            connectTimerCallback = Connect;
            connectTimer = new Timer(connectTimerCallback);
            _FinAgent.FireLogMessage("Finesse XMPP Client Object is ready");
        }

        private void OnReceiveXml(object sender, Matrix.TextEventArgs e)
        {
        }

        private void OnValidateCertificate(object sender, Matrix.CertificateEventArgs e)
        {
            e.AcceptCertificate = true;
            _FinAgent.FireLogMessage("Cetificate was accepted by XMPP server");
        }


        #region xmpp error handlers
        private void OnBindError(object sender, IqEventArgs e)
        {
            if (e != null)
                _FinAgent.FireLogMessage("XMPP Bind Error and error is:" + e.ToString());
            else
                _FinAgent.FireLogMessage("XMPP Bind Error without Excpetion. Exception is null");
            Disconnect();
        }

        private void OnStreamError(object sender, Matrix.StreamErrorEventArgs e)
        {
            if (e != null)
                _FinAgent.FireLogMessage("XMPP Stream Error and error is:" + e.ToString());
            else
                _FinAgent.FireLogMessage("XMPP Stream Error without Excpetion. Exception is null");
        }

        private void OnXmlError(object sender, Matrix.ExceptionEventArgs e)
        {
            if (e != null)
                _FinAgent.FireLogMessage("XMPP XML Error and error is:" + e.ToString());
            else
                _FinAgent.FireLogMessage("XMPP XML Error without Excpetion. Exception is null");
        }

        private void OnAuthError(object sender, Matrix.Xmpp.Sasl.SaslEventArgs e)
        {
            if (e != null)
                _FinAgent.FireLogMessage("XMPP Authentication Error and error is: " + e.ToString());
            else
                _FinAgent.FireLogMessage("XMPP Authentication Error without Excpetion. Exception is null");
            if (connectTimer != null)
            {
            connectTimer.Dispose();
                connectTimer = null;
            }
            _FinAgent.FireLoadLoginScreen();
            _FinAgent.FireErrorMessage("Error in user name or password");
        }

        private void OnError(object sender, Matrix.ExceptionEventArgs e)
        {
            string msg = (e != null ? (e.Exception != null ? e.Exception.Message : "") : "");

            _FinAgent.FireLogMessage("XMPP Error: "+msg);
            _FinAgent.FireLogMessage("XMPP Error and Current state is :"+e.State as string);
           
            _FinAgent.FireErrorMessage(msg);
            if (!onLogin && connectionTrials < 10)
            {
                IsConnected = false;
                StartConnectTimer();
            }
            else
            {
                if (IsConnected && connectionTrials < 10)
                {
                    StartConnectTimer();
                    _FinAgent.FireLoadingMessage("XMPP Disconnect , will try to reconnect");
                    _FinAgent.FireDisconnectEvent();
                }
                else 
                {
                    if (connectTimer != null)
                    {
                connectTimer.Dispose();
                        connectTimer = null;
                    }
                _FinAgent.FireLoadLoginScreen();
            }
        }
        }
        #endregion

        #region << XMPP handlers >>

        private void OnLogin(object sender, Matrix.EventArgs e)
        {
            _FinAgent.FireLoadingMessage("Agent Logged into XMPP");
            _FinAgent.FireLogMessage("XMPP Login Completed");
            onLogin = true;
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            string message = e.Message.Document.ToString();
            _FinAgent.FireNewMessage(message);
        }

        private void OnBind(object sender, Matrix.JidEventArgs e)
        {
            _FinAgent.FireLoadingMessage("JID: " + e.Jid);
            _FinAgent.FireLogMessage("JID: " + e.Jid);
            IsConnected = true;
            _FinAgent.finRestClient.WebappPath = uriPrefix + xmppClient.XmppDomain + ":" + _FinAgent.AgentInformation.HTTPPort+ _FinAgent.AgentInformation.HTTPURL;
            _FinAgent.FireLoadingMessage("Connecting Rest ....");
            _FinAgent.FireLogMessage("Connecting Rest on side: "+xmppClient.XmppDomain);
            string result = _FinAgent.finRestClient.SignIn(_FinAgent.AgentInformation.AgentID, _FinAgent.AgentInformation.AgentID, _FinAgent.AgentInformation.Extension, _FinAgent.AgentInformation.Password);
            if (result == "" || result == "" || result == null)
            {// Sucess
                _FinAgent.FireLoadingMessage("Rest Connected ....");
                _FinAgent.FireLogMessage("Rest connected to side: " + _FinAgent.finRestClient.WebappPath);
                _FinAgent.FireLoadingMessage("");
                _FinAgent.FireReLoginEvent();
                connectionTrials = 0;
            }
            else
            {
                if (connectTimer != null)
                {
                connectTimer.Dispose();
                    connectTimer = null;
                }
                _FinAgent.FireLogMessage("Failed to connect to rest with error :"+result);
                _FinAgent.FireErrorMessage(result + ", Can not connect to Finesse Rest, Check finesse http configuration");
                Disconnect();
                //_FinAgent.FireLoadLoginScreen();
                
            }

        }

        private void OnClose(object sender, Matrix.EventArgs e)
        {
            _FinAgent.FireLogMessage("XMPP Closing Connection .. : "+e.ToString());
            IsConnected = false;

            if (!forceDisconnect && connectionTrials < 10)
                StartConnectTimer();
            else
                _FinAgent.FireLoadLoginScreen();
        }
        #endregion


        private void StartConnectTimer()
        {
            _FinAgent.FireDisconnectEvent();
            _FinAgent.FireErrorMessage("Starting to reconnect ...");
            _FinAgent.FireLogMessage("XMPP will try to reconnect, disconnect event was fired");
            if(connectTimer != null)
            connectTimer.Change(5000, Timeout.Infinite);
        }

        public void Connect(Object obj)
        {
            if (_FinAgent.AgentInformation.SSL)
            {
                System.Net.ServicePointManager.Expect100Continue = true;
                if (_FinAgent.AgentInformation.XMPPConnectionType == null)
                    return;
                else if (_FinAgent.AgentInformation.XMPPConnectionType.Equals("Ssl3"))
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;
                else if (_FinAgent.AgentInformation.XMPPConnectionType.Equals("Tls"))
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;
                else if (_FinAgent.AgentInformation.XMPPConnectionType.Equals("Tls11"))
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11;
                else if (_FinAgent.AgentInformation.XMPPConnectionType.Equals("Tls12"))
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }


            if (xmppClient.XmppDomain.Equals(_FinAgent.AgentInformation.DomainA))
            {
                xmppClient.XmppDomain = _FinAgent.AgentInformation.DomainB;
                xmppClient.Uri = new System.Uri(uriPrefix + _FinAgent.AgentInformation.DomainB + ":" + _FinAgent.AgentInformation.XMPPPort + _FinAgent.AgentInformation.XMPPURL);
            }
            else
            {
                xmppClient.XmppDomain = _FinAgent.AgentInformation.DomainA;
                xmppClient.Uri = new System.Uri(uriPrefix + _FinAgent.AgentInformation.DomainA + ":" + _FinAgent.AgentInformation.XMPPPort + _FinAgent.AgentInformation.XMPPURL);
            }
            _FinAgent.FireLoadingMessage("connect: XMPP connecting.... ");
            _FinAgent.FireLogMessage("XMPP Connect to side : "+xmppClient.XmppDomain);
            onLogin = false;
            connectionTrials += 1;
            xmppClient.Open();
            _FinAgent.FireLoadingMessage("Waiting for XMPP connection ....  trial no: "+connectionTrials);
            _FinAgent.FireLogMessage("Waiting XMPP Server Response on IP : " + xmppClient.Uri);
        }
        public void Disconnect()
        {
            _FinAgent.FireLogMessage("XMPP will disconnect now");
            forceDisconnect = true;
            if (connectTimer != null)
            {
            connectTimer.Dispose();
                connectTimer = null;
            }
            xmppClient.Close();
        }

        public string Status()
        {
            return xmppClient.Status;
        }
    }
}
