using Matrix;
using Matrix.Xmpp.Client;
using System;
using System.Net;
using System.Security.Authentication;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;

namespace FinesseClient.Core.XMPP
{
    public class FinMatrixClient
    {

        private XmppClient xmppClient;
        private FinAgent _FinAgent;
        private bool onLogin;
        private Timer connectTimer;
        private TimerCallback connectTimerCallback;

        private Timer disconnectTimer;
        private TimerCallback disconnectTimerCallback;

        private Timer restConnectTimer;
        private TimerCallback restConnectTimerCallback;

        private Timer loginFireTimer;
        private TimerCallback loginFireTimerCallback;

        private bool forceDisconnect;
        private bool typicalMessage;
        public bool IsConnected;

        private int connectionTrials = 0;
        private Jid jid;
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
            try
            {
                if (finAgent.AgentInformation.DomainB == null)
                    finAgent.AgentInformation.DomainB = finAgent.AgentInformation.DomainA;
                if (finAgent.AgentInformation.DomainA == null)
                    finAgent.AgentInformation.DomainA = finAgent.AgentInformation.DomainB;
                _FinAgent.FireLogMessage("Finesse XMPP Client Object Initiation");

                if (finAgent.AgentInformation.SSL)
                    uriPrefix = "https://";
                else
                    uriPrefix = "http://";

                xmppClient = new XmppClient
                {
                    XmppDomain = finAgent.AgentInformation.DomainB,
                    Uri = new System.Uri(uriPrefix + finAgent.AgentInformation.DomainB + ":" + finAgent.AgentInformation.XMPPPort + finAgent.AgentInformation.XMPPURL),
                    Username = finAgent.AgentInformation.AgentID,
                    Password = finAgent.AgentInformation.Password,
                    Transport = Matrix.Net.Transport.Bosh,
                    Port = (finAgent.AgentInformation.SSL) ? 5223 : 5222,
                    Resource = "cisco",
                    ResolveSrvRecords = true,
                    AutoReplyToPing = true,
                    AutoRoster = false,
                    AutoPresence = true
                };
                if (_FinAgent.AgentInformation.XmppKeepAliveInterval != -1)
                    xmppClient.KeepAliveInterval = _FinAgent.AgentInformation.XmppKeepAliveInterval;
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
                xmppClient.Disposed += XmppClient_Disposed;

                connectTimerCallback = Connect;
                disconnectTimerCallback = Disconnect;
                restConnectTimerCallback = ConnectRest;
                loginFireTimerCallback = ValidateFireLogin;
                connectTimer = new Timer(connectTimerCallback);
                disconnectTimer = new Timer(disconnectTimerCallback);
                restConnectTimer = new Timer(restConnectTimerCallback);
                loginFireTimer = new Timer(loginFireTimerCallback);
                _FinAgent.FireLogMessage("Finesse XMPP Client Object is ready");
            }
            catch (Exception ex)
            {
                _FinAgent.FireLogMessage("Error during xmpp object creation for the first time , ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void ValidateFireLogin(Object obj)
        {
            try
            {
                _FinAgent.FireLogMessage("Start to Validate Fire Login");
                if (!typicalMessage)
                {
                    _FinAgent.FireLogMessage("First Message was not fired , seems agent session was still active");
                    _FinAgent.FireLoadingMessage("First Message was not received. Will load it manually");
                    if (!forceDisconnect)
                    {
                        _FinAgent.FireReLoginEvent();
                        typicalMessage = true;
                    }
                }
                else
                    _FinAgent.FireLogMessage("First Message Already delivered");
            }
            catch(Exception ex)
            {
                _FinAgent.FireDebugLogMessage("Error during Validate Fire Login , ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void ConnectRest(Object obj)
        {
            try
            {
                if (IsConnected && !forceDisconnect)
                {
                    _FinAgent.finRestClient.WebappPath = uriPrefix + xmppClient.XmppDomain + ":" + _FinAgent.AgentInformation.HTTPPort + _FinAgent.AgentInformation.HTTPURL;
                    //_FinAgent.FireLoadingMessage("Getting system information ....");
                    //_FinAgent.LoadSystemInformation();
                    _FinAgent.FireLoadingMessage("Connecting Rest ....");
                    _FinAgent.FireLogMessage("Connecting Rest on side: " + xmppClient.XmppDomain);
                    //_FinAgent.SubscribeToUserEvent();
                    //_FinAgent.SubscribeToDialogEvent();

                    string result = _FinAgent.finRestClient.SignIn(_FinAgent.AgentInformation.AgentID, _FinAgent.AgentInformation.AgentID, _FinAgent.AgentInformation.Extension, _FinAgent.AgentInformation.Password);
                    if (result == "" || result == "" || result == null)
                    {// Sucess
                        _FinAgent.FireLoadingMessage("Rest Connected ....");
                        _FinAgent.FireLogMessage("Rest connected to side: " + _FinAgent.finRestClient.WebappPath);

                        //_FinAgent.FireReLoginEvent(); //Temp stop to ensure that first message is coming
                        StartLoginFireTimer();
                        connectionTrials = 0;
                    }
                    else
                    {
                        Disconnect(null);
                        _FinAgent.FireLogMessage("Failed to connect to rest with error :" + result);
                        _FinAgent.FireErrorMessage(result + ", Can not connect to Finesse Rest, Check finesse http configuration");
                    }
                }
            }
            catch (Exception ex)
            {
                _FinAgent.FireLogMessage("Error during rest connection , ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void XmppClient_Disposed(object sender, System.EventArgs e)
        {
            try
            {
                _FinAgent.FireLogMessage("XMPP is disposed , will Fire load login screen now");
                _FinAgent.FireLoadLoginScreen();
                _FinAgent.FireLogMessage("XMPP is disposed , Fired load login screen successfully");
            }
            catch (Exception ex)
            {
                _FinAgent.FireLogMessage("Error during XmppClient_Disposed , ex:" + ex.Message + "\n" + ex.StackTrace);
            }
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
            Disconnect(null);
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
            Disconnect(null);
            _FinAgent.FireErrorMessage("Error in user name or password");
        }

        private void OnError(object sender, Matrix.ExceptionEventArgs e)
        {
            try
            {
                string msg = (e != null ? (e.Exception != null ? e.Exception.Message : "") : "");
                _FinAgent.FireLogMessage("XMPP Error: " + msg + " and xmpp client status is:" + xmppClient.Status);
                _FinAgent.FireLogMessage("XMPP Error and Current state is :" + e.State as string);

                _FinAgent.FireErrorMessage(msg);
                //Mamdouh , we need to check the status of error.
                if (!msg.Equals("Unable to send data"))
                {
                    if (!forceDisconnect)
                    {
                        if (!onLogin && connectionTrials < 10)
                        {
                            IsConnected = false;
                            StartConnectTimer();
                        }
                        else
                        {
                            //if(xmppClient.Status != null && xmppClient.Status.Equals(""))
                            if (IsConnected && connectionTrials < 10)
                            {
                                StartConnectTimer();
                                _FinAgent.FireLoadingMessage("XMPP Disconnect , will try to reconnect");
                                _FinAgent.FireDisconnectEvent();
                            }
                            else if(connectionTrials >= 10)
                                _FinAgent.FireLoadLoginScreen();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _FinAgent.FireLogMessage("Error in OnError ex:" + ex.Message + "\n" + ex.StackTrace);
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
            if(!typicalMessage) // Check for the first message
            {
                _FinAgent.FireNewMessage(message);
                if (!forceDisconnect) // If still connected after the first message then we are alive
                {
                    _FinAgent.FireReLoginEvent();
                    typicalMessage = true;
                }
            }else
                _FinAgent.FireNewMessage(message);
        }

        private void OnBind(object sender, Matrix.JidEventArgs e)
        {
            _FinAgent.FireLoadingMessage("JID: " + e.Jid);
            _FinAgent.FireLogMessage("JID: " + e.Jid);
            jid = e.Jid;
            IsConnected = true;
            StartRestConnectTimer();

        }

        private void OnClose(object sender, Matrix.EventArgs e)
        {
            _FinAgent.FireLogMessage("XMPP Closing Connection .. : " + e.ToString());
            IsConnected = false;
            try
            {
                if (!forceDisconnect && connectionTrials < 10)
                    StartConnectTimer();
                else
                {
                    xmppClient.Dispose();
                    if (disconnectTimer != null)
                        disconnectTimer.Dispose();
                }
            }
            catch (Exception ex)
            {
                _FinAgent.FireLogMessage("Error during OnClose , ex:" + ex.Message + "\n" + ex.StackTrace);
            }

        }
        #endregion

        public void StartDisconnectTimer()
        {
            try
            {
                _FinAgent.FireLogMessage("Starting to Disconnect ...");
                _FinAgent.FireLogMessage("XMPP will try to Disconnect");
                if (_FinAgent.FinesseVersion.Equals("UCCE_11.5"))
                    Disconnect(null);
                else
                {
                    if (disconnectTimer != null)
                    {
                        forceDisconnect = true;
                        disconnectTimer.Change(3000, Timeout.Infinite);
                    }
                }
            }
            catch (Exception ex)
            {
                _FinAgent.FireLogMessage("Error to start the disconnection timer ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void StartConnectTimer()
        {
            try
            {
                _FinAgent.FireDisconnectEvent();
                _FinAgent.FireErrorMessage("Starting to reconnect ...");
                _FinAgent.FireLogMessage("XMPP will try to reconnect, disconnect event was fired");
                if (connectTimer != null)
                    connectTimer.Change(5000, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                _FinAgent.FireLogMessage("Error during StartConnectTimer , ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void StartRestConnectTimer()
        {
            try
            {
                _FinAgent.FireLoadingMessage("Connecting to rest in 3 Secs ...");
                _FinAgent.FireLogMessage("Connecting to rest in 3 Secs ...");
                if (restConnectTimer != null)
                    restConnectTimer.Change(3000, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                _FinAgent.FireLogMessage("Error during StartRestConnectTimer , ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void StartLoginFireTimer()
        {
            try
            {
                _FinAgent.FireLoadingMessage("Login final validation...");
                _FinAgent.FireLogMessage("Login final validation ...");
                if (loginFireTimer != null)
                    loginFireTimer.Change(3000, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                _FinAgent.FireLogMessage("Error during StartLoginFireTimer , ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void Reinitiate()
        {
            try
            {
                connectionTrials = 0;
                forceDisconnect = false;
                IsConnected = false;
                typicalMessage = false;
                if (_FinAgent.AgentInformation.DomainB == null && _FinAgent.AgentInformation.DomainA != null)
                    _FinAgent.AgentInformation.DomainB = _FinAgent.AgentInformation.DomainA;
                if (_FinAgent.AgentInformation.DomainA == null && _FinAgent.AgentInformation.DomainB != null)
                    _FinAgent.AgentInformation.DomainA = _FinAgent.AgentInformation.DomainB;
                _FinAgent.FireLogMessage("Finesse XMPP Client Object Reinitializing");

                if (_FinAgent.AgentInformation.SSL)
                    uriPrefix = "https://";
                else
                    uriPrefix = "http://";

                if (xmppClient != null)
                {
                    xmppClient.XmppDomain = _FinAgent.AgentInformation.DomainB;
                    xmppClient.Uri = new System.Uri(uriPrefix + _FinAgent.AgentInformation.DomainB + ":" + _FinAgent.AgentInformation.XMPPPort + _FinAgent.AgentInformation.XMPPURL);
                    xmppClient.Username = _FinAgent.AgentInformation.AgentID;
                    xmppClient.Password = _FinAgent.AgentInformation.Password;
                    xmppClient.Transport = Matrix.Net.Transport.Bosh;
                    xmppClient.Port = (_FinAgent.AgentInformation.SSL) ? 5223 : 5222;
                    xmppClient.Resource = "cisco";
                    xmppClient.ResolveSrvRecords = true;
                    xmppClient.ResolveSrvRecords = true;
                    xmppClient.AutoReplyToPing = true;
                    xmppClient.AutoRoster = false;
                    xmppClient.AutoPresence = true;

                    if (_FinAgent.AgentInformation.XmppKeepAliveInterval != -1)
                        xmppClient.KeepAliveInterval = _FinAgent.AgentInformation.XmppKeepAliveInterval;
                }
                else
                {
                    xmppClient = new XmppClient
                    {
                        XmppDomain = _FinAgent.AgentInformation.DomainB,
                        Uri = new System.Uri(uriPrefix + _FinAgent.AgentInformation.DomainB + ":" + _FinAgent.AgentInformation.XMPPPort + _FinAgent.AgentInformation.XMPPURL),
                        Username = _FinAgent.AgentInformation.AgentID,
                        Password = _FinAgent.AgentInformation.Password,
                        Transport = Matrix.Net.Transport.Bosh,
                        Port = (_FinAgent.AgentInformation.SSL) ? 5223 : 5222,
                        Resource = "cisco",
                        ResolveSrvRecords = true,
                        AutoReplyToPing = true,
                        AutoRoster = false,
                        AutoPresence = true

                    };
                    if (_FinAgent.AgentInformation.XmppKeepAliveInterval != -1)
                        xmppClient.KeepAliveInterval = _FinAgent.AgentInformation.XmppKeepAliveInterval;

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
                    xmppClient.Disposed += XmppClient_Disposed;
                }

                connectTimerCallback = Connect;
                disconnectTimerCallback = Disconnect;
                restConnectTimerCallback = ConnectRest;
                loginFireTimerCallback = ValidateFireLogin;
                connectTimer = new Timer(connectTimerCallback);
                disconnectTimer = new Timer(disconnectTimerCallback);
                restConnectTimer = new Timer(restConnectTimerCallback);
                loginFireTimer = new Timer(loginFireTimerCallback);


                _FinAgent.FireLogMessage("Finesse XMPP Client Object Reinitialized");
            }
            catch (Exception ex)
            {
                _FinAgent.FireLogMessage("Error in XMPP reinitiate ex:" + ex.Message + "\n" + ex.StackTrace);
            }

        }

        public void Connect(Object obj)
        {
            try
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
                        System.Net.ServicePointManager.SecurityProtocol = NetworkCustomProtocols.Tls11;
                    else if (_FinAgent.AgentInformation.XMPPConnectionType.Equals("Tls12"))
                        System.Net.ServicePointManager.SecurityProtocol = NetworkCustomProtocols.Tls12;
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
                _FinAgent.FireLogMessage("XMPP Connect to side : " + xmppClient.XmppDomain);
                onLogin = false;
                typicalMessage = false;
                connectionTrials += 1;
                xmppClient.Open();
                _FinAgent.FireLoadingMessage("Waiting for XMPP connection ....  trial no: " + connectionTrials);
                _FinAgent.FireLogMessage("Waiting XMPP Server Response on IP : " + xmppClient.Uri);
            }
            catch (Exception ex)
            {
                _FinAgent.FireLogMessage("Error in XMPP Connect ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void Disconnect(Object obj)
        {
            try
            {
                _FinAgent.FireLogMessage("XMPP will disconnect now");

                forceDisconnect = true;
                if (connectTimer != null)
                {
                    connectTimer.Dispose();
                    connectTimer = null;
                }
                //xmppClient.SendUnavailablePresence("unavaliable");
                //RequestDisconnect();
                //xmppClient.Dispose();
                if (xmppClient.StreamActive)
                {
                    //_FinAgent.UnSubscribeFromUserEvent();
                    //_FinAgent.UnSubscribeFromDialogEvent();
                    xmppClient.SendUnavailablePresence();
                    xmppClient.Close();
                    _FinAgent.FireLogMessage("XMPP close request sent");
                }
                else
                    _FinAgent.FireLoadLoginScreen();
            }
            catch (Exception ex)
            {
                _FinAgent.FireLogMessage("Error in XMPP Disconnect ex:" + ex.Message + "\n" + ex.StackTrace);
            }

            // xmppClient = null;
        }
        public string Status()
        {
            return xmppClient.Status;
        }

        public void UnsubscibeFromUri(string uri)
        {
            if (xmppClient == null)
            {
                _FinAgent.FireErrorMessage("Unable to un-subscribe from uri as XMPP Client is null, Please contact system admin and try to restart the application.");
                _FinAgent.FireDebugLogMessage("Unable to un-subscribe from uri as XMPP Client is null, Please contact system admin and try to restart the application.");
                return;
            }
            if (jid == null)
            {
                _FinAgent.FireErrorMessage("Unable to un-subscribe from uri as jid is not binded yet, Please contact system admin and try to restart the application.");
                _FinAgent.FireDebugLogMessage("Unable to un-subscribe from uri as jid is not binded yet, Please contact system admin and try to restart the application.");
                return;
            }
            try
            {
                _FinAgent.FireLogMessage("Unsubscribe from uri:" + uri);
                string xmlQueuesNotification = "<iq type='set' from='" + _FinAgent.AgentInformation.AgentID + "@" + _FinAgent.SystemXmppDomain + "' to='" + _FinAgent.SystemXmppPubSubDomain + "' id='sub9'>" +
                 "<pubsub xmlns='http://jabber.org/protocol/pubsub'>" +
                 "<unsubscribe node='" + uri + "' jid='" + _FinAgent.AgentInformation.AgentID + "@" + _FinAgent.SystemXmppDomain + "'/>" +
                 "</pubsub>" +
                 "</iq>";

                Matrix.Xml.XmppXElement xmppXElement = new Matrix.Xml.XmppXElement(XElement.Parse(xmlQueuesNotification));
                if (xmppClient.StreamActive)
                    xmppClient.Send(xmppXElement);

            }
            catch (Exception e)
            {
                _FinAgent.FireDebugLogMessage("Error during uri unsubscription. please check excption:\n" + e.StackTrace);
            }
        }
        public void SubscribeToUri(string uri)
        {

            if (xmppClient == null)
            {
                _FinAgent.FireErrorMessage("Unable to subscribe to uri as XMPP Client is null, Please contact system admin and try to restart the application.");
                _FinAgent.FireDebugLogMessage("Unable to subscribe to uri as XMPP Client is null, Please contact system admin and try to restart the application.");
                return;
            }
            if (jid == null)
            {
                _FinAgent.FireErrorMessage("Unable to subscribe to uri as jid is not binded yet, Please contact system admin and try to restart the application.");
                _FinAgent.FireDebugLogMessage("Unable to subscribe to uri as jid is not binded yet, Please contact system admin and try to restart the application.");
                return;
            }
            try
            {
                _FinAgent.FireLoadingMessage("Subscribe to uri: " + uri);
                _FinAgent.FireLogMessage("Subscribe to uri:" + uri);
                string xmlQueuesNotification = "<iq type='set' from='" + _FinAgent.AgentInformation.AgentID + "@" + _FinAgent.SystemXmppDomain + "' to='" + _FinAgent.SystemXmppPubSubDomain + "' id='sub9'>" +
                 "<pubsub xmlns='http://jabber.org/protocol/pubsub'>" +
                 "<subscribe node='" + uri + "' jid='" + _FinAgent.AgentInformation.AgentID + "@" + _FinAgent.SystemXmppDomain + "'/>" + //_FinAgent.AgentInformation.AgentID + "@" + _FinAgent.SystemXmppDomain
                 "</pubsub>" +
                 "</iq>";


                Matrix.Xml.XmppXElement xmppXElement = new Matrix.Xml.XmppXElement(XElement.Parse(xmlQueuesNotification));
                xmppClient.Send(xmppXElement);
                _FinAgent.FireLoadingMessage("Subscribed to uri: " + uri);

            }
            catch (Exception e)
            {
                _FinAgent.FireDebugLogMessage("Error during queue subscription. please check excption:\n" + e.StackTrace);
            }

        }
    }
}
