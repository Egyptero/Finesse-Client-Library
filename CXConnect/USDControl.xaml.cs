// =====================================================================
//  This file is part of the Microsoft Dynamics CRM SDK code samples.
//
//  Copyright (C) Microsoft Corporation.  All rights reserved.
//
//  This source code is intended only as a supplement to Microsoft
//  Development Tools and/or on-line documentation.  See these other
//  materials for detailed information regarding Microsoft code samples.
//
//  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//  PARTICULAR PURPOSE.
// =====================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using CXConnect.ARB;
using CXConnect.Model;
using CXConnect.Tools;
using FinesseClient;
using FinesseClient.Common;
using FinesseClient.Model;
using FinesseClient.View;
using Microsoft.Crm.UnifiedServiceDesk.CommonUtility;
using Microsoft.Crm.UnifiedServiceDesk.Dynamics;
using Microsoft.Crm.UnifiedServiceDesk.Dynamics.Utilities;
using Microsoft.Uii.Common;
using Microsoft.Uii.Csr;
using Microsoft.Uii.Desktop.Cti.Core;
using Microsoft.Uii.Desktop.SessionManager;
using static FinesseClient.Model.Dialog.MediaPropertiesClass;

namespace CXConnect
{
    /// <summary>
    /// Interaction logic for USDControl.xaml
    /// This is a base control for building Unified Service Desk Aware add-ins
    /// See USD API documentation for full API Information available via this control.
    /// </summary>
    public partial class USDControl : DynamicsBaseHostedControl,FinView
    {
        #region Vars
        /// <summary>
        /// Log writer for USD 
        /// </summary>
        public TraceLogger LogWriter = null;
        FinAgent _finAgent = new FinAgent();
        private enum View { Main, Loading, Login, Queue, About, Telephony, MakecallDialPad, Home, DebugPage, RecentPage, Apps };
        private enum CallAction { Hold, Resume, TransferToSurvey, SSTransfer, TransferToIVR, Answer, Release, MakeCall, Consult, Transfer, Conference, DTMF };
        private Timer enableStatusTimer;
        private TimerCallback enableStatusTimerCallback;
        #endregion

        /// <summary>
        /// UII Constructor 
        /// </summary>
        /// <param name="appID">ID of the application</param>
        /// <param name="appName">Name of the application</param>
        /// <param name="initString">Initializing XML for the application</param>
        public USDControl(Guid appID, string appName, string initString)
            : base(appID, appName, initString)
        {
            _finAgent.FinView = this;
            _finAgent.AgentInformation.XmppKeepAliveInterval = 30;
            _finAgent.FinesseVersion = "UCCE_11.6";
            AgentInformationExtension agentInformationExtension = new AgentInformationExtension();
            _finAgent.AgentInformation.ReservedObject = agentInformationExtension;

            try
            {
            LoadConfiguration();
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error during configuration loading, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
            }
            enableStatusTimerCallback = EnableStatus;
            enableStatusTimer = new Timer(enableStatusTimerCallback);
            DataContext = _finAgent;
            InitializeComponent();
            SwitchView(View.Login);
            // This will create a log writer with the default provider for Unified Service desk
            LogWriter = new TraceLogger();


            #region Enhanced LogProvider Info
            // This will create a log writer with the same name as your hosted control. 
            // LogWriter = new TraceLogger(traceSourceName:"MyTraceSource");

            // If you utilize this feature,  you would need to add a section to the system.diagnostics settings area of the UnifiedServiceDesk.exe.config
            //<source name="MyTraceSource" switchName="MyTraceSwitchName" switchType="System.Diagnostics.SourceSwitch">
            //    <listeners>
            //        <add name="console" type="System.Diagnostics.DefaultTraceListener"/>
            //        <add name="fileListener"/>
            //        <add name="USDDebugListener" />
            //        <remove name="Default"/>
            //    </listeners>
            //</source>

            // and then in the switches area : 
            //<add name="MyTraceSwitchName" value="Verbose"/>

            #endregion

        }
        /// <summary>
        /// Raised when the Desktop Ready event is fired. 
        /// </summary>
        protected override void DesktopReady()
        {
            // this will populate any toolbars assigned to this control in config. 
            //PopulateToolbars(ProgrammableToolbarTray);
            base.DesktopReady();
        }

