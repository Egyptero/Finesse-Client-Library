using CXDE.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Threading;
using System.Xml.Linq;

namespace CXDE.Server_Side
{
    public class FinAgent
    {
        private FinMatrixClient finMatrixClient;
        public FinRestClient finRestClient;
        private MainWindow UIScreen { get; set; }
        public string TraceStatus { get; set; }
        public AgentInformation _agentInformation { get; private set; }
        public FinAgent(string _agentID, string _password, string _extension, string _domainA, string _domainB, MainWindow UI)
        {
            if (DateTime.Now > new DateTime(2019, 4, 1))
                throw new Exception("Error License is expired .. please contact istnetworks");

            _agentInformation = new AgentInformation
            {
                AgentID = _agentID,
                UserName = _agentID,
                Password = _password,
                Extension = _extension,
                DomainA = _domainA,
                DomainB = _domainB
            };
            UIScreen = UI;
            TraceStatus = "false";
            finRestClient = new FinRestClient(this);
            FireLogMessage("FinAgent Object is created for user:"+_agentID);
        }
        private bool ValidateAgentLoginInformation()
        {
            FireLogMessage("Validate Login Information");
            bool _valid = false;
            string msg = string.Empty;
            if (_agentInformation.AgentID == "" || _agentInformation.AgentID == null)
                msg = "Missing Agent ID Please enter Agent ID Example (1072)";
            else if (_agentInformation.Password == "" || _agentInformation.Password == null)
                msg = "Missing Password Please enter your password";
            else if (_agentInformation.Extension == "" || _agentInformation.Extension == null)
                msg = "Missing Extension Please enter your extension";
            else if (_agentInformation.DomainA == null && _agentInformation.DomainB == null)
                msg = "Finesse Server Domain is not defined";
            else
                _valid = true;
            if (_agentInformation.XmppPort == null)
            {
                FireLogMessage("Finesse XMPP Port is not defined. Default Port will be used");
                _agentInformation.XmppPort = "7071";
                _agentInformation.Ssl = false;
        }

            if (_agentInformation.XmppURL == null)
        {
                FireLogMessage("Finesse XMPP URL Suffix is not defined. Default will be used /http-bind/");
                _agentInformation.XmppURL = "/http-bind/";
            }

            if (_agentInformation.HttpPort == null)
            {
                FireLogMessage("Finesse HTTP Port is not defined. None SSL default is 80 will be used");
                _agentInformation.HttpPort = "80";
                _agentInformation.Ssl = false;
            }
            if (_agentInformation.HttpURL == null)
                {
                FireLogMessage("Finesse HTTP URL Suffix is not defined. Default is /finesse");
                _agentInformation.HttpURL = "/finesse";
                }
            if (_agentInformation.Ssl)
            {
                if (_agentInformation.XmppConnectionType == null)
                {
                    FireLogMessage("Finesse XMPP Connection type is not defined. Default is Tls");
                    _agentInformation.XmppConnectionType = "Tls";
            }
                if (_agentInformation.HttpConnectionType == null)
            {
                    FireLogMessage("Finesse Http Connection type is not defined. Default is Tls");
                    _agentInformation.HttpConnectionType = "Tls";
                }
            }

            if (_valid)
                FireLogMessage("Agent information is valid," + msg);
                    else
                    {
                FireLoadLoginScreen();
                FireLogMessage("Agent information verification failed ,check :" + msg);
                FireErrorMessage(msg);
                    }
            return _valid;
                }
        public bool SignIn()
                {
            FireLogMessage("Agent login requested");

            if (!ValidateAgentLoginInformation())
                    return false;
            try
            {
                finRestClient = new FinRestClient(this);
                finMatrixClient = new FinMatrixClient(this);
                
                finMatrixClient.Connect(null);
                return true;
                }
            catch (Exception)
            {
                return false;
            }
        }
        public bool SignOut(string status, string reasonCodeLabel)
        {
            FireLogMessage("Signout");
            if (finRestClient == null)
                return false;

            string result = finRestClient.ChangeAgentState(status, FindReasonCode(reasonCodeLabel, true), _agentInformation.AgentID, _agentInformation.UserName, _agentInformation.Password);
            if (result == null || result == "")
            {
                if (finMatrixClient != null && finMatrixClient.IsConnected)
                    finMatrixClient.Disconnect();
                return true;
            }
            else
                return false;
        }
        public bool AnswerCall(string dialogID)
        {
            FireLogMessage("Answer Call");
            string result = finRestClient.AnswerCall(_agentInformation.UserName, _agentInformation.Extension, _agentInformation.Password, dialogID);
            if (result == null || result == "")
                return true;

            return false;
        }
        public bool ReleaseCall(string dialogID)
        {
            FireLogMessage("Release Call");
            string result = finRestClient.ReleaseCall(_agentInformation.UserName, _agentInformation.Extension, _agentInformation.Password, dialogID);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool HoldCall(string dialogID)
        {
            FireLogMessage("Hold Call");
            string result = finRestClient.HoldCall(_agentInformation.UserName, _agentInformation.Extension, _agentInformation.Password, dialogID);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool TransferCall(string dialogID)
        {
            FireLogMessage("Transfer Call");
            string result = finRestClient.TransferCall(_agentInformation.UserName, _agentInformation.Extension, _agentInformation.Password, dialogID);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool ResumeCall(string dialogID)
        {
            FireLogMessage("Resume Call");
            string result = finRestClient.RetriveCall(_agentInformation.UserName, _agentInformation.Extension, _agentInformation.Password, dialogID);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool SSTransferCall(string dialogID, string dialedNumber)
        {
            FireLogMessage("Single Step Transfer Call");
            string result = finRestClient.SSTransferCall(_agentInformation.UserName, _agentInformation.Extension, _agentInformation.Password, dialogID, dialedNumber);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool ConsultCall(string dialogID, string dialedNumber)
        {
            FireLogMessage("Consult Call");
            string result = finRestClient.ConsultCall(_agentInformation.UserName, _agentInformation.Extension, _agentInformation.Password, dialogID, dialedNumber);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool ConferenceCall(string dialogID, string dialedNumber)
        {
            FireLogMessage("Conference Call");
            string result = finRestClient.ConferenceCall(_agentInformation.UserName, _agentInformation.Extension, _agentInformation.Password, dialogID, dialedNumber);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool KeypadSendDTMF(string dialogID, string dtmfString)
        {
            FireLogMessage("Keypad DTMF in Call");
            string result = finRestClient.KeypadSendDTMF(_agentInformation.UserName, _agentInformation.Extension, _agentInformation.Password, dialogID, dtmfString);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool MakeCall(string dialedNumber)
        {
            FireLogMessage("Make Call to:" + dialedNumber);
            string result = finRestClient.MakeCall(_agentInformation.UserName, _agentInformation.AgentID, _agentInformation.Extension, dialedNumber, _agentInformation.Password);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool UpdateCallData(string dialogID, Dictionary<string, string> callVariables, string wrapupReason)
        {
            FireLogMessage("Update Call Data");
            string result = finRestClient.UpdateCallData(_agentInformation.UserName, _agentInformation.Extension, _agentInformation.Password, dialogID, callVariables, wrapupReason);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool ChangeStatus(string status, string reasonCodeLabel)
        {
            FireLogMessage("Change Agent Status");
            if (finRestClient == null)
                return false;

            string result = finRestClient.ChangeAgentState(status, FindReasonCode(reasonCodeLabel, false), _agentInformation.AgentID, _agentInformation.UserName, _agentInformation.Password);
            if (result == null || result == "")
                return true;
            else
                return false;
        }
        public void LoadNotReadyReasonCodeList()
        {
            string _response = finRestClient.GetResonCodeList(_agentInformation.UserName, _agentInformation.AgentID, _agentInformation.Password, "NOT_READY");
            XElement xml = XElement.Parse(_response);
            if (xml == null || xml.Element("ErrorMessage") != null)
                return;
            if (xml.Elements("ReasonCode") != null)
                _agentInformation.NotReadyReasonCodeList = new ArrayList();
            foreach (XElement reasonCode in xml.Elements("ReasonCode"))
            {
                AgentInformation.ReasonCode _ReasonCode = new AgentInformation.ReasonCode();
                _ReasonCode.URI = reasonCode.Element("uri").Value;
                _ReasonCode.Category = reasonCode.Element("category").Value;
                _ReasonCode.Code = reasonCode.Element("code").Value;
                _ReasonCode.Label = reasonCode.Element("label").Value;
                _ReasonCode.ForAll = reasonCode.Element("forAll").Value;
                _agentInformation.NotReadyReasonCodeList.Add(_ReasonCode);
            }

        }
        public void LoadLogoutReasonCodeList()
        {
            string _response = finRestClient.GetResonCodeList(_agentInformation.UserName, _agentInformation.AgentID, _agentInformation.Password, "LOGOUT");
            XElement xml = XElement.Parse(_response);
            if (xml == null || xml.Element("ErrorMessage") != null)
                return;
            if (xml.Elements("ReasonCode") != null)
                _agentInformation.LogoutReasonCodeList = new ArrayList();

            foreach (XElement reasonCode in xml.Elements("ReasonCode"))
            {
                AgentInformation.ReasonCode _ReasonCode = new AgentInformation.ReasonCode();
                _ReasonCode.URI = reasonCode.Element("uri").Value;
                _ReasonCode.Category = reasonCode.Element("category").Value;
                _ReasonCode.Code = reasonCode.Element("code").Value;
                _ReasonCode.Label = reasonCode.Element("label").Value;
                _ReasonCode.ForAll = reasonCode.Element("forAll").Value;
                _agentInformation.LogoutReasonCodeList.Add(_ReasonCode);
            }
        }
        public void LoadCallInformation()
        {
            FireLogMessage("Load Call Information with no data");
            string _response = finRestClient.GetUserDialogs(_agentInformation.UserName, _agentInformation.AgentID, _agentInformation.Password);
            if (_response == null || _response == "")
                return;
            if (_response.IndexOf("<Dialogs>") < 0 || _response.IndexOf("</Dialogs>") < 0)
                return;
            try
            {

                string responseTruncated = _response.Substring(_response.IndexOf("<Dialogs>"), _response.IndexOf("</Dialogs>") - _response.IndexOf("<Dialogs>") + "</Dialogs>".Length);
                responseTruncated = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + responseTruncated;

                XElement xml = XElement.Parse(responseTruncated);
                foreach (XElement dialog in xml.Elements("Dialog"))
                {
                    LoadCallInformation(dialog);
                    //In case of fetch call at system login , fire USD event

                    Dialog messageDialog = ResolveDialog(dialog);
                    messageDialog._DialogEvent = "RunningCall";
                   if (!messageDialog._MediaProperties._CallType.Equals("AGENT_INSIDE"))
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => FireUSDEvent(messageDialog)));

                }
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
            {
                    Trace.Write("CXConnect: Error in load call information," + ex.Message);
                    Trace.Write(ex);
                }
                FireLogMessage(ex.ToString());
            }
        }
        public void LoadCallInformation(XElement xmlDialog)
        {
            FireLogMessage("Load Call Information from XML" + xmlDialog.ToString());
            LoadCallInformation(ResolveDialog(xmlDialog));
        }
        public void LoadCallInformation(Dialog dialog)
        {
            FireLogMessage("Load Call Data from Finesse Event");
            if (_agentInformation == null) // In case of no Agent Information
                return;
            if (dialog == null) //Null Dialog is sent
                return;
            Dialog existingDialog = FindDialog(dialog._ID);

            if (dialog._DialogEvent == null)
                if (!dialog._State.Equals("DROPPED") && !dialog._State.Equals("FAILED")) // A call is loaded at the begaining of login
                    dialog._DialogEvent = "RunningCall";

            if (dialog._DialogEvent != null)
            {
                if (dialog._DialogEvent.Equals("DELETE")) // Call is terminated
                {
                    if (existingDialog != null)
            {
                        CheckDeleteDialog(dialog);
                        FireLogMessage("Dialog is removed .. ID" + dialog._ID);
            }
            else
            {
                        FireLogMessage("We have a delete call for non existing call dialog with ID: " + dialog._ID);
                    }
                }
                else if (dialog._DialogEvent.Equals("PUT") || dialog._DialogEvent.Equals("POST") || dialog._DialogEvent.Equals("RunningCall")) // Call Update
                {
                    if (existingDialog != null) // Update Call Data
                    {
                        existingDialog._State = dialog._State;
                        existingDialog._ToAddress = dialog._ToAddress;
                        existingDialog._MediaProperties = dialog._MediaProperties;
                        existingDialog._MediaType = dialog._MediaType;
                        existingDialog._Participants = dialog._Participants;
                        existingDialog._DialogEvent = dialog._DialogEvent;
                        existingDialog._FromAddress = dialog._FromAddress;
                        existingDialog._URI = dialog._URI;

                        FireLogMessage("Call Data Updates for ID: " + dialog._ID);
                }
                    else // New Call Data
                {
                    if (_agentInformation.Dialogs == null)
                        {
                        _agentInformation.Dialogs = new ArrayList();
                            FireLogMessage("Create Dialogs List for the first time");
                            System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => _agentInformation.Dialogs.Add(dialog)));
                        }
                        else
                            System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => _agentInformation.Dialogs.Add(dialog)));

                        FireLogMessage("New Call Posted for ID: " + dialog._ID);
                }
            }
        }
        }
        public bool CheckDeleteDialog(Dialog dialog)
        {
            bool isFound = false;
            if (_agentInformation.Dialogs != null && _agentInformation.Dialogs.Count >= 1)
            {
                for (int counter = _agentInformation.Dialogs.Count - 1; counter >= 0; counter--)
                //foreach (Dialog _dialog in _agentInformation.Dialogs)
                {
                    //if (_dialog != null && _dialog._ID.Equals(dialog._ID))
                    if (((Dialog)_agentInformation.Dialogs[counter])._ID.Equals(dialog._ID))
                    {
                        _agentInformation.Dialogs.RemoveAt(counter);
                        isFound = true;
                    }

                }
            }
            return isFound;
        }
        public Dialog FindDialog(string dialogID)
        {
            Dialog _Dialog = null;
            if (dialogID == null)
                return _Dialog;
            if (_agentInformation.Dialogs != null && _agentInformation.Dialogs.Count >= 1)
            {
                for (int counter = 0; counter < _agentInformation.Dialogs.Count; counter++)
                {
                    if (((Dialog)_agentInformation.Dialogs[counter])._ID.Equals(dialogID))
                    {
                        _Dialog = (Dialog)_agentInformation.Dialogs[counter];
                    }

                }
            }
            return _Dialog;
        }
        public bool CheckDialog(string dialogID)
        {
            bool isFound = false;
            if (_agentInformation.Dialogs != null && _agentInformation.Dialogs.Count >= 1)
            {
                for (int counter = 0; counter < _agentInformation.Dialogs.Count; counter++)
                //foreach (Dialog _dialog in _agentInformation.Dialogs)
                {
                    //if (_dialog != null && _dialog._ID.Equals(dialog._ID))
                    if (((Dialog)_agentInformation.Dialogs[counter])._ID.Equals(dialogID))
                    {
                        isFound = true;
                    }

                }
            }
            return isFound;
        }
        public void LoadAgentInformation()
        {
            try
            {
                LoadAgentInformation(null);
            }
            catch (Exception) { }
        }
        private void LoadAgentInformation(string userMessage)
        {
            FireLogMessage("Load Agent Information");
            string _response = userMessage;
            if (_response == null || _response == "")
            {
                if (_agentInformation.UserName == null)
                    _agentInformation.UserName = _agentInformation.AgentID;
                _response = finRestClient.GetUserInfo(_agentInformation.UserName, _agentInformation.AgentID, _agentInformation.Password);
            }
            if (_response.Contains("(401)") || _response.Contains("(404)"))
                return;

            XElement xml = XElement.Parse(_response);
            XNamespace updateNameSpace = xml.Name.Namespace;
            if (xml.Element(updateNameSpace + "ErrorMessage") != null)
                return;
            //Fill In Roles
            if (xml.Element(updateNameSpace + "roles") != null && xml.Element(updateNameSpace + "roles").Value != null)
            {
                _agentInformation.Roles = new ArrayList();

                foreach (XElement role in xml.Element(updateNameSpace + "roles").Elements(updateNameSpace + "role"))
                    _agentInformation.Roles.Add(role.Value);
            }
            else
                _agentInformation.Roles = null;

            _agentInformation.UserName = xml.Element(updateNameSpace + "loginName").Value;
            _agentInformation.FirstName = xml.Element(updateNameSpace + "firstName").Value;
            _agentInformation.LastName = xml.Element(updateNameSpace + "lastName").Value;
            _agentInformation.TeamID = xml.Element(updateNameSpace + "teamId").Value;
            _agentInformation.TeamName = xml.Element(updateNameSpace + "teamName").Value;
            _agentInformation.Name = _agentInformation.FirstName + " " + _agentInformation.LastName;

            _agentInformation.Status = xml.Element(updateNameSpace + "state").Value;
            _agentInformation.StateChangeTime = xml.Element(updateNameSpace + "stateChangeTime").Value;
            _agentInformation.PendingStatus = xml.Element(updateNameSpace + "pendingState").Value;
            //_agentInformation.ReasonCodeId = xml.Element(updateNameSpace + "reasonCodeId").Value;

            //Fill In Reason Code Details
            if (xml.Element(updateNameSpace + "reasonCode") != null && xml.Element(updateNameSpace + "reasonCode").Value != null)
            {
                _agentInformation._ReasonCode = new AgentInformation.ReasonCode();
                _agentInformation._ReasonCode.Category = xml.Element(updateNameSpace + "reasonCode").Element(updateNameSpace + "category").Value;
                _agentInformation._ReasonCode.URI = xml.Element(updateNameSpace + "reasonCode").Element(updateNameSpace + "uri").Value;
                _agentInformation._ReasonCode.Code = xml.Element(updateNameSpace + "reasonCode").Element(updateNameSpace + "code").Value;
                _agentInformation._ReasonCode.Label = xml.Element(updateNameSpace + "reasonCode").Element(updateNameSpace + "label").Value;
                _agentInformation._ReasonCode.ForAll = xml.Element(updateNameSpace + "reasonCode").Element(updateNameSpace + "forAll").Value;
                //_agentInformation._ReasonCode.ID = xml.Element(updateNameSpace + "ReasonCode").Element(updateNameSpace + "id").Value;
            }
            else
                _agentInformation._ReasonCode = null;
            //Fill In Setting
            if (xml.Element(updateNameSpace + "settings") != null && xml.Element(updateNameSpace + "settings").Value != null)
            {
                _agentInformation._Setting = new AgentInformation.Setting();
                _agentInformation._Setting.WrapUpOnIncoming = xml.Element(updateNameSpace + "settings").Element(updateNameSpace + "wrapUpOnIncoming").Value;
            }
            else
                _agentInformation._Setting = null;

            //Fill In Team Information
            if (xml.Element(updateNameSpace + "teams") != null && xml.Element(updateNameSpace + "teams").Value != null)
            {
                _agentInformation.Teams = new ArrayList();
                foreach (XElement team in xml.Element(updateNameSpace + "teams").Elements(updateNameSpace + "team"))
                {
                    AgentInformation.Team _team = new AgentInformation.Team();
                    _team.URI = team.Element(updateNameSpace + "uri").Value;
                    _team.ID = team.Element(updateNameSpace + "ID").Value;
                    _team.Name = team.Element(updateNameSpace + "name").Value;

                    _agentInformation.Teams.Add(_team);
                }
            }
            else
                _agentInformation.Teams = null;
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
            if (_agentInformation.NotReadyReasonCodeList == null)
                LoadNotReadyReasonCodeList();
            if (_agentInformation.LogoutReasonCodeList == null)
                LoadLogoutReasonCodeList();

        }
        private Dialog ResolveDialog(XElement dialog)
        {
            XNamespace updateNameSpace = dialog.Name.Namespace;

            if (!dialog.HasElements)
                return null;
            Dialog _dialog = new Dialog();
            _dialog._AssociatedDialogUri = dialog.Element(updateNameSpace + "associatedDialogUri").Value;
            _dialog._FromAddress = dialog.Element(updateNameSpace + "fromAddress").Value;
            _dialog._ID = dialog.Element(updateNameSpace + "id").Value;
            _dialog._MediaType = dialog.Element(updateNameSpace + "mediaType").Value;
            if (_dialog._MediaProperties == null)
                _dialog._MediaProperties = new Dialog.MediaProperties();
            //_dialog._MediaProperties._MediaID = dialog.Element(updateNameSpace +"mediaProperties").Element(updateNameSpace +"mediaId").Value;
            _dialog._MediaProperties._DNIS = dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "DNIS").Value;
            _dialog._MediaProperties._CallType = dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "callType").Value;
            _dialog._MediaProperties._DialedNumber = dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "dialedNumber").Value;
            _dialog._MediaProperties._OutboundClassification = dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "outboundClassification").Value;
            if (dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "callvariables") != null && dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "callvariables").Value != null)
            {
                if (_dialog._MediaProperties._CallVariables == null)
                    _dialog._MediaProperties._CallVariables = new ArrayList();
                foreach (XElement callVariable in dialog.Element(updateNameSpace + "mediaProperties").Element(updateNameSpace + "callvariables").Elements(updateNameSpace + "CallVariable"))
                {
                    Dialog.MediaProperties.CallVariable _callVariable = new Dialog.MediaProperties.CallVariable();
                    _callVariable._Name = callVariable.Element(updateNameSpace + "name").Value;
                    _callVariable._Value = callVariable.Element(updateNameSpace + "value").Value;

                    _dialog._MediaProperties._CallVariables.Add(_callVariable);
                }
            }
            if (dialog.Element(updateNameSpace + "participants") != null && dialog.Element(updateNameSpace + "participants").Value != null)
            {
                if (_dialog._Participants == null)
                    _dialog._Participants = new ArrayList();
                foreach (XElement participant in dialog.Element(updateNameSpace + "participants").Elements(updateNameSpace + "Participant"))
                {
                    Dialog.Participant _Participant = new Dialog.Participant();
                    _Participant._MediaAddress = participant.Element(updateNameSpace + "mediaAddress").Value;
                    _Participant._MediaAddressType = participant.Element(updateNameSpace + "mediaAddressType").Value;
                    _Participant._StartTime = participant.Element(updateNameSpace + "startTime").Value;
                    _Participant._State = participant.Element(updateNameSpace + "state").Value;
                    _Participant._StateCause = participant.Element(updateNameSpace + "stateCause").Value;
                    _Participant._StateChangeTime = participant.Element(updateNameSpace + "stateChangeTime").Value;
                    if (participant.Element(updateNameSpace + "actions") != null && participant.Element(updateNameSpace + "actions").Value != null)
                    {
                        if (_Participant._Actions == null)
                            _Participant._Actions = new ArrayList();
                        foreach (XElement action in participant.Element(updateNameSpace + "actions").Elements(updateNameSpace + "action"))
                            _Participant._Actions.Add(action.Value);
                    }
                    _dialog._Participants.Add(_Participant);
                }
            }
            _dialog._State = dialog.Element(updateNameSpace + "state").Value;
            _dialog._ToAddress = dialog.Element(updateNameSpace + "toAddress").Value;
            _dialog._URI = dialog.Element(updateNameSpace + "uri").Value;

