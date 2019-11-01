/*****************************************************************************************************************************
 * FinAgent Main File
 * File is designed to be the finesse agent client interface. It is the main class of the SDK. 
 * Fin Agent is representing the Model
 * User Client should bind to the SDK through the FinAgent Object. Also the object should be created one
 * Version 2.284
 *  Change the logout mechansim for finesse 11.6
 *  Add reserved object for user own model on the AgentInformation , Dialog  and FinAgent
 * Version 2.285
 *  Fix a bug is set Selected Voice Status
 *  Fix a bug is updating voice status list
 * Version 2.286
 *  Add Execption handling with log messages in the FinAgent Methods.
 *  Update the XMPP Client and add exception handling in all methods
 *  Update the XMPP Client in case of error at XMPP to avoid disconnecting the system
 * Version 2.287
 *  Change the observable collection to be MT where we can change the list in real time from different thread without dispatcher
 * Version 2.288
 *  Add condition at the Unsubcribe Queue Events to ensure that XMPPClient Stream is still active
 *  Reinitiate the dialogs and queues at the load time. Make Load Call Information Private method
 *  Add version setting in FinAgent. Defaullt is UCCE_11.5
 *  Add Keep Live Interval Parameter for the Agent Information
 * Version 2.289
 *  Changes the login to rest mechanism (Move it in timer method at the bind of XMPP with 3 seconds delay)
 *  Ensure the receive of the XMPP first message
 *  Add API Error Handling at login time for all login failures
 *  Add Error Handling for system failure
 *  Change the state timer mechansim to initiate at the SignIn time
 *  Implement Fire Login Event Validation via timer with 3 seconds at the sucess of rest connection
 *  Total login time will be 7 to 10 seconds at minimum
 * Version 2.290
 *  Change the Update Voice Status to add work ready spelling correction in the status label and pending status
 *  Set the iconname for the work_ready , work_not_ready status.
 * Version 2.291
 *  Change the finesse rest client to format the outcome message before replying back to the requester. Becuase sometime the parse fail
 * Version 2.292
 *  Fix bug in error handling as dialog error code 1 conflict with user error code 1. Dialog Error Code 1 is not mentioned in Cisco Development Guide
 *  Add Trace Levels with 4 levels of traces
 * Version 2.293
 *  Change the Log Message of the FireNewMessage to be inside the execute message where we can apply different levels for the Queue to be Level 4 instead of 3
 *****************************************************************************************************************************/

using FinesseClient.Common;
using FinesseClient.Core.Rest;
using FinesseClient.Core.XMPP;
using FinesseClient.Model;
using FinesseClient.View;
using FinesseClient.License;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using static FinesseClient.Model.AgentInformation;
using System.IO;
using FinesseClient.License.Security;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Globalization;

namespace FinesseClient
{
    public class FinAgent : ObservableObject
    {
        #region Fields
        private FinMatrixClient finMatrixClient;
        public FinRestClient finRestClient;
        private FinView _finView;
        private AgentInformation _agentInformation;
        private string _loadingMessage = "Loading ...";
        private MTObservableCollection<string> _logMessages = new MTObservableCollection<string>();
        private string _errorMessage;
        private bool _isInteractionSpace;
        private bool _isMenuSpace;
        private StateTimer _stateTimer;
        private bool _traceStatus;
        private bool _saveLog;
        private string _logLocation;
        private string _lastEventMessage;
        private string _systemStatus;
        private string _systemXmppDomain;
        private string _systemXmppPubSubDomain;
        private bool isSecondMessage;
        private Object _reservedObject;
        private string _finesseVersion = "UCCE_11.5";
        private int _traceLevel = 1;
        private bool _validLicense = false;
        //private System.Collections.Queue _messageQueue = new System.Collections.Queue();
        //private QueueManager _queueManager;
        //private Thread _queueThread;
        #endregion

        #region Properties
        //public System.Collections.Queue MessageQueue { get { return _messageQueue; } set { _messageQueue = value; OnPropertyChanged("MessageQueue"); } }
        public Object ReservedObject { get { return _reservedObject; } set { _reservedObject = value; OnPropertyChanged("ReservedObject"); } }
        public Boolean IsInteractionSpace { get { return _isInteractionSpace; } set {
                if (_isInteractionSpace != value)
                {
                    _isInteractionSpace = value;
                    if (value)
                        IsMenuSpace = false;
                    OnPropertyChanged("IsInteractionSpace");
                }
            }
        }

        public Boolean IsMenuSpace { get { return _isMenuSpace; } set {
                if (_isMenuSpace != value)
                {
                    _isMenuSpace = value;
                    if (value)
                        IsInteractionSpace = false;
                    else IsInteractionSpace = true;
                    OnPropertyChanged("IsMenuSpace");
                }
            }
        }
        public AgentInformation AgentInformation { get { return _agentInformation; } set { _agentInformation = value; OnPropertyChanged("AgentInformation"); } }
        public FinView FinView { get { return _finView; } set { _finView = value; } }
        public Boolean TraceStatus { get { return _traceStatus; } set { _traceStatus = value; OnPropertyChanged("TraceStatus"); } }
        public Boolean SaveLog { get { return _saveLog; } set { _saveLog = value; OnPropertyChanged("SaveLog"); } }
        public String LogLocation { get { return _logLocation; } set { _logLocation = value; OnPropertyChanged("LogLocation"); } }
        public String LoadingMessage { get { return _loadingMessage; }
            set {
                _loadingMessage = value;
                OnPropertyChanged("LoadingMessage");
                
            } }
        public String ErrorMessage { get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged("ErrorMessage");
                
            }
        }
        public StateTimer StateTimer { get { return _stateTimer; } set { _stateTimer = value; OnPropertyChanged("StateTimer"); } }
        public MTObservableCollection<string> LogMessages { get { return _logMessages; } set { _logMessages = value; OnPropertyChanged("LogMessages"); } }
        public String SystemStatus { get { return _systemStatus; } set { _systemStatus = value; OnPropertyChanged("SystemStatus"); } }
        public String SystemXmppDomain { get { return _systemXmppDomain; } set { _systemXmppDomain = value; OnPropertyChanged("SystemXmppDomain"); } }
        public String SystemXmppPubSubDomain { get { return _systemXmppPubSubDomain; } set { _systemXmppPubSubDomain = value; OnPropertyChanged("SystemXmppPubSubDomain"); } }
        public String FinesseVersion { get { return _finesseVersion; } set { _finesseVersion = value; OnPropertyChanged("FinesseVersion"); } }
        public int TraceLevel { get { return _traceLevel; } set { _traceLevel = value; OnPropertyChanged("TraceLevel"); } }
        #endregion

        #region Constructors
        public FinAgent()
        {
            FireLogMessage("FinAgent Object creation", 3);
            FireLogMessage("License is being verified", 3);
            bool valid = checkLicense();
            if (!valid)
            {
                License.License myLicense = new License.License();
                myLicense.Show();
            }
            else
            {
                _validLicense = true;
            }
            initiateAgent();

        }