        /// <summary>
        /// Raised when an action is sent to this control
        /// </summary>
        /// <param name="args">args for the action</param>
        protected override void DoAction(Microsoft.Uii.Csr.RequestActionEventArgs args)
        {
            // Log process.
            LogWriter.Log(string.Format(CultureInfo.CurrentCulture, "{0} -- DoAction called for action: {1}", this.ApplicationName, args.Action), System.Diagnostics.TraceEventType.Information);

            #region Example process action
            //// Process Actions. 
            //if (args.Action.Equals("your action name", StringComparison.OrdinalIgnoreCase))
            //{
            //    // Do some work

            //    // Access CRM and fetch a Record
            //    Microsoft.Xrm.Sdk.Messages.RetrieveRequest req = new Microsoft.Xrm.Sdk.Messages.RetrieveRequest(); 
            //    req.Target = new Microsoft.Xrm.Sdk.EntityReference( "account" , Guid.Parse("0EF05F4F-0D39-4219-A3F5-07A0A5E46FD5")); 
            //    req.ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet("accountid" , "name" );
            //    Microsoft.Xrm.Sdk.Messages.RetrieveResponse response = (Microsoft.Xrm.Sdk.Messages.RetrieveResponse)this._client.CrmInterface.ExecuteCrmOrganizationRequest(req, "Requesting Account"); 


            //    // Example of pulling some data out of the passed in data array
            //    List<KeyValuePair<string, string>> actionDataList = Utility.SplitLines(args.Data, CurrentContext, localSession);
            //    string valueIwant = Utility.GetAndRemoveParameter(actionDataList, "mykey"); // asume there is a myKey=<value> in the data. 



            //    // Example of pushing data to USD
            //    string global = Utility.GetAndRemoveParameter(actionDataList, "global"); // Assume there is a global=true/false in the data
            //    bool saveInGlobalSession = false;
            //    if (!String.IsNullOrEmpty(global))
            //        saveInGlobalSession = bool.Parse(global);

            //    Dictionary<string, CRMApplicationData> myDataToSet = new Dictionary<string, CRMApplicationData>();
            //    // add a string: 
            //    myDataToSet.Add("myNewKey", new CRMApplicationData() { name = "myNewKey", type = "string", value = "TEST" });

            //    // add a entity lookup:
            //    myDataToSet.Add("myNewKey", new CRMApplicationData() { name = "myAccount", type = "lookup", value = "account,0EF05F4F-0D39-4219-A3F5-07A0A5E46FD5,MyAccount" }); 

            //    if (saveInGlobalSession) 
            //    {
            //        // add context item to the global session
            //        ((DynamicsCustomerRecord)((AgentDesktopSession)localSessionManager.GlobalSession).Customer.DesktopCustomer).MergeReplacementParameter(this.ApplicationName, myDataToSet, true);
            //    }
            //    else
            //    {
            //        // Add context item to the current session. 
            //        ((DynamicsCustomerRecord)((AgentDesktopSession)localSessionManager.ActiveSession).Customer.DesktopCustomer).MergeReplacementParameter(this.ApplicationName, myDataToSet, true);
            //    }
            //}
            #endregion

            base.DoAction(args);
        }

        /// <summary>
        /// Raised when a context change occurs in USD
        /// </summary>
        /// <param name="context"></param>
        public override void NotifyContextChange(Microsoft.Uii.Csr.Context context)
        {
            base.NotifyContextChange(context);
        }