            return _dialog;
        }
        private string FindReasonCode(string label, bool isLogout)
        {
            if (label == null)
                return null;
            string code = null;
            ArrayList searchingList;
            if (isLogout)
                searchingList = _agentInformation.LogoutReasonCodeList;
            else
                searchingList = _agentInformation.NotReadyReasonCodeList;
            if (searchingList == null || searchingList.Count == 0)
                return code;

            foreach (AgentInformation.ReasonCode reasonCode in searchingList)
                if (reasonCode.Label.Equals(label))
                    code = reasonCode.URI.Split('/')[reasonCode.URI.Split('/').Length - 1]; // Reason Code at the URI field.
            return code;
        }
        public void FireErrorMessage(string msg)
        {
            UIScreen.FireErrorMessage(msg);
        }
        public void FireNewMessage(string msg)
        {
            XElement xml;
            if (msg == null || msg == "")
                return;
            string message = msg;
            string messageTruncated = "";
            try
            {

                message = msg.Replace("&lt;", "<").Replace("&amp;", "&").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&apos;", "'");
                //message = message.Replace("</body>", "");
                //message = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + message;
                xml = XElement.Parse(message);
                //FireLogMessage("Resolving message :\n" + xml.ToString());

                foreach (XElement xmlMessage in xml.Elements())
                    foreach (XElement xmlEvent in xmlMessage.Elements())
                        foreach (XElement xmlItems in xmlEvent.Elements())
                            foreach (XElement xmlItem in xmlItems.Elements())
                                foreach (XElement xmlNotification in xmlItem.Elements())
                                    foreach (XElement xmlUpdate in xmlNotification.Elements())
                                        ExecuteMessage(xmlUpdate);





               // if (message.IndexOf("<Update>") < 0 || message.IndexOf("</Update>") < 0)
               //     return;

                //messageTruncated = message.Substring(message.IndexOf("<Update>"), message.IndexOf("</Update>") - message.IndexOf("<Update>") + "</Update>".Length);
                //messageTruncated = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" + messageTruncated;


                //if (TraceStatus.Equals("true"))
                  //  Trace.Write("CXConnect: New Message," + messageTruncated);


                //xmlUpdate = xml;
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
            {
                    Trace.Write("CXConnect: Error in parsing new XMPP Message Event," + ex.Message);
                    Trace.Write("CXConnect: Error in parsing new XMPP Message Event," + messageTruncated);
                    Trace.Write(ex);
                }
                FireLogMessage("Error during message processing:" + ex.ToString());
                return;
            }
        }
        private void ExecuteMessage(XElement xmlUpdate)
        {
            XNamespace updateNameSpace = xmlUpdate.Name.Namespace;
            FireLogMessage("Start Executing Update for the message:\n" + xmlUpdate);
            if (xmlUpdate != null)
            {
                _agentInformation._MessageEvent = new MessageEvent();
                _agentInformation._MessageEvent.Event = xmlUpdate.Element(updateNameSpace + "event").Value;
                XElement xmlData = xmlUpdate.Element(updateNameSpace + "data");
                if (xmlData.Element(updateNameSpace + "user") != null) //User Message
                {
                    FireLogMessage("New User Message and will try load it");
                    _agentInformation._MessageEvent.MessageType = "user";
                    LoadAgentInformation(xmlData.Element(updateNameSpace + "user").ToString());
                    FireLogMessage("New User Message should be rendered now");
                    UIScreen.FireNewEvent();
                    FireLogMessage("New User Message also is fired in front end");
                }
                else if (xmlData.Element(updateNameSpace + "dialogs") != null) //Dialogs Message
                {
                    FireLogMessage("New Dialog Message and will try load it");
                    //System.IO.File.WriteAllText(@"C:\Dialogs_" + new Random().Next() + ".xml", message);
                    //System.Windows.MessageBox.Show("A new Message Received from server :" + messageTruncated);
                    _agentInformation._MessageEvent.MessageType = "call";
                    foreach (XElement xmlDialog in xmlData.Element(updateNameSpace + "dialogs").Elements(updateNameSpace + "Dialog"))
                    {
                        Dialog messageDialog = ResolveDialog(xmlDialog);
                        if (messageDialog != null) // Empty Message
                        {
                            messageDialog._DialogEvent = xmlUpdate.Element(updateNameSpace + "event").Value;
                        LoadCallInformation(messageDialog);

                        }
                    }
                    FireLogMessage("New Dialog Message should be rendered now");
                    UIScreen.FireNewEvent();
                    FireLogMessage("New Dialog Message also is fired in front end");

                    foreach (XElement xmlDialog in xmlData.Element(updateNameSpace + "dialogs").Elements(updateNameSpace + "Dialog"))
                    {
                        Dialog messageDialog = ResolveDialog(xmlDialog);
                        if (messageDialog != null) // Empty Message
                        {
                            messageDialog._DialogEvent = xmlUpdate.Element(updateNameSpace + "event").Value;
                            if (!messageDialog._MediaProperties._CallType.Equals("AGENT_INSIDE"))
                                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => FireUSDEvent(messageDialog)));
                        }
                    }
                    FireLogMessage("New Dialog Message also is fired USD");
                }
                else if (xmlData.Element(updateNameSpace + "dialog") != null) // Call Data Update
                {
                    FireLogMessage("New Dialog Message and will try load it");
                    //System.IO.File.WriteAllText(@"C:\UpdateDialog_" + new Random().Next() + ".xml", message);

                    //System.Windows.MessageBox.Show("A new Message Received from server :" + messageTruncated);
                    _agentInformation._MessageEvent.MessageType = "call";
                    Dialog messageDialog = ResolveDialog(xmlData.Element(updateNameSpace + "dialog"));
                    if (messageDialog != null)
                    {// Empty Message 
                        messageDialog._DialogEvent = xmlUpdate.Element(updateNameSpace + "event").Value;
                    }
                    LoadCallInformation(messageDialog);
                    FireLogMessage("New Dialog Message should be rendered now");
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => UIScreen.FireNewEvent()));
                     if (!messageDialog._MediaProperties._CallType.Equals("AGENT_INSIDE"))
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => UIScreen.FireUSDEvent(messageDialog)));
                    FireLogMessage("New Dialog Update Message also is fired in front end, and USD also");
                }
                else if (xmlData.Element(updateNameSpace + "apiErrors") != null) // Some Error Happen
                {
                    FireLogMessage("New Error Message and will try to fire it:" + xmlData.Element(updateNameSpace + "apiErrors").ToString());
                    _agentInformation._MessageEvent.MessageType = "error";
                    foreach (XElement apiError in xmlData.Element(updateNameSpace + "apiErrors").Elements(updateNameSpace + "apiError"))
                    {
                        _agentInformation._MessageEvent.errorCode = apiError.Element(updateNameSpace + "errorData").Value;
                        _agentInformation._MessageEvent.errorMsg = apiError.Element(updateNameSpace + "errorMessage").Value;
                        _agentInformation._MessageEvent.errorType = apiError.Element(updateNameSpace + "errorType").Value;

                        //Handle Invalid Device Errors and signout XMPP
                        if (_agentInformation._MessageEvent.errorType.Equals("Invalid Device") || _agentInformation._MessageEvent.errorType.Equals("Device Busy"))
                        {
                            finMatrixClient.Disconnect();
                            FireLogMessage("Error Message is received and we have to disconnect XMPP, error type is:" + _agentInformation._MessageEvent.errorType);
                        }

                        UIScreen.FireNewEvent();
                        FireLogMessage("New Dialog Message also is fired in front end, No USD this time");
                    }
                    }
                else
                {
                    if (TraceStatus.Equals("true"))
                        Trace.Write("CXConnect: Unknown XMPP Message Event," + xmlUpdate.ToString());
                    FireLogMessage("Unknown handled message type:" + xmlUpdate.ToString());
                }
            }

        }
        public void FireLoadingMessage(string msg)
        {
            UIScreen.FireLoadingMessage(msg);
        }
        public void FireReLoginEvent()
        {
            UIScreen.FireReLoginEvent();
        }
        public void FireLoadLoginScreen()
        {
            //SignOut("LOGOUT", null);
            UIScreen.FireLoadLoginScreen();
        }
        public void FireUSDEvent(Dialog dialog)
        {
            UIScreen.FireUSDEvent(dialog);
        }
        public void FireLogMessage(string msg)
        {
            if (!TraceStatus.Equals("true"))
                return;
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(() =>
                {
                    new LogWriter(msg);
                }));
            }
            catch (Exception)
            {

            }
        }
        public void FireDisconnectEvent()
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (Action)(() => UIScreen.FireDisconnectEvent()));
            }
            catch (Exception) { }
        }

    }
}
