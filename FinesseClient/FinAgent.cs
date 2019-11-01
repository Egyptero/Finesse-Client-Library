using FinesseClient.Common;
using FinesseClient.Core.Rest;
using FinesseClient.Core.XMPP;
using FinesseClient.Model;
using FinesseClient.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using static FinesseClient.Model.AgentInformation;

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
        private ObservableCollection<string> _logMessages = new ObservableCollection<string>();
        private string _errorMessage;
        private bool _isInteractionSpace;
        private bool _isMenuSpace;
        private StateTimer _stateTimer;
        private bool _traceStatus;
        private bool _saveLog;
        #endregion

        #region Properties
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
        public ObservableCollection<string> LogMessages { get { return _logMessages; } set { _logMessages = value; OnPropertyChanged("LogMessages"); } }
        #endregion

        #region Constructors
        public FinAgent()
        {
            if (DateTime.Now > new DateTime(2019, 4, 1))
            {
                MessageBox.Show("License is expired .. please contact Novelvox System Admin", "Error", MessageBoxButton.OK);
                throw new Exception("Error License is expired .. please contact istnetworks");
            }
            FireDebugLogMessage("Finesse Agent Object Created");
            AgentInformation = new AgentInformation();
            
        }
        public FinAgent(string _agentID, string _password, string _extension, string _domainA, string _domainB, bool _traceStatus, FinView UI)
        {
            if (DateTime.Now > new DateTime(2019, 4, 1))
            {
                MessageBox.Show("License is expired .. please contact Novelvox System Admin","Error",MessageBoxButton.OK);
                throw new Exception("Error License is expired .. please contact istnetworks");
            }

            AgentInformation = new AgentInformation
            {
                AgentID = _agentID,
                UserName = _agentID,
                Password = _password,
                Extension = _extension,
                DomainA = _domainA,
                DomainB = _domainB
            };
            FinView = UI;
            TraceStatus = _traceStatus;
            finRestClient = new FinRestClient(this);
        }
        #endregion

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


            if (AgentInformation.XMPPPort == null)
            {
                FireLogMessage("Finesse XMPP Port is not defined. Default Port will be used");
                AgentInformation.XMPPPort = "7071";
                AgentInformation.SSL = false;
            }

            if (AgentInformation.XMPPURL == null)
            {
                FireLogMessage("Finesse XMPP URL Suffix is not defined. Default will be used /http-bind/");
                AgentInformation.XMPPURL = "/http-bind/";
            }

            if (AgentInformation.HTTPPort == null)
            {
                FireLogMessage("Finesse HTTP Port is not defined. None SSL default is 80 will be used");
                AgentInformation.HTTPPort = "80";
                AgentInformation.SSL = false;
            }
            if (AgentInformation.HTTPURL == null)
            {
                FireLogMessage("Finesse HTTP URL Suffix is not defined. Default is /finesse");
                AgentInformation.HTTPURL = "/finesse";
            }
            if (AgentInformation.SSL)
            {
                if (AgentInformation.XMPPConnectionType == null)
                {
                    FireLogMessage("Finesse XMPP Connection type is not defined. Default is Tls11");
                    AgentInformation.XMPPConnectionType = "Tls11";
                }
                if (AgentInformation.HTTPConnectionType == null)
                {
                    FireLogMessage("Finesse Http Connection type is not defined. Default is Tls12");
                    AgentInformation.HTTPConnectionType = "Tls12";
                }
            }

            if (_valid)
                FireLogMessage("Agent information is valid,"+msg);
            else
            {
                FireLoadLoginScreen();
                FireLogMessage("Agent information verification failed ,check :"+msg);
                FireErrorMessage(msg);
            }
            return _valid;
        }
        public bool SignIn()
        {
            FireLogMessage("Agent login requested");

            if (!ValidateAgentLoginInformation())
                return false;

            finRestClient = new FinRestClient(this);
            finMatrixClient = new FinMatrixClient(this);
            finMatrixClient.Connect(null);
            return true;
        }
        public bool SignOut(string status, string reasonCodeLabel)
        {
            FireLogMessage("Agent logout requested");
            if (finRestClient == null)
                return false;

            string result = finRestClient.ChangeAgentState(status, FindReasonCode(reasonCodeLabel, true), AgentInformation.AgentID, AgentInformation.UserName, AgentInformation.Password);
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
            FireLogMessage("Answer requested for dialog ID:"+dialogID);
            string result = finRestClient.AnswerCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID);
            if (result == null || result == "")
                return true;

            return false;
        }
        public bool ReleaseCall(string dialogID)
        {
            FireLogMessage("Release requested for dialog ID:" + dialogID);
            string result = finRestClient.ReleaseCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool HoldCall(string dialogID)
        {
            FireLogMessage("Hold requested for dialog ID:" + dialogID);
            string result = finRestClient.HoldCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool TransferCall(string dialogID)
        {
            FireLogMessage("Transfer call requested for dialog ID:" + dialogID);
            string result = finRestClient.TransferCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool ResumeCall(string dialogID)
        {
            FireLogMessage("Resume call requested for dialog ID:" + dialogID);
            string result = finRestClient.RetriveCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool SSTransferCall(string dialogID, string dialedNumber)
        {
            FireLogMessage("SSTransfer requested for dialog ID:" + dialogID);
            string result = finRestClient.SSTransferCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID, dialedNumber);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool ConsultCall(string dialogID, string dialedNumber)
        {
            FireLogMessage("Consult requested for dialog ID:" + dialogID + "and dial number is :"+ dialedNumber);
            string result = finRestClient.ConsultCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID, dialedNumber);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool ConferenceCall(string dialogID, string dialedNumber)
        {
            FireLogMessage("Conference requested for dialog ID:" + dialogID + "and dial number is :" + dialedNumber);
            string result = finRestClient.ConferenceCall(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID, dialedNumber);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool KeypadSendDTMF(string dialogID, string dtmfString)
        {
            FireLogMessage("Send DTMF requested for dialog ID:" + dialogID + "and DTMF is :" + dtmfString);
            string result = finRestClient.KeypadSendDTMF(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID, dtmfString);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool MakeCall(string dialedNumber)
        {
            FireLogMessage("Make Call requested for dial number:" + dialedNumber);
            string result = finRestClient.MakeCall(AgentInformation.UserName, AgentInformation.AgentID, AgentInformation.Extension, dialedNumber, AgentInformation.Password);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool UpdateCallData(string dialogID, Dictionary<string, string> callVariables, string wrapupReason)
        {
            FireLogMessage("Update Call Data requested for dialog ID:" + dialogID);
            string result = finRestClient.UpdateCallData(AgentInformation.UserName, AgentInformation.Extension, AgentInformation.Password, dialogID, callVariables, wrapupReason);
            if (result == null || result == "")
                return true;
            return false;
        }
        public bool ChangeStatus(string status, string reasonCodeLabel)
        {
            FireLogMessage("Agent change status to: " + status + ", and reason code: "+reasonCodeLabel);
            if (finRestClient == null)
                return false;

            string result = finRestClient.ChangeAgentState(status, FindReasonCode(reasonCodeLabel, false), AgentInformation.AgentID, AgentInformation.UserName, AgentInformation.Password);
            if (result == null || result == "")
                return true;
            else
                return false;
        }
        public void LoadNotReadyReasonCodeList()
        {
            FireLogMessage("Loading not ready reason code");
            string _response = finRestClient.GetResonCodeList(AgentInformation.UserName, AgentInformation.AgentID, AgentInformation.Password, "NOT_READY");
            if (_response.Contains("(401)") || _response.Contains("(404)"))
                return;

            XElement xml = XElement.Parse(_response);
            if (xml == null || xml.Element("ErrorMessage") != null)
                return;
            if (xml.Elements("ReasonCode") != null)
                AgentInformation.NotReadyReasonCodeList = new ObservableCollection<ReasonCodeClass>();
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
        }
        public void LoadLogoutReasonCodeList()
        {
            FireLogMessage("Loading logout reason code");
            string _response = finRestClient.GetResonCodeList(AgentInformation.UserName, AgentInformation.AgentID, AgentInformation.Password, "LOGOUT");
            if (_response.Contains("(401)") || _response.Contains("(404)"))
                return;

            XElement xml = XElement.Parse(_response);
            if (xml == null || xml.Element("ErrorMessage") != null)
                return;
            if (xml.Elements("ReasonCode") != null)
                AgentInformation.LogoutReasonCodeList = new ObservableCollection<ReasonCodeClass>();

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
        }
        public void ChangeAgentVoiceStatus(VoiceStatus voiceStatus)
        {
            string newStatus;
            string reasonCodeLabel = null;

            string status = voiceStatus.StatusLabel;

            if (status.Contains("->"))
                return;

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

        }
        public void UpdateVoiceStatusList()
        {
            FireLogMessage("Update Voice Channel Status List");

            bool statusSelected = false;
            if (AgentInformation.VoiceStatusList == null)
                AgentInformation.VoiceStatusList = new ObservableCollection<VoiceStatus>();
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => AgentInformation.VoiceStatusList.Clear()));

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
            }

            System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));

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
                        }
                        else if (reasonCode.Label != null && AgentInformation.ReasonCode != null && AgentInformation.ReasonCode.Label != null)
                        {
                            if (reasonCode.Label.Equals(AgentInformation.ReasonCode.Label))
                            {
                                voiceStatus.Selected = true;
                                statusSelected = true;
                                AgentInformation.MakeCallVisible = true;
                            }
                        }

                    }
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
                }
            } else
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
                }
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
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
                        currentStatus = "Talking";
                    else if (currentStatus.Equals("HOLD"))
                        currentStatus = "Hold";
                    else if (currentStatus.Equals("RESERVED"))
                        currentStatus = "Reserved";
                    else if (currentStatus.Equals("LOGOUT"))
                        currentStatus = "Logout";
                    else if (currentStatus.Equals("LOGIN"))
                    {
                        currentStatus = "Login";
                        AgentInformation.MakeCallVisible = true;
                    }
                }

                if (pendingStatus != null)
                {
                    if (pendingStatus.Equals("NOT_READY"))
                        pendingStatus = "Not Ready";
                    else if (pendingStatus.Equals("TALKING"))
                        pendingStatus = "Talking";
                    else if (pendingStatus.Equals("HOLD"))
                        pendingStatus = "Hold";
                    else if (pendingStatus.Equals("RESERVED"))
                        pendingStatus = "Reserved";
                    else if (pendingStatus.Equals("LOGOUT"))
                        pendingStatus = "Logout";
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

                voiceStatus = new VoiceStatus()
                {
                    Status = AgentInformation.Status,
                    StatusLabel = currentStatus,
                    ReasonCode = currentLabel,
                    IconName = iconName
                };
                voiceStatus.Selected = true;
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
            }

            if (AgentInformation.StateChangeTime != null)
                if(AgentInformation.StateChangeTime.Trim().Length > 0)
                {
                    DateTime stateChangeTime = Convert.ToDateTime(AgentInformation.StateChangeTime);
                    if (StateTimer == null)
                        StateTimer = new StateTimer(stateChangeTime, "");
                    else
                        StateTimer.ResetTimer(stateChangeTime);
                }

            if (!AgentInformation.Status.Equals("LOGOUT"))
            {
                if (!AgentInformation.Status.Equals("NOT_READY"))
                {
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => SetSelectedVoiceStatus()));
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

                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
                    }

                } else
                {
                    voiceStatus = new VoiceStatus()
                    {
                        Status = "LOGOUT",
                        StatusLabel = "Logout",
                        IconName = "Other"
                    };

                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => AgentInformation.VoiceStatusList.Add(voiceStatus)));
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => SetSelectedVoiceStatus()));
                    return;
                }
            }
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => SetSelectedVoiceStatus()));
        }
        private void SetSelectedVoiceStatus()
        {
            if (AgentInformation.VoiceStatusList != null)
            {
                foreach (VoiceStatus voiceStatus in AgentInformation.VoiceStatusList)
                {
                    if (voiceStatus.Selected)
                        AgentInformation.SelectedVoiceStatus = voiceStatus;
                }
            }
        }
        public void LoadCallInformation()
        {
            FireLogMessage("Checking server if there is a call at the begaining of connection");
            string _response = finRestClient.GetUserDialogs(AgentInformation.UserName, AgentInformation.AgentID, AgentInformation.Password);
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
                    Dialog messageDialog = ResolveDialog(dialog);
                    messageDialog.DialogEvent = "RunningCall";
                    FireCallEvent(messageDialog);
                    LoadCallInformation(dialog);
                    //In case of fetch call at system login , fire USD event
                }
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error in load call information," + ex.Message);
                    Trace.Write(ex);
                }
            }
        }
        public void LoadCallInformation(XElement xmlDialog)
        {
            LoadCallInformation(ResolveDialog(xmlDialog));
        }
        public void LoadCallInformation(Dialog dialog)
        {
            FireLogMessage("Load Call Data from Finesse Event");
            if (AgentInformation == null) // In case of no Agent Information
                return;
            if (dialog == null) //Null Dialog is sent
                return;
            Dialog existingDialog =  FindDialog(dialog.ID);

            if (dialog.DialogEvent == null)
                if (!dialog.State.Equals("DROPPED") && !dialog.State.Equals("FAILED")) // A call is loaded at the begaining of login
                    dialog.DialogEvent = "RunningCall";

            if (dialog.DialogEvent != null)
            {
                if(dialog.DialogEvent.Equals("DELETE")) // Call is terminated
                {
                    if (existingDialog != null)
                    {
                        CheckDeleteDialog(dialog);
                        FireDebugLogMessage("Dialog is removed .. ID" + dialog.ID);
                    }else
                    {
                        FireDebugLogMessage("We have a delete call for non existing call dialog with ID: "+dialog.ID);
                    }
                }else if(dialog.DialogEvent.Equals("PUT") || dialog.DialogEvent.Equals("POST") || dialog.DialogEvent.Equals("RunningCall")) // Call Update
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
                        AgentInformation.SelectedDialog = existingDialog;

                        FireLogMessage("Call Data Updates for ID: "+dialog.ID);
                    }
                    else // New Call Data
                    {

                        foreach (Dialog.Participant participant in dialog.Participants)
                        {
                            DateTime stateChangeTime = Convert.ToDateTime(participant.StartTime);
                            if (dialog.DialogStateTimer == null)
                            {
                                FireLogMessage("Update Call Timer for ID: "+dialog.ID);
                                dialog.DialogStateTimer = new StateTimer(stateChangeTime, "");
                            }
                        }
                        if (AgentInformation.Dialogs == null)
                        {
                            AgentInformation.Dialogs = new ObservableCollection<Dialog>();
                            FireLogMessage("Create Dialogs List for the first time");
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => AgentInformation.Dialogs.Add(dialog)));
                            AgentInformation.SelectedDialog = dialog;
                        }else
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => AgentInformation.Dialogs.Add(dialog)));

                        FireLogMessage("New Call Posted for ID: " + dialog.ID);
                    }
                }
            } 
        }
        public bool CheckDeleteDialog(Dialog dialog)
        {
            bool isFound = false;
            if (AgentInformation.Dialogs != null && AgentInformation.Dialogs.Count >= 1)
            {
                for (int counter = AgentInformation.Dialogs.Count - 1; counter >= 0; counter--)
                //foreach (Dialog _dialog in _agentInformation.Dialogs)
                {
                    //if (_dialog != null && _dialog._ID.Equals(dialog._ID))
                    if (((Dialog)AgentInformation.Dialogs[counter]).ID.Equals(dialog.ID))
                    {
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.ContextIdle, (Action)(() => AgentInformation.Dialogs.RemoveAt(counter)));
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
            return _Dialog;
        }
        public bool CheckDialog(string dialogID)
        {
            bool isFound = false;
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
            return isFound;
        }
        public void LoadAgentInformation()
        {
            LoadAgentInformation(null);
        }
        private void LoadAgentInformation(string userMessage)
        {
            FireLogMessage("Load Agent Information");
            string _response = userMessage;
            if (_response == null || _response == "")
            {
                if (AgentInformation.UserName == null)
                    AgentInformation.UserName = AgentInformation.AgentID;
                _response = finRestClient.GetUserInfo(AgentInformation.UserName, AgentInformation.AgentID, AgentInformation.Password);
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
                AgentInformation.Roles = new ObservableCollection<string>();

                foreach (XElement role in xml.Element(updateNameSpace + "roles").Elements(updateNameSpace + "role"))
                    AgentInformation.Roles.Add(role.Value);
            }
            else
                AgentInformation.Roles = null;

            AgentInformation.UserName = xml.Element(updateNameSpace + "loginName").Value;
            AgentInformation.FirstName = xml.Element(updateNameSpace + "firstName").Value;
            AgentInformation.LastName = xml.Element(updateNameSpace + "lastName").Value;
            AgentInformation.TeamID = xml.Element(updateNameSpace + "teamId").Value;
            AgentInformation.TeamName = xml.Element(updateNameSpace + "teamName").Value;
            AgentInformation.Name = AgentInformation.FirstName + " " + AgentInformation.LastName;

            AgentInformation.Status = xml.Element(updateNameSpace + "state").Value;
            AgentInformation.StateChangeTime = xml.Element(updateNameSpace + "stateChangeTime").Value;
            AgentInformation.PendingStatus = xml.Element(updateNameSpace + "pendingState").Value;
            //AgentInformation.ReasonCodeId = xml.Element(updateNameSpace + "reasonCodeId").Value;

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
                AgentInformation.Teams = new ObservableCollection<Team>();
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
            if(AgentInformation.NotReadyReasonCodeList == null)
                LoadNotReadyReasonCodeList();
            if (AgentInformation.LogoutReasonCodeList == null)
                LoadLogoutReasonCodeList();
            UpdateVoiceStatusList();

        }
        #endregion
        #region Internal Methods
        private Dialog ResolveDialog(XElement dialog)
        {
            XNamespace updateNameSpace = dialog.Name.Namespace;

            if (!dialog.HasElements)
                return null;
            Dialog _dialog = new Dialog(this);
            _dialog.AssociatedDialogUri = dialog.Element(updateNameSpace +"associatedDialogUri").Value;
            _dialog.FromAddress = dialog.Element(updateNameSpace +"fromAddress").Value;
            _dialog.ID = dialog.Element(updateNameSpace +"id").Value;
            _dialog.MediaType = dialog.Element(updateNameSpace +"mediaType").Value;
            if (_dialog.MediaProperties == null)
                _dialog.MediaProperties = new Dialog.MediaPropertiesClass();
            //_dialog._MediaProperties._MediaID = dialog.Element(updateNameSpace +"mediaProperties").Element(updateNameSpace +"mediaId").Value;
            _dialog.MediaProperties.DNIS = dialog.Element(updateNameSpace +"mediaProperties").Element(updateNameSpace +"DNIS").Value;
            _dialog.MediaProperties.CallType = dialog.Element(updateNameSpace +"mediaProperties").Element(updateNameSpace +"callType").Value;
            _dialog.MediaProperties.DialedNumber = dialog.Element(updateNameSpace +"mediaProperties").Element(updateNameSpace +"dialedNumber").Value;
            _dialog.MediaProperties.OutboundClassification = dialog.Element(updateNameSpace +"mediaProperties").Element(updateNameSpace +"outboundClassification").Value;
            if (dialog.Element(updateNameSpace +"mediaProperties").Element(updateNameSpace +"callvariables") != null && dialog.Element(updateNameSpace +"mediaProperties").Element(updateNameSpace +"callvariables").Value != null)
            {
                if (_dialog.MediaProperties.CallVariables == null)
                    _dialog.MediaProperties.CallVariables = new ObservableCollection<Dialog.MediaPropertiesClass.CallVariableClass>();
                foreach (XElement callVariable in dialog.Element(updateNameSpace +"mediaProperties").Element(updateNameSpace +"callvariables").Elements(updateNameSpace +"CallVariable"))
                {
                    Dialog.MediaPropertiesClass.CallVariableClass _callVariable = new Dialog.MediaPropertiesClass.CallVariableClass();
                    _callVariable.Name = callVariable.Element(updateNameSpace +"name").Value;
                    _callVariable.Value = callVariable.Element(updateNameSpace +"value").Value;

                    _dialog.MediaProperties.CallVariables.Add(_callVariable);
                }
            }
            if (dialog.Element(updateNameSpace +"participants") != null && dialog.Element(updateNameSpace +"participants").Value != null)
            {
                if (_dialog.Participants == null)
                    _dialog.Participants = new ObservableCollection<Dialog.Participant>();
                foreach (XElement participant in dialog.Element(updateNameSpace +"participants").Elements(updateNameSpace +"Participant"))
                {
                    Dialog.Participant _Participant = new Dialog.Participant();
                    _Participant.MediaAddress = participant.Element(updateNameSpace +"mediaAddress").Value;
                    _Participant.MediaAddressType = participant.Element(updateNameSpace +"mediaAddressType").Value;
                    _Participant.StartTime = participant.Element(updateNameSpace +"startTime").Value;
                    _Participant.State = participant.Element(updateNameSpace +"state").Value;
                    _Participant.StateCause = participant.Element(updateNameSpace +"stateCause").Value;
                    _Participant.StateChangeTime = participant.Element(updateNameSpace +"stateChangeTime").Value;
                    if (participant.Element(updateNameSpace +"actions") != null && participant.Element(updateNameSpace +"actions").Value != null)
                    {
                        if (_Participant.Actions == null)
                            _Participant.Actions = new ObservableCollection<string>();
                        foreach (XElement action in participant.Element(updateNameSpace +"actions").Elements(updateNameSpace +"action"))
                            _Participant.Actions.Add(action.Value);
                    }
                    _dialog.Participants.Add(_Participant);
                }
            }
            _dialog.State = dialog.Element(updateNameSpace +"state").Value;
            _dialog.ToAddress = dialog.Element(updateNameSpace +"toAddress").Value;
            _dialog.URI = dialog.Element(updateNameSpace +"uri").Value;

            return _dialog;
        }
        private string FindReasonCode(string label, bool isLogout)
        {
            if (label == null)
                return null;
            string code = null;
            ObservableCollection<ReasonCodeClass> searchingList;
            if (isLogout)
                searchingList = AgentInformation.LogoutReasonCodeList;
            else
                searchingList = AgentInformation.NotReadyReasonCodeList;
            if (searchingList == null || searchingList.Count == 0)
                return code;

            foreach (AgentInformation.ReasonCodeClass reasonCode in searchingList)
                if (reasonCode.Label.Equals(label))
                    code = reasonCode.URI.Split('/')[reasonCode.URI.Split('/').Length - 1]; // Reason Code at the URI field.
            return code;
        }
        #endregion
        #region Events
        public void FireErrorMessage(string msg)
        {
            if(FinView != null)
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireErrorMessage(msg)));
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => ErrorMessage = msg));
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
                xml = XElement.Parse(message);
                FireDebugLogMessage("New Event Message Received as \n" + xml.ToString());
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
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error in parsing new XMPP Message Event," + ex.Message);
                    Trace.Write("CXConnect: Error in parsing new XMPP Message Event," + messageTruncated);
                    Trace.Write(ex);
                }
                return;
            }
        }
        private void ExecuteMessage(XElement xmlUpdate)
        {
            XNamespace updateNameSpace = xmlUpdate.Name.Namespace;
            if (xmlUpdate != null)
            {
                AgentInformation.MessageEvent = new MessageEvent();
                AgentInformation.MessageEvent.Event = xmlUpdate.Element(updateNameSpace + "event").Value;
                XElement xmlData = xmlUpdate.Element(updateNameSpace + "data");
                if (xmlData.Element(updateNameSpace + "user") != null) //User Message
                {
                    AgentInformation.MessageEvent.MessageType = "user";
                    LoadAgentInformation(xmlData.Element(updateNameSpace + "user").ToString());
                    FinView.FireNewEvent();
                }
                else if (xmlData.Element(updateNameSpace + "dialogs") != null) //Dialogs Message
                {
                    //System.IO.File.WriteAllText(@"C:\Dialogs_" + new Random().Next() + ".xml", message);
                    //System.Windows.MessageBox.Show("A new Message Received from server :" + messageTruncated);
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
                else if (xmlData.Element(updateNameSpace + "apiErrors") != null) // Some Error Happen
                {
                    AgentInformation.MessageEvent.MessageType = "error";
                    foreach (XElement apiError in xmlData.Element(updateNameSpace + "apiErrors").Elements(updateNameSpace + "apiError"))
                    {
                        AgentInformation.MessageEvent.ErrorCode = apiError.Element(updateNameSpace + "errorData").Value;
                        AgentInformation.MessageEvent.ErrorMsg = apiError.Element(updateNameSpace + "errorMessage").Value;
                        AgentInformation.MessageEvent.ErrorType = apiError.Element(updateNameSpace + "errorType").Value;

                        //Handle Invalid Device Errors and signout XMPP
                        if (AgentInformation.MessageEvent.ErrorType.Equals("Invalid Device") || AgentInformation.MessageEvent.ErrorType.Equals("Device Busy"))
                        {
                            finMatrixClient.Disconnect();
                        }

                        FinView.FireNewEvent();
                    }
                }
                else
                {
                    if (TraceStatus.Equals("true"))
                        Trace.Write("CXConnect: Unknown XMPP Message Event," + xmlUpdate.ToString());
                }
            }

        }

        public void FireLoadingMessage(string msg)
        {
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => LoadingMessage = msg));
            if (FinView != null)
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireLoadingMessage(msg)));
        }
        public void FireLogMessage(string msg)
        {
            if (SaveLog)
                new LogWriter(msg);
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => LogMessages.Add(msg)));
            if (FinView != null)
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireLogMessage(msg)));
        }
        public void FireDebugLogMessage(string msg)
        {
            if (!TraceStatus)
                return;
            FireLogMessage(msg);
        }
        public void FireReLoginEvent()
        {
            LoadAgentInformation();
            LoadCallInformation();
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireReLoginEvent()));
        }
        public void FireLoadLoginScreen()
        {
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireLoadLoginScreen()));
        }
        public void FireCallEvent(Dialog dialog)
        {
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireCallEvent(dialog)));
        }
        public void FireDisconnectEvent()
        {
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => FinView.FireDisconnectEvent()));
        }
        #endregion
    }
}
