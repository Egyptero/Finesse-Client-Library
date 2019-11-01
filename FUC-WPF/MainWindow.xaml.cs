using FinesseClient;
using FinesseClient.Common;
using FinesseClient.Model;
using FinesseClient.View;
using FUC_WPF.ARB;
using FUC_WPF.Model;
using FUC_WPF.Tools;
using MaterialDesignThemes.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using static FinesseClient.Model.Dialog.MediaPropertiesClass;

namespace FUC_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, FinView
    {
        FinAgent _finAgent = new FinAgent();
        private enum View {Main,Loading,Login,Queue,About,Telephony};
        private enum CallAction { Hold,Resume,TransferToSurvey,TransferToExt,TransferToIVR,Answer,Release};
        public MainWindow()
        {
            _finAgent.AgentInformation.AgentID = "1072";
            _finAgent.AgentInformation.Extension = "1072";
            _finAgent.AgentInformation.DomainA = "finesse1.dcloud.cisco.com";
            _finAgent.FinView = this;
            _finAgent.SaveLog = true;
            _finAgent.TraceStatus = true;
            _finAgent.AgentInformation.SSL = true;
            AgentInformationExtension agentInformationExtension = new AgentInformationExtension();
            agentInformationExtension.ConfigureDetails = true;
            _finAgent.AgentInformation.ReservedObject = agentInformationExtension;

            LoadConfiguration();

            DataContext = _finAgent;
            InitializeComponent();
            SwitchView(View.Login);
        }

        private void ProcessLoginFromText(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                PerformLogin();
        }
        private void LoginClick(object sender, RoutedEventArgs e)
        {
            PerformLogin();
        }
        private void VoiceStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (StatusComboBox.SelectedItem == null)
                    return;
                _finAgent.FireLoadingMessage("Change Status to: "+(StatusComboBox.SelectedItem as VoiceStatus).StatusLabel);
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => _finAgent.ChangeAgentVoiceStatus(StatusComboBox.SelectedItem as VoiceStatus)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Unable to change Agent Status with error:"+ex.StackTrace);
                _finAgent.FireErrorMessage("Can not change Agent Status");
            }
        }
        private void AgentInformation_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem == null)
                return;
            if(menuItem.Name.Equals("QueueMenu"))
                SwitchView(View.Queue);
            else if(menuItem.Name.Equals("AboutMenu"))
                SwitchView(View.About);
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            SwitchView(View.Telephony);
        }
        private void Calls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Dialog dialog = _finAgent.FindDialog(button.Tag as string);
            DialogExtension dialogExtension = dialog.ReservedObject as DialogExtension;
            if (dialogExtension == null)
            {
                dialogExtension = BuildDialogExtension(button.Tag as string);
                dialog.ReservedObject = dialogExtension;
            }

            if (button.Name.Equals("Answer"))
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => DoCallAction(CallAction.Answer, button.Tag as string)));
            else if (button.Name.Equals("TransferDialPad"))
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() =>
                {
                    _finAgent.FireLoadingMessage("Open dialpad");
                    ToggleDialPad(true, button.Tag as string);
                }));
            }
            else if (button.Name.Equals("TransferIVR"))
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => 
                {
                    _finAgent.FireLoadingMessage("Open Send to IVR");
                    ToggleUpdateRoutingData(true, button.Tag as string);
                }));

            }
            else if(button.Name.Equals("TransferSurvey"))
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => DoCallAction(CallAction.TransferToSurvey, button.Tag as string)));
            else if (button.Name.Equals("Hold"))
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => DoCallAction(CallAction.Hold, button.Tag as string)));
            else if (button.Name.Equals("Resume"))
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => DoCallAction(CallAction.Resume,button.Tag as string)));
            else if (button.Name.Equals("Release"))
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => DoCallAction(CallAction.Release, button.Tag as string)));
        }
        private void UpdateRoutingDataButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Dialog dialog = _finAgent.FindDialog(button.Tag as string);

            if (button.Name.Equals("callRoute"))
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => DoCallAction(CallAction.TransferToIVR, button.Tag as string)));
            else if(button.Name.Equals("closeRoute"))
                ToggleUpdateRoutingData(false, button.Tag as string);
        }
        private void DialPadButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Dialog dialog = _finAgent.FindDialog(button.Tag as string);
            DialogExtension dialogExtension = dialog.ReservedObject as DialogExtension;

            if (dialogExtension.DialNumber == null)
                dialogExtension.DialNumber = "";
            if (button.Name.Equals("one"))
                dialogExtension.DialNumber += "1";
            else if (button.Name.Equals("two"))
                dialogExtension.DialNumber += "2";
            else if (button.Name.Equals("three"))
                dialogExtension.DialNumber += "3";
            else if (button.Name.Equals("four"))
                dialogExtension.DialNumber += "4";
            else if (button.Name.Equals("five"))
                dialogExtension.DialNumber += "5";
            else if (button.Name.Equals("six"))
                dialogExtension.DialNumber += "6";
            else if (button.Name.Equals("seven"))
                dialogExtension.DialNumber += "7";
            else if (button.Name.Equals("eight"))
                dialogExtension.DialNumber += "8";
            else if (button.Name.Equals("nine"))
                dialogExtension.DialNumber += "9";
            else if (button.Name.Equals("star"))
                dialogExtension.DialNumber += "*";
            else if (button.Name.Equals("zero"))
                dialogExtension.DialNumber += "0";
            else if (button.Name.Equals("hash"))
                dialogExtension.DialNumber += "#";
            else if (button.Name.Equals("call"))
            {
                if(dialogExtension.DialNumber != null)
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => DoCallAction(CallAction.TransferToExt, button.Tag as string)));
            }
            else if (button.Name.Equals("clear"))
                dialogExtension.DialNumber = "";
            else if (button.Name.Equals("close"))
                ToggleDialPad(false, button.Tag as string);

            dialog.ReservedObject = dialogExtension;
        }
        private void MakeCallClick(object sender, RoutedEventArgs e)
        {

        }
        private void ToggleDialPad(bool status,string dialogID)
        {
            if (dialogID == null)
                return;
            Dialog dialog = _finAgent.FindDialog(dialogID);
            if (dialog.ReservedObject == null)
                dialog.ReservedObject = BuildDialogExtension(dialogID);

            DialogExtension dialogExtension = dialog.ReservedObject as DialogExtension;
            dialogExtension.DialPadStatus = status;
            if (status)
                dialogExtension.UpdateRoutingData = false;
            dialog.ReservedObject = dialogExtension;
        }
        private void ToggleUpdateRoutingData(bool status, string dialogID)
        {
            if (dialogID == null)
                return;
            Dialog dialog = _finAgent.FindDialog(dialogID);
            if (dialog.ReservedObject == null)
                dialog.ReservedObject = BuildDialogExtension(dialogID);
            DialogExtension dialogExtension = dialog.ReservedObject as DialogExtension;
            dialogExtension.UpdateRoutingData = status;
            if (status)
                dialogExtension.DialPadStatus = false;
            dialog.ReservedObject = dialogExtension;
        }
        private void PerformLogin()
        {
            _finAgent.AgentInformation.Password = AgentPassword.Password;
            SwitchView(View.Loading);
            if (!_finAgent.SignIn())
                SwitchView(View.Login);
        }
        private void DoCallAction(CallAction action,string dialogID)
        {
            if (dialogID == null)
                return;
            Dialog dialog = _finAgent.FindDialog(dialogID);
            DialogExtension dialogExtension = dialog.ReservedObject as DialogExtension;

            ToggleUpdateRoutingData(false, dialogID);
            ToggleDialPad(false, dialogID);
            _finAgent.FireLoadingMessage("Call Action: "+action);
            dialogExtension.Message = "Trying to "+action;
            dialogExtension.Disabled = true;
            dialog.ReservedObject = dialogExtension;

            switch (action)
            {
                case CallAction.Answer:
                    if (!_finAgent.AnswerCall(dialogID))
                        dialogExtension.Disabled = false;
                    break;
                case CallAction.Hold:
                    if (!_finAgent.HoldCall(dialogID))
                        dialogExtension.Disabled = false;
                    break;
                case CallAction.Release:
                    if (!_finAgent.ReleaseCall(dialogID))
                        if (dialog != null)
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => _finAgent.AgentInformation.Dialogs.Remove(dialog)));
                    break;
                case CallAction.Resume:
                    if (!_finAgent.ResumeCall(dialogID))
                        dialogExtension.Disabled = false;
                    break;
                case CallAction.TransferToExt:
                    if (!_finAgent.SSTransferCall(dialogID, dialogExtension.DialNumber))
                        dialogExtension.Disabled = false;
                    break;
                case CallAction.TransferToIVR:
                    string extIVR = string.Empty;
                    if (dialogExtension.IsInquiry)
                        extIVR = dialogExtension.IVROptions.GetExtension(dialogExtension.SelectedIVROption, dialogExtension.IsMaleOnly, ARB.IVROptions.TransferType.INQ);
                    else
                        extIVR = dialogExtension.IVROptions.GetExtension(dialogExtension.SelectedIVROption, dialogExtension.IsMaleOnly, ARB.IVROptions.TransferType.COMP);

                    if (!_finAgent.SSTransferCall(dialogID, extIVR))
                        dialogExtension.Disabled = false;
                    break;
                case CallAction.TransferToSurvey:
                    string extSurvey = dialogExtension.IVROptions.GetExtension("Survey", true, ARB.IVROptions.TransferType.INQ);

                    if (!_finAgent.SSTransferCall(dialogID, extSurvey))
                        dialogExtension.Disabled = false;
                    break;
            }
            dialog.ReservedObject = dialogExtension;
        }
        private void SwitchView(View view)
        {
            switch (view)
            {
                case View.Loading:
                    LoadingForm.Visibility = Visibility.Visible;
                    LoginForm.Visibility = Visibility.Hidden;
                    MainForm.Visibility = Visibility.Hidden;
                    break;
                case View.Login:
                    LoadingForm.Visibility = Visibility.Hidden;
                    LoginForm.Visibility = Visibility.Visible;
                    MainForm.Visibility = Visibility.Hidden;
                    break;
                case View.Main:
                    LoadingForm.Visibility = Visibility.Hidden;
                    LoginForm.Visibility = Visibility.Hidden;
                    MainForm.Visibility = Visibility.Visible;
                    break;
                case View.Queue:
                    Telephony.Visibility = Visibility.Hidden;
                    Queues.Visibility = Visibility.Visible;
                    About.Visibility = Visibility.Hidden;
                    break;
                case View.About:
                    Telephony.Visibility = Visibility.Hidden;
                    Queues.Visibility = Visibility.Hidden;
                    About.Visibility = Visibility.Visible;
                    break;
                case View.Telephony:
                    Telephony.Visibility = Visibility.Visible;
                    Queues.Visibility = Visibility.Hidden;
                    About.Visibility = Visibility.Hidden;
                    break;
            }
        }
        public void FireLoadingMessage(string msg)
        {
        }

        public void FireLoadLoginScreen()
        {
            _finAgent.FireLoadingMessage("Please login..");
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => SwitchView(View.Login)));
        }

        public void FireErrorMessage(string msg)
        {
            ErrorBar.MessageQueue.Enqueue(msg);
        }

        public void FireLogMessage(string msg)
        {
        }

        public Screen GetLocation()
        {
            return Screen.MainGrid;
        }

        public void SetContext(IModel model, FinView finView)
        {
        }

        public void FireNewEvent()
        {
            //ErrorBar.IsActive = false;

            if (_finAgent.AgentInformation.MessageEvent.MessageType.Equals("error"))
            {
                if(_finAgent.AgentInformation.MessageEvent.ErrorCode.Equals("1") && _finAgent.AgentInformation.MessageEvent.ErrorType.Equals("Generic Error"))
                {
                    FireErrorMessage("Can not complete call transfer");
                    FireLogMessage("Finesse Server Error Message: " + _finAgent.AgentInformation.MessageEvent.ErrorMsg + " ,Finesse Server Error Type: " + _finAgent.AgentInformation.MessageEvent.ErrorType + " , Finesse Server Error Code: " + _finAgent.AgentInformation.MessageEvent.ErrorCode);

                }else
                {
                    FireErrorMessage("Code:" + _finAgent.AgentInformation.MessageEvent.ErrorCode + "," + _finAgent.AgentInformation.MessageEvent.ErrorMsg + " ,Type: " + _finAgent.AgentInformation.MessageEvent.ErrorType);
                    FireLogMessage("Finesse Server Error Message: " + _finAgent.AgentInformation.MessageEvent.ErrorMsg + " ,Finesse Server Error Type: " + _finAgent.AgentInformation.MessageEvent.ErrorType + " , Finesse Server Error Code: " + _finAgent.AgentInformation.MessageEvent.ErrorCode);
                }
                foreach (Dialog dialog in _finAgent.AgentInformation.Dialogs)
                {
                    if (dialog.ReservedObject != null)
                    {
                        DialogExtension dialogExtension = dialog.ReservedObject as DialogExtension;
                        dialogExtension.Disabled = false;
                        dialog.ReservedObject = dialogExtension;
                    }
                }
            }
        }

        public void FireReLoginEvent()
        {
            _finAgent.FireLoadingMessage("Agent logged in..");
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => SwitchView(View.Main)));
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => SwitchView(View.Telephony)));
        }

        public void FireDisconnectEvent()
        {
            _finAgent.FireLoadingMessage("System disconnected , will try to relogin.");
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => SwitchView(View.Loading)));
        }

        public void FireCallEvent(Dialog dialog)
        {
            Dialog locatedDialog = _finAgent.FindDialog(dialog.ID);
            if (locatedDialog == null)
                return;

            SwitchView(View.Telephony);

            DialogExtension dialogExtension = locatedDialog.ReservedObject as DialogExtension;
            if (dialogExtension == null)
                dialogExtension = BuildDialogExtension(dialog.ID);

            dialogExtension.Disabled = false;

            locatedDialog.ReservedObject = dialogExtension;

            bool isCallStart = false;
            bool isCallEnd = false;

            if (dialog.DialogEvent != null && dialog.State != null)
            {
                if (dialog.DialogEvent.Equals("POST") && dialog.State.Equals("ALERTING")) // Call Ringing Event
                {
                    _finAgent.FireLogMessage("New Call started : Call From: " + dialog.FromAddress + " and Call To: " + dialog.ToAddress + " and dialog status is : " + dialog.State);
                    isCallStart = true;
                }
                else if (dialog.DialogEvent.Equals("DELETE") && dialog.State.Equals("ACTIVE")) // Call Terminated and call will be Transferred
                {
                    _finAgent.FireLogMessage("Call Termination event as you released the call : Call From: " + dialog.FromAddress + " and Call To: " + dialog.ToAddress);
                    isCallEnd = true;
                }
                else if (dialog.DialogEvent.Equals("DELETE") && dialog.State.Equals("DROPPED")) // Call Terminated and caller hangup
                {
                    if (!dialog.MediaProperties.DNIS.Equals("8999") && !dialog.MediaProperties.DNIS.Equals("8999"))
                    {
                        _finAgent.FireLogMessage("Call Termination event and caller dropped the call : Call From: " + dialog.FromAddress + " and Call To: " + dialog.ToAddress);
                        isCallEnd = true;
                    }
                }
                else if (dialog.DialogEvent.Equals("DELETE")) // Call Terminated and caller hangup for any other reason
                {
                    if (!dialog.MediaProperties.DNIS.Equals("8999") && !dialog.MediaProperties.DNIS.Equals("8999"))
                    {
                        _finAgent.FireLogMessage("Call Termination event  : Call From: " + dialog.FromAddress + " and Call To: " + dialog.ToAddress + ", Call State is: " + dialog.State);
                        isCallEnd = true;
                    }
                }
                else if (dialog.DialogEvent.Equals("RunningCall") && !dialog.State.Equals("DROPPED") && !dialog.State.Equals("FAILED")) // We found running call
                {
                    if (!dialog.MediaProperties.DNIS.Equals("8999") && !dialog.MediaProperties.DNIS.Equals("8999"))
                    {
                        _finAgent.FireLogMessage("Call running event and call still active : Call From: " + dialog.FromAddress + " and Call To: " + dialog.ToAddress);
                        isCallStart = true;
                    }
                }
            }
            else
            {
                foreach (Dialog.Participant participant in dialog.Participants)
                {
                    if (participant.MediaAddress.Equals(_finAgent.AgentInformation.Extension)) //Checking my status
                    {
                        if (participant.State.Equals("DROPPED")) // call is not active , and this is call terminate event
                        {
                            _finAgent.FireLogMessage("We received message event without event discription. Seems system just started while call was active, we will terminate the call without firing end event");
                        }
                        else if (participant.State.Equals("INITIATING"))
                        {
                            _finAgent.FireLogMessage("We received message event without event discription. Seems system just started while call is active, your status is: " + participant.State);
                        }
                    }
                }
            }

            if (isCallStart)
            {
                try
                {
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        if ((_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).CallAUDStart)
                        {
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new System.Action(() =>
                            {
                                new AUDOption(_finAgent, dialog, (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).CtiWeb).StartCallAUDRecord();
                            }));
                        }
                        if ((_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).LogCallStart)
                        {
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new System.Action(() =>
                            {
                                new ExtraServlets(_finAgent, dialog, (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).CtiWeb).OnCallStartEvent();
                            }));
                        }

                        if ((_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).FireCallStartEvent)
                            FireStartCallEvent(dialog, true);
                    }).Start();
                }
                catch (Exception e)
                {
                    _finAgent.FireErrorMessage("CXConnect: Error in call initiation" + e.Message);
                    _finAgent.FireDebugLogMessage("CXConnect: Error in call initiation" + e.Message);
                    return;
                }
            }

            if (isCallEnd)
            {
                try
                {
                    new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        if ((_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).CallAUDEnd)
                        {
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new System.Action(() =>
                            {
                                new AUDOption(_finAgent, dialog, (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).CtiWeb).EndCallAUDRecord();
                            }));
                        }
                        if ((_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).LogCallEnd)
                        {
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new System.Action(() =>
                            {
                                new ExtraServlets(_finAgent, dialog, (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).CtiWeb).OnCallEndEvent();
                            }));
                        }
                        if ((_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).FireCallEndEvent)
                            FireEndCallEvent(dialog);
                    }).Start();
                }
                catch (Exception e)
                {
                    _finAgent.FireErrorMessage("CXConnect: Error in call termination" + e.Message);
                    _finAgent.FireDebugLogMessage("CXConnect: Error in call termination" + e.Message);
                    return;
                }
            }

        }

        public void FireQueueEvent(Queue queue)
        {
        }
        private DialogExtension BuildDialogExtension(string dialogID)
        {
             DialogExtension dialogExtension = new DialogExtension();

            if (dialogExtension.IVROptions == null)
                dialogExtension.IVROptions = new ARB.IVROptions(_finAgent, dialogID);
            else
                dialogExtension.IVROptions.UpdateDialogData(dialogID);

            dialogExtension.LoadTransferOptions();
            return dialogExtension;
        }
        private void LoadConfiguration()
        {
            _finAgent.FireLoadingMessage("Loading Configuration ..");
            _finAgent.FireLogMessage("Loading configuration");
            try
            {
                //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                var fileName = System.IO.Path.Combine("CXConnectConfig.properties");

                //Load Configuration File
                CXProperties properties = new CXProperties(fileName);
                if (properties.fileExist)
                {
                    if (properties.get("FinesseA") != null)
                        _finAgent.AgentInformation.DomainA = properties.get("FinesseA");//, "hofinesseaprd.alrajhi.bank"
                    if (properties.get("FinesseB") != null)
                        _finAgent.AgentInformation.DomainB = properties.get("FinesseB");//, "hofinessebprd.alrajhi.bank"
                    if (properties.get("CTIWebServer") != null)
                        (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).CtiWeb = properties.get("CTIWebServer");//, "http://10.252.66.74:8088/CTIWEBIE11/"
                    if (properties.get("LogCallStart") != null)
                        (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).LogCallStart = bool.Parse(properties.get("LogCallStart"));
                    if (properties.get("LogCallEnd") != null)
                        (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).LogCallEnd = bool.Parse(properties.get("LogCallEnd"));
                    if (properties.get("LogCallTransfer") != null)
                        (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).LogCallTransfer = bool.Parse(properties.get("LogCallTransfer"));
                    if (properties.get("CallAUDStart") != null)
                        (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).CallAUDStart = bool.Parse(properties.get("CallAUDStart"));
                    if (properties.get("CallAUDEnd") != null)
                        (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).CallAUDEnd = bool.Parse(properties.get("CallAUDEnd"));
                    if (properties.get("FireCallStartEvent") != null)
                        (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).FireCallStartEvent = bool.Parse(properties.get("FireCallStartEvent"));
                    if (properties.get("FireCallEndEvent") != null)
                        (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).FireCallEndEvent = bool.Parse(properties.get("FireCallEndEvent"));
                    if (properties.get("Trace") != null)
                        _finAgent.TraceStatus = bool.Parse(properties.get("Trace", "false"));
                    if (properties.get("Ssl") != null)
                        _finAgent.AgentInformation.SSL = bool.Parse(properties.get("Ssl"));
                    if (properties.get("ShowServerDetails") != null)
                        (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).ConfigureDetails = bool.Parse(properties.get("ShowServerDetails"));
                }
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage(ex.ToString());
            }
            _finAgent.FireLoadingMessage("Configuration Loaded..");
            _finAgent.FireLogMessage("Configuration Loaded ...");

        }
        public void FireStartCallEvent(Dialog dialog, bool activityLog)
        {
            //    try
            //    {
            //        List<LookupRequestItem> itemsList = new List<LookupRequestItem>();
            //        itemsList.Add(new LookupRequestItem("AgentID", _finAgent.AgentInformation.AgentID));
            //        itemsList.Add(new LookupRequestItem("AgentExt", _finAgent.AgentInformation.Extension));
            //        itemsList.Add(new LookupRequestItem("FromAddress", dialog.FromAddress));
            //        itemsList.Add(new LookupRequestItem("ToAddress", dialog.ToAddress));
            //        itemsList.Add(new LookupRequestItem("DialNumber", dialog.MediaProperties.DialedNumber));
            //        itemsList.Add(new LookupRequestItem("TrackLog", activityLog.ToString()));
            //        itemsList.Add(new LookupRequestItem("CallStart", "true"));


            //        //CIC Exist
            //        if (((CallVariableClass)dialog.MediaProperties.CallVariables[3]).Value != null)
            //        {
            //            //check the CIC length is 16 digits
            //            string CICFormatted = ((CallVariableClass)dialog.MediaProperties.CallVariables[3]).Value.Trim();

            //            while (CICFormatted.Length < 16)
            //                CICFormatted = "0" + CICFormatted;
            //            itemsList.Add(new LookupRequestItem("CallVar4", CICFormatted));
            //        }
            //        else
            //            itemsList.Add(new LookupRequestItem("CallVar4", "NA"));

            //        Dispatcher.Invoke(() =>
            //        {
            //            CtiLookupRequest data = new CtiLookupRequest(Guid.NewGuid(), base.ApplicationName, "phonecall", dialog.FromAddress, dialog.MediaProperties.DNIS);
            //            data.Items.AddRange(itemsList);
            //            FireRequestAction(new RequestActionEventArgs("*", CtiLookupRequest.CTILOOKUPACTIONNAME, GeneralFunctions.Serialize<CtiLookupRequest>(data)));
            //        });

            //    }
            //    catch (Exception e)
            //    {
            //        _finAgent.FireErrorMessage("CXConnect: Error during firing CRM event start" + e.Message);
            //        _finAgent.FireLogMessage("CXConnect: Error during firing CRM event start" + e.Message);
            //    }
        }
        public void FireEndCallEvent(Dialog dialog)
        {
            //    try
            //    {
            //        List<LookupRequestItem> itemsList = new List<LookupRequestItem>();
            //        itemsList.Add(new LookupRequestItem("AgentID", _finAgent.AgentInformation.AgentID));
            //        itemsList.Add(new LookupRequestItem("AgentExt", _finAgent.AgentInformation.Extension));
            //        itemsList.Add(new LookupRequestItem("FromAddress", dialog.FromAddress));
            //        itemsList.Add(new LookupRequestItem("ToAddress", dialog.ToAddress));
            //        itemsList.Add(new LookupRequestItem("DialNumber", dialog.MediaProperties.DialedNumber));
            //        itemsList.Add(new LookupRequestItem("TrackLog", "false"));
            //        itemsList.Add(new LookupRequestItem("CallStart", "false"));

            //        //CIC Exist
            //        if (((CallVariableClass)dialog.MediaProperties.CallVariables[3]).Value != null)
            //        {
            //            //check the CIC length is 16 digits
            //            string CICFormatted = ((CallVariableClass)dialog.MediaProperties.CallVariables[3]).Value;

            //            while (CICFormatted.Length < 16)
            //                CICFormatted = "0" + CICFormatted;
            //            itemsList.Add(new LookupRequestItem("CallVar4", CICFormatted));
            //        }
            //        else
            //            itemsList.Add(new LookupRequestItem("CallVar4", "NA"));

            //        Dispatcher.Invoke(() =>
            //        {
            //            CtiLookupRequest data = new CtiLookupRequest(Guid.NewGuid(), base.ApplicationName, "phonecall", dialog.FromAddress, dialog.MediaProperties.DNIS);
            //            data.Items.AddRange(itemsList);
            //            FireRequestAction(new RequestActionEventArgs("*", CtiLookupRequest.CTILOOKUPACTIONNAME, GeneralFunctions.Serialize<CtiLookupRequest>(data)));
            //        });

            //    }
            //    catch (Exception e)
            //    {
            //        if (TraceStatus.Equals("true"))
            //        {
            //            _finAgent.FireErrorMessage("CXConnect: Error during firing CRM event end" + e.Message);
            //            _finAgent.FireLogMessage("CXConnect: Error during firing CRM event end" + e.Message);
            //        }

            //        //Nothing to do in case of error. It will not popup. that is it.
            //    }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_finAgent.AgentInformation.Status == null || _finAgent.AgentInformation.Status.Equals("LOGOUT"))
                Environment.Exit(0);
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure, you want to close the application.. Please note that your status does not allow this, System will logout.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (_finAgent.SignOut("LOGOUT", null))
                        Environment.Exit(0);
                    else
                    {
                        MessageBox.Show("We can not sign you out automatically.. Your request logout already sent to Finesse and it will happen soon. please wait", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        _finAgent.FireLogMessage("We can not sign you out automatically.. Your request logout already sent to Finesse and it will happen soon. please wait");
                        Environment.Exit(0);
                    }
                }
                else
                    e.Cancel = true;

            }
        }
    }
}
