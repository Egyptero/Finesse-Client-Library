using agsXMPP;
using System;
using System.Diagnostics;
using System.Threading;

namespace CXDE.Server_Side
{
    public class FinXMPPClient
    {

        private XmppClientConnection xmpp;
        private FinAgent _FinAgent;
        public bool RetryConnection { get; set; }
        private bool IsConnected;
        private bool _wait { get; set; }
        private bool _error { get; set; }
        public FinXMPPClient(FinAgent finAgent)
        {
            _FinAgent = finAgent;
            IsConnected = false;
            RetryConnection = false;
        }
        public void DisconnectXMPP()
        {
            RetryConnection = false;
            if (xmpp != null)
                xmpp.Close();
            xmpp = null;
        }
        private bool ConnectXMPP()
        {
            string Username = _FinAgent._agentInformation.AgentID;
            string url_link;
            string finesseServer;
            if (_FinAgent._agentInformation.ActiveSite != null)
            {
                if (_FinAgent._agentInformation.ActiveSite.Equals("A"))
                {
                    if (_FinAgent.TraceStatus.Equals("true"))
                        Trace.Write("CXConnect: Reconnecting to XMPP Side A");
                    url_link = "http://" + _FinAgent._agentInformation.DomainA + ":7071/http-bind/";//":7071/http-bind/";
                    finesseServer = _FinAgent._agentInformation.DomainA;
                    return ConnectXMPP(Username, _FinAgent._agentInformation.AgentID, _FinAgent._agentInformation.Password, _FinAgent._agentInformation.DomainA, url_link, finesseServer);

        }
                else if (_FinAgent._agentInformation.ActiveSite.Equals("B"))
                {
                    if (_FinAgent.TraceStatus.Equals("true"))
                        Trace.Write("CXConnect: Reconnecting to XMPP Side B");
                    url_link = "http://" + _FinAgent._agentInformation.DomainB + ":7071/http-bind/";//":7071/http-bind/";
                    finesseServer = _FinAgent._agentInformation.DomainB;
                    return ConnectXMPP(Username, _FinAgent._agentInformation.AgentID, _FinAgent._agentInformation.Password, _FinAgent._agentInformation.DomainB, url_link, finesseServer);
                }
            } else
            {
                if (_FinAgent.TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: Connecting to XMPP Side A as default");

                url_link = "http://" + _FinAgent._agentInformation.DomainA + ":7071/http-bind/";//":7071/http-bind/";
                finesseServer = _FinAgent._agentInformation.DomainA;
                return ConnectXMPP(Username, _FinAgent._agentInformation.AgentID, _FinAgent._agentInformation.Password, _FinAgent._agentInformation.DomainA, url_link, finesseServer);
            }

            if (_FinAgent.TraceStatus.Equals("true"))
                Trace.Write("CXConnect: Error during connecting to Finesse XMPP !!!!!");

            return false;
        }
        public bool ConnectXMPP(string userName, string agentID, string password, string domain, string url, string finesseServer)
        {

            System.Net.ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            _FinAgent.FireLoadingMessage("Setting XMPP Connection Defination");
            try
            {
               // if(xmpp == null)
                xmpp = new XmppClientConnection();

                xmpp.Server = domain;
                xmpp.ConnectServer = url;
                xmpp.Username = agentID;
                xmpp.Password = password;
                xmpp.SocketConnectionType = agsXMPP.net.SocketConnectionType.Bosh;//Direct;//Bosh;
                xmpp.Port = 5222;
                xmpp.Resource = "cisco";
                xmpp.AutoRoster = true;
                xmpp.AutoAgents = false;
                xmpp.AutoPresence = true;
                xmpp.AutoResolveConnectServer = true;
                xmpp.KeepAlive = true;
                xmpp.KeepAliveInterval = 3; 

                xmpp.OnXmppConnectionStateChanged += Xmpp_OnXmppConnectionStateChanged;
                xmpp.OnError += Xmpp_OnError;
                xmpp.OnAuthError += Xmpp_OnAuthError;
                xmpp.OnMessage += Xmpp_OnMessage;
                xmpp.OnSocketError += Xmpp_OnSocketError;
                xmpp.OnStreamError += Xmpp_OnStreamError;
                xmpp.OnRegisterError += Xmpp_OnRegisterError;
                xmpp.OnLogin += Xmpp_OnLogin;

                _FinAgent.FireLoadingMessage("Open XMPP Connection");
                xmpp.Open();
            }
            catch (Exception e)
            {
                if (_FinAgent.TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect:" + e.Message);
                    Trace.Write(e);
                }
                _FinAgent.FireErrorMessage(e.Message);
                return false;
            }
            _wait = true;
            _error = false;
            var _maxWaitCounter = 0;
            do
            {
                _FinAgent.FireLoadingMessage("Waiting for XMPP Connection ....");
                Thread.Sleep(500);
                _maxWaitCounter++;
            } while (_wait && _maxWaitCounter < 20);

            if (!xmpp.Authenticated)
            {
                if (_FinAgent.TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: Error in XMPP Login for Agent ID ="+agentID+" & password ="+password);

                _FinAgent.FireErrorMessage("Please check Agent ID and Password");
                xmpp.Close();
                xmpp = null;
                return false;
            }

            if (!_error && !_wait)
            {
                IsConnected = true;
                if (_FinAgent.TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: XMPP connected for Agent ID =" + agentID);

                //RetryConnection = true;
                return true;
            }
            xmpp.Close();
            xmpp = null;
            return false;
        }

        private void Xmpp_OnLogin(object sender)
        {
            _FinAgent.FireLoadingMessage("Logged in to XMPP");
            _wait = false;

        }

        private void Xmpp_OnRegisterError(object sender, agsXMPP.Xml.Dom.Element e)
        {
            if (_FinAgent.TraceStatus.Equals("true"))
                Trace.Write("CXConnect: Error in XMPP connection (OnRegisterError)");

            _error = true;
            _wait = false;
        }
        private void Xmpp_OnMessage(object sender, agsXMPP.Xml.Dom.Element msg)
        {
            string message = msg.ToString();
            _FinAgent.FireNewMessage(message);

        }
        public void SubscribeToEvents()
        {
            //int uriteamslistCounter = 0;
            //string xmppDomain = string.Empty;
            //string xmppPubSubDomain = string.Empty;
            //string uri = string.Empty;
            //getSystemInfo.webappPath = "http://" + Domain + "/finesse";
            //getSystemInfo.systemInfo(UserName, AgentId, Password);
            //string userResponseSystemInfo = getSystemInfo.getSystemInfo.Response;
            //getUser.webappPath = "http://" + Domain + "/finesse";
            //getUser.getUser(UserName, AgentId, Password);
            //string getUserResponse = getUser.getUserResponse.Response;
            //// need to know the number of the teams
            //string[] uriteamslist = new string[getUserResponse.Length];
            //if (userResponseSystemInfo != string.Empty)
            //{
            //    try
            //    {
            //        //for testing only to check it working fine or not
            //        //string textXml = "<User><uri>/finesse/api/User/1234</uri><roles><role>Agent</role><role>Supervisor</role></roles><teamId>500</teamId><teamName>Sales</teamName><dialogs>/finesse/api/User/1234/Dialogs</dialogs><teams><Team><uri>/finesse/api/Team/2001</uri><id>2001</id><name>First Line Support</name></Team><Team><uri>/finesse/api/Team/2002</uri><id>2002</id><name>Second Line Support</name></Team></teams></User>";
            //        //userResponseSystemInfo = textXml;
            //        XElement xmlroot = XElement.Parse(userResponseSystemInfo);
            //        xmppDomain = (xmlroot.Element("xmppDomain").Value);
            //        xmppPubSubDomain = (xmlroot.Element("xmppPubSubDomain").Value);

            //        //MessageBox.Show(xmppPubSubDomain.ToString());
            //    }
            //    catch (Exception e)
            //    {
            //        System.Windows.MessageBox.Show(e.ToString());
            //    }
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("userResponseSystemInfo is empty ");
            //}
            //if (getUserResponse != string.Empty)
            //{
            //    try
            //    {
            //        XElement xmlroots = XElement.Parse(getUserResponse);
            //        //not sure need more testing 
            //        // var teamID = xmlroots.Descendants("Team");
            //        var team = xmlroots.Element("Team");
            //        if (team != null)
            //        {
            //            foreach (XElement second in xmlroots.Descendants("Team"))
            //            {
            //                uri = second.Element("uri").Value;
            //                uriteamslist[uriteamslistCounter] = uri;
            //                System.Windows.MessageBox.Show(second.Element("uri").Value);
            //                uriteamslistCounter++;
            //            }
            //            System.Windows.MessageBox.Show(uri.ToString());
            //        }
            //        else
            //        {
            //            string singleTeamID = string.Empty;
            //            singleTeamID = (xmlroots.Element("teamId").Value);
            //            uriteamslist[uriteamslistCounter] = singleTeamID;
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        System.Windows.MessageBox.Show(e.ToString());
            //    }
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("getUserResponse is empty ");
            //}

            //// we will make foor loop to get all nodes of teams 
            //string jid = UserName + "@" + FinesseServer;
            //for (int uriteams = 0; uriteams < uriteamslist.Length; uriteams++)
            //{
            //    if (uriteamslist[uriteams] == null)
            //        break;
            //    // this means that uriteamslist has team id only will concatunate it 
            //    if (Regex.IsMatch(uriteamslist[uriteams], @"^\d+"))
            //    {
            //        string xml = "<iq type='set'" +
            //        "from = '" + jid + "'" +
            //        "to = pubsub.'" + FinesseServer + "'" +
            //        "id = 'sub1' >" +
            //        "< pubsub xmlns = 'http://jabber.org/protocol/pubsub' />" +
            //        "< subscribe" +
            //        "node = '/finesse/api/Team/" + uriteamslist[uriteams] + "/ Users'" +
            //        "jid = '" + jid + "' />" +
            //        "</ pubsub >" +
            //        "</ iq > ";
            //        xmpp.Send(xml);
            //    }
            //    //else
            //    //{
            //    //    string xml = "<iq type='set'" +
            //    //   "from = '" + jid + "'" +
            //    //   "to = '" + xmppDomain + "'" +
            //    //   "id = 'sub1' >" +
            //    //   "< pubsub xmlns = '" + xmppPubSubDomain + "' >" +
            //    //   "< subscribe" +
            //    //   "node = '" + uriteamslist[uriteams] + "'" +
            //    //   "jid = '" + jid + "' />" +
            //    //   "</ pubsub >" +
            //    //   "</ iq > ";
            //    //    xmpp.Send(xml);
            //    //}

            //}

        }
        private void Xmpp_OnStreamError(object sender, agsXMPP.Xml.Dom.Element e)
        {
            if (_FinAgent.TraceStatus.Equals("true"))
                Trace.Write("CXConnect: Error in XMPP connection (OnStreamError)");

            _FinAgent.FireErrorMessage("Please check Finesse Server");
            _error = true;
            _wait = false;
        }
        private void Xmpp_OnSocketError(object sender, Exception ex)
        {
            if (_FinAgent.TraceStatus.Equals("true"))
                Trace.Write("CXConnect: Error in XMPP connection (OnSocketError)");

            _FinAgent.FireErrorMessage(ex.Message);
            _error = true;
            _wait = false;
        }
        private void Xmpp_OnXmppConnectionStateChanged(object sender, XmppConnectionState state)
        {
            if (state == XmppConnectionState.SessionStarted)
                _wait = false;
            else if (state == XmppConnectionState.Disconnected && RetryConnection)
            {
                var _maxWaitCounter = 0;
                do
                {
                    if (_FinAgent.TraceStatus.Equals("true")) 
                        Trace.Write("CXConnect: Error in XMPP connection (System was disconnected)");

                    _FinAgent.FireErrorMessage("Connection to Finesse server is lost .. trying to reconnect");
                    Thread.Sleep(5000);
                    _maxWaitCounter++;
                    IsConnected = ConnectXMPP();
                } while (_maxWaitCounter < 10 && !IsConnected);

                if (!IsConnected)
                {
                    _FinAgent.FireLoadLoginScreen();
                }
                else
                    _FinAgent.FireReLoginEvent();
            }
            //We need to handle auto disconnection
        }
        private void Xmpp_OnAuthError(object sender, agsXMPP.Xml.Dom.Element e)
        {
            if (_FinAgent.TraceStatus.Equals("true"))
                Trace.Write("CXConnect: Error in XMPP connection (OnAuthError)");

            _FinAgent.FireErrorMessage("Invalid Agent ID or Password");
            _error = true;
            _wait = false;
        }
        private void Xmpp_OnError(object sender, Exception ex)
        {
            if (_FinAgent.TraceStatus.Equals("true"))
                Trace.Write("CXConnect: Error in XMPP connection (OnError)");
            _FinAgent.FireErrorMessage(ex.Message);
            _error = true;
            _wait = false;
        }
    }
}