        #region User Code Area
        private void EnableStatus(Object obj)
        {
            try
            {
                if (!_finAgent.AgentInformation.SelectedVoiceStatus.Status.Equals("LOGOUT") && (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).AgentStatusDisabled)
                {
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() =>
                    {
                        FireErrorMessage("Application Failed to change status");

                        AgentInformationExtension agentInformationExtension = _finAgent.AgentInformation.ReservedObject as AgentInformationExtension;
                        agentInformationExtension.AgentStatusDisabled = false;
                        _finAgent.AgentInformation.ReservedObject = agentInformationExtension;
                        _finAgent.LoadAgentInformation();
                    }));
                }
                else if (_finAgent.AgentInformation.SelectedVoiceStatus.Status.Equals("LOGOUT") && (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).AgentStatusDisabled)
                {
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() =>
                    {
                        FireErrorMessage("Please wait for logout or restart app");
                    }));

                }
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Error in Enabling Agent Status:" + ex.Message + "\n" + ex.StackTrace);
            }

        }
        private void LoginSetting_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ToggleButton toggleButton = sender as ToggleButton;
                if (toggleButton == null)
                    return;
                if (toggleButton.IsChecked == true)
                {
                    LoginMainScreen.Visibility = Visibility.Hidden;
                    LoginSettingScreen.Visibility = Visibility.Visible;
                }
                else
                {
                    LoginMainScreen.Visibility = Visibility.Visible;
                    LoginSettingScreen.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error LoginSetting_Checked, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error during switch inside login page");
            }

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
                bool shouldDisable = true;
                if (StatusComboBox.SelectedItem == null)
                    return;
                if ((StatusComboBox.SelectedItem as VoiceStatus).Status.Equals(_finAgent.AgentInformation.Status))
                {
                    if ((StatusComboBox.SelectedItem as VoiceStatus).ReasonCode == null && _finAgent.AgentInformation.ReasonCode == null)
                        shouldDisable = false;
                    else if ((StatusComboBox.SelectedItem as VoiceStatus).ReasonCode != null && _finAgent.AgentInformation.ReasonCode != null && _finAgent.AgentInformation.ReasonCode.Label != null)
                        if ((StatusComboBox.SelectedItem as VoiceStatus).ReasonCode.Equals(_finAgent.AgentInformation.ReasonCode.Label))
                            shouldDisable = false;
                }
                if (shouldDisable && sender != null)
                {
                _finAgent.FireLoadingMessage("Change Status to: " + (StatusComboBox.SelectedItem as VoiceStatus).StatusLabel);
                    AgentInformationExtension agentInformationExtension = _finAgent.AgentInformation.ReservedObject as AgentInformationExtension;
                    agentInformationExtension.AgentStatusDisabled = true;
                    _finAgent.AgentInformation.ReservedObject = agentInformationExtension;
                    if (enableStatusTimer != null)
                        enableStatusTimer.Change(8000, Timeout.Infinite);
                }
                //Dispatcher.CurrentDispatcher
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => _finAgent.ChangeAgentVoiceStatus(StatusComboBox.SelectedItem as VoiceStatus)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Unable to change Agent Status with error:" + ex.StackTrace);
                _finAgent.FireErrorMessage("Can not change Agent Status");
            }
        }
        private void Keypad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.MakecallDialPad)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error Keypad_Click, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error Keypad_Click");
            }
        }
        private void Queues_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Queue)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error Queues_Click, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error Queues_Click");
            }
        }
        private void ActiveCalls_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Telephony)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error ActiveCalls_Click, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error ActiveCalls_Click");
            }
        }
        private void AppsPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Apps)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error AppsPage_Click, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error AppsPage_Click");
            }
        }
        private void HomePage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Home)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error HomePage_Click, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error HomePage_Click");
            }
        }
        private void AboutPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.About)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error AboutPage_Click, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error AboutPage_Click");
            }
        }
        private void Debug_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.DebugPage)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error Debug_Click, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error Debug_Click");
            }
        }
        private void Recents_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.RecentPage)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error Recents_Click, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error Recents_Click");
            }
        }

        private void AgentInformation_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem == null)
                return;
            if (menuItem.Name.Equals("QueueMenu"))
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Queue)));
            else if (menuItem.Name.Equals("AboutMenu"))
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.About)));
                else if (menuItem.Name.Equals("MakeCallMenu"))
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.MakecallDialPad)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error AgentInformation_MenuItem_Click, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error AgentInformation_MenuItem_Click");
            }

        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Telephony)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error CloseWindow_Click, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error CloseWindow_Click");
            }
        }
        private void Calls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Dialog dialog = sender as Dialog;
                if (dialog != null)
                    _finAgent.AgentInformation.SelectedDialog = dialog;
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error Calls_SelectionChanged, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error Calls_SelectionChanged");
            }

        }
        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => DoCallAction(CallAction.Answer, button.Tag as string, null)));
                else if (button.Name.Equals("Transfer"))
            {
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() =>
                {
                    _finAgent.FireLoadingMessage("Open dialpad");
                        ToggleDialPad(true, button.Tag as string, "SSTransfer");
                }));
            }
                else if (button.Name.Equals("Consult"))
            {
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() =>
                    {
                        _finAgent.FireLoadingMessage("Open dialpad");
                        ToggleDialPad(true, button.Tag as string, "Consult");
                    }));
                }
                else if (button.Name.Equals("Keypad"))
                {
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() =>
                    {
                        _finAgent.FireLoadingMessage("Open dialpad");
                        ToggleDialPad(true, button.Tag as string, "Keypad");
                    }));
                }
                else if (button.Name.Equals("IVRTransfer"))
                {
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() =>
                {
                    _finAgent.FireLoadingMessage("Open Send to IVR");
                    ToggleUpdateRoutingData(true, button.Tag as string);
                }));

            }
                else if (button.Name.Equals("CallData"))
                {
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() =>
                    {
                        _finAgent.FireLoadingMessage("Open Call Data");
                        TogglePhoneData(true, button.Tag as string);
                    }));

                }
                else if (button.Name.Equals("Conference"))
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => DoCallAction(CallAction.Conference, button.Tag as string, CallActionNumberToDial.Text)));
                else if (button.Name.Equals("CTransfer"))
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => DoCallAction(CallAction.Transfer, button.Tag as string, null)));
                else if (button.Name.Equals("SurveyTransfer"))
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => DoCallAction(CallAction.TransferToSurvey, button.Tag as string, null)));
            else if (button.Name.Equals("Hold"))
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => DoCallAction(CallAction.Hold, button.Tag as string, null)));
            else if (button.Name.Equals("Resume"))
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => DoCallAction(CallAction.Resume, button.Tag as string, null)));
            else if (button.Name.Equals("Release"))
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => DoCallAction(CallAction.Release, button.Tag as string, null)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error CallButtonClick, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error CallButtonClick");
            }
        }
        private void CallDataHide_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TogglePhoneData(false, null);
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error CallDataHide_Click, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error CallDataHide_Click");
            }
        }
        private void UpdateRoutingDataButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
            Button button = sender as Button;
            Dialog dialog = _finAgent.FindDialog(button.Tag as string);

                if (button.Name.Equals("IVRRoute"))
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => DoCallAction(CallAction.TransferToIVR, button.Tag as string, null)));
                else if (button.Name.Equals("IVRHide"))
                    ToggleUpdateRoutingData(false, null);
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error UpdateRoutingDataButtonClick, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error UpdateRoutingDataButtonClick");
            }
        }
        private void DialPadButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
            Button button = sender as Button;

                Dialog dialog = CallDialPadForm.DataContext as Dialog;
                string action = CallDialPadForm.Tag as string;
                if (CallActionNumberToDial.Text == null)
                    CallActionNumberToDial.Text = "";
                if (button.Name.Equals("CallActionOne"))
                {
                    CallActionNumberToDial.Text += "1";
                    if (action.Equals("Keypad"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => _finAgent.KeypadSendDTMF(dialog.ID, "1")));
                }
                else if (button.Name.Equals("CallActionTwo"))
                {
                    CallActionNumberToDial.Text += "2";
                    if (action.Equals("Keypad"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => _finAgent.KeypadSendDTMF(dialog.ID, "2")));
                }
                else if (button.Name.Equals("CallActionThree"))
                {
                    CallActionNumberToDial.Text += "3";
                    if (action.Equals("Keypad"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => _finAgent.KeypadSendDTMF(dialog.ID, "3")));
                }
                else if (button.Name.Equals("CallActionFour"))
                {
                    CallActionNumberToDial.Text += "4";
                    if (action.Equals("Keypad"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => _finAgent.KeypadSendDTMF(dialog.ID, "4")));
                }
                else if (button.Name.Equals("CallActionFive"))
                {
                    CallActionNumberToDial.Text += "5";
                    if (action.Equals("Keypad"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => _finAgent.KeypadSendDTMF(dialog.ID, "5")));
                }
                else if (button.Name.Equals("CallActionSix"))
                {
                    CallActionNumberToDial.Text += "6";
                    if (action.Equals("Keypad"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => _finAgent.KeypadSendDTMF(dialog.ID, "6")));
                }
                else if (button.Name.Equals("CallActionSeven"))
                {
                    CallActionNumberToDial.Text += "7";
                    if (action.Equals("Keypad"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => _finAgent.KeypadSendDTMF(dialog.ID, "7")));
                }
                else if (button.Name.Equals("CallActionEight"))
                {
                    CallActionNumberToDial.Text += "8";
                    if (action.Equals("Keypad"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => _finAgent.KeypadSendDTMF(dialog.ID, "8")));
                }
                else if (button.Name.Equals("CallActionNine"))
                {
                    CallActionNumberToDial.Text += "9";
                    if (action.Equals("Keypad"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => _finAgent.KeypadSendDTMF(dialog.ID, "9")));
                }
                else if (button.Name.Equals("CallActionStar"))
                {
                    CallActionNumberToDial.Text += "*";
                    if (action.Equals("Keypad"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => _finAgent.KeypadSendDTMF(dialog.ID, "*")));
                }
                else if (button.Name.Equals("CallActionZero"))
                {
                    CallActionNumberToDial.Text += "0";
                    if (action.Equals("Keypad"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => _finAgent.KeypadSendDTMF(dialog.ID, "0")));
                }
                else if (button.Name.Equals("CallActionHash"))
                {
                    CallActionNumberToDial.Text += "#";
                    if (action.Equals("Keypad"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => _finAgent.KeypadSendDTMF(dialog.ID, "#")));
                }
                else if (button.Name.Equals("CallActionCall"))
                {
                    if (CallActionNumberToDial.Text == null || CallActionNumberToDial.Text.Trim().Length == 0)
                        return;

                    if (action.Equals("SSTransfer"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => DoCallAction(CallAction.SSTransfer, dialog.ID, CallActionNumberToDial.Text)));
                    else if (action.Equals("Consult"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => DoCallAction(CallAction.Consult, dialog.ID, CallActionNumberToDial.Text)));
                    else if (action.Equals("Keypad"))
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => DoCallAction(CallAction.DTMF, dialog.ID, CallActionNumberToDial.Text)));
                    //if (CallActionNumberToDial.Text != null)
                    //    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (Action)(() => DoCallAction(CallAction.TransferToExt, button.Tag as string)));
                }
                else if (button.Name.Equals("CallActionHide"))
                    ToggleDialPad(false, null, null);

                try
                {
                    CallActionNumberToDial.CaretIndex = CallActionNumberToDial.Text.Length;
                }
                catch (Exception) { }
                CallActionNumberToDial.Focus();
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error DialPadButtonClick, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error DialPadButtonClick");
            }
        }
        private void MakeDialPadButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                AgentInformationExtension agentInformationExtension = _finAgent.AgentInformation.ReservedObject as AgentInformationExtension;

                if (agentInformationExtension.DialNumber == null)
                    agentInformationExtension.DialNumber = "";
            if (button.Name.Equals("one"))
                    agentInformationExtension.DialNumber += "1";
            else if (button.Name.Equals("two"))
                    agentInformationExtension.DialNumber += "2";
            else if (button.Name.Equals("three"))
                    agentInformationExtension.DialNumber += "3";
            else if (button.Name.Equals("four"))
                    agentInformationExtension.DialNumber += "4";
            else if (button.Name.Equals("five"))
                    agentInformationExtension.DialNumber += "5";
            else if (button.Name.Equals("six"))
                    agentInformationExtension.DialNumber += "6";
            else if (button.Name.Equals("seven"))
                    agentInformationExtension.DialNumber += "7";
            else if (button.Name.Equals("eight"))
                    agentInformationExtension.DialNumber += "8";
            else if (button.Name.Equals("nine"))
                    agentInformationExtension.DialNumber += "9";
            else if (button.Name.Equals("star"))
                    agentInformationExtension.DialNumber += "*";
            else if (button.Name.Equals("zero"))
                    agentInformationExtension.DialNumber += "0";
            else if (button.Name.Equals("hash"))
                    agentInformationExtension.DialNumber += "#";
            else if (button.Name.Equals("call"))
            {
                    if (agentInformationExtension.DialNumber != null)
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => DoCallAction(CallAction.MakeCall, null, agentInformationExtension.DialNumber)));
                    SwitchView(View.Telephony);
            }

                try
                {
                    NumberToDial.CaretIndex = NumberToDial.Text.Length;
                }
                catch (Exception) { }
                NumberToDial.Focus();

                _finAgent.AgentInformation.ReservedObject = agentInformationExtension;
        }
            catch (Exception ex)
        {
                _finAgent.FireLogMessage("Application Error MakeDialPadButtonClick, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error MakeDialPadButtonClick");
            }
        }
        private void ToggleDialPad(bool status, string dialogID, string action)
        {
            try
            {
                if (status)
        {
            if (dialogID == null)
                return;
            Dialog dialog = _finAgent.FindDialog(dialogID);
                    CallDialPadForm.DataContext = dialog;
                    CallDialPadForm.Tag = action;
                    try
                    {
                        CallActionNumberToDial.CaretIndex = CallActionNumberToDial.Text.Length;
                    }
                    catch (Exception) { }
                    if (CallActionsForm.SelectedIndex != 1)
                        CallActionsForm.SelectedIndex = 1;
                    CallActionNumberToDial.Focus();

                    if (action.Equals("SSTransfer"))
                        CallActionDialPadTitle.Text = "Single Step Transfer";
                    else if (action.Equals("Consult"))
                        CallActionDialPadTitle.Text = "Consult Transfer";
                    else if (action.Equals("Keypad"))
                        CallActionDialPadTitle.Text = "Send DTMF";

                }
                else
                {
                    CallDialPadForm.DataContext = null;
                    CallDialPadForm.Tag = null;
                    if (CallActionsForm.SelectedIndex != 0)
                        CallActionsForm.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error ToggleDialPad, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error ToggleDialPad");
            }
        }
        private void ToggleUpdateRoutingData(bool status, string dialogID)
        {
            try
            {
                if (status)
                {
            if (dialogID == null)
                return;
            Dialog dialog = _finAgent.FindDialog(dialogID);
                    CallTransferIVRForm.DataContext = dialog;
                    if (CallActionsForm.SelectedIndex != 2)
                        CallActionsForm.SelectedIndex = 2;
                }
                else
                {
                    CallTransferIVRForm.DataContext = null;
                    if (CallActionsForm.SelectedIndex != 0)
                        CallActionsForm.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error ToggleUpdateRoutingData, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error ToggleUpdateRoutingData");
            }
        }
        private void TogglePhoneData(bool status, string dialogID)
        {
            try
            {
            if (status)
                {
                    if (dialogID == null)
                        return;
                    Dialog dialog = _finAgent.FindDialog(dialogID);
                    CallAttachedDataForm.DataContext = dialog;
                    if (CallActionsForm.SelectedIndex != 3)
                        CallActionsForm.SelectedIndex = 3;
                }
                else
                {
                    CallAttachedDataForm.DataContext = null;
                    if (CallActionsForm.SelectedIndex != 0)
                        CallActionsForm.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error TogglePhoneData, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error TogglePhoneData");
            }
        }
        private void PerformLogin()
        {
            try
            {
            _finAgent.AgentInformation.Password = AgentPassword.Password;
            SwitchView(View.Loading);
            if (!_finAgent.SignIn())
                SwitchView(View.Login);
        }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error PerformLogin, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error PerformLogin");
            }
        }
        private void DoCallAction(CallAction action, string dialogID, Object parameter)
        {
            try
        {
                if (dialogID == null && action != CallAction.MakeCall)
                return;
                Dialog dialog = null;
                DialogExtension dialogExtension = null;

                if (dialogID != null)
                {
                    dialog = _finAgent.FindDialog(dialogID);
                    dialogExtension = dialog.ReservedObject as DialogExtension;

                    ToggleUpdateRoutingData(false, null);
                    ToggleDialPad(false, null, null);
            dialogExtension.Message = "Trying to " + action;
            dialogExtension.Disabled = true;
            dialog.ReservedObject = dialogExtension;
                }
                _finAgent.FireLoadingMessage("Call Action: " + action);

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
                                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (System.Action)(() => _finAgent.AgentInformation.Dialogs.Remove(dialog)));
                    break;
                case CallAction.Resume:
                    if (!_finAgent.ResumeCall(dialogID))
                        dialogExtension.Disabled = false;
                    break;
                    case CallAction.Conference:
                        if (!_finAgent.ConferenceCall(dialogID, parameter as string))
                            dialogExtension.Disabled = false;
                        break;
                    case CallAction.Transfer:
                        if (!_finAgent.TransferCall(dialogID))
                            dialogExtension.Disabled = false;
                        break;
                    case CallAction.SSTransfer:
                        if (!_finAgent.SSTransferCall(dialogID, parameter as string))
                            dialogExtension.Disabled = false;
                        break;
                    case CallAction.Consult:
                        if (!_finAgent.ConsultCall(dialogID, parameter as string))
                            dialogExtension.Disabled = false;
                        break;
                    case CallAction.DTMF:
                        _finAgent.KeypadSendDTMF(dialogID, parameter as string);
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
                    case CallAction.MakeCall:
                        _finAgent.MakeCall(parameter as string);
                        break;
            }
                if (dialog != null && dialogExtension != null)
            dialog.ReservedObject = dialogExtension;
        }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error Do Call Action, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
            }
        }
        private void SwitchView(View view)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() =>
                {

            switch (view)
            {
                case View.Loading:
                            LoadingPage.Visibility = Visibility.Visible;
                            MainPage.Visibility = Visibility.Hidden;
                            LoginPage.Visibility = Visibility.Hidden;
                    break;
                case View.Login:
                            LoadingPage.Visibility = Visibility.Hidden;
                            MainPage.Visibility = Visibility.Hidden;
                            LoginPage.Visibility = Visibility.Visible;
                    break;
                case View.Main:
                            LoadingPage.Visibility = Visibility.Hidden;
                            MainPage.Visibility = Visibility.Visible;
                            LoginPage.Visibility = Visibility.Hidden;
                            SwitchOffMainViews();
                            HomeScreen.Visibility = Visibility.Visible;
                            HomeShortCut.IsChecked = true;
                            HomeScreenTransitioner.SelectedIndex = 0;
                    break;
                case View.Queue:
                            SwitchOffMainViews();
                            QueuesScreen.Visibility = Visibility.Visible;
                    break;
                case View.About:
                            SwitchOffMainViews();
                            AboutScreen.Visibility = Visibility.Visible;
                    break;
                case View.Telephony:
                            ToggleDialPad(false, null, null);
                            ToggleUpdateRoutingData(false, null);
                            TogglePhoneData(false, null);
                            SwitchOffMainViews();
                            PhoneScreen.Visibility = Visibility.Visible;
                            CallsShortCut.IsChecked = true;
                            break;
                        case View.MakecallDialPad:
                            NumberToDial.ScrollToEnd();
                            NumberToDial.Focus();
                            SwitchOffMainViews();
                            DialPadScreen.Visibility = Visibility.Visible;
                            KeypadShortCut.IsChecked = true;
                            break;
                        case View.Home:
                            SwitchOffMainViews();
                            HomeScreen.Visibility = Visibility.Visible;
                            HomeShortCut.IsChecked = true;
                            HomeScreenTransitioner.SelectedIndex = 0;
                            break;
                        case View.Apps:
                            SwitchOffMainViews();
                            HomeScreen.Visibility = Visibility.Visible;
                            HomeScreenTransitioner.SelectedIndex = 1;
                            AppsShortCut.IsChecked = true;
                            break;
                        case View.DebugPage:
                            SwitchOffMainViews();
                            DebugScreen.Visibility = Visibility.Visible;
                    break;
                        case View.RecentPage:
                            SwitchOffMainViews();
                            RecentScreen.Visibility = Visibility.Visible;
                            RecentsShortCut.IsChecked = true;
                            break;
                    }
                }));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error SwitchView, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application error SwitchView");
            }
        }
        public void FireLoadingMessage(string msg)
        {
        }

        public void FireLoadLoginScreen()
        {
            try
            {
            _finAgent.FireLoadingMessage("Please login..");
                AgentInformationExtension agentInformationExtension = _finAgent.AgentInformation.ReservedObject as AgentInformationExtension;
                if (agentInformationExtension == null)
                    agentInformationExtension = new AgentInformationExtension();
                agentInformationExtension.AgentStatusDisabled = false;
                _finAgent.AgentInformation.ReservedObject = agentInformationExtension;

                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Login)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error Loading login screen, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application Error in loading login screen");
            }
        }

        public void FireErrorMessage(string msg)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() =>
                {
            ErrorBar.MessageQueue.Enqueue(msg);
                }));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error during Fire Error Message, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application Error in firing error message");
            }
        }

        public void FireLogMessage(string msg)
        {
            //You can save your custom log here;
        }

        public Screen GetLocation()
        {
            return Screen.MainGrid;
        }

        public void SetContext(IModel model, FinView finView)
        {
            //Ignor it , it is related to FDE implementation
        }

        public void FireNewEvent()
        {
            try
            {
                if (_finAgent.AgentInformation.MessageEvent != null)
            {
                    if (_finAgent.AgentInformation.MessageEvent.MessageType != null)
                {
                        if (_finAgent.AgentInformation.MessageEvent.MessageType.Equals("error"))
                {
                            //FireErrorMessage("Code:" + _finAgent.AgentInformation.MessageEvent.ErrorCode + "," + _finAgent.AgentInformation.MessageEvent.ErrorMsg + " ,Type: " + _finAgent.AgentInformation.MessageEvent.ErrorType);
                    FireLogMessage("Finesse Server Error Message: " + _finAgent.AgentInformation.MessageEvent.ErrorMsg + " ,Finesse Server Error Type: " + _finAgent.AgentInformation.MessageEvent.ErrorType + " , Finesse Server Error Code: " + _finAgent.AgentInformation.MessageEvent.ErrorCode);
                            if (_finAgent.AgentInformation.Dialogs != null)
                            {
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
                        else if (_finAgent.AgentInformation.MessageEvent.MessageType.Equals("user"))
                        {
                            if (!_finAgent.AgentInformation.SelectedVoiceStatus.Status.Equals("LOGOUT"))
                            {
                                AgentInformationExtension agentInformationExtension = _finAgent.AgentInformation.ReservedObject as AgentInformationExtension;
                                agentInformationExtension.AgentStatusDisabled = false;
                                _finAgent.AgentInformation.ReservedObject = agentInformationExtension;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error during fire new event, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application Error during fire new event");
            }
        }

        public void FireReLoginEvent()
        {
            try
            {
            _finAgent.FireLoadingMessage("Agent logged in..");
                //Rest Parameters after login
                AgentInformationExtension agentInformationExtension = _finAgent.AgentInformation.ReservedObject as AgentInformationExtension;
                if (agentInformationExtension == null)
                    agentInformationExtension = new AgentInformationExtension();
                agentInformationExtension.AgentStatusDisabled = false;
                _finAgent.AgentInformation.ReservedObject = agentInformationExtension;

                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Main)));
                if (_finAgent.AgentInformation.Dialogs != null)
                {
                    if (_finAgent.AgentInformation.Dialogs.Count > 0)
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Telephony)));
                    else
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Home)));
                }
                else
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Home)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error during fire Relogin event, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application Error during fire Relogin event");
            }
        }

        public void FireDisconnectEvent()
        {
            try
            {
            _finAgent.FireLoadingMessage("System disconnected , will try to relogin.");

                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Loading)));
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error during fire disconnect event, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                _finAgent.FireErrorMessage("Application Error during fire disconnect event");
            }
        }

        public void FireCallEvent(Dialog dialog)
        {
            try
            {
                if (dialog == null)
                    dialog = _finAgent.AgentInformation.SelectedDialog;
                Dialog locatedDialog = null;
                if (dialog != null)
                    locatedDialog = _finAgent.FindDialog(dialog.ID);
            if (locatedDialog == null)
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Home)));
                else
                {
                    if (PhoneScreen.Visibility != Visibility.Visible)
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => SwitchView(View.Telephony)));

            DialogExtension dialogExtension = locatedDialog.ReservedObject as DialogExtension;
            if (dialogExtension == null)
                dialogExtension = BuildDialogExtension(dialog.ID);

            dialogExtension.Disabled = false;

            locatedDialog.ReservedObject = dialogExtension;
                }
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, (System.Action)(() => UpdateRecent(dialog)));

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
                        if (!dialog.MediaProperties.CallType.Equals("AGENT_INSIDE") && _finAgent.AgentInformation.MessageEvent.MessageType.Equals("call"))
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
                    }
                    catch (Exception e)
                    {
                        _finAgent.FireErrorMessage("Application: Error in call initiation in fire call event" + e.Message);
                        _finAgent.FireDebugLogMessage("Application: Error in call initiation in fire call event\n" + e.StackTrace);
                        return;
                    }
                }

                if (isCallEnd)
                {

                    try
                    {
                        if (!dialog.MediaProperties.CallType.Equals("AGENT_INSIDE") && _finAgent.AgentInformation.MessageEvent.MessageType.Equals("call"))
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
                    }
                    catch (Exception e)
                    {
                        _finAgent.FireErrorMessage("Application: Error in call termination during fire call event" + e.Message);
                        _finAgent.FireDebugLogMessage("Application: Error in call termination in fire call event\n" + e.StackTrace);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error during fire call event, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace + "\nMore\n" + ex.ToString());
                _finAgent.FireErrorMessage("Application Error during fire call event");
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
                    if (properties.get("SaveLog") != null)
                        _finAgent.SaveLog = bool.Parse(properties.get("SaveLog", "false"));
                    if (properties.get("Ssl") != null)
                        _finAgent.AgentInformation.SSL = bool.Parse(properties.get("Ssl"));
                    if (properties.get("HttpConnectionType") != null)
                        _finAgent.AgentInformation.HTTPConnectionType = properties.get("HttpConnectionType");
                    if (properties.get("XMPPConnectionType") != null)
                        _finAgent.AgentInformation.XMPPConnectionType = properties.get("XMPPConnectionType");
                    if (properties.get("HttpConnectionPort") != null)
                        _finAgent.AgentInformation.HTTPPort = properties.get("HttpConnectionPort");
                    if (properties.get("XMPPConnectionPort") != null)
                        _finAgent.AgentInformation.XMPPPort = properties.get("XMPPConnectionPort");
                    if (properties.get("ShowServerDetails") != null)
                        (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).ConfigureDetails = bool.Parse(properties.get("ShowServerDetails"));
                }
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage(ex.Message + "\n" + ex.StackTrace);
            }
            _finAgent.FireLoadingMessage("Configuration Loaded..");
            _finAgent.FireLogMessage("Configuration Loaded ...");

        }
        public void FireStartCallEvent(Dialog dialog, bool activityLog)
        {
            _finAgent.FireLogMessage("Start The Call Event of USD invoke for call ANI:"+dialog.FromAddress);
            try
            {
                List<LookupRequestItem> itemsList = new List<LookupRequestItem>();
                itemsList.Add(new LookupRequestItem("AgentID", _finAgent.AgentInformation.AgentID));
                itemsList.Add(new LookupRequestItem("AgentExt", _finAgent.AgentInformation.Extension));
                itemsList.Add(new LookupRequestItem("FromAddress", dialog.FromAddress));
                itemsList.Add(new LookupRequestItem("ToAddress", dialog.ToAddress));
                itemsList.Add(new LookupRequestItem("DialNumber", dialog.MediaProperties.DialedNumber));
                itemsList.Add(new LookupRequestItem("TrackLog", activityLog.ToString()));
                itemsList.Add(new LookupRequestItem("CallStart", "true"));


                //CIC Exist
                if (((CallVariableClass)dialog.MediaProperties.CallVariables[3]).Value != null)
                {
                    //check the CIC length is 16 digits
                    string CICFormatted = ((CallVariableClass)dialog.MediaProperties.CallVariables[3]).Value.Trim();

                    while (CICFormatted.Length < 16)
                        CICFormatted = "0" + CICFormatted;
                    itemsList.Add(new LookupRequestItem("CallVar4", CICFormatted));
                }
                else
                    itemsList.Add(new LookupRequestItem("CallVar4", "NA"));

                Dispatcher.Invoke(() =>
                {
                    CtiLookupRequest data = new CtiLookupRequest(Guid.NewGuid(), base.ApplicationName, "phonecall", dialog.FromAddress, dialog.MediaProperties.DNIS);
                    data.Items.AddRange(itemsList);
                    FireRequestAction(new RequestActionEventArgs("*", CtiLookupRequest.CTILOOKUPACTIONNAME, GeneralFunctions.Serialize<CtiLookupRequest>(data)));
                });

            }
            catch (Exception e)
            {
                _finAgent.FireErrorMessage("CXConnect: Error during firing CRM event start" + e.Message);
                _finAgent.FireLogMessage("CXConnect: Error during firing CRM event start" + e.Message);
            }
        }
        public void FireEndCallEvent(Dialog dialog)
        {
            _finAgent.FireLogMessage("End The Call Event of USD invoke for call ANI:" + dialog.FromAddress);
            try
            {
                List<LookupRequestItem> itemsList = new List<LookupRequestItem>();
                itemsList.Add(new LookupRequestItem("AgentID", _finAgent.AgentInformation.AgentID));
                itemsList.Add(new LookupRequestItem("AgentExt", _finAgent.AgentInformation.Extension));
                itemsList.Add(new LookupRequestItem("FromAddress", dialog.FromAddress));
                itemsList.Add(new LookupRequestItem("ToAddress", dialog.ToAddress));
                itemsList.Add(new LookupRequestItem("DialNumber", dialog.MediaProperties.DialedNumber));
                itemsList.Add(new LookupRequestItem("TrackLog", "false"));
                itemsList.Add(new LookupRequestItem("CallStart", "false"));

                //CIC Exist
                if (((CallVariableClass)dialog.MediaProperties.CallVariables[3]).Value != null)
                {
                    //check the CIC length is 16 digits
                    string CICFormatted = ((CallVariableClass)dialog.MediaProperties.CallVariables[3]).Value;

                    while (CICFormatted.Length < 16)
                        CICFormatted = "0" + CICFormatted;
                    itemsList.Add(new LookupRequestItem("CallVar4", CICFormatted));
                }
                else
                    itemsList.Add(new LookupRequestItem("CallVar4", "NA"));

                Dispatcher.Invoke(() =>
                {
                    CtiLookupRequest data = new CtiLookupRequest(Guid.NewGuid(), base.ApplicationName, "phonecall", dialog.FromAddress, dialog.MediaProperties.DNIS);
                    data.Items.AddRange(itemsList);
                    FireRequestAction(new RequestActionEventArgs("*", CtiLookupRequest.CTILOOKUPACTIONNAME, GeneralFunctions.Serialize<CtiLookupRequest>(data)));
                });

            }
            catch (Exception e)
            {
                _finAgent.FireErrorMessage("CXConnect: Error during firing CRM event end" + e.Message);
                _finAgent.FireLogMessage("CXConnect: Error during firing CRM event end" + e.Message);
                //Nothing to do in case of error. It will not popup. that is it.
            }
        }
        private void DynamicsBaseHostedControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_finAgent.AgentInformation.Status == null || _finAgent.AgentInformation.Status.Equals("LOGOUT"))
            {
            }
            else
            {
                if (_finAgent.SignOut("LOGOUT", null))
                {
                }
                else
                    _finAgent.FireLogMessage("We can not sign you out automatically.. Your request logout already sent to Finesse and it will happen soon. please wait");
            }

        }
        private void UpdateRecent(Dialog dialog)
        {
            try
            {
                if (dialog == null)
                    return;
                AgentInformationExtension agentInformationExtension = _finAgent.AgentInformation.ReservedObject as AgentInformationExtension;
                if (agentInformationExtension == null)
                    agentInformationExtension = new AgentInformationExtension();
                if (agentInformationExtension.Recent == null)
                    agentInformationExtension.Recent = new MTObservableCollection<Dialog>();

                Dialog existingDialog = null;
                foreach (Dialog _dialog in agentInformationExtension.Recent)
                {
                    if (_dialog.ID.Equals(dialog.ID)) // Same Dialog , so let us update it
                        existingDialog = _dialog;
                }
                if (existingDialog != null)
                    agentInformationExtension.Recent.Remove(existingDialog);
                agentInformationExtension.Recent.Insert(0, dialog);
                agentInformationExtension.Recent = agentInformationExtension.Recent;

                _finAgent.AgentInformation.ReservedObject = agentInformationExtension;
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error during update Recent calls, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
            }
        }
        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            try
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
                    {
                    }

                }
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error during windows closing, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
                Environment.Exit(0);
            }
        }

        private void ShowRecentCallData_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string dialogID = button.Tag as string;
            Dialog locatedDialog = null;
            MTObservableCollection<Dialog> recentDialogs = (_finAgent.AgentInformation.ReservedObject as AgentInformationExtension).Recent;
            foreach (Dialog dialog in recentDialogs)
                if (dialog.ID.Equals(dialogID))
                    locatedDialog = dialog;
            CallView.DataContext = locatedDialog;
            RecentViews.SelectedIndex = 1;
        }

        private void CloseRecentDialogDetails_Click(object sender, RoutedEventArgs e)
        {
            CallView.DataContext = null;
            RecentViews.SelectedIndex = 0;
        }
        private void SwitchOffMainViews()
        {
            try
            {
                DialPadScreen.Visibility = Visibility.Hidden;
                PhoneScreen.Visibility = Visibility.Hidden;
                HomeScreen.Visibility = Visibility.Hidden;
                DebugScreen.Visibility = Visibility.Hidden;
                QueuesScreen.Visibility = Visibility.Hidden;
                RecentScreen.Visibility = Visibility.Hidden;
                AboutScreen.Visibility = Visibility.Hidden;

                CallsShortCut.IsChecked = false;
                KeypadShortCut.IsChecked = false;
                HomeShortCut.IsChecked = false;
                RecentsShortCut.IsChecked = false;
                AppsShortCut.IsChecked = false;
            }
            catch (Exception ex)
            {
                _finAgent.FireLogMessage("Application Error during switch views off in the main screen, ex:" + ex.Message + "\nStack trace\n" + ex.StackTrace);
            }
        }


        #endregion

    }
}