        public FinAgent(string license)
        {
            FireLogMessage("FinAgent Object creation",3);
            FireLogMessage("License is being verified", 3);
            //TODO We need to check the license here //If valid license , then continue , if not then popup
            bool valid = validateLicense(license);
            if (!valid)
            {
                License.License myLicense = new License.License();
                myLicense.Show();
            }
            else
            {
                _validLicense = true;
            }
            initiateAgent();
        }
        private bool checkLicense()
        {
            //We need to load the token.
            string license = string.Empty;

            string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), FingerPrint.Value().Replace("-", "") + ".lic");
            if (!System.IO.File.Exists(filename))
                return false;

            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            license = file.ReadLine();
            file.Close();

            return validateLicense(license);
        }

        private bool validateLicense(string license)
        {
            string machineId = FingerPrint.Value();
            string t_expiry = string.Empty;
            string t_machineId= string.Empty;
            string t_verified = string.Empty;
            try
            {
                //Assume the input is in a control called txtJwtIn,
                //and the output will be placed in a control called txtJwtOut
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtInput = license;

                //Check if readable token (string is in a JWT format)
                var readableToken = jwtHandler.CanReadToken(jwtInput);

                if (readableToken != true)
                {
                    MessageBox.Show("Invalid License");
                }
                if (readableToken == true)
                {
                    var token = jwtHandler.ReadJwtToken(jwtInput);

                    //Extract the payload of the JWT
                    var claims = token.Claims;
                    var jwtPayload = "{";
                    foreach (Claim c in claims)
                    {
                        if (c.Type == "expiry")
                            t_expiry = c.Value.Replace("\"","");
                        if (c.Type == "machineId")
                            t_machineId = c.Value.Replace("\"", "");
                        if (c.Type == "verified")
                            t_verified = c.Value.Replace("\"", "");

                        jwtPayload += '"' + c.Type + "\":\"" + c.Value + "\",";
                    }
                    jwtPayload += "}";
                    //string result = "\r\nPayload:\r\n" + JToken.Parse(jwtPayload).ToString(Formatting.Indented);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Console.Write(ex);
            }
            if(string.IsNullOrEmpty(t_expiry) || string.IsNullOrEmpty(t_machineId) || !t_machineId.Equals(machineId))
            {
                MessageBox.Show("Invalid license");
                return false;
            }
            DateTime expiryDate = DateTime.Parse(t_expiry.Replace("T", " "));
            if (expiryDate < DateTime.Now && !bool.Parse(t_verified))
            {
                MessageBox.Show("License expired. Please go ahead and order or contact maref@firemisc.com");
                return false;
            }
            return true;
        }
        #endregion
        private void initiateAgent()
        {
            FireLogMessage("License verified", 3);
            FireLogMessage("Agent Information object will be created", 3);
            AgentInformation = new AgentInformation();
            FireLogMessage("FinAgent Object Created Successfully", 3);
        }
        #region Methods
        private bool ValidateAgentLoginInformation() 
        {
            bool _valid = false;
            string msg = string.Empty;
            if (AgentInformation.AgentID == "" || AgentInformation.AgentID == null)
                msg = "Missing Agent ID Please enter Agent ID Example (1072)";
            else if (AgentInformation.Password == "" || AgentInformation.Password == null)
                msg = "Missing Password Please enter your password";
            else if (AgentInformation.Extension == "" || AgentInformation.Extension == null)
                msg = "Missing Extension Please enter your extension";
            else if (AgentInformation.DomainA == null && AgentInformation.DomainB == null)
                msg = "Finesse Server Domain is not defined";
            else
                _valid = true;

            if (AgentInformation.XMPPURL == null)
            {
                FireLogMessage("Finesse XMPP URL Suffix is not defined. Default will be used /http-bind/",1);
                AgentInformation.XMPPURL = "/http-bind/";
            }
            if (AgentInformation.HTTPURL == null)
            {
                FireLogMessage("Finesse HTTP URL Suffix is not defined. Default is /finesse",1);
                AgentInformation.HTTPURL = "/finesse";
            }

            if (AgentInformation.SSL)
            {
                if (AgentInformation.XMPPConnectionType == null)
                {
                    FireLogMessage("Finesse XMPP Connection type is not defined. Default is Tls",1);
                    AgentInformation.XMPPConnectionType = "Tls12";
                }
                if (AgentInformation.HTTPConnectionType == null)
                {
                    FireLogMessage("Finesse Http Connection type is not defined. Default is Tls",1);
                    AgentInformation.HTTPConnectionType = "Tls12";
                }
                if (AgentInformation.XMPPPort == null)
                {
                    FireLogMessage("Finesse XMPP Port is not defined. Default Port is 7443 will be used",1);
                    AgentInformation.XMPPPort = "7443";
                }
                if (AgentInformation.HTTPPort == null)
                {
                    FireLogMessage("Finesse HTTP Port is not defined. SSL default is 8445 will be used",1);
                    AgentInformation.HTTPPort = "8445";
                }
            }
            else
            {
                if (AgentInformation.XMPPPort == null)
                {
                    FireLogMessage("Finesse XMPP Port is not defined. Default Port 7071 will be used",1);
                    AgentInformation.XMPPPort = "7071";
                    AgentInformation.SSL = false;
                }
                if (AgentInformation.HTTPPort == null)
                {
                    FireLogMessage("Finesse HTTP Port is not defined. None SSL default is 80 will be used",1);
                    AgentInformation.HTTPPort = "80";
                    AgentInformation.SSL = false;
                }
            }

            if (_valid)
                FireLogMessage("Agent information is valid,"+msg,1);
            else
            {
                //FireLoadLoginScreen();
                FireLogMessage("Agent information verification failed ,check :"+msg,1);
                FireErrorMessage(msg);
            }

            FireLogMessage("Current Configuration is :\nSSL:"+AgentInformation.SSL+"\nXMPPPort:"+AgentInformation.XMPPPort+"\nHTTPPort:"+AgentInformation.HTTPPort+"\nXMPPConnectionType:"+AgentInformation.XMPPConnectionType+"\nHTTPConnectionType:"+AgentInformation.HTTPConnectionType,2);
            return _valid;
        }
        public bool SignIn()
        {
            if (!_validLicense)
                return false;
            FireLogMessage("Agent login requested",1);
            if (!ValidateAgentLoginInformation())
                return false;
            //if (MessageQueue == null)
            //    MessageQueue = new System.Collections.Queue();
            //if (MessageQueue.Count > 0)
            //    MessageQueue.Clear();
            try
            {
                FireLogMessage("Clearing the dialogs list at login time", 4);
                if (AgentInformation.Dialogs != null)
                    AgentInformation.Dialogs.Clear();
                FireLogMessage("Dialog List Cleared at login time", 4);

                if (StateTimer != null)
                    StateTimer.StopTimer();

                //Mamdouh StateTimer = null; 
                StateTimer = new StateTimer(new DateTime(), ""); //Mamdouh Initiate state timer to ensure the same thread
                StateTimer.StopTimer();
                StateTimer.TimerLabel = "--:--";

                FireLogMessage("Creating FinRestClient at login time", 4);
                finRestClient = new FinRestClient(this);
                FireLogMessage("FinRestClient created at login time", 4);


                if (finMatrixClient != null)
                {
                    FireLogMessage("Reinitiate FinMatrixClient at login time", 4);
                    finMatrixClient.Reinitiate();
                    FireLogMessage("FinMatrixClient reinitiated at login time", 4);
                }
                else
                {
                    FireLogMessage("Creating FinMatrixClient at login time", 4);
                    finMatrixClient = new FinMatrixClient(this);
                    FireLogMessage("FinMatrixClient created at login time", 4);
                }
                //TODO We should load agent ID here in case the user used the Login Name
                //LoadAgentID();
                FireLogMessage("Establishing XMPP Connection from FinMatrixClient at login time", 4);
                finMatrixClient.Connect(null);
                FireLogMessage("XMPP Connection requested at login time", 4);
                return true;
            }
            catch (Exception ex)
            {
                FireLogMessage("Error in Sign In , ex:"+ex.Message+"\n"+ex.StackTrace,1);
                return false;
            }
        }
        public bool SignOut(string status, string reasonCodeLabel)
        {
            FireLogMessage("Agent logout requested",1);
            if (finRestClient == null)
                return false;

            FireLogMessage("Logout request status:"+status+" and reason code label:"+reasonCodeLabel, 1);
            //finMatrixClient.Predisconnect(3);
            string result = finRestClient.ChangeAgentState(status, FindReasonCode(reasonCodeLabel, true), AgentInformation.AgentID, AgentInformation.UserName, AgentInformation.Password);
            if (result == null || result == "")
            {
                FireLogMessage("Logout request sent successfully to server and empty message response returned which is okay", 3);
                return true;
            }
            else
            {
                FireLogMessage("Logout request failed and server response is:"+result, 1);
                return false;
            }
        }
        public bool AnswerCall(string dialogID)
        {
            FireLogMessage("Answer requested for dialog ID:"+dialogID,1);
            string result = finRestClient.AnswerCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID);
            if (result == null || result == "")
            {
                FireLogMessage("Answer request sent successfully to server and empty message response returned which is okay,dialog ID:"+dialogID, 3);
                return true;
            }
            FireLogMessage("Answer request failed, dialog ID:"+dialogID+" and server response is:" + result, 1);
            return false;
        }
        public bool ReleaseCall(string dialogID)
        {
            FireLogMessage("Release requested for dialog ID:" + dialogID,1);
            string result = finRestClient.ReleaseCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID);
            if (result == null || result == "")
            {
                FireLogMessage("Release request sent successfully to server and empty message response returned which is okay,dialog ID:" + dialogID, 2);
                return true;
            }
            FireLogMessage("Release request failed, dialog ID:" + dialogID + " and server response is:" + result, 1);
            return false;
        }
        public bool HoldCall(string dialogID)
        {
            FireLogMessage("Hold requested for dialog ID:" + dialogID,1);
            string result = finRestClient.HoldCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID);
            if (result == null || result == "")
            {
                FireLogMessage("Hold request sent successfully to server and empty message response returned which is okay,dialog ID:" + dialogID, 3);
                return true;
            }
            FireLogMessage("Hold request failed, dialog ID:" + dialogID + " and server response is:" + result, 1);
            return false;
        }
        public bool TransferCall(string dialogID)
        {
            FireLogMessage("Transfer call requested for dialog ID:" + dialogID,1);
            string result = finRestClient.TransferCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID);
            if (result == null || result == "")
            {
                FireLogMessage("Transfer Call request sent successfully to server and empty message response returned which is okay,dialog ID:" + dialogID, 3);
                return true;
            }
            FireLogMessage("Transfer Call request failed, dialog ID:" + dialogID + " and server response is:" + result, 1);
            return false;
        }
        public bool ResumeCall(string dialogID)
        {
            FireLogMessage("Resume call requested for dialog ID:" + dialogID,1);
            string result = finRestClient.RetriveCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID);
            if (result == null || result == "")
            {
                FireLogMessage("Retreive Call request sent successfully to server and empty message response returned which is okay,dialog ID:" + dialogID, 3);
                return true;
            }
            FireLogMessage("Retreive Call request failed, dialog ID:" + dialogID + " and server response is:" + result, 1);
            return false;
        }
        public bool SSTransferCall(string dialogID, string dialedNumber)
        {
            FireLogMessage("SSTransfer requested for dialog ID:" + dialogID,1);
            string result = finRestClient.SSTransferCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID, dialedNumber);
            if (result == null || result == "")
            {
                FireLogMessage("SSTransfer Call request sent successfully to server and empty message response returned which is okay,dialog ID:" + dialogID, 3);
                return true;
            }
            FireLogMessage("SSTransfer Call request failed, dialog ID:" + dialogID + " and server response is:" + result, 1);
            return false;
        }
        public bool ConsultCall(string dialogID, string dialedNumber)
        {
            FireLogMessage("Consult requested for dialog ID:" + dialogID + "and dial number is :"+ dialedNumber,1);
            string result = finRestClient.ConsultCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID, dialedNumber);
            if (result == null || result == "")
            {
                FireLogMessage("Consult Call request sent successfully to server and empty message response returned which is okay,dialog ID:" + dialogID, 3);
                return true;
            }
            FireLogMessage("Consult Call request failed, dialog ID:" + dialogID + " and server response is:" + result, 1);
            return false;
        }
        public bool ConferenceCall(string dialogID, string dialedNumber)
        {
            FireLogMessage("Conference requested for dialog ID:" + dialogID + "and dial number is :" + dialedNumber);
            string result = finRestClient.ConferenceCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID, dialedNumber);
            if (result == null || result == "")
            {
                FireLogMessage("Conference Call request sent successfully to server and empty message response returned which is okay,dialog ID:" + dialogID, 3);
                return true;
            }
            FireLogMessage("Conference Call request failed, dialog ID:" + dialogID + " and server response is:" + result, 1);
            return false;
        }
        public bool KeypadSendDTMF(string dialogID, string dtmfString)
        {
            FireLogMessage("Send DTMF requested for dialog ID:" + dialogID + "and DTMF is :" + dtmfString,1);
            string result = finRestClient.KeypadSendDTMF(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID, dtmfString);
            if (result == null || result == "")
            {
                FireLogMessage("Send DTMF request sent successfully to server and empty message response returned which is okay,dialog ID:" + dialogID, 3);
                return true;
            }
            FireLogMessage("Send DTMF request failed, dialog ID:" + dialogID + " and server response is:" + result, 1);
            return false;
        }
        public bool MakeCall(string dialedNumber)
        {
            FireLogMessage("Make Call requested for dial number:" + dialedNumber,1);
            string result = finRestClient.MakeCall(AgentInformation.UserName, AgentInformation.AgentID, AgentInformation.Extension, dialedNumber, AgentInformation.Password);
            if (result == null || result == "")
            {
                FireLogMessage("Make Call request sent successfully to server and empty message response returned which is okay, dial number:" + dialedNumber, 3);
                return true;
            }
            FireLogMessage("Make call request failed, dial number:" + dialedNumber + " and server response is:" + result, 1);
            return false;
        }
        public bool UpdateCallData(string dialogID, Dictionary<string, string> callVariables, string wrapupReason)
        {
            FireLogMessage("Update Call Data requested for dialog ID:" + dialogID,1);
            string result = finRestClient.UpdateCallData(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID, callVariables, wrapupReason);
            if (result == null || result == "")
            {
                FireLogMessage("Update Call Data request sent successfully to server and empty message response returned which is okay,dialog ID:" + dialogID, 3);
                return true;
            }
            FireLogMessage("Update Call Data request failed, dialog ID:" + dialogID + " and server response is:" + result, 1);
            return false;
        }
        public bool ChangeStatus(string status, string reasonCodeLabel)
        {
            FireLogMessage("Agent change status to: " + status + ", and reason code: "+reasonCodeLabel,1);
            if (finRestClient == null)
                return false;

            string result = finRestClient.ChangeAgentState(status, FindReasonCode(reasonCodeLabel, false), AgentInformation.AgentID, AgentInformation.UserName, AgentInformation.Password);
            if (result == null || result == "")
            {
                FireLogMessage("Change Status request sent successfully to server and empty message response returned which is okay,status:" + status, 3);
                return true;
            }

            FireLogMessage("Change Status request failed, requested status:" + status + " and reason code label is:"+reasonCodeLabel+" and server response is:" + result, 1);
            return false;
        }
        public void LoadNotReadyReasonCodeList()
        {
            try
            {
                FireLogMessage("Loading not ready reason code",1);
                string _response = finRestClient.GetResonCodeList(AgentInformation.UserName, AgentInformation.AgentID, AgentInformation.Password, "NOT_READY");

                FireLogMessage("Finesse Response of Loading Not Ready Reason Code List:" + _response, 3);
                if (_response.Contains("(401)") || _response.Contains("(404)"))
                {
                    FireLogMessage("Unable to load Not Ready Reason Code List and response is:"+_response,1);
                    return;
                }

                XElement xml = XElement.Parse(_response);
                if (xml == null || xml.Element("ErrorMessage") != null)
                    return;
                if (xml.Elements("ReasonCode") != null)
                    AgentInformation.NotReadyReasonCodeList = new MTObservableCollection<ReasonCodeClass>();
                else
                    return;
                foreach (XElement reasonCode in xml.Elements("ReasonCode"))
                {
                    AgentInformation.ReasonCodeClass _ReasonCode = new AgentInformation.ReasonCodeClass();
                    _ReasonCode.URI = reasonCode.Element("uri").Value;
                    _ReasonCode.Category = reasonCode.Element("category").Value;
                    _ReasonCode.Code = reasonCode.Element("code").Value;
                    _ReasonCode.Label = reasonCode.Element("label").Value;
                    _ReasonCode.ForAll = reasonCode.Element("forAll").Value;
                    AgentInformation.NotReadyReasonCodeList.Add(_ReasonCode);
                }
            }catch(Exception ex)
            {
                FireLogMessage("Error is loading not ready reason code list , ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void LoadLogoutReasonCodeList()
        {
            try
            {
                FireLogMessage("Loading logout reason code",1);
                string _response = finRestClient.GetResonCodeList(AgentInformation.UserName, AgentInformation.AgentID, AgentInformation.Password, "LOGOUT");

                FireLogMessage("Finesse Response of Loading Logout Reason Code List:" + _response, 3);
                if (_response.Contains("(401)") || _response.Contains("(404)"))
                {
                    FireLogMessage("Unable to load Logout Reason Code List and response is:" + _response, 1);
                    return;
                }

                XElement xml = XElement.Parse(_response);
                if (xml == null || xml.Element("ErrorMessage") != null)
                    return;
                if (xml.Elements("ReasonCode") != null)
                    AgentInformation.LogoutReasonCodeList = new MTObservableCollection<ReasonCodeClass>();
                else
                    return;
                foreach (XElement reasonCode in xml.Elements("ReasonCode"))
                {
                    AgentInformation.ReasonCodeClass _ReasonCode = new AgentInformation.ReasonCodeClass();
                    _ReasonCode.URI = reasonCode.Element("uri").Value;
                    _ReasonCode.Category = reasonCode.Element("category").Value;
                    _ReasonCode.Code = reasonCode.Element("code").Value;
                    _ReasonCode.Label = reasonCode.Element("label").Value;
                    _ReasonCode.ForAll = reasonCode.Element("forAll").Value;
                    AgentInformation.LogoutReasonCodeList.Add(_ReasonCode);
                }
            }catch(Exception ex)
            {
                FireLogMessage("Error is loading logout reason code list , ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void ChangeAgentVoiceStatus(VoiceStatus voiceStatus)
        {
            try
            {
                FireLogMessage("Change Agent Voice Status to:"+voiceStatus.Status,1);
                string newStatus;
                string reasonCodeLabel = null;

                string status = voiceStatus.StatusLabel;

                if (status.Contains("->"))
                {
                    FireLogMessage("Can not change status due to pending request. Requested Status:" + voiceStatus.Status, 3);
                    return;
                }

                newStatus = voiceStatus.Status;
                reasonCodeLabel = voiceStatus.ReasonCode;

                if (newStatus.Equals(AgentInformation.Status))
                {
                    if (reasonCodeLabel == null && AgentInformation.ReasonCode == null)
                        return;
                    else if (reasonCodeLabel != null && AgentInformation.ReasonCode != null && AgentInformation.ReasonCode.Label != null)
                    {
                        if (reasonCodeLabel.Equals(AgentInformation.ReasonCode.Label))
                            return;
                    }

                }

                if (newStatus.Equals("LOGOUT"))
                {
                    if (SignOut(newStatus, reasonCodeLabel))
                    {
                        //FireLoadLoginScreen();
                        return;
                    }
                }

                if (ChangeStatus(newStatus, reasonCodeLabel))
                {
                    if (AgentInformation.Status.Equals("TALKING") || AgentInformation.Status.Equals("RESERVED") || AgentInformation.Status.Equals("HOLD"))
                    {
                        AgentInformation.PendingStatus = status;
                        UpdateVoiceStatusList();
                    }
                }
            }catch(Exception ex)
            {
                FireLogMessage("Error is changing voice status , ex:" + ex.Message + "\n" + ex.StackTrace);
            }

        }
        public void UpdateVoiceStatusList()
        {
            try
            {
                FireLogMessage("Update Voice Channel Status List",1);

                bool statusSelected = false;
                if (AgentInformation.VoiceStatusList == null)
                    AgentInformation.VoiceStatusList = new MTObservableCollection<VoiceStatus>();

                FireLogMessage("Update Voice Channel Status List, Clear Status List before updating", 4);
                AgentInformation.VoiceStatusList.Clear();

                //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action) (() => AgentInformation.VoiceStatusList.Clear()));
                //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => AgentInformation.VoiceStatusList.Clear()));

                //Add Ready Option
                VoiceStatus voiceStatus = new VoiceStatus()
                {
                    Status = "READY",
                    StatusLabel = "Ready",
                    IconName = "READY"
                };


                if ("READY".Equals(AgentInformation.Status))
                {
                    voiceStatus.Selected = true;
                    statusSelected = true;
                    AgentInformation.MakeCallVisible = false;
                    FireLogMessage("Update Voice Channel Status List, Agent in ready status", 4);
                }

                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
                //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));

                //Add Wrap Up Option
                //comboBoxItem = BuildComboBoxItem("WORK_READY", "","NotReady");
                //if ("WRAPUP".Equals(finAgent._agentInformation.Status))
                //    comboBoxItem.IsSelected = true;
                //StatusDropDown.Items.Add(comboBoxItem);

                //In case no reason code , will add not ready button
                //if (finAgent._agentInformation.NotReadyReasonCodeList == null)
                //{
                //    comboBoxItem = BuildComboBoxItem("NOT_READY", null, "NotReady");
                //    if ("NOT_READY".Equals(finAgent._agentInformation.Status))
                //        comboBoxItem.IsSelected = true;
                //    StatusDropDown.Items.Add(comboBoxItem);
                //    PopulateLogoutDropDown();
                //    return;
                //}

                if (AgentInformation.NotReadyReasonCodeList != null && AgentInformation.NotReadyReasonCodeList.Count > 0)
                {
                    foreach (AgentInformation.ReasonCodeClass reasonCode in AgentInformation.NotReadyReasonCodeList)
                    {
                        voiceStatus = new VoiceStatus()
                        {
                            Status = reasonCode.Category,
                            StatusLabel = "Not Ready - " + reasonCode.Label,
                            ReasonCode = reasonCode.Label,
                            IconName = "NotReady"
                        };

                        if (reasonCode.Category.Equals(AgentInformation.Status))
                        {
                            //Check Reason Code
                            if (reasonCode.Label == null && AgentInformation.ReasonCode == null)
                            {
                                voiceStatus.Selected = true;
                                statusSelected = true;
                                AgentInformation.MakeCallVisible = true;
                                FireLogMessage("Update Voice Channel Status List, Agent in not ready status", 4);
                            }
                            else if (reasonCode.Label != null && AgentInformation.ReasonCode != null && AgentInformation.ReasonCode.Label != null)
                            {
                                if (reasonCode.Label.Equals(AgentInformation.ReasonCode.Label))
                                {
                                    voiceStatus.Selected = true;
                                    statusSelected = true;
                                    AgentInformation.MakeCallVisible = true;
                                    FireLogMessage("Update Voice Channel Status List, Agent in not ready status and reason code is:"+reasonCode.Label, 4);
                                }
                            }

                        }
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
                        //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
                    }
                }
                else
                {
                    voiceStatus = new VoiceStatus()
                    {
                        Status = "NOT_READY",
                        StatusLabel = "Not Ready",
                        IconName = "NotReady"
                    };


                    if ("NOT_READY".Equals(AgentInformation.Status))
                    {
                        voiceStatus.Selected = true;
                        statusSelected = true;
                        AgentInformation.MakeCallVisible = true;
                        FireLogMessage("Update Voice Channel Status List, Agent in not ready status", 4);
                    }
                    AgentInformation.VoiceStatusList.Add(voiceStatus);//Mamdouh remove multi threading
                                                                      //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
                                                                      //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
                }
                //Agent Status Neither Ready Nor Not ready Nor Wrap up
                if (!statusSelected)
                {
                    string currentStatus = AgentInformation.Status;
                    string currentLabel = null;
                    string pendingStatus = AgentInformation.PendingStatus;
                    if (currentStatus != null)
                    {
                        if (currentStatus.Equals("NOT_READY"))
                        {
                            currentStatus = "Not Ready";
                            AgentInformation.MakeCallVisible = true;
                        }
                        else if (currentStatus.Equals("TALKING"))
                        {
                            currentStatus = "Talking";
                            AgentInformation.MakeCallVisible = true;
                        }
                        else if (currentStatus.Equals("HOLD"))
                            currentStatus = "Hold";
                        else if (currentStatus.Equals("RESERVED"))
                            currentStatus = "Reserved";
                        else if (currentStatus.Equals("WORK_READY"))
                            currentStatus = "Work Ready";
                        else if (currentStatus.Equals("WORK_NOT_READY"))
                            currentStatus = "Work Not Ready";
                        else if (currentStatus.Equals("LOGOUT"))
                        {
                            //if (pendingStatus == null || pendingStatus == "") // Agent Requested Logout 
                            //{
                            //UnSubscribeFromQueuesEvents();
                            //UnSubscribeFromUserEvent();
                            //UnSubscribeFromDialogEvent();
                            //if (finMatrixClient != null && finMatrixClient.IsConnected)
                            //    finMatrixClient.Disconnect();
                            currentStatus = "Logout";
                            //}
                            //else if (pendingStatus.Equals("LOGIN"))
                            //{
                            //    AgentInformation.Status = "NOT_READY";
                            //    currentStatus = "Not Ready";
                            //    pendingStatus = null;
                            //    AgentInformation.MakeCallVisible = true;

                            //}

                        }
                        else if (currentStatus.Equals("LOGIN"))
                        {
                            currentStatus = "Login";
                            pendingStatus = null;

                            AgentInformation.MakeCallVisible = true;
                        }
                    }

                    if (pendingStatus != null)
                    {
                        if (pendingStatus.Equals("READY"))
                            pendingStatus = "Ready";
                        else if (pendingStatus.Equals("NOT_READY"))
                            pendingStatus = "Not Ready";
                        else if (pendingStatus.Equals("TALKING"))
                            pendingStatus = "Talking";
                        else if (pendingStatus.Equals("HOLD"))
                            pendingStatus = "Hold";
                        else if (pendingStatus.Equals("RESERVED"))
                            pendingStatus = "Reserved";
                        else if (pendingStatus.Equals("LOGOUT"))
                            pendingStatus = "Logout";
                        else if (pendingStatus.Equals("WORK_READY"))
                            pendingStatus = "Work Ready";
                        else if (pendingStatus.Equals("WORK_NOT_READY"))
                            pendingStatus = "Work Not Ready";
                        else if (pendingStatus.Equals("LOGIN"))
                        {
                            pendingStatus = "Login";
                            AgentInformation.MakeCallVisible = true;
                        }
                    }

                    if (AgentInformation.ReasonCode != null)
                    {
                        if (AgentInformation.ReasonCode.Label != null)
                            currentLabel = AgentInformation.ReasonCode.Label;
                    }
                    string iconName = "NotReady";
                    if (pendingStatus != null && pendingStatus != "")
                        currentStatus += "->" + pendingStatus;

                    if (AgentInformation.Status.Equals("TALKING") || AgentInformation.Status.Equals("HOLD") || AgentInformation.Status.Equals("RESERVED"))
                        iconName = "Reserved";
                    if (AgentInformation.Status.Equals("LOGOUT"))
                        iconName = "Other";
                    if (AgentInformation.Status.Equals("WORK_READY"))
                        iconName = "READY";
                    if (AgentInformation.Status.Equals("WORK_NOT_READY"))
                        iconName = "NotReady";

                    voiceStatus = new VoiceStatus()
                    {
                        Status = AgentInformation.Status,
                        StatusLabel = currentStatus,
                        ReasonCode = currentLabel,
                        IconName = iconName
                    };
                    voiceStatus.Selected = true;
                    AgentInformation.VoiceStatusList.Add(voiceStatus);//Mamdouh remove multiThreading
                                                                      //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
                                                                      //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
                }
                if (AgentInformation.StateChangeTime != null)
                {
                    DateTime stateChangeTime = DateTime.Parse(AgentInformation.StateChangeTime);
                    //Convert.ToDateTime(AgentInformation.StateChangeTime);
                    FireLogMessage("Setting State Time from time :" + stateChangeTime.ToLongDateString(),1);
                    if (StateTimer == null)
                        StateTimer = new StateTimer(stateChangeTime, "");
                    else
                        StateTimer.ResetTimer(stateChangeTime);
                }

                if (!AgentInformation.Status.Equals("LOGOUT"))
                {
                    if (!AgentInformation.Status.Equals("NOT_READY"))
                    {
                        SetSelectedVoiceStatus();//Mamdouh removing of multi threading as it is moved to observaleCollection Layer
                                                 //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => SetSelectedVoiceStatus()));
                                                 //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => SetSelectedVoiceStatus()));
                        return;
                    }
                    if (AgentInformation.LogoutReasonCodeList != null && AgentInformation.LogoutReasonCodeList.Count > 0)
                    {
                        foreach (AgentInformation.ReasonCodeClass reasonCode in AgentInformation.LogoutReasonCodeList)
                        {
                            voiceStatus = new VoiceStatus()
                            {
                                Status = reasonCode.Category,
                                StatusLabel = "Logout - " + reasonCode.Label,
                                ReasonCode = reasonCode.Label,
                                IconName = "Other"
                            };

                            AgentInformation.VoiceStatusList.Add(voiceStatus); //Mamdouh remove thread management as it is moved to observablecollection layer
                                                                               //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
                                                                               //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
                        }

                    }
                    else
                    {
                        voiceStatus = new VoiceStatus()
                        {
                            Status = "LOGOUT",
                            StatusLabel = "Logout",
                            IconName = "Other"
                        };
                        AgentInformation.VoiceStatusList.Add(voiceStatus); //Mamdouh remove thread management as it is moved to observablecollection layer
                                                                           //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
                                                                           //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));

                        SetSelectedVoiceStatus();//Mamdouh remove thread management as it is moved to observablecollection layer
                                                 //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => SetSelectedVoiceStatus()));
                                                 //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => SetSelectedVoiceStatus()));
                        return;
                    }
                }
                SetSelectedVoiceStatus(); //Mamdouh remove thread management as it is moved to observablecollection layer
                                          //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => SetSelectedVoiceStatus()));
                                          //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => SetSelectedVoiceStatus()));
            }catch(Exception ex)
            {
                FireLogMessage("Error is updating voice status List , ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void SetSelectedVoiceStatus() 
        {
            try
            {
                if (AgentInformation.VoiceStatusList != null)
                {
                    foreach (VoiceStatus voiceStatus in AgentInformation.VoiceStatusList)
                    {
                        if (voiceStatus.Selected)
                            AgentInformation.SelectedVoiceStatus = voiceStatus;
                    }
                }
            }catch(Exception ex)
            {
                FireLogMessage("Error is setting selected voice status , ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void LoadSystemInformation()
        {
            try
            {
                FireLogMessage("Loading System Information from rest API");
                FireLoadingMessage("Loading System Information");

                string _response = finRestClient.GetSystemInfo(AgentInformation.UserName, AgentInformation.AgentID, AgentInformation.Password);
                FireLogMessage("Load System Information , Finesse Response:"+_response,4);
                if (_response == null || _response == "")
                    return;
                if (_response.IndexOf("<SystemInfo>") < 0 || _response.IndexOf("</SystemInfo>") < 0)
                    return;

                string responseTruncated = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + _response;

                XElement xml = XElement.Parse(responseTruncated);
                XNamespace nameSpace = xml.Name.Namespace;
                SystemStatus = xml.Element(nameSpace + "status").Value;
                SystemXmppDomain = xml.Element(nameSpace + "xmppDomain").Value;
                SystemXmppPubSubDomain = xml.Element(nameSpace + "xmppPubSubDomain").Value;
            }
            catch (Exception ex)
            {
                FireLogMessage("Error is Loading system information , ex:" + ex.Message + "\n" + ex.StackTrace);
            }

        }

        private void LoadQueuesInformation()
        {
            try
            {
                AgentInformation.Queues = new MTObservableCollection<Model.Queue>(); // Reinitiate Queues
                FireLogMessage("Loading Queue Information from rest API",1);
                FireLoadingMessage("Loading Queues Information");
                string _response = finRestClient.GetUserQueues(AgentInformation.UserName, AgentInformation.AgentID, AgentInformation.Password);
                FireLogMessage("Load Queue Information , Finesse Response:" + _response, 4);
                if (_response == null || _response == "")
                    return;
                if (_response.IndexOf("<Queues>") < 0 || _response.IndexOf("</Queues>") < 0)
                    return;

                string responseTruncated = _response.Substring(_response.IndexOf("<Queues>"), _response.IndexOf("</Queues>") - _response.IndexOf("<Queues>") + "</Queues>".Length);
                responseTruncated = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + responseTruncated;

                XElement xml = XElement.Parse(responseTruncated);
                foreach (XElement queue in xml.Elements("Queue"))
                {
                    Model.Queue queueMessage = ResolveQueue(queue);
                    FireQueueEvent(queueMessage);
                    LoadQueueInformation(queueMessage);

                    //In case of fetch call at system login , fire USD event
                }
            }
            catch (Exception ex)
            {
                FireLogMessage("Error is Loading Queue information , ex:" + ex.Message + "\n" + ex.StackTrace);
            }

        }
        public void LoadQueueInformation(Model.Queue queue)
        {
            try
            {
                FireLogMessage("Load Queue Data from Finesse Event or the first time of loading",1);
                if (AgentInformation == null) // In case of no Agent Information
                    return;
                if (queue == null) //Null queue is sent
                    return;

                Model.Queue existingQueue = FindQueue(queue.Uri);
                if (existingQueue != null)
                {
                    existingQueue.Name = queue.Name;
                    existingQueue.StartTimeOfLongestCallInQueue = queue.StartTimeOfLongestCallInQueue;
                    existingQueue.CallsInQueue = queue.CallsInQueue;
                    existingQueue.AgentsReady = queue.AgentsReady;
                    existingQueue.AgentsNotReady = queue.AgentsNotReady;
                    existingQueue.AgentsTalkingInbound = queue.AgentsTalkingInbound;
                    existingQueue.AgentsTalkingInternal = queue.AgentsTalkingInternal;
                    existingQueue.AgentsTalkingOutbound = queue.AgentsTalkingOutbound;
                    existingQueue.AgentsWrapUpReady = queue.AgentsWrapUpReady;
                    existingQueue.AgentsWrapUpNotReady = queue.AgentsWrapUpNotReady;
                }
                else
                {
                    if (AgentInformation.Queues == null)
                        AgentInformation.Queues = new MTObservableCollection<Model.Queue>();
                    AgentInformation.Queues.Add(queue);

                }
            }catch(Exception ex)
            {
                FireLogMessage("Error loading queue information ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        public Model.Queue FindQueue(string uri)
        {
            Model.Queue _Queue = null;
            if (uri == null)
                return _Queue;
            try
            {
                if (AgentInformation.Queues != null && AgentInformation.Queues.Count >= 1)
                {
                    for (int counter = 0; counter < AgentInformation.Queues.Count; counter++)
                    {
                        if (((Model.Queue)AgentInformation.Queues[counter]).Uri.Equals(uri))
                        {
                            _Queue = (Model.Queue)AgentInformation.Queues[counter];
                        }

                    }
                }
            }catch(Exception ex)
            {
                FireLogMessage("Error finding the queue ex:"+ex.Message+"\n"+ex.StackTrace);
            }
            return _Queue;
        }
        private void SubscribeToQueuesEvents()
        {
            try
            {
                if (AgentInformation.Queues == null)
                    return;
                if (finMatrixClient == null)
                    return;
                foreach (Model.Queue queue in AgentInformation.Queues)
                {
                    finMatrixClient.SubscribeToUri(queue.Uri);
                }
            }
            catch(Exception ex)
            {
                FireLogMessage("Error during Subscribe to Queues ex:"+ex.Message+"\n"+ex.StackTrace);
            }
        }
        public void SubscribeToUserEvent()
        {
            if (AgentInformation == null)
                return;
            if (finMatrixClient == null)
                return;
            finMatrixClient.SubscribeToUri("/finesse/api/User/" + AgentInformation.AgentID);
        }
        public void SubscribeToDialogEvent()
        {
            if (AgentInformation.Queues == null)
                return;
            if (finMatrixClient == null)
                return;
            finMatrixClient.SubscribeToUri("/finesse/api/User/" + AgentInformation.AgentID + "/Dialogs");
        }
        private void UnSubscribeFromQueuesEvents()
        {
            try
            {
                if (AgentInformation.Queues == null)
                    return;
                if (finMatrixClient == null)
                    return;
                foreach (Model.Queue queue in AgentInformation.Queues)
                {
                    finMatrixClient.UnsubscibeFromUri(queue.Uri);
                }
            }catch(Exception ex)
            {
                FireLogMessage("Error during queue unsubscribe, ex:"+ex.Message+"\n"+ex.StackTrace);
            }
        }
        public void UnSubscribeFromUserEvent()
        {
            if (AgentInformation == null)
                return;
            if (finMatrixClient == null)
                return;
            finMatrixClient.UnsubscibeFromUri("/finesse/api/User/"+AgentInformation.AgentID);
        }
        public void UnSubscribeFromDialogEvent()
        {
            if (AgentInformation.Queues == null)
                return;
            if (finMatrixClient == null)
                return;
            finMatrixClient.UnsubscibeFromUri("/finesse/api/User/" + AgentInformation.AgentID+ "/Dialogs");
        }

        private void LoadCallInformation()
        {
            try
            {
                AgentInformation.Dialogs = new MTObservableCollection<Dialog>(); // Reinitiate Dialogs
                FireLogMessage("Checking server if there is a call at the begaining of connection");
                string _response = finRestClient.GetUserDialogs(AgentInformation.UserName, AgentInformation.AgentID, AgentInformation.Password);
                FireLogMessage("Load Call Information , and Finesse Response is:"+_response,4);
                if (_response == null || _response == "")
                    return;
                if (_response.IndexOf("<Dialogs>") < 0 || _response.IndexOf("</Dialogs>") < 0)
                    return;


                string responseTruncated = _response.Substring(_response.IndexOf("<Dialogs>"), _response.IndexOf("</Dialogs>") - _response.IndexOf("<Dialogs>") + "</Dialogs>".Length);
                responseTruncated = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + responseTruncated;

                XElement xml = XElement.Parse(responseTruncated);
                foreach (XElement dialog in xml.Elements("Dialog"))
                {
                    Dialog messageDialog = ResolveDialog(dialog);
                    messageDialog.DialogEvent = "RunningCall";
                    FireCallEvent(messageDialog);
                    LoadCallInformation(dialog);
                    //In case of fetch call at system login , fire USD event
                }
            }
            catch (Exception ex)
            {
                FireLogMessage("Error in load call information," + ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void LoadCallInformation(XElement xmlDialog)
        {
            LoadCallInformation(ResolveDialog(xmlDialog));
        }
        public void LoadCallInformation(Dialog dialog)
        {
            try
            {
                FireLogMessage("Load Call Data from Finesse Event");
                if (AgentInformation == null) // In case of no Agent Information
                    return;
                if (dialog == null) //Null Dialog is sent
                    return;
                Dialog existingDialog = FindDialog(dialog.ID);

                if (dialog.DialogEvent == null)
                    if (!dialog.State.Equals("DROPPED") && !dialog.State.Equals("FAILED")) // A call is loaded at the begaining of login
                        dialog.DialogEvent = "RunningCall";

                if (dialog.DialogEvent != null)
                {
                    if (dialog.DialogEvent.Equals("DELETE")) // Call is terminated
                    {
                        if (existingDialog != null)
                        {
                            CheckDeleteDialog(dialog);
                            if (AgentInformation.Dialogs.Count == 0)
                                AgentInformation.SelectedDialog = null;
                            else
                                AgentInformation.SelectedDialog = AgentInformation.Dialogs[AgentInformation.Dialogs.Count - 1];
                            FireLogMessage("Dialog is removed .. ID" + dialog.ID,2);
                        }
                        else
                        {
                            FireLogMessage("We have a delete call for non existing call dialog with ID: " + dialog.ID,2);
                        }
                    }
                    else if (dialog.DialogEvent.Equals("PUT") || dialog.DialogEvent.Equals("POST") || dialog.DialogEvent.Equals("RunningCall")) // Call Update
                    {
                        if (existingDialog != null) // Update Call Data
                        {
                            existingDialog.State = dialog.State;
                            existingDialog.ToAddress = dialog.ToAddress;
                            existingDialog.MediaProperties = dialog.MediaProperties;
                            existingDialog.MediaType = dialog.MediaType;
                            existingDialog.Participants = dialog.Participants;
                            existingDialog.DialogEvent = dialog.DialogEvent;
                            existingDialog.FromAddress = dialog.FromAddress;
                            existingDialog.URI = dialog.URI;
                            existingDialog.Header = dialog.Header;
                            existingDialog.CallDirection = dialog.CallDirection;

                            AgentInformation.SelectedDialog = existingDialog;

                            FireLogMessage("Call Data Updates for ID: " + dialog.ID,2);
                        }
                        else // New Call Data
                        {

                            foreach (Dialog.Participant participant in dialog.Participants)
                            {
                                DateTime stateChangeTime = DateTime.Parse(participant.StartTime);
                                //Convert.ToDateTime(participant.StartTime);
                                if (dialog.DialogStateTimer == null)
                                {
                                    FireLogMessage("Update Call Timer for ID: " + dialog.ID,2);
                                    dialog.DialogStateTimer = new StateTimer(stateChangeTime, "");
                                }
                            }
                            if (AgentInformation.Dialogs == null)
                            {
                                AgentInformation.Dialogs = new MTObservableCollection<Dialog>();
                                FireLogMessage("Create Dialogs List for the first time");
                                //AgentInformation.Dialogs.Add(dialog); //Mamdouh Aref important change to verify that dupliate dialog is removed
                                // Running the app in background thread is making a delay which result in duplicate cards.

                                //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() =>
                                //{
                                AgentInformation.Dialogs.Add(dialog);
                                AgentInformation.SelectedDialog = dialog;
                                //}));
                            }
                            else
                            {
                                //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() =>
                                //{
                                AgentInformation.Dialogs.Add(dialog);
                                AgentInformation.SelectedDialog = dialog;
                                //}));
                            }
                            //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => AgentInformation.Dialogs.Add(dialog)));
                            //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => AgentInformation.Dialogs.Add(dialog)));

                            FireLogMessage("New Call Posted for ID: " + dialog.ID);
                        }
                    }
                }
            }catch(Exception ex)
            {
                FireLogMessage("Error loading call information ex:"+ex.Message+"\n"+ex.Message);
            }
        }
        public bool CheckDeleteDialog(Dialog dialog)
        {
            bool isFound = false;
            try
            {
                if (AgentInformation.Dialogs != null && AgentInformation.Dialogs.Count >= 1)
                {
                    for (int counter = AgentInformation.Dialogs.Count - 1; counter >= 0; counter--)
                    //foreach (Dialog _dialog in _agentInformation.Dialogs)
                    {
                        //if (_dialog != null && _dialog._ID.Equals(dialog._ID))
                        if (((Dialog)AgentInformation.Dialogs[counter]).ID.Equals(dialog.ID))
                        {
                            AgentInformation.Dialogs.RemoveAt(counter);//Mamdouh Remove multi Threading
                                                                       //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.ContextIdle, (Action)(() => AgentInformation.Dialogs.RemoveAt(counter)));
                                                                       //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, (Action)(() => AgentInformation.Dialogs.RemoveAt(counter)));
                            isFound = true;
                        }

                    }
                }
            }catch(Exception ex)
            {
                FireLogMessage("Error in check and delete dialog ex"+ex.Message+"\n"+ex.StackTrace);
            }
            return isFound;
        }
        public Dialog FindDialog(string dialogID)
        {
            Dialog _Dialog = null;
            if (dialogID == null)
                return _Dialog;
            try
            {
                if (AgentInformation.Dialogs != null && AgentInformation.Dialogs.Count >= 1)
                {
                    for (int counter = 0; counter < AgentInformation.Dialogs.Count; counter++)
                    {
                        if (((Dialog)AgentInformation.Dialogs[counter]).ID.Equals(dialogID))
                        {
                            _Dialog = (Dialog)AgentInformation.Dialogs[counter];
                        }

                    }
                }
            }catch(Exception ex)
            {
                FireLogMessage("Error in finding dialog , ex:" + ex.Message + "\n" + ex.StackTrace);
            }
            return _Dialog;
        }
        public bool CheckDialog(string dialogID)
        {
            bool isFound = false;
            try
            {
                if (AgentInformation.Dialogs != null && AgentInformation.Dialogs.Count >= 1)
                {
                    for (int counter = 0; counter < AgentInformation.Dialogs.Count; counter++)
                    //foreach (Dialog _dialog in _agentInformation.Dialogs)
                    {
                        //if (_dialog != null && _dialog._ID.Equals(dialog._ID))
                        if (((Dialog)AgentInformation.Dialogs[counter]).ID.Equals(dialogID))
                        {
                            isFound = true;
                        }

                    }
                }
            }catch(Exception ex)
            {
                FireLogMessage("Error in checking dialog , ex:" + ex.Message + "\n" + ex.StackTrace);
            }
            return isFound;
        }
        public bool LoadAgentInformation()
        {
            try
            {
                return LoadAgentInformation(null);
            }
            catch (Exception ex) {
                FireLogMessage("Error loading agent information ex:" + ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }
        private bool LoadAgentInformation(string userMessage)
        {
            try
            {
                FireLogMessage("Load Agent Information");
                string _response = userMessage;
                if (_response == null || _response == "")
                {
                    if (AgentInformation.UserName == null)
                        AgentInformation.UserName = AgentInformation.AgentID;
                    _response = finRestClient.GetUserInfo(AgentInformation.UserName, AgentInformation.AgentID, AgentInformation.Password);
                    FireLogMessage("We requested agent information from server and response is:"+_response,2);
                }
                if (_response.Contains("(401)") || _response.Contains("(404)"))
                    return false;

                XElement xml = XElement.Parse(_response);
                XNamespace updateNameSpace = xml.Name.Namespace;
                if (xml.Element(updateNameSpace + "ErrorMessage") != null)
                    return false;
                //Fill In Roles
                if (xml.Element(updateNameSpace + "roles") != null && xml.Element(updateNameSpace + "roles").Value != null)
                {
                    AgentInformation.Roles = new MTObservableCollection<string>();

                    foreach (XElement role in xml.Element(updateNameSpace + "roles").Elements(updateNameSpace + "role"))
                        AgentInformation.Roles.Add(role.Value);
                }
                else
                    AgentInformation.Roles = null;

                AgentInformation.UserName = xml.Element(updateNameSpace + "loginName").Value;
                AgentInformation.FirstName = xml.Element(updateNameSpace + "firstName").Value;
                if (xml.Element(updateNameSpace + "extension") != null && xml.Element(updateNameSpace + "extension").Value != null && !xml.Element(updateNameSpace + "extension").Value.Trim().Equals(""))
                    AgentInformation.Extension = xml.Element(updateNameSpace + "extension").Value;

                AgentInformation.LastName = xml.Element(updateNameSpace + "lastName").Value;
                AgentInformation.TeamID = xml.Element(updateNameSpace + "teamId").Value;
                AgentInformation.TeamName = xml.Element(updateNameSpace + "teamName").Value;
                AgentInformation.Name = AgentInformation.FirstName + " " + AgentInformation.LastName;

                AgentInformation.Status = xml.Element(updateNameSpace + "state").Value;
                AgentInformation.StateChangeTime = xml.Element(updateNameSpace + "stateChangeTime").Value;
                AgentInformation.PendingStatus = xml.Element(updateNameSpace + "pendingState").Value;
                if (xml.Element(updateNameSpace + "reasonCodeId") != null && xml.Element(updateNameSpace + "reasonCodeId").Value != null)
                    AgentInformation.ReasonCodeId = xml.Element(updateNameSpace + "reasonCodeId").Value;

                if (userMessage != null && AgentInformation.Status.Equals("LOGOUT"))
                {
                    if (isSecondMessage || FinesseVersion.Equals("UCCE_11.5"))
                    {
                        isSecondMessage = false;
                        //We have Signout Message
                        UnSubscribeFromQueuesEvents(); 
                        //UnSubscribeFromUserEvent();
                        //UnSubscribeFromDialogEvent();

                        if (finMatrixClient != null && finMatrixClient.IsConnected)
                            finMatrixClient.StartDisconnectTimer();
                        return false;
                    }
                    else
                        isSecondMessage = true;
                }
                //Mamdouh                else if (AgentInformation.Status.Equals("LOGOUT") && userMessage == null && xml.Element(updateNameSpace + "extension").Value.Trim().Equals(""))
                //Mamdouh                {
                //MamdouhFireDebugLogMessage("Agent was not able to login into Finesse ... xmpp will disconnect");
                //MamdouhFireErrorMessage("Invalid Device .. please check your extension ");
                //MamdouhUnSubscribeFromQueuesEvents(); //No need to unsubscribe from queue events
                //UnSubscribeFromUserEvent();
                //UnSubscribeFromDialogEvent();

                //Mamdouhif (finMatrixClient != null && finMatrixClient.IsConnected)
                //MamdouhfinMatrixClient.StartDisconnectTimer();
                //Mamdouhreturn false;

                //finRestClient.ChangeAgentState("NOT_READY", null, AgentInformation.AgentID, AgentInformation.UserName, AgentInformation.Password);
                //return;
                //Mamdouh}
                else if (AgentInformation.Status.Equals("LOGOUT") && userMessage == null)
                {
                    FireLogMessage("Agent Logged into LOGOUT status, will try to change it to login");
                    AgentInformation.Status = "NOT_READY";
                    //finRestClient.ChangeAgentState("NOT_READY", null, AgentInformation.AgentID, AgentInformation.UserName, AgentInformation.Password);
                    //return;
                }

                //Fill In Reason Code Details
                if (xml.Element(updateNameSpace + "reasonCode") != null && xml.Element(updateNameSpace + "reasonCode").Value != null)
                {
                    AgentInformation.ReasonCode = new AgentInformation.ReasonCodeClass();
                    AgentInformation.ReasonCode.Category = xml.Element(updateNameSpace + "reasonCode").Element(updateNameSpace + "category").Value;
                    AgentInformation.ReasonCode.URI = xml.Element(updateNameSpace + "reasonCode").Element(updateNameSpace + "uri").Value;
                    AgentInformation.ReasonCode.Code = xml.Element(updateNameSpace + "reasonCode").Element(updateNameSpace + "code").Value;
                    AgentInformation.ReasonCode.Label = xml.Element(updateNameSpace + "reasonCode").Element(updateNameSpace + "label").Value;
                    AgentInformation.ReasonCode.ForAll = xml.Element(updateNameSpace + "reasonCode").Element(updateNameSpace + "forAll").Value;
                    //AgentInformation.ReasonCode.ID = xml.Element(updateNameSpace + "ReasonCode").Element(updateNameSpace + "id").Value;
                }
                else
                    AgentInformation.ReasonCode = null;
                //Fill In Setting
                if (xml.Element(updateNameSpace + "settings") != null && xml.Element(updateNameSpace + "settings").Value != null)
                {
                    AgentInformation.Setting = new AgentInformation.SettingClass();
                    AgentInformation.Setting.WrapUpOnIncoming = xml.Element(updateNameSpace + "settings").Element(updateNameSpace + "wrapUpOnIncoming").Value;
                }
                else
                    AgentInformation.Setting = null;

                //Fill In Team Information
                if (xml.Element(updateNameSpace + "teams") != null && xml.Element(updateNameSpace + "teams").Value != null)
                {
                    AgentInformation.Teams = new MTObservableCollection<Team>();
                    foreach (XElement team in xml.Element(updateNameSpace + "teams").Elements(updateNameSpace + "team"))
                    {
                        AgentInformation.Team _team = new AgentInformation.Team();
                        _team.URI = team.Element(updateNameSpace + "uri").Value;
                        _team.ID = team.Element(updateNameSpace + "ID").Value;
                        _team.Name = team.Element(updateNameSpace + "name").Value;

                        AgentInformation.Teams.Add(_team);
                    }
                }
                else
                    AgentInformation.Teams = null;
                //Fill In Dialogs
                //if (xml.Element(updateNameSpace + "dialogs") != null && xml.Element(updateNameSpace + "dialogs").Value != null)
                //{
                //    foreach (XElement dialog in xml.Element(updateNameSpace + "dialogs").Elements(updateNameSpace + "Dialog"))
                //    {
                //        if (_agentInformation.Dialogs == null)
                //            _agentInformation.Dialogs = new ArrayList();

                //        LoadCallInformation(dialog);
                //    }
                //}
                if (AgentInformation.NotReadyReasonCodeList == null)
                    LoadNotReadyReasonCodeList();
                if (AgentInformation.LogoutReasonCodeList == null)
                    LoadLogoutReasonCodeList();
                UpdateVoiceStatusList();
            }
            catch (Exception ex)
            {
                FireLogMessage("Error in loading Agent information , ex:"+ex.Message+"\n"+ex.StackTrace);
            }
            return true;
            
        }
        #endregion
        #region Internal Methods
        private Model.Queue ResolveQueue(XElement queue)
        {
            try
            {
                XNamespace updateNameSpace = queue.Name.Namespace;
                FireLogMessage("Queue Message is:" + queue.ToString(),3);
                if (!queue.HasElements)
                    return null;
                Model.Queue _queue = new Model.Queue(this);

                _queue.Uri = queue.Element(updateNameSpace + "uri").Value;
                _queue.Name = queue.Element(updateNameSpace + "name").Value;
                _queue.CallsInQueue = queue.Element(updateNameSpace + "statistics").Element(updateNameSpace + "callsInQueue").Value;
                _queue.StartTimeOfLongestCallInQueue = queue.Element(updateNameSpace + "statistics").Element(updateNameSpace + "startTimeOfLongestCallInQueue").Value;
                _queue.AgentsReady = queue.Element(updateNameSpace + "statistics").Element(updateNameSpace + "agentsReady").Value;
                _queue.AgentsNotReady = queue.Element(updateNameSpace + "statistics").Element(updateNameSpace + "agentsNotReady").Value;
                _queue.AgentsTalkingInbound = queue.Element(updateNameSpace + "statistics").Element(updateNameSpace + "agentsTalkingInbound").Value;
                _queue.AgentsTalkingInternal = queue.Element(updateNameSpace + "statistics").Element(updateNameSpace + "agentsTalkingInternal").Value;
                _queue.AgentsTalkingOutbound = queue.Element(updateNameSpace + "statistics").Element(updateNameSpace + "agentsTalkingOutbound").Value;
                _queue.AgentsWrapUpReady = queue.Element(updateNameSpace + "statistics").Element(updateNameSpace + "agentsWrapUpReady").Value;
                _queue.AgentsWrapUpNotReady = queue.Element(updateNameSpace + "statistics").Element(updateNameSpace + "agentsWrapUpNotReady").Value;
                return _queue;
            }catch(Exception ex)
            {
                FireLogMessage("Error in resolving queue ex:"+ex.Message+"\n"+ex.StackTrace);
                return null;
            }
        }
        private Dialog ResolveDialog(XElement dialog)
        {
            try
            {
                XNamespace updateNameSpace = dialog.Name.Namespace;

                if (!dialog.HasElements)
                    return null;
                Dialog _dialog = new Dialog(this);
                _dialog.AssociatedDialogUri = dialog.Element(updateNameSpace + "associatedDialogUri").Value;
                _dialog.FromAddress = dialog.Element(updateNameSpace + "fromAddress").Value;
                _dialog.ID = dialog.Element(updateNameSpace + "id").Value;
                _dialog.MediaType = dialog.Element(updateNameSpace + "mediaType").Value;
                _dialog.State = dialog.Element(updateNameSpace + "state").Value;
                _dialog.ToAddress = dialog.Element(updateNameSpace + "toAddress").Value;
                _dialog.URI = dialog.Element(updateNameSpace + "uri").Value;
                _dialog.Header = "";
                _dialog.CallDirection = "";
                try
                {
                    if (_dialog.FromAddress.Equals(AgentInformation.Extension)) // Outgoing call
                    {
                        _dialog.Header = _dialog.ToAddress;
                        _dialog.CallDirection = "Out";
                    }
                    else // This is will fix header issue
                    {
                        _dialog.Header = _dialog.FromAddress;
                        _dialog.CallDirection = "In";
                    }
                }
                catch (Exception ex)
                {
                    FireErrorMessage("Can not set header of call:" + _dialog.ID + " Exception is:" + ex.Message + "\n" + ex.StackTrace);
                }

                //_dialog.Header = null; // Recalculate header

                if (_dialog.MediaProperties == null)
                    _dialog.MediaProperties = new Dialog.MediaPropertiesClass();
                //_dialog._MediaProperties._MediaID = dialog.Element(updateNameSpace +"mediaProperties").Element(updateNameSpace +"mediaId").Value;
                _dialog.MediaProperties.DNIS = dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "DNIS").Value;
                _dialog.MediaProperties.CallType = dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "callType").Value;
                _dialog.MediaProperties.DialedNumber = dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "dialedNumber").Value;
                _dialog.MediaProperties.OutboundClassification = dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "outboundClassification").Value;
                if (dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "callvariables") != null && dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "callvariables").Value != null)
                {
                    if (_dialog.MediaProperties.CallVariables == null)
                        _dialog.MediaProperties.CallVariables = new MTObservableCollection<Dialog.MediaPropertiesClass.CallVariableClass>();
                    foreach (XElement callVariable in dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "callvariables").Elements(updateNameSpace + "CallVariable"))
                    {
                        Dialog.MediaPropertiesClass.CallVariableClass _callVariable = new Dialog.MediaPropertiesClass.CallVariableClass();
                        _callVariable.Name = callVariable.Element(updateNameSpace + "name").Value;
                        _callVariable.Value = callVariable.Element(updateNameSpace + "value").Value;

                        _dialog.MediaProperties.CallVariables.Add(_callVariable);
                    }
                }
                if (dialog.Element(updateNameSpace + "participants") != null && dialog.Element(updateNameSpace + "participants").Value != null)
                {
                    if (_dialog.Participants == null)
                        _dialog.Participants = new MTObservableCollection<Dialog.Participant>();
                    foreach (XElement participant in dialog.Element(updateNameSpace + "participants").Elements(updateNameSpace + "Participant"))
                    {
                        Dialog.Participant _Participant = new Dialog.Participant();
                        _Participant.MediaAddress = participant.Element(updateNameSpace + "mediaAddress").Value;
                        _Participant.MediaAddressType = participant.Element(updateNameSpace + "mediaAddressType").Value;
                        _Participant.StartTime = participant.Element(updateNameSpace + "startTime").Value;
                        _Participant.State = participant.Element(updateNameSpace + "state").Value;
                        _Participant.StateCause = participant.Element(updateNameSpace + "stateCause").Value;
                        _Participant.StateChangeTime = participant.Element(updateNameSpace + "stateChangeTime").Value;
                        if (participant.Element(updateNameSpace + "actions") != null && participant.Element(updateNameSpace + "actions").Value != null)
                        {
                            if (_Participant.Actions == null)
                                _Participant.Actions = new MTObservableCollection<string>();
                            foreach (XElement action in participant.Element(updateNameSpace + "actions").Elements(updateNameSpace + "action"))
                                _Participant.Actions.Add(action.Value);
                        }
                        _dialog.Participants.Add(_Participant);
                    }
                }

                return _dialog;
            }catch(Exception ex)
            {
                FireLogMessage("Error in resolving dialog ex:" + ex.Message + "\n" + ex.StackTrace);
                return null;
            }
        }
        private string FindReasonCode(string label, bool isLogout)
        {
            if (label == null)
                return null;
            string code = null;
            try
            {
                MTObservableCollection<ReasonCodeClass> searchingList;
                if (isLogout)
                    searchingList = AgentInformation.LogoutReasonCodeList;
                else
                    searchingList = AgentInformation.NotReadyReasonCodeList;
                if (searchingList == null || searchingList.Count == 0)
                    return code;

                foreach (AgentInformation.ReasonCodeClass reasonCode in searchingList)
                    if (reasonCode.Label.Equals(label))
                        code = reasonCode.URI.Split('/')[reasonCode.URI.Split('/').Length - 1]; // Reason Code at the URI field.
            }catch(Exception ex)
            {
                FireLogMessage("Error in Finding Reason Code ex:" + ex.Message + "\n" + ex.StackTrace);
            }
            return code;
        }
        //private void LoadAgentID()
        //{
        //    FireLoadingMessage("Loading Agent ID");
        //    //Try to resolve Agent Information.
        //    if(AgentInformation.LoginID == null) //Login ID is null
        //    {
        //        if(AgentInformation.AgentID != null) // We have Agent ID
        //        {
        //            if (int.TryParse(AgentInformation.AgentID, out int agentID)) // We have a valid Agent ID
        //                return;
        //            else // Agent ID field has non numeric value // let us put it as login ID
        //            {
        //                AgentInformation.LoginID = AgentInformation.AgentID; // Now we have a login ID
        //            }
        //        } else if(AgentInformation.UserName != null) // We have a user Name instead
        //        {
        //            AgentInformation.LoginID = AgentInformation.UserName;
        //        }

        //    } else
        //    {
        //        if(int.TryParse(AgentInformation.LoginID, out int agentID)) // We have Agent ID typed into Login ID
        //        {
        //            AgentInformation.AgentID = AgentInformation.LoginID;
        //            return;
        //        }

        //    }

        //    if (AgentInformation.LoginID == null) // We do not have a login ID
        //        return;

        //    string uriPrefix = "http://";
        //    if (AgentInformation.SSL)
        //        uriPrefix = "https://";
        //    if (AgentInformation.DomainA != null)
        //        finRestClient.WebappPath = uriPrefix + AgentInformation.DomainA + ":" + AgentInformation.HTTPPort + AgentInformation.HTTPURL;
        //    else if (AgentInformation.DomainB != null)
        //        finRestClient.WebappPath = uriPrefix + AgentInformation.DomainB + ":" + AgentInformation.HTTPPort + AgentInformation.HTTPURL;
        //    FireLoadingMessage("Trying to load Agent ID");

        //   // finRestClient.SignIn(AgentInformation.LoginID, AgentInformation.LoginID, AgentInformation.Extension, AgentInformation.Password);
        //    //string userInfo = finRestClient.GetUserInfo(AgentInformation.LoginID, AgentInformation.LoginID, AgentInformation.Password);

        //    //FireLogMessage("User Information Loaded :"+userInfo);
        //    //LoadAgentInformation(userInfo);
        //    FireLoadingMessage("Agent ID loaded as:"+AgentInformation.AgentID);
        //}
        #endregion
        #region Events
        public void FireErrorMessage(string msg)
        {
            try
            {
                
                if (FinView != null)
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireErrorMessage(msg)));
                //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireErrorMessage(msg)));

                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => ErrorMessage = msg));
                //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => ErrorMessage = msg));
            }
            catch (Exception ex) {
                FireLogMessage("Error in Firing Error Message ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void FireNewMessage(string msg)
        {
            XElement xml;
            if (msg == null || msg == "")
                return;
            string message = msg;
            try
            {

                message = msg.Replace("&lt;", "<").Replace("&amp;", "&").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&apos;", "'");
                xml = XElement.Parse(message);
                if (_lastEventMessage == null)
                    _lastEventMessage = xml.ToString(); //First Message Received
                else if (_lastEventMessage.Equals(xml.ToString())) //Same Message Received before
                {
                    FireLogMessage("Will ignor message as we just proceed it",1);
                    return;
                }
                else
                    _lastEventMessage = xml.ToString(); // New Message Received

                foreach (XElement xmlMessage in xml.Elements())
                    foreach (XElement xmlEvent in xmlMessage.Elements())
                        foreach (XElement xmlItems in xmlEvent.Elements())
                            foreach (XElement xmlItem in xmlItems.Elements())
                                foreach (XElement xmlNotification in xmlItem.Elements())
                                    foreach (XElement xmlUpdate in xmlNotification.Elements())
                                        ExecuteMessage(xmlUpdate);

            }
            catch (Exception ex)
            {
                FireLogMessage("Error in Fire New Event ,ex:" + ex.Message+"\n"+ex.StackTrace);
            }
        }
        public void ExecuteMessage(XElement xmlUpdate)
        {
            if (!_validLicense)
                return;

            try
            {
                XNamespace updateNameSpace = xmlUpdate.Name.Namespace;
                if (xmlUpdate != null)
                {
                    AgentInformation.MessageEvent = new MessageEvent();
                    AgentInformation.MessageEvent.Event = xmlUpdate.Element(updateNameSpace + "event").Value;
                    if(xmlUpdate.Element(updateNameSpace + "source") != null)
                        AgentInformation.MessageEvent.Source = xmlUpdate.Element(updateNameSpace + "source").Value;

                    if (xmlUpdate.Element(updateNameSpace + "requestId") != null)
                        AgentInformation.MessageEvent.RequestID = xmlUpdate.Element(updateNameSpace + "requestId").Value;

                    XElement xmlData = xmlUpdate.Element(updateNameSpace + "data");
                    if (xmlData.Element(updateNameSpace + "user") != null) //User Message
                    {
                        FireLogMessage("New Event Message Received as \n" + xmlUpdate.ToString(), 3);

                        AgentInformation.MessageEvent.MessageType = "user";
                        LoadAgentInformation(xmlData.Element(updateNameSpace + "user").ToString());
                        FinView.FireNewEvent();
                    }
                    else if (xmlData.Element(updateNameSpace + "dialogs") != null) //Dialogs Message
                    {
                        //System.IO.File.WriteAllText(@"C:\Dialogs_" + new Random().Next() + ".xml", message);
                        //System.Windows.MessageBox.Show("A new Message Received from server :" + messageTruncated);
                        FireLogMessage("New Event Message Received as \n" + xmlUpdate.ToString(), 3);

                        AgentInformation.MessageEvent.MessageType = "call";
                        foreach (XElement xmlDialog in xmlData.Element(updateNameSpace + "dialogs").Elements(updateNameSpace + "Dialog"))
                        {
                            Dialog messageDialog = ResolveDialog(xmlDialog);
                            if (messageDialog != null) // Empty Message
                            {
                                messageDialog.DialogEvent = xmlUpdate.Element(updateNameSpace + "event").Value;
                                LoadCallInformation(messageDialog);
                                FinView.FireNewEvent();
                                FireCallEvent(messageDialog);
                            }
                        }
                    }
                    else if (xmlData.Element(updateNameSpace + "dialog") != null) // Call Data Update
                    {
                        //System.IO.File.WriteAllText(@"C:\UpdateDialog_" + new Random().Next() + ".xml", message);

                        //System.Windows.MessageBox.Show("A new Message Received from server :" + messageTruncated);
                        FireLogMessage("New Event Message Received as \n" + xmlUpdate.ToString(), 3);

                        AgentInformation.MessageEvent.MessageType = "callupdate";
                        Dialog messageDialog = ResolveDialog(xmlData.Element(updateNameSpace + "dialog"));
                        if (messageDialog != null)
                        {// Empty Message 
                            messageDialog.DialogEvent = xmlUpdate.Element(updateNameSpace + "event").Value;
                        }
                        LoadCallInformation(messageDialog);

                        FinView.FireNewEvent();
                        FireCallEvent(messageDialog);
                    }
                    else if (xmlData.Element(updateNameSpace + "queue") != null) // Queue Message Event
                    {
                        FireLogMessage("New Event Message Received as \n" + xmlUpdate.ToString(), 4);

                        AgentInformation.MessageEvent.MessageType = "queue";
                        Model.Queue queue = ResolveQueue(xmlData.Element(updateNameSpace + "queue"));
                        LoadQueueInformation(queue);
                        FireQueueEvent(queue);
                        FinView.FireNewEvent();
                    }
                    else if (xmlData.Element(updateNameSpace + "apiErrors") != null) // Some Error Happen
                    {
                        FireLogMessage("New Event Message Received as \n" + xmlUpdate.ToString(), 1);

                        AgentInformation.MessageEvent.MessageType = "error";
                        foreach (XElement apiError in xmlData.Element(updateNameSpace + "apiErrors").Elements(updateNameSpace + "apiError"))
                        {



                            AgentInformation.MessageEvent.ErrorCode = apiError.Element(updateNameSpace + "errorData").Value;
                            AgentInformation.MessageEvent.ErrorMsg = apiError.Element(updateNameSpace + "errorMessage").Value;
                            AgentInformation.MessageEvent.ErrorType = apiError.Element(updateNameSpace + "errorType").Value;

                            FinView.FireNewEvent();
                            FinesseErrorHandler(AgentInformation.MessageEvent);
                        }
                    }
                    else
                    {
                        FireLogMessage("New Event Message Received as \n" + xmlUpdate.ToString(), 1);
                        FireLogMessage("CXConnect: Unknown XMPP Message Event," + xmlUpdate.ToString());
                    }
                }
            }catch(Exception ex)
            {
                FireLogMessage("Error in Execute new message ,ex:" + ex.Message + "\n" + ex.StackTrace);
            }

        }
        private void FinesseErrorHandler(MessageEvent msgEvent)
        {
            try
            {
                FireLogMessage("Finesse Error Handler Invoked for the msgEvent with reason code:"+msgEvent.ErrorCode+" and source is:"+msgEvent.Source+" and description:"+msgEvent.ErrorMsg+" and error type:"+msgEvent.ErrorType,1);
                int errorCode = int.Parse(msgEvent.ErrorCode);
                bool disconnect = false;
                if(msgEvent.Source != null && msgEvent.Source.Contains("User") && !msgEvent.Source.Contains("Dialog")) // User Message
                {
                    switch (errorCode)
                    {
                        case 1://Agent Already Logged In Message
                            disconnect = true;
                            break;
                        case 2://The sepcificed Agent is not configured in CCE
                            disconnect = true;
                            break;
                        case 3://The Specified Media Routing Domain is not configured in CCE
                            disconnect = true;
                            break;
                        case 6://The Sepicified Agent is not logged in MRD
                            disconnect = true;
                            break;
                        case 11://Agent Can not login to the voice MRD
                            disconnect = true;
                            break;
                        case 12://Invalid Device
                            disconnect = true;
                            break;
                        case 27://The MRD and peripheral specified in the agent login request are not members of the application path associated with the Finesse server that sent the request.
                            disconnect = true;
                            break;
                        case 34://When the PG reaches the Maximum Concurrent Number of Logged in Agents
                            disconnect = true;
                            break;
                        case 36://Login Request failed because Center Controller offline
                            disconnect = true;
                            break;
                        case 37://Login Request Timeout
                            disconnect = true;
                            break;
                        case 38://Agent login request to the percision queue failed
                            disconnect = true;
                            break;
                        case 41://Pending request for that agent to login into MRD
                            disconnect = true;
                            break;
                        case 70://Unable to login
                            disconnect = true;
                            break;
                        default:
                            break;
                    }

                }


                switch (errorCode)
                {
                    case 5: //Internal Error
                        disconnect = true;
                        break;
                    case 260:
                        disconnect = true;
                        break;
                    default:
                        break;
                }
                FireErrorMessage(errorCode + ":" + msgEvent.ErrorType + ", " + msgEvent.ErrorMsg);
                if (AgentInformation.MessageEvent.ErrorType.Equals("Invalid Device") || AgentInformation.MessageEvent.ErrorType.Equals("Device Busy") || disconnect)
                {
                    finMatrixClient.StartDisconnectTimer();
                }

            }
            catch (Exception ex)
            {
                FireLogMessage("Error in fire finesse error and ex message:" + ex.Message + " , stack trace:\n" + ex.StackTrace);
            }
        }

        public void FireLoadingMessage(string msg)
        {
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => LoadingMessage = msg));
                //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => LoadingMessage = msg));
                if (FinView != null)
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireLoadingMessage(msg)));
                //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireLoadingMessage(msg)));
            }
            catch (Exception ex) {
                FireLogMessage("Error in Fire loading message ,ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void FireLogMessage(string msg)
        {
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => {
                    if (SaveLog)
                        new LogWriter(msg, LogLocation);
                    if (LogMessages.Count > 50)
                        LogMessages.RemoveAt(0);
                    LogMessages.Add(msg);
                    
                    }));
                //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => LogMessages.Add(msg)));
                if (FinView != null)
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireLogMessage(msg)));
                //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireLogMessage(msg)));
            }
            catch (Exception ex) {
                FireDebugLogMessage("Error in fire log message ,ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void FireDebugLogMessage(string msg)
        {
            if (!TraceStatus)
                return;
            FireLogMessage(msg);
        }
        public void FireLogMessage(string msg, int level)
        {
            if (level > 0 && level <= TraceLevel)
                FireLogMessage(msg);
        }
        public void FireReLoginEvent()
        {
            if (!_validLicense)
                return;
            try
            {
                LoadNotReadyReasonCodeList();
                LoadLogoutReasonCodeList();
                LoadSystemInformation();
                LoadCallInformation();
                LoadQueuesInformation();
                SubscribeToQueuesEvents();
                if (LoadAgentInformation())
                {
                    FireLoadingMessage("Agent Logged In with Status :" + AgentInformation.SelectedVoiceStatus.StatusLabel);
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireReLoginEvent()));
                }
                //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireReLoginEvent()));
            }
            catch (Exception ex) {
                FireLogMessage("Error in Fire ReLogin Event ,ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void FireLoadLoginScreen()
        {
            if (!_validLicense)
                return;
            try
            {
                //We need to reset the finAgent Here
                //AgentInformation = new AgentInformation();
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireLoadLoginScreen()));
                //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireLoadLoginScreen()));
            }
            catch (Exception ex) {
                FireLogMessage("Error in Fire Load Login Screen ,ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void FireQueueEvent(Model.Queue queue)
        {
            if (!_validLicense)
                return;
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireQueueEvent(queue)));
                //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireCallEvent(dialog)));
            }
            catch (Exception ex)
            {
                FireLogMessage("Error in Fire Queue Event ,ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void FireCallEvent(Dialog dialog)
        {
            if (!_validLicense)
                return;
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireCallEvent(dialog)));
                //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireCallEvent(dialog)));
            }catch(Exception ex) {
                FireLogMessage("Error in Fire Call Event ,ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        public void FireDisconnectEvent()
        {
            if (!_validLicense)
                return;
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireDisconnectEvent()));
                //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireDisconnectEvent()));
            }catch(Exception ex)
            {
                FireLogMessage("Error in Fire Disconnect Event ,ex:" + ex.Message + "\n" + ex.StackTrace);
            }
        }
        #endregion
    }
}
