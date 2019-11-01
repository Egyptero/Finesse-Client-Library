using CXDE.RJB_Customization;
using CXDE.Server_Side;
using CXDE.Tools;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static CXDE.Server_Side.Dialog.MediaProperties;

namespace CXDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum CallButton
        {
            Answer,
            Release,
            Hold,
            Resume,
            IVR,
            SSTransfer,
            Transfer,
            Consult,
            Conference,
            Keypad
        }
        public enum ScreenName
        {
            LoginScreen,
            HomeScreen,
            LoginLoadingScreen,
        }
        public enum CallEvent
        {
            CallAlert,
            CallAnswered,
            CallHold,
            CallRetreive,
            CallRelease
        }
        private FinAgent finAgent;
        private StateTimer stateTimer;
        public LogWriter logWriter;
        private string CTIWebServer = "http://10.252.66.74:8088/CTIWEBIE11/";
        private string LogCallStart = "true";
        private string LogCallEnd = "true";
        private string LogCallTransfer = "true";
        private string CallAUDStart = "true";
        private string CallAUDEnd = "true";
        private string FireCallStartEvent = "true";
        private string FireCallEndEvent = "true";
        private string TraceStatus = "false";
        private string XmppPort = "7071";
        private string XmppURL = "/http-bind/";
        private string HttpPort = "80";
        private string HttpURL = "/finesse";
        private string XmppConnectionType = "Tls11";
        private string HttpConnectionType = "Tls12";
        private bool Ssl = false;


        public MainWindow()
        {
            InitializeComponent();
            LogMessage("CXConnect Started");
            try
            {
                //Load Configuration File
                var fileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CXConnectConfig.properties");
                CXProperties properties = new CXProperties(fileName);
                //Check Finesse Side A Parameter
                if (properties.get("FinesseA") == null)
                    properties.set("FinesseA", DomainAInfo.Text);
                else
                    DomainAInfo.Text = properties.get("FinesseA", "hofinesseaprd.alrajhi.bank");

                //Check Finesse Side B Parameter
                if (properties.get("FinesseB") == null)
                    properties.set("FinesseB", DomainBInfo.Text);
                else
                    DomainBInfo.Text = properties.get("FinesseB", "hofinessebprd.alrajhi.bank");

                //Check CTIWebServer
                if (properties.get("CTIWebServer") == null)
                    properties.set("CTIWebServer", CTIWebServer);
                else
                    CTIWebServer = properties.get("CTIWebServer", "http://10.252.66.74:8088/CTIWEBIE11/");

                //Check Log Call Start
                if (properties.get("LogCallStart") == null)
                    properties.set("LogCallStart", LogCallStart);
                else
                    LogCallStart = properties.get("LogCallStart", "true");

                //Check Log Call End
                if (properties.get("LogCallEnd") == null)
                    properties.set("LogCallEnd", LogCallEnd);
                else
                    LogCallEnd = properties.get("LogCallEnd", "true");

                //Check Log Call Transfer
                if (properties.get("LogCallTransfer") == null)
                    properties.set("LogCallTransfer", LogCallTransfer);
                else
                    LogCallTransfer = properties.get("LogCallTransfer", "true");

                //Check AUD Start Call Log
                if (properties.get("CallAUDStart") == null)
                    properties.set("CallAUDStart", CallAUDStart);
                else
                    CallAUDStart = properties.get("CallAUDStart", "true");

                //Check AUD End Call Log
                if (properties.get("CallAUDEnd") == null)
                    properties.set("CallAUDEnd", CallAUDEnd);
                else
                    CallAUDEnd = properties.get("CallAUDEnd", "true");

                //Check Fire Call Start Event
                if (properties.get("FireCallStartEvent") == null)
                    properties.set("FireCallStartEvent", FireCallStartEvent);
                else
                    FireCallStartEvent = properties.get("FireCallStartEvent", "true");

                //Check Fire Call End Event
                if (properties.get("FireCallEndEvent") == null)
                    properties.set("FireCallEndEvent", FireCallEndEvent);
                else
                    FireCallEndEvent = properties.get("FireCallEndEvent", "false");
                //Check if Trace Enabled
                if (properties.get("Trace") == null)
                    properties.set("Trace", TraceStatus);
                else
                    TraceStatus = properties.get("Trace", "false");

                //Check if Trace Enabled
                if (properties.get("XmppPort") == null)
                    properties.set("XmppPort", XmppPort);
                else
                    XmppPort = properties.get("XmppPort", "7071");

                //Check if Trace Enabled
                if (properties.get("XmppURL") == null)
                    properties.set("XmppURL", XmppURL);
                else
                    XmppURL = properties.get("XmppURL", "/http-bind/");

                //Check if Trace Enabled
                if (properties.get("XmppConnectionType") == null)
                    properties.set("XmppConnectionType", XmppConnectionType);
                else
                    XmppConnectionType = properties.get("XmppConnectionType", "Tls11");

                //Check if Trace Enabled
                if (properties.get("HttpPort") == null)
                    properties.set("HttpPort", HttpPort);
                else
                    HttpPort = properties.get("HttpPort", "80");

                //Check if Trace Enabled
                if (properties.get("HttpURL") == null)
                    properties.set("HttpURL", HttpURL);
                else
                    HttpURL = properties.get("HttpURL", "/finesse");

                //Check if Trace Enabled
                if (properties.get("HttpConnectionType") == null)
                    properties.set("HttpConnectionType", HttpConnectionType);
                else
                    HttpConnectionType = properties.get("HttpConnectionType", "Tls12");

                //Check if Trace Enabled
                if (properties.get("Ssl") == null)
                    properties.set("Ssl", Convert.ToString(Ssl));
                else
                    Ssl = Boolean.Parse(properties.get("Ssl", "false"));

                //Check the show server details
                if (properties.get("ShowServerDetails") == null)
                    properties.set("ShowServerDetails", "false");
                else
                {
                    if (properties.get("ShowServerDetails").Equals("false"))
                    {
                        ServerSideA.Visibility = Visibility.Hidden;
                        ServerSideB.Visibility = Visibility.Hidden;
                        DomainAInfo.Visibility = Visibility.Hidden;
                        DomainBInfo.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        ServerSideA.Visibility = Visibility.Visible;
                        ServerSideB.Visibility = Visibility.Visible;
                        DomainAInfo.Visibility = Visibility.Visible;
                        DomainBInfo.Visibility = Visibility.Visible;
                    }
                }
                if (TraceStatus.Equals("true"))
                    Trace.Write("CXConnect:Loading Configuration File");
                LogMessage("Loading configuration file");
                properties.Save();
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during CX Connect Initiation," + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }

            //properties.reload();

            LoadScreen(ScreenName.LoginScreen);
        }
        private void LogMessage(string v)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                {
                    new LogWriter(v);
                }));
            }
            catch (Exception)
            {

            }
        }

        #region User Code Area
        private void ProcessLoginFromText(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                PerformLogin();
        }
        private void DialNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DialNumber.Text != null && DialNumber.Text.Equals("Type Number"))
                DialNumber.Text = "";
        }
        private void DialNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DialNumber.Text != null && DialNumber.Text.Equals(""))
                DialNumber.Text = "Type Number";
        }
        private void AnswerCallClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string dialogID = null;
            try
            {
                dialogID = button.Name.Split('_')[1];
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: (Error During Call Answer) " + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage("CXConnect: (Error During Call Answer) " + ex.ToString());
            }
            if (dialogID == null)
            {
                if (TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: (Error During Call Answer) answer button is not linked to Dialog ID !!!");
                LogMessage("CXConnect: (Error During Call Answer) answer button is not linked to Dialog ID !!!");
                return;
            }
            try
            {
                new Thread(delegate ()
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                {
                    ModifyCallButton(button, false, Visibility.Visible);
                    if (!finAgent.AnswerCall(dialogID))
                        PopulateErrorMessage("Error during call answer");
                }));
                }).Start();
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during call answer," + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }

        }
        private void EndCallClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string dialogID = null;
            try
            {
                dialogID = button.Name.Split('_')[1];
            }
            catch (Exception ex)
            {

                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: (Error During Call End) " + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
            if (dialogID == null)
            {
                if (TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: (Error During Call End) end button is not linked to Dialog ID !!!");
                LogMessage("CXConnect: (Error During Call End) end button is not linked to Dialog ID !!!");
                return;
            }
            try
            {
                new Thread(delegate ()
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                    {
                        ModifyCallButton(button, false, Visibility.Visible);
                        if (!finAgent.ReleaseCall(dialogID))
                            PopulateErrorMessage("Error during call release");
                    }));
                }).Start();
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during call release," + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
        }
        private void HoldCallClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string dialogID = null;
            try
            {
                dialogID = button.Name.Split('_')[1];
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: (Error During Call Hold) " + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
            if (dialogID == null)
            {
                if (TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: (Error During Call Hold) hold button is not linked to Dialog ID !!!");
                LogMessage("CXConnect: (Error During Call Hold) hold button is not linked to Dialog ID !!!");
                return;
            }
            try
            {
                new Thread(delegate ()
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                {
                    ModifyCallButton(button, false, Visibility.Visible);
                    if (!finAgent.HoldCall(dialogID))
                        PopulateErrorMessage("Error during call hold");
                }));
                }).Start();
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during call hold," + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
        }
        private void TransferCallClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string dialogID = null;
            try
            {
                dialogID = button.Name.Split('_')[1];
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: (Error During Call Transfer) " + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
            if (dialogID == null)
            {
                if (TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: (Error During Call Transfer) transfer button is not linked to Dialog ID !!!");
                LogMessage("CXConnect: (Error During Call Transfer) transfer button is not linked to Dialog ID !!!");
                return;
            }
            try
            {
                new Thread(delegate ()
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                    {
                        ModifyCallButton(button, false, Visibility.Visible);
                        if (!finAgent.TransferCall(dialogID))
                            PopulateErrorMessage("Error during call transfer");
                    }));
                }).Start();
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during call transfer," + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
        }
        private void ResumeCallClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string dialogID = null;
            try
            {
                dialogID = button.Name.Split('_')[1];
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: (Error During Call Resume) " + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
            if (dialogID == null)
            {
                if (TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: (Error During Call Resume) resume button is not linked to Dialog ID !!!");
                LogMessage("CXConnect: (Error During Call Resume) resume button is not linked to Dialog ID !!!");
                return;
            }
            try
            {
                new Thread(delegate ()
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                {
                    ModifyCallButton(button, false, Visibility.Visible);
                    if (!finAgent.ResumeCall(dialogID))
                        PopulateErrorMessage("Error during call retreive");
                }));
                }).Start();
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during call retreive," + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
        }
        private void SSTransferCallClick(object sender, RoutedEventArgs e)
        {
            if (DialNumber.Text != null && (DialNumber.Text.Equals("") || DialNumber.Text.Equals("Type Number")))
            {
                DialNumber.Focus();
                PopulateErrorMessage("Missing dialed number");
                return;
            }
            Button button = sender as Button;
            string dialogID = null;
            try
            {
                dialogID = button.Name.Split('_')[1];
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: (Error During Call Transfer) " + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
            if (dialogID == null)
            {
                if (TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: (Error During Call Transfer) transfer button is not linked to Dialog ID !!!");
                LogMessage("CXConnect: (Error During Call Transfer) transfer button is not linked to Dialog ID !!!");
                return;
            }
            try
            {
                new Thread(delegate ()
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                {
                    ModifyCallButton(button, false, Visibility.Visible);
                    if (!finAgent.SSTransferCall(dialogID, DialNumber.Text))
                        PopulateErrorMessage("Invalid Transfer Request: Check Extension");
                }));
                }).Start();
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during call transfer," + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
        }
        private void ConsultCallClick(object sender, RoutedEventArgs e)
        {
            if (DialNumber.Text != null && (DialNumber.Text.Equals("") || DialNumber.Text.Equals("Type Number")))
            {
                DialNumber.Focus();
                PopulateErrorMessage("Missing dialed number");
                return;
            }
            Button button = sender as Button;
            string dialogID = null;
            try
            {
                dialogID = button.Name.Split('_')[1];
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: (Error During Call Consult) " + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
            if (dialogID == null)
            {
                if (TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: (Error During Call Consult) Consult button is not linked to Dialog ID !!!");
                LogMessage("CXConnect: (Error During Call Consult) Consult button is not linked to Dialog ID !!!");
                return;
            }
            try
            {
                new Thread(delegate ()
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                {
                    ModifyCallButton(button, false, Visibility.Visible);
                    if (!finAgent.ConsultCall(dialogID, DialNumber.Text))
                        PopulateErrorMessage("Error during call consult");
                }));
                }).Start();
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during call consult," + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
        }
        private void ConferenceCallClick(object sender, RoutedEventArgs e)
        {
            if (DialNumber.Text != null && (DialNumber.Text.Equals("") || DialNumber.Text.Equals("Type Number")))
            {
                DialNumber.Focus();
                PopulateErrorMessage("Missing dialed number");
                return;
            }
            Button button = sender as Button;
            string dialogID = null;
            try
            {
                dialogID = button.Name.Split('_')[1];
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: (Error During Call Conference) " + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
            if (dialogID == null)
            {
                if (TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: (Error During Call Conference) conference button is not linked to Dialog ID !!!");
                LogMessage("CXConnect: (Error During Call Conference) conference button is not linked to Dialog ID !!!");
                return;
            }
            try
            {
                new Thread(delegate ()
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                {
                    ModifyCallButton(button, false, Visibility.Visible);
                    if (!finAgent.ConferenceCall(dialogID, DialNumber.Text))
                        PopulateErrorMessage("Error during call conference");
                }));
                }).Start();
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during call conference," + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
        }
        private void KeyPadClick(object sender, RoutedEventArgs e)
        {
            if (DialNumber.Text != null && (DialNumber.Text.Equals("") || DialNumber.Text.Equals("Type Number")))
            {
                DialNumber.Focus();
                PopulateErrorMessage("Missing dtmf string. Please enter DTMF String");
                return;
            }
            Button button = sender as Button;
            string dialogID = null;
            try
            {
                dialogID = button.Name.Split('_')[1];
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: (Error During Call Keypad) " + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
            if (dialogID == null)
            {
                if (TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: (Error During Call Keypad) Keypad button is not linked to Dialog ID !!!");
                LogMessage("CXConnect: (Error During Call Keypad) Keypad button is not linked to Dialog ID !!!");
                return;
            }
            try
            {
                new Thread(delegate ()
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                {
                    ModifyCallButton(button, false, Visibility.Visible);
                    if (!finAgent.KeypadSendDTMF(dialogID, DialNumber.Text))
                        PopulateErrorMessage("Error during sending DTMF");
                }));
                }).Start();
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during call send DTMF," + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
        }
        private void MakeCallClick(object sender, RoutedEventArgs e)
        {
            if (DialNumber.Text != null && (DialNumber.Text.Equals("") || DialNumber.Text.Equals("Type Number")))
            {
                DialNumber.Focus();
                PopulateErrorMessage("Missing dialed number");
                return;
            }
            string dialNumber = DialNumber.Text;
            DialNumber.Text = "Type Number";
            try
            {
                new Thread(delegate ()
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                {
                    MakeCallButton.IsEnabled = false;
                    if (!finAgent.MakeCall(dialNumber))
                        PopulateErrorMessage("Error while dialing: " + DialNumber.Text);
                    MakeCallButton.IsEnabled = true;
                }));
                }).Start();
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during make call," + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
        }
        private void SendBackToIVRClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string dialogID = null;
            try
            {
                dialogID = button.Name.Split('_')[1];
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: (Error During Call Send Back to IVR) " + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
            if (dialogID == null)
            {
                if (TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: (Error During Call Send Back to IVR) Send Back to IVR button is not linked to Dialog ID !!!");

                return;
            }
            //Get Parameters from UI
            string selectionOption = string.Empty;
            bool isMale = true;
            IVROptions.TransferType transferType = IVROptions.TransferType.INQ;
            string extension = string.Empty;
            //Read Transfer Option
            ComboBox IVRSelection = (ComboBox)Calls.FindName("IVRSelection_" + dialogID);
            if (IVRSelection != null)
            {
                ComboBoxItem IVRSelectionItem = IVRSelection.SelectedItem as ComboBoxItem;
                if (IVRSelectionItem != null)
                    selectionOption = IVRSelectionItem.Content as string;
            }
            else
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: (Error During Call Send Back to IVR) For some reason the IVR Selection Combo is not their. Abnormal Behavior !!!");
                    Trace.Write("CXConnect: Error will be published to screen and call clear should happen now");
                }
                LogMessage("CXConnect: (Error During Call Send Back to IVR) For some reason the IVR Selection Combo is not their. Abnormal Behavior !!!");
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => PopulateErrorMessage("Call is not active any more. It will be deleted soon.")));
                PopulateCallInformation();
                return;
            }
            if (selectionOption == null || selectionOption.Equals("No option Avaliable") || selectionOption == "")
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => PopulateErrorMessage("Missing Parameters while transfer caller to survey")));
                return;
            }

            CheckBox MaleGenderCheckBox = (CheckBox)Calls.FindName("MaleGenderCheckBox_" + dialogID);
            if (MaleGenderCheckBox != null)
                isMale = MaleGenderCheckBox.IsChecked.Value;
            RadioButton InqueryRadioButton = (RadioButton)Calls.FindName("InqueryRadioButton_" + dialogID);
            if (InqueryRadioButton != null && InqueryRadioButton.IsChecked.Value)
                transferType = IVROptions.TransferType.INQ;
            RadioButton ComplaintRadioButton = (RadioButton)Calls.FindName("ComplaintRadioButton_" + dialogID);
            if (ComplaintRadioButton != null && ComplaintRadioButton.IsChecked.Value)
                transferType = IVROptions.TransferType.COMP;
            RadioButton SupportRadioButton = (RadioButton)Calls.FindName("SupportRadioButton_" + dialogID);
            if (SupportRadioButton != null && SupportRadioButton.IsChecked.Value)
                transferType = IVROptions.TransferType.SUPP;

            IVROptions ivrOptions = new IVROptions(finAgent, dialogID);
            extension = ivrOptions.GetExtension(selectionOption, isMale, transferType);
            if (extension == null) //Nothing Selected
                return;

            if (extension == IVROptions.SKILLGROUP_EXT)
            {
                try
                {
                    new Thread(delegate ()
                    {
                        if (LogCallTransfer.Equals("true") || LogCallTransfer.Equals("yes"))
                        {
                            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                            {
                                new ExtraServlets(finAgent, dialogID, CTIWebServer).OnCallSkillTransfer(ivrOptions._CurrentSkill, ivrOptions._TargetSkill);
                            }));
                        }
                    }).Start();
                }
                catch (Exception ex)
                {
                    if (TraceStatus.Equals("true"))
                    {
                        Trace.Write("CXConnect: Error during loggin call transfer to CTIWEB ," + ex.Message);
                        Trace.Write(ex);
                    }
                    FireErrorMessage("Can not log Transfer History:" + ex.Message);
                    LogMessage(ex.ToString());
                }
            }
            try
            {
                new Thread(delegate ()
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                    {
                        MakeCallButton.IsEnabled = false;
                        ModifyCallButton(button, false, Visibility.Visible);
                        if (!finAgent.SSTransferCall(dialogID, extension))
                        {
                            if (TraceStatus.Equals("true"))
                                Trace.Write("CXConnect: Failed to send call back to IVR at SSTransferCall");
                        }
                        MakeCallButton.IsEnabled = true;
                    }));
                }).Start();
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during call send to IVR," + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
        }
        private void OpenCRMButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string dialogID = null;
            try
            {
                dialogID = button.Name.Split('_')[1];
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: (Error During Open CRM Button) " + ex.Message);
                    Trace.Write(ex);
                }
            }
            if (dialogID == null)
            {
                if (TraceStatus.Equals("true"))
                    Trace.Write("CXConnect: (Error During Open CRM Button) open crm button is not linked to Dialog ID !!!");

                return;
            }

            try
            {
                new Thread(delegate ()
                {
                    if (CallAUDStart.Equals("true") || CallAUDStart.Equals("yes"))
                    {
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                        {
                            new AUDOption(finAgent, finAgent.FindDialog(dialogID), CTIWebServer).StartCallAUDRecord();
                        }));
                    }

                    if (FireCallStartEvent.Equals("true") || FireCallStartEvent.Equals("yes"))
                        FireStartCallEvent(finAgent.FindDialog(dialogID), false);
                }).Start();
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    System.Diagnostics.Trace.Write("CXConnect: Error in call initiation" + ex.Message);
                    System.Diagnostics.Trace.Write(ex);
                }
                return;
            }

        }
        private void ErrorBarCloseClick(object sender, RoutedEventArgs e)
        {
            CloseErrorBar();
        }
        private void CloseErrorBar()
        {
            if (LoginScreen.Visibility == Visibility.Visible)
                LoginMessageBar.Visibility = Visibility.Hidden;
            else
                MessageBar.Visibility = Visibility.Hidden;

        }
        //submit warpup button
        private void WrapUpSubmit(object sender, RoutedEventArgs e)
        {
        }
        private void LoginClick(object sender, RoutedEventArgs e)
        {
            LogMessage("Login Clicked");
            //TestEvent();
            //Call Agent Login
            PerformLogin();
        }
        private void PerformLogin()
        {
            LogMessage("Perform Login");
            //            if (finAgent == null)
            finAgent = new FinAgent(AgentIDInfo.Text, PasswordInfo.Password, AgentExtInfo.Text, DomainAInfo.Text, DomainBInfo.Text, this);
            finAgent.TraceStatus = TraceStatus;
            finAgent._agentInformation.Ssl = Ssl;
            finAgent._agentInformation.XmppPort = XmppPort;
            finAgent._agentInformation.XmppURL = XmppURL;
            finAgent._agentInformation.XmppConnectionType = XmppConnectionType;
            finAgent._agentInformation.HttpPort = HttpPort;
            finAgent._agentInformation.HttpURL = HttpURL;
            finAgent._agentInformation.HttpConnectionType = HttpConnectionType;
            //else
            //{
            //    finAgent._agentInformation.AgentID = AgentIDInfo.Text;
            //    finAgent._agentInformation.UserName = AgentIDInfo.Text;
            //    finAgent._agentInformation.Password = PasswordInfo.Password;
            //    finAgent._agentInformation.Extension = AgentExtInfo.Text;
            //    finAgent._agentInformation.DomainA = DomainAInfo.Text;
            //    finAgent._agentInformation.DomainB = DomainBInfo.Text;
            //}
            new Thread(delegate ()
            {
                LogMessage("Will try to sign in now");
                if (finAgent.SignIn())
                {
                    LogMessage("Sign in is requested , will try to load data");
                    //System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                    //{
                    //    MessageBar.Visibility = Visibility.Hidden;
                    //    LoginMessageBar.Visibility = Visibility.Hidden;
                    //    LogMessage("Loading Agent Information from login");
                    //    finAgent.LoadAgentInformation();
                    //    LogMessage("Loading Call Information from login");
                    //    finAgent.LoadCallInformation();
                    //    LogMessage("Render Agent Information from login");
                    //    PopulateAgentInformation();
                    //    LogMessage("Render Call Information from login");
                    //    PopulateCallInformation();
                    //    LogMessage("Now time to adjust screen");
                    //    LoadScreen(ScreenName.HomeScreen);
                    //    //LogMessage("Load Not Ready Reason Code List");
                    //    //finAgent.LoadNotReadyReasonCodeList();
                    //    //LogMessage("Load Logout Reason Code List");
                    //    //finAgent.LoadLogoutReasonCodeList();
                    //    LogMessage("Build Status Menu");
                    //    PopulateStatusDropDown();
                    //}));
                }
                else
                {
                    if (TraceStatus.Equals("true"))
                        Trace.Write("CXConnect: Unable to login");
                    LogMessage("Can not login with the agent ID");
                    try
                    {
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => LoadScreen(ScreenName.LoginScreen)));
                    }
                    catch (Exception ex)
                    {
                        if (TraceStatus.Equals("true"))
                        {
                            Trace.Write("CXConnect: (Error during loading login window after login trial)," + ex.Message);
                            Trace.Write(ex);
                        }
                        LogMessage(ex.ToString());
                    }
                }

            }).Start();
            LoadScreen(ScreenName.LoginLoadingScreen);
        }
        private void StatusDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LogMessage("Agent Status is changing");
            // Get the status selection
            if (StatusDropDown.SelectedItem == null)
                return;
            string newStatus;
            string reasonCodeLabel = null;

            ComboBoxItem comboBoxItem = StatusDropDown.SelectedItem as ComboBoxItem;
            WrapPanel wrapPanel = comboBoxItem.Content as WrapPanel;
            TextBlock textBlock;

            if (wrapPanel.Children.Count > 1)
                textBlock = wrapPanel.Children[1] as TextBlock;
            else
                textBlock = wrapPanel.Children[0] as TextBlock;
            string status = textBlock.Text;
            if (status.Contains("->"))
                return;
            if (status.Contains("-"))
            {
                newStatus = status.Split('-')[0];
                reasonCodeLabel = status.Split('-')[1];
            }
            else
                newStatus = status;
            if (newStatus.Equals(finAgent._agentInformation.Status))
            {
                if (reasonCodeLabel == null && finAgent._agentInformation._ReasonCode == null)
                    return;
                else if (reasonCodeLabel != null && finAgent._agentInformation._ReasonCode != null && finAgent._agentInformation._ReasonCode.Label != null)
                {
                    if (reasonCodeLabel.Equals(finAgent._agentInformation._ReasonCode.Label))
                        return;
                }

            }
            if (newStatus.Equals("LOGOUT"))
            {
                LogMessage("Logout the agent");
                if (finAgent.SignOut(newStatus, reasonCodeLabel))
                {
                    FireLoadingMessage("Signout in process");
                    LoadScreen(ScreenName.LoginLoadingScreen);
                }
                return;
            }

            LogMessage("Change Agent Status to " + newStatus);
            if (finAgent.ChangeStatus(newStatus, reasonCodeLabel))
            {
                if (finAgent._agentInformation.Status.Equals("TALKING") || finAgent._agentInformation.Status.Equals("RESERVED") || finAgent._agentInformation.Status.Equals("HOLD"))
                {
                    finAgent._agentInformation.PendingStatus = status;
                    PopulateStatusDropDown();
                }
            }
        }
        public void FireNewEvent()
        {
            if (finAgent._agentInformation._MessageEvent == null)
                return;
            if (finAgent._agentInformation._MessageEvent.MessageType == null)
                return;
            if (finAgent._agentInformation._MessageEvent.MessageType.Equals("user"))
            {
                LogMessage("New User Message Received at the front end");
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => PopulateStatusDropDown()));
                //System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => AdjustScreen()));
            }
            else if (finAgent._agentInformation._MessageEvent.MessageType.Equals("call"))
            {
                LogMessage("New Call Message Received at the front end");
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => PopulateCallInformation()));
            }
            else if (finAgent._agentInformation._MessageEvent.MessageType.Equals("error"))
            {
                LogMessage("New Error Message Received at the front end:" + finAgent._agentInformation._MessageEvent.errorMsg);
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => PopulateErrorMessage(null)));
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => PopulateCallInformation()));
            }
        }
        public void FireErrorMessage(string msg)
        {
            if (msg != null)
            {
                try
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => PopulateErrorMessage(msg)));
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => PopulateCallInformation()));
                }
                catch (Exception ex)
                {
                    if (TraceStatus.Equals("true"))
                    {
                        Trace.Write("CXConnect: (Error during Fire Error Message)" + ex.Message);
                        Trace.Write(ex);
                    }
                    LogMessage(ex.ToString());
                }
            }
        }
        public void FireLoadingMessage(string msg)
        {
            LogMessage(msg);
            if (msg != null)
            {
                try
                {
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => PopulateLoadingMessage(msg)));
                }
                catch (Exception ex)
                {
                    if (TraceStatus.Equals("true"))
                    {
                        Trace.Write("CXConnect: (Error during Fire Loading Message)" + ex.Message);
                        Trace.Write(ex);
                    }
                }
            }
        }
        public void FireReLoginEvent()
        {
            LogMessage("Relogin is happened , will try to reload agent and call information");
            try
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
                {
                    MessageBar.Visibility = Visibility.Hidden;
                    LoginMessageBar.Visibility = Visibility.Hidden;
                    finAgent.LoadAgentInformation();
                    if (finAgent._agentInformation.Dialogs != null)
                        finAgent._agentInformation.Dialogs.Clear();
                    finAgent.LoadCallInformation();
                    PopulateAgentInformation();
                    PopulateCallInformation();
                    PopulateStatusDropDown();
                    LoadScreen(ScreenName.HomeScreen);
                }));
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: (Error during Fire relogin)" + ex.Message);
                    Trace.Write(ex);
                }
                LogMessage(ex.ToString());
            }
        }
        public void FireLoadLoginScreen()
        {
            LogMessage("Load Login Screen Again as per FinAgent Request. seems XMPP was down");
            try
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => LoadScreen(ScreenName.LoginScreen)));
            }
            catch (Exception)
            {

            }
        }
        private void PopulateLoadingMessage(string msg)
        {
            CurrentLoadingStep.Text = msg;
        }
        private void PopulateErrorMessage(string msg)
        {
            try
            {
                if (MainScreen.Visibility == Visibility.Visible)
                {
                    if (msg == null)
                    {
                        if (finAgent == null)
                            return;
                        if (finAgent._agentInformation == null)
                            return;
                        if (finAgent._agentInformation._MessageEvent != null)
                        {
                            UserMessageText.Text = "";
                            if (finAgent._agentInformation._MessageEvent.errorType != null)
                                UserMessageText.Text += "(" + finAgent._agentInformation._MessageEvent.errorType + ") ";
                            if (finAgent._agentInformation._MessageEvent.errorMsg != null)
                                UserMessageText.Text += finAgent._agentInformation._MessageEvent.errorMsg.Replace("_", " ").ToLower();
                            if (UserMessageText.Text != null && UserMessageText.Text != "")
                                MessageBar.Visibility = Visibility.Visible;
                        }
                    }
                    else if (msg != "")
                    {
                        UserMessageText.Text = msg;
                        MessageBar.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (msg == null)
                    {
                        if (finAgent == null)
                            return;
                        if (finAgent._agentInformation == null)
                            return;

                        if (finAgent._agentInformation._MessageEvent != null)
                        {
                            UserMessageTextLogin.Text = "";
                            if (finAgent._agentInformation._MessageEvent.errorType != null)
                                UserMessageTextLogin.Text += "(" + finAgent._agentInformation._MessageEvent.errorType + ") ";
                            if (finAgent._agentInformation._MessageEvent.errorMsg != null)
                                UserMessageTextLogin.Text += finAgent._agentInformation._MessageEvent.errorMsg.Replace("_", " ").ToLower();
                            if (UserMessageTextLogin.Text != null && UserMessageTextLogin.Text != "")
                                LoginMessageBar.Visibility = Visibility.Visible;

                        }

                    }
                    else if (msg != "")
                    {
                        UserMessageTextLogin.Text = msg;
                        LoginMessageBar.Visibility = Visibility.Visible;
                    }
                }

            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during populating error message," + ex.Message);
                    Trace.Write(ex);
                }
            }

        }
        private void PopulateAgentInformation()
        {
            AgentNameInfo.Text = "Agent Name: " + finAgent._agentInformation.Name;
            AgentExtensionInfo.Text = "Extension: " + finAgent._agentInformation.Extension;
        }
        private void PopulateCallInformation()
        {
            try
            {
                for (int index = Calls.Children.Count - 1; index >= 0; index--)
                {
                    if (((Border)Calls.Children[index]).Name.Contains("_"))
                    {
                        if (!finAgent.CheckDialog(((Border)Calls.Children[index]).Name.Split('_')[1]))
                        {
                            ((Border)Calls.Children[index]).Visibility = Visibility.Hidden;
                            //                        MessageBox.Show("Removing call with id: "+((Border)Calls.Children[index]).Name.Split('_')[1]);
                            Calls.Children.RemoveAt(index);
                        }
                    }
                    else
                    {
                        ((Border)Calls.Children[index]).Visibility = Visibility.Hidden;
                        //                   MessageBox.Show("Removing call with id : " + ((Border)Calls.Children[index]).Name.Split('_')[1]);
                        Calls.Children.RemoveAt(index);
                    }
                }
                // If there is calls , then update it
                if (finAgent._agentInformation.Dialogs == null || finAgent._agentInformation.Dialogs.Count < 1)
                {
                    //Clean All Elements in case nothing existing in the dialogs.
                    if (Calls.Children.Count > 0)
                        Calls.Children.Clear();
                    return;
                }

                for (int counter = 0; counter < finAgent._agentInformation.Dialogs.Count; counter++)
                {
                    Dialog _Dialog = finAgent._agentInformation.Dialogs[counter] as Dialog;
                    if (_Dialog != null && _Dialog._MediaType.Equals("Voice"))
                    {
                        Border border = BuildDialogBorder(_Dialog);
                        if (border != null)
                        {
                            //Remove Error Bar in case of new call will be populated
                            CloseErrorBar();
                            Calls.Children.Add(border);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error during call population," + ex.Message);
                    Trace.Write(ex);
                }
            }
            //            Focus();
        }
        private void PopulateStatusDropDown()
        {
            StatusDropDown.Items.Clear();

            //Add Ready Option
            ComboBoxItem comboBoxItem = BuildComboBoxItem("READY", null, "Ready");
            if ("READY".Equals(finAgent._agentInformation.Status))
                comboBoxItem.IsSelected = true;
            StatusDropDown.Items.Add(comboBoxItem);

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

            if (finAgent._agentInformation.NotReadyReasonCodeList != null)
            {
                foreach (AgentInformation.ReasonCode reasonCode in finAgent._agentInformation.NotReadyReasonCodeList)
                {
                    comboBoxItem = BuildComboBoxItem(reasonCode.Category, reasonCode.Label, "NotReady");

                    if (reasonCode.Category.Equals(finAgent._agentInformation.Status))
                    {
                        //Check Reason Code
                        if (reasonCode.Label == null && finAgent._agentInformation._ReasonCode == null)
                            comboBoxItem.IsSelected = true;
                        else if (reasonCode.Label != null && finAgent._agentInformation._ReasonCode != null && finAgent._agentInformation._ReasonCode.Label != null)
                        {
                            if (reasonCode.Label.Equals(finAgent._agentInformation._ReasonCode.Label))
                                comboBoxItem.IsSelected = true;
                        }

                    }
                    StatusDropDown.Items.Add(comboBoxItem);

                }
            }
            //Agent Status Neither Ready Nor Not ready Nor Wrap up
            if (StatusDropDown.SelectedItem == null)
            {
                string currentStatus = finAgent._agentInformation.Status;
                string currentLabel = null;
                if (finAgent._agentInformation._ReasonCode != null)
                {
                    if (finAgent._agentInformation._ReasonCode.Label != null)
                        currentLabel = finAgent._agentInformation._ReasonCode.Label;
                }
                string iconName = "NotReady";
                if (finAgent._agentInformation.PendingStatus != null && finAgent._agentInformation.PendingStatus != "")
                    currentStatus = finAgent._agentInformation.Status + "->" + finAgent._agentInformation.PendingStatus;

                if (finAgent._agentInformation.Status.Equals("TALKING") || finAgent._agentInformation.Status.Equals("HOLD") || finAgent._agentInformation.Status.Equals("RESERVED"))
                    iconName = "Reserved";
                comboBoxItem = BuildComboBoxItem(currentStatus, currentLabel, iconName);
                comboBoxItem.IsSelected = true;
                StatusDropDown.Items.Add(comboBoxItem);
            }

            if (!finAgent._agentInformation.Status.Equals("LOGOUT") && finAgent._agentInformation.StateChangeTime != null)
            {
                DateTime stateChangeTime = Convert.ToDateTime(finAgent._agentInformation.StateChangeTime);
                if (stateTimer == null)
                    stateTimer = new StateTimer(stateChangeTime, InStatusTimer, "In status since: ");
                else
                    stateTimer.ResetTimer(stateChangeTime);
            }

            if (!finAgent._agentInformation.Status.Equals("LOGOUT"))
                PopulateLogoutDropDown();

        }
        private void PopulateLogoutDropDown()
        {

            //SignoutDropDown.Items.Clear();

            //Add Default Selection
            ComboBoxItem comboBoxItem;

            if (!finAgent._agentInformation.Status.Equals("NOT_READY"))
            {
                return;
            }
            if (finAgent._agentInformation.LogoutReasonCodeList == null || finAgent._agentInformation.LogoutReasonCodeList.Count == 0)
            {
                comboBoxItem = BuildComboBoxItem("LOGOUT", null, "Other");
                StatusDropDown.Items.Add(comboBoxItem);
                return;
            }


            foreach (AgentInformation.ReasonCode reasonCode in finAgent._agentInformation.LogoutReasonCodeList)
            {
                comboBoxItem = BuildComboBoxItem(reasonCode.Category, reasonCode.Label, "Other");
                StatusDropDown.Items.Add(comboBoxItem);

            }
        }
        public void AdjustScreen()
        {
            LoadScreen(ScreenName.HomeScreen);
        }
        public void LoadScreen(ScreenName screenName)
        {
            switch (screenName)
            {
                case ScreenName.LoginScreen:
                    HideAllComponents();
                    LoginScreen.Visibility = Visibility.Visible;
                    break;
                case ScreenName.HomeScreen:
                    HideAllComponents();
                    MainScreen.Visibility = Visibility.Visible;
                    break;
                case ScreenName.LoginLoadingScreen:
                    HideAllComponents();
                    LoginLoadingScreen.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }

        }
        private ComboBoxItem BuildComboBoxItem(string Category, string Label, string IconName)
        {
            ComboBoxItem comboBoxItem = new ComboBoxItem();
            WrapPanel wrapPanel = new WrapPanel();
            TextBlock textBlock = new TextBlock();
            if (IconName != null)
            {
                Image image = new Image();
                BitmapImage icon = new BitmapImage();

                icon.BeginInit();
                icon.UriSource = new Uri("Images/" + IconName + ".png", UriKind.Relative);
                icon.EndInit();

                image.Stretch = Stretch.Fill;
                image.Source = icon;
                image.Width = 20;
                image.Height = 20;
                image.VerticalAlignment = VerticalAlignment.Center;
                image.HorizontalAlignment = HorizontalAlignment.Left;
                wrapPanel.Children.Add(image);
            }

            textBlock.Text = Category;
            if (Label != null && Label != "")
                textBlock.Text += "-" + Label;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Right;
            wrapPanel.Children.Add(textBlock);

            comboBoxItem.Content = wrapPanel;
            return comboBoxItem;
        }
        private void HideAllComponents()
        {
            MainScreen.Visibility = Visibility.Hidden;
            LoginScreen.Visibility = Visibility.Hidden;
            LoginLoadingScreen.Visibility = Visibility.Hidden;

        }
        private Line BuildLineSeparator()
        {
            Line line = new Line();
            line.Margin = new Thickness(0, 5, 0, 5);
            line.Stroke = new SolidColorBrush(Colors.DarkGray);
            line.X2 = 1;

            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.ShadowDepth = 2;
            dropShadowEffect.Direction = 330;
            dropShadowEffect.Color = Colors.Black;
            dropShadowEffect.Opacity = 0.5;
            dropShadowEffect.BlurRadius = 2;

            line.Effect = dropShadowEffect;
            return line;
        }
        private Button BuildCallButton(string dialogID, CallButton buttonType, bool enabled, Visibility visibility)
        {
            string toolTip = string.Empty;
            string name = string.Empty;
            string imageName = string.Empty;

            switch (buttonType)
            {
                case CallButton.Answer:
                    toolTip = "Answer Call";
                    name = "AnswerCall_" + dialogID;
                    if (enabled)
                        imageName = "answercall.png";
                    else
                        imageName = "answercall_dis.png";
                    break;
                case CallButton.Conference:
                    toolTip = "Conference Call";
                    name = "ConferenceCall_" + dialogID;
                    if (enabled)
                        imageName = "conferencecall.png";
                    else
                        imageName = "conferencecall_dis.png";
                    break;
                case CallButton.Consult:
                    toolTip = "Consult Call";
                    name = "ConsultCall_" + dialogID;
                    if (enabled)
                        imageName = "consultcall.png";
                    else
                        imageName = "consultcall_dis.png";
                    break;
                case CallButton.Hold:
                    toolTip = "Hold Call";
                    name = "HoldCall_" + dialogID;
                    if (enabled)
                        imageName = "holdcall.png";
                    else
                        imageName = "holdcall_dis.png";
                    break;
                case CallButton.IVR:
                    toolTip = "Send Call to IVR";
                    name = "IVRCall_" + dialogID;
                    if (enabled)
                        imageName = "ivrcall.png";
                    else
                        imageName = "ivrcall_dis.png";
                    break;
                case CallButton.Keypad:
                    toolTip = "Send DTMF";
                    name = "KeypadCall_" + dialogID;
                    if (enabled)
                        imageName = "keypadcall.png";
                    else
                        imageName = "keypadcall_dis.png";
                    break;
                case CallButton.Release:
                    toolTip = "Release Call";
                    name = "ReleaseCall_" + dialogID;
                    if (enabled)
                        imageName = "endcall.png";
                    else
                        imageName = "endcall_dis.png";
                    break;
                case CallButton.Resume:
                    toolTip = "Retreive Call";
                    name = "ResumeCall_" + dialogID;
                    if (enabled)
                        imageName = "resumecall.png";
                    else
                        imageName = "resumecall_dis.png";
                    break;
                case CallButton.SSTransfer:
                    toolTip = "SS Transfer Call";
                    name = "TransferCall_" + dialogID;
                    if (enabled)
                        imageName = "transfercall.png";
                    else
                        imageName = "transfercall_dis.png";
                    break;
                case CallButton.Transfer:
                    toolTip = "Transfer Call";
                    name = "CTransferCall_" + dialogID;
                    if (enabled)
                        imageName = "transfercall.png";
                    else
                        imageName = "transfercall_dis.png";
                    break;
            }

            Button button = new Button();
            button.Name = name;
            RegisterName(button.Name, button);

            button.ToolTip = toolTip;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.IsEnabled = enabled;
            button.Visibility = visibility;
            button.Width = 35;
            button.Height = 35;

            string template =
            "<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType =\"Button\">" +
             "<Grid>" +
                  "<ContentPresenter HorizontalAlignment=\"Center\" VerticalAlignment = \"Center\" />" +
             "</Grid>" +
         "</ControlTemplate>";
            button.Template = (ControlTemplate)XamlReader.Parse(template);

            StackPanel buttonStackPanel = new StackPanel();
            Image image = new Image();
            image.Name = "Img_" + name;
            RegisterName(image.Name, image);

            image.HorizontalAlignment = HorizontalAlignment.Stretch;
            image.Source = new BitmapImage(new Uri("Images/" + imageName, UriKind.Relative));
            buttonStackPanel.Children.Add(image);

            button.Content = buttonStackPanel;

            return button;
        }
        private Button ModifyCallButton(Button _button, bool enabled, Visibility visibility)
        {
            if (_button == null)
                return null;
            Image image = (Image)Calls.FindName("Img_" + _button.Name);
            if (enabled)
                image.Source = new BitmapImage(new Uri("Images/" + _button.Name.Split('_')[0] + ".png", UriKind.Relative));
            else
                image.Source = new BitmapImage(new Uri("Images/" + _button.Name.Split('_')[0] + "_dis.png", UriKind.Relative));

            _button.Visibility = visibility;
            _button.IsEnabled = enabled;
            return _button;
        }
        private Border BuildDialogBorder(Dialog dialog)
        {
            if (dialog == null || dialog._ID == null || dialog._ID == "")
                return null;
            Border border = (Border)Calls.FindName("Dialog_" + dialog._ID);
            if (border != null)
            {
                try
                {
                    BuildCallVariablesBar(dialog);
                    BuildCallStatusBar(dialog);
                    BuildCallDetails(dialog);
                    BuildSendTOIVR(dialog);
                    BuildRJBCallInformation(dialog);
                    //BuildCallVariablesExpander(dialog);
                }
                catch (Exception e)
                {
                    if (TraceStatus.Equals("true"))
                    {
                        System.Diagnostics.Trace.Write("CXConnect: Error during update call card" + e.Message);
                        System.Diagnostics.Trace.Write(e);
                    }
                }
                return null;
            }

            border = new Border();
            border.Name = "Dialog_" + dialog._ID;
            //MessageBox.Show("New Call Card Created for Name:"+border.Name);


            RegisterName(border.Name, border); // Register Name of Dialog
            border.HorizontalAlignment = HorizontalAlignment.Stretch;
            border.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            border.BorderThickness = new Thickness(1);
            border.Margin = new Thickness(5, 5, 5, 5);
            border.Padding = new Thickness(2, 2, 2, 2);
            try
            {
                //expander.IsExpanded = true;
                //Build Expander header
                StackPanel stackPanelExpanderHeader = new StackPanel();
                stackPanelExpanderHeader.Name = "CallHeader_" + dialog._ID;
                stackPanelExpanderHeader.Margin = new Thickness(0, 0, 0, 5);
                StackPanel stackPanelExpanderContent = new StackPanel();
                stackPanelExpanderContent.Name = "CallExpanderDetails_" + dialog._ID;
                stackPanelExpanderContent.Margin = new Thickness(10, 0, 10, 0);

                Grid callVariablesBar = BuildCallVariablesBar(dialog);
                Grid callStatusBar = BuildCallStatusBar(dialog);
                Grid callDetails = BuildCallDetails(dialog);
                Grid sendToIVR = BuildSendTOIVR(dialog);
                Grid rjbCallInfo = BuildRJBCallInformation(dialog);
                Expander callVarExpander = BuildCallVariablesExpander(dialog);

                //Expander Header
                if (callVariablesBar != null)
                    stackPanelExpanderHeader.Children.Add(callVariablesBar);
                if (callStatusBar != null)
                    stackPanelExpanderHeader.Children.Add(callStatusBar);
                if (callDetails != null)
                    stackPanelExpanderHeader.Children.Add(callDetails);

                //Expander Content
                if (sendToIVR != null)
                {
                    Line horzLine = new Line();
                    horzLine.Stretch = Stretch.Fill;
                    horzLine.X2 = 1;
                    horzLine.Stroke = new SolidColorBrush(Colors.Gray);
                    stackPanelExpanderContent.Children.Add(horzLine);
                    stackPanelExpanderContent.Children.Add(sendToIVR);
                }
                if (rjbCallInfo != null)
                {
                    Line horzLine = new Line();
                    horzLine.Stretch = Stretch.Fill;
                    horzLine.X2 = 1;
                    horzLine.Stroke = new SolidColorBrush(Colors.Gray);
                    stackPanelExpanderContent.Children.Add(horzLine);
                    stackPanelExpanderContent.Children.Add(rjbCallInfo);
                }
                //if(callVarExpander != null)
                //{
                //    Line horzLine = new Line();
                //    horzLine.Stretch = Stretch.Fill;
                //    horzLine.X2 = 1;
                //    horzLine.Stroke = new SolidColorBrush(Colors.Gray);
                //    stackPanelExpanderContent.Children.Add(horzLine);
                //    stackPanelExpanderContent.Children.Add(callVarExpander);
                //}
                StackPanel borderInnerPanel = new StackPanel();
                borderInnerPanel.Children.Add(stackPanelExpanderHeader);
                borderInnerPanel.Children.Add(stackPanelExpanderContent);
                border.Child = borderInnerPanel;//
            }
            catch (Exception e)
            {
                if (TraceStatus.Equals("true"))
                {
                    System.Diagnostics.Trace.Write("CXConnect: Error during render call card" + e.Message);
                    System.Diagnostics.Trace.Write(e);
                }
            }
            //stackPanelExpanderHeader.Loaded += Expander_StackPanel_Loaded;
            return border;
        }
        private void Expander_StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel stackPanel = sender as StackPanel;
            Binding binding = new Binding("HorizontalAlignment");//HorizontalAlignment
            binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ContentPresenter), 1);
            binding.Mode = BindingMode.OneWayToSource;
            stackPanel.SetBinding(StackPanel.HorizontalAlignmentProperty, binding);
        }
        private Grid BuildCallStatusBar(Dialog dialog)
        {
            Dialog.Participant myParticipantObject = null;
            foreach (Dialog.Participant participant in dialog._Participants)
            {
                if (finAgent._agentInformation.Extension.Equals(participant._MediaAddress))
                    myParticipantObject = participant;
            }

            if (myParticipantObject == null)
                return null;
            Grid grid = (Grid)Calls.FindName("CallStatusBar_" + dialog._ID);
            if (grid != null)
            {
                TextBlock eCallStatusBlock = (TextBlock)Calls.FindName("CallStatusBlock_" + dialog._ID);
                if (dialog._FromAddress.Equals(finAgent._agentInformation.Extension))
                    eCallStatusBlock.Text = "Outgoing ";
                else
                    eCallStatusBlock.Text = "Incoming ";

                eCallStatusBlock.Text += "call status: " + dialog._State;
                return null;
            }

            grid = new Grid();
            grid.Name = "CallStatusBar_" + dialog._ID;
            RegisterName(grid.Name, grid); // Register Name of Dialog
            grid.Margin = new Thickness(10, 10, 10, 10);
            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col1);
            grid.ColumnDefinitions.Add(col2);

            TextBlock CallStatusBlock = new TextBlock();
            CallStatusBlock.Name = "CallStatusBlock_" + dialog._ID;
            RegisterName(CallStatusBlock.Name, CallStatusBlock); // Register Name of Dialog

            CallStatusBlock.HorizontalAlignment = HorizontalAlignment.Left;
            if (dialog._FromAddress.Equals(finAgent._agentInformation.Extension))
                CallStatusBlock.Text = "Outgoing ";
            else
                CallStatusBlock.Text = "Incoming ";

            CallStatusBlock.Text += "call Status: " + dialog._State;

            CallStatusBlock.Foreground = new SolidColorBrush(Colors.Gray);
            Grid.SetColumn(CallStatusBlock, 0);

            TextBlock CallDurationBlock = new TextBlock();
            CallDurationBlock.HorizontalAlignment = HorizontalAlignment.Right;
            CallDurationBlock.Text = "Call Duration: 00:00";
            CallDurationBlock.Foreground = new SolidColorBrush(Colors.Gray);
            StateTimer callStateTime = new StateTimer(Convert.ToDateTime(myParticipantObject._StartTime), CallDurationBlock, "Call Duration: "); // Not sure if this code will work
            Grid.SetColumn(CallDurationBlock, 1);

            grid.Children.Add(CallStatusBlock);
            grid.Children.Add(CallDurationBlock);
            return grid;
        }
        private Grid BuildCallVariablesBar(Dialog dialog)
        {
            Dialog.Participant myParticipantObject = null;
            foreach (Dialog.Participant participant in dialog._Participants)
            {
                if (finAgent._agentInformation.Extension.Equals(participant._MediaAddress))
                    myParticipantObject = participant;
            }

            if (myParticipantObject == null)
                return null;
            Color BarBackGroundColor = Colors.Red;
            string CICPrefix = "No CIC ";
            //CIC Exist
            if (((CallVariable)dialog._MediaProperties._CallVariables[3])._Value != null && ((CallVariable)dialog._MediaProperties._CallVariables[3])._Value.Length > 5)
            {
                BarBackGroundColor = Colors.Green;
                CICPrefix = "";
            }

            Grid grid = (Grid)Calls.FindName("CallVarBar_" + dialog._ID);
            if (grid != null)
            {
                TextBlock eSkillHighLight = (TextBlock)Calls.FindName("SkillHighLight_" + dialog._ID);
                if (dialog._MediaProperties._CallVariables[4] != null)
                    eSkillHighLight.Text = CICPrefix + "Skill: " + ((CallVariable)dialog._MediaProperties._CallVariables[4])._Value;

                else
                    eSkillHighLight.Text = CICPrefix + "Skill:";
                eSkillHighLight.Background = new SolidColorBrush(BarBackGroundColor);

                TextBlock eLangHighLight = (TextBlock)Calls.FindName("LangHighLight_" + dialog._ID);
                if (dialog._MediaProperties._CallVariables[1] != null)
                    eLangHighLight.Text = "Lang: " + ((CallVariable)dialog._MediaProperties._CallVariables[1])._Value;
                else
                    eLangHighLight.Text = "Lang:";
                eLangHighLight.Background = new SolidColorBrush(BarBackGroundColor);

                TextBlock eCTIHighLight = (TextBlock)Calls.FindName("CTIHighLight_" + dialog._ID);
                if (dialog._MediaProperties._CallVariables[5] != null)
                    eCTIHighLight.Text = ((CallVariable)dialog._MediaProperties._CallVariables[5])._Value;
                else
                    eCTIHighLight.Text = "IVR";
                eCTIHighLight.Background = new SolidColorBrush(BarBackGroundColor);
                return null;
            }

            grid = new Grid();
            grid.Name = "CallVarBar_" + dialog._ID;
            RegisterName(grid.Name, grid); // Register Name of Dialog
            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(4, GridUnitType.Star);
            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition col3 = new ColumnDefinition();
            col3.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col1);
            grid.ColumnDefinitions.Add(col2);
            grid.ColumnDefinitions.Add(col3);

            TextBlock SkillHighLight = new TextBlock();
            SkillHighLight.Name = "SkillHighLight_" + dialog._ID;
            RegisterName(SkillHighLight.Name, SkillHighLight); // Register Name of Dialog
            SkillHighLight.HorizontalAlignment = HorizontalAlignment.Stretch;
            SkillHighLight.TextAlignment = TextAlignment.Center;
            SkillHighLight.FontSize = 14;
            if (dialog._MediaProperties._CallVariables[4] != null)
                SkillHighLight.Text = CICPrefix + "Skill: " + ((CallVariable)dialog._MediaProperties._CallVariables[4])._Value;
            else
                SkillHighLight.Text = CICPrefix + "Skill:";
            SkillHighLight.Foreground = new SolidColorBrush(Colors.White);
            SkillHighLight.Background = new SolidColorBrush(BarBackGroundColor);
            Grid.SetColumn(SkillHighLight, 0);

            TextBlock LangHighLight = new TextBlock();
            LangHighLight.Name = "LangHighLight_" + dialog._ID;
            RegisterName(LangHighLight.Name, LangHighLight); // Register Name of Dialog
            LangHighLight.HorizontalAlignment = HorizontalAlignment.Stretch;
            LangHighLight.TextAlignment = TextAlignment.Center;
            LangHighLight.FontSize = 14;
            if (dialog._MediaProperties._CallVariables[1] != null)
                LangHighLight.Text = "Lang: " + ((CallVariable)dialog._MediaProperties._CallVariables[1])._Value;
            else
                LangHighLight.Text = "Lang:";
            LangHighLight.Foreground = new SolidColorBrush(Colors.White);
            LangHighLight.Background = new SolidColorBrush(BarBackGroundColor);
            Grid.SetColumn(LangHighLight, 1);

            TextBlock CTIHighLight = new TextBlock();
            CTIHighLight.Name = "CTIHighLight_" + dialog._ID;
            RegisterName(CTIHighLight.Name, CTIHighLight); // Register Name of Dialog
            CTIHighLight.HorizontalAlignment = HorizontalAlignment.Stretch;
            CTIHighLight.TextAlignment = TextAlignment.Center;
            CTIHighLight.FontSize = 14;
            if (dialog._MediaProperties._CallVariables[5] != null)
                CTIHighLight.Text = ((CallVariable)dialog._MediaProperties._CallVariables[5])._Value;
            else
                CTIHighLight.Text = "IVR";
            CTIHighLight.Foreground = new SolidColorBrush(Colors.White);
            CTIHighLight.Background = new SolidColorBrush(BarBackGroundColor);
            Grid.SetColumn(CTIHighLight, 2);

            grid.Children.Add(SkillHighLight);
            grid.Children.Add(LangHighLight);
            grid.Children.Add(CTIHighLight);
            return grid;
        }
        private Grid BuildCallDetails(Dialog dialog)
        {
            Dialog.Participant myParticipantObject = null;
            foreach (Dialog.Participant participant in dialog._Participants)
            {
                if (finAgent._agentInformation.Extension.Equals(participant._MediaAddress))
                    myParticipantObject = participant;
            }

            if (myParticipantObject == null)
                return null;
            Grid grid = (Grid)Calls.FindName("CallDetails_" + dialog._ID);
            if (grid != null)
            {
                TextBlock eFromAddressBlock = (TextBlock)Calls.FindName("FromAddress_" + dialog._ID);
                if (dialog._FromAddress.Equals(finAgent._agentInformation.Extension))
                    eFromAddressBlock.Text = dialog._ToAddress;
                else
                    eFromAddressBlock.Text = dialog._FromAddress;

                Button eAnswerButton = (Button)Calls.FindName("AnswerCall_" + dialog._ID);
                Button eReleaseButton = (Button)Calls.FindName("ReleaseCall_" + dialog._ID);
                Button eHoldButton = (Button)Calls.FindName("HoldCall_" + dialog._ID);
                Button eResumeButton = (Button)Calls.FindName("ResumeCall_" + dialog._ID);
                Button eTransferButton = (Button)Calls.FindName("TransferCall_" + dialog._ID);
                Button eCTransferButton = (Button)Calls.FindName("CTransferCall_" + dialog._ID);
                Button eConferenceButton = (Button)Calls.FindName("ConferenceCall_" + dialog._ID);
                Button eConsultButton = (Button)Calls.FindName("ConsultCall_" + dialog._ID);
                Button eKeyPadButton = (Button)Calls.FindName("KeypadCall_" + dialog._ID);

                ModifyCallButton(eAnswerButton, false, Visibility.Hidden);
                ModifyCallButton(eReleaseButton, false, Visibility.Hidden);
                ModifyCallButton(eHoldButton, false, Visibility.Hidden);
                ModifyCallButton(eResumeButton, false, Visibility.Hidden);
                ModifyCallButton(eTransferButton, false, Visibility.Hidden);
                ModifyCallButton(eConferenceButton, false, Visibility.Hidden);
                ModifyCallButton(eConsultButton, false, Visibility.Hidden);
                ModifyCallButton(eKeyPadButton, false, Visibility.Hidden);
                ModifyCallButton(eCTransferButton, false, Visibility.Hidden);

                foreach (string action in myParticipantObject._Actions)
                {
                    if (action.Equals("ANSWER"))
                        ModifyCallButton(eAnswerButton, true, Visibility.Visible);
                    else if (action.Equals("TRANSFER_SST"))
                        ModifyCallButton(eTransferButton, true, Visibility.Visible);

                    if (dialog._MediaProperties._CallType.Equals("AGENT_INSIDE"))
                    {
                        if (action.Equals("HOLD"))
                            ModifyCallButton(eHoldButton, true, Visibility.Visible);
                        else if (action.Equals("RETRIEVE"))
                            ModifyCallButton(eResumeButton, true, Visibility.Visible);
                        else if (action.Equals("DROP"))
                            ModifyCallButton(eReleaseButton, true, Visibility.Visible);
                        else if (action.Equals("SEND_DTMF"))
                            ModifyCallButton(eKeyPadButton, true, Visibility.Visible);
                        else if (action.Equals("CONSULT_CALL"))
                            ModifyCallButton(eConsultButton, true, Visibility.Visible);
                        else if (action.Equals("CONFERENCE"))
                            ModifyCallButton(eConferenceButton, true, Visibility.Visible);
                        else if (action.Equals("TRANSFER"))
                            ModifyCallButton(eCTransferButton, true, Visibility.Visible);
                    }
                }
                return null;
            }

            //Build Main Call Details Grid
            grid = new Grid();
            grid.Name = "CallDetails_" + dialog._ID;
            RegisterName(grid.Name, grid); // Register Name of Dialog
            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(1.5, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col1);
            grid.ColumnDefinitions.Add(col2);

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(1, GridUnitType.Star);
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(1, GridUnitType.Star);
            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);

            TextBlock FromAddressBlock = new TextBlock();
            FromAddressBlock.Name = "FromAddress_" + dialog._ID;
            RegisterName(FromAddressBlock.Name, FromAddressBlock); // Register Name of Dialog

            FromAddressBlock.HorizontalAlignment = HorizontalAlignment.Center;
            FromAddressBlock.VerticalAlignment = VerticalAlignment.Top;
            FromAddressBlock.FontSize = 18;
            FromAddressBlock.FontFamily = new FontFamily("Arial");
            FromAddressBlock.Foreground = new SolidColorBrush(Colors.Gray);
            if (dialog._FromAddress.Equals(finAgent._agentInformation.Extension))
                FromAddressBlock.Text = dialog._ToAddress;
            else
                FromAddressBlock.Text = dialog._FromAddress;

            Grid.SetRow(FromAddressBlock, 0);
            Grid.SetColumn(FromAddressBlock, 0);

            grid.Children.Add(FromAddressBlock);

            //Build Call Actions Grid
            Grid callActionsGrid = new Grid();
            Grid.SetColumn(callActionsGrid, 1);
            Grid.SetRow(callActionsGrid, 1);
            callActionsGrid.Name = "CallActions_" + dialog._ID;
            callActionsGrid.VerticalAlignment = VerticalAlignment.Bottom;

            for (int counter = 0; counter < 5; counter++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new GridLength(1, GridUnitType.Star);
                callActionsGrid.ColumnDefinitions.Add(colDef);
            }
            Button answerButton = BuildCallButton(dialog._ID, CallButton.Answer, false, Visibility.Hidden);
            answerButton.Click += AnswerCallClick;
            Grid.SetColumn(answerButton, 4);

            Button releaseButton = BuildCallButton(dialog._ID, CallButton.Release, false, Visibility.Hidden);
            releaseButton.Click += EndCallClick;
            Grid.SetColumn(releaseButton, 4);

            Button holdButton = BuildCallButton(dialog._ID, CallButton.Hold, false, Visibility.Hidden);
            holdButton.Click += HoldCallClick;
            Grid.SetColumn(holdButton, 1);

            Button resumeButton = BuildCallButton(dialog._ID, CallButton.Resume, false, Visibility.Hidden);
            resumeButton.Click += ResumeCallClick;
            Grid.SetColumn(resumeButton, 1);

            Button transferButton = BuildCallButton(dialog._ID, CallButton.SSTransfer, false, Visibility.Hidden);
            transferButton.Click += SSTransferCallClick;
            Grid.SetColumn(transferButton, 2);

            Button cTransferButton = BuildCallButton(dialog._ID, CallButton.Transfer, false, Visibility.Hidden);
            cTransferButton.Click += TransferCallClick;
            Grid.SetColumn(transferButton, 2);

            Button conferenceButton = BuildCallButton(dialog._ID, CallButton.Conference, false, Visibility.Hidden);
            conferenceButton.Click += ConferenceCallClick;
            Grid.SetColumn(conferenceButton, 3);

            Button consultButton = BuildCallButton(dialog._ID, CallButton.Consult, false, Visibility.Hidden);
            consultButton.Click += ConsultCallClick;
            Grid.SetColumn(consultButton, 3);

            Button keyPadButton = BuildCallButton(dialog._ID, CallButton.Keypad, false, Visibility.Hidden);
            keyPadButton.Click += KeyPadClick;
            Grid.SetColumn(keyPadButton, 0);

            foreach (string action in myParticipantObject._Actions)
            {
                if (action.Equals("ANSWER"))
                    answerButton = ModifyCallButton(answerButton, true, Visibility.Visible);
                else if (action.Equals("TRANSFER_SST"))
                    transferButton = ModifyCallButton(transferButton, true, Visibility.Visible);


                if (dialog._MediaProperties._CallType.Equals("AGENT_INSIDE"))
                {
                    if (action.Equals("HOLD"))
                        holdButton = ModifyCallButton(holdButton, true, Visibility.Visible);
                    else if (action.Equals("RETRIEVE"))
                        resumeButton = ModifyCallButton(resumeButton, true, Visibility.Visible);
                    else if (action.Equals("DROP"))
                        releaseButton = ModifyCallButton(releaseButton, true, Visibility.Visible);
                    else if (action.Equals("SEND_DTMF"))
                        keyPadButton = ModifyCallButton(keyPadButton, true, Visibility.Visible);
                    else if (action.Equals("CONSULT_CALL"))
                        consultButton = ModifyCallButton(consultButton, true, Visibility.Visible);
                    else if (action.Equals("CONFERENCE")) // Single Step Transfer Only Implemented action.Equals("TRANSFER") || 
                        conferenceButton = ModifyCallButton(conferenceButton, true, Visibility.Visible);
                    else if (action.Equals("TRANSFER")) // Single Step Transfer Only Implemented action.Equals("TRANSFER") || 
                        cTransferButton = ModifyCallButton(cTransferButton, true, Visibility.Visible);
                }


            }
            callActionsGrid.Children.Add(answerButton);
            callActionsGrid.Children.Add(releaseButton);
            callActionsGrid.Children.Add(holdButton);
            callActionsGrid.Children.Add(resumeButton);
            callActionsGrid.Children.Add(transferButton);
            callActionsGrid.Children.Add(cTransferButton);
            callActionsGrid.Children.Add(consultButton);
            callActionsGrid.Children.Add(conferenceButton);
            callActionsGrid.Children.Add(keyPadButton);

            grid.Children.Add(callActionsGrid);
            return grid;
        }

        private Grid BuildSendTOIVR(Dialog dialog)
        {
            bool canSendToIVR = false;
            Dialog.Participant myParticipantObject = null;
            Grid grid = (Grid)Calls.FindName("SendCallToIVR_" + dialog._ID);

            foreach (Dialog.Participant participant in dialog._Participants)
            {
                if (finAgent._agentInformation.Extension.Equals(participant._MediaAddress))
                    myParticipantObject = participant;
            }

            if (myParticipantObject == null)
            {
                if (grid != null)
                    grid.Visibility = Visibility.Hidden;
                return null;
            }

            foreach (string action in myParticipantObject._Actions)
            {
                if (action.Equals("TRANSFER_SST"))
                {
                    canSendToIVR = true;
                }
            }

            if (!canSendToIVR)
            {
                if (grid != null)
                {
                    grid.Visibility = Visibility.Hidden;
                    return null;
                }
            }
            else
            {
                if (grid != null)
                {
                    grid.Visibility = Visibility.Visible;
                    return null;
                }
            }
            IVROptions ivrOptions = new IVROptions(finAgent, dialog._ID);

            if (grid == null)
            {
                grid = new Grid();
                grid.Name = "SendCallToIVR_" + dialog._ID;
                grid.Margin = new Thickness(0, 5, 0, 5);
                RegisterName(grid.Name, grid);//Register Name
                grid.Margin = new Thickness(0, 10, 0, 10);

                ColumnDefinition col1 = new ColumnDefinition();
                col1.Width = new GridLength(1, GridUnitType.Star);
                ColumnDefinition col2 = new ColumnDefinition();
                col2.Width = new GridLength(2, GridUnitType.Star);
                ColumnDefinition col3 = new ColumnDefinition();
                col3.Width = new GridLength(0.4, GridUnitType.Star);

                RowDefinition row1 = new RowDefinition();
                row1.Height = new GridLength(1, GridUnitType.Star);
                RowDefinition row2 = new RowDefinition();
                row2.Height = new GridLength(1, GridUnitType.Star);

                grid.ColumnDefinitions.Add(col1);
                grid.ColumnDefinitions.Add(col2);
                grid.ColumnDefinitions.Add(col3);

                grid.RowDefinitions.Add(row1);
                grid.RowDefinitions.Add(row2);

                if (!canSendToIVR)
                    grid.Visibility = Visibility.Hidden;
                else
                    grid.Visibility = Visibility.Visible;
            }

            TextBlock sendTOIVRBlock = (TextBlock)Calls.FindName("SendToIVRLabel_" + dialog._ID);
            if (sendTOIVRBlock == null)
            {
                sendTOIVRBlock = new TextBlock();
                sendTOIVRBlock.Name = "SendToIVRLabel_" + dialog._ID;
                RegisterName(sendTOIVRBlock.Name, sendTOIVRBlock);//Register Name

                sendTOIVRBlock.Foreground = new SolidColorBrush(Colors.Gray);
                sendTOIVRBlock.VerticalAlignment = VerticalAlignment.Center;
                sendTOIVRBlock.Text = "Send to IVR:";

                Grid.SetColumn(sendTOIVRBlock, 0);
                Grid.SetRow(sendTOIVRBlock, 0);
                grid.Children.Add(sendTOIVRBlock);
            }

            ComboBox IVRSelection = (ComboBox)Calls.FindName("IVRSelection_" + dialog._ID);
            if (IVRSelection == null)
            {
                IVRSelection = new ComboBox();
                IVRSelection.Name = "IVRSelection_" + dialog._ID;
                RegisterName(IVRSelection.Name, IVRSelection);//Register Name
                IVRSelection.HorizontalAlignment = HorizontalAlignment.Center;
                IVRSelection.VerticalAlignment = VerticalAlignment.Center;
                IVRSelection.Foreground = new SolidColorBrush(Colors.Gray);

                string[] ivrComboOptions = ivrOptions.GetTransferOptions();

                if (ivrComboOptions != null)
                {
                    foreach (string ivrComboOption in ivrComboOptions)
                    {
                        ComboBoxItem IVRSelectionOption = new ComboBoxItem();
                        IVRSelectionOption.Content = ivrComboOption;
                        if (ivrComboOption.Equals("Survey"))
                            IVRSelectionOption.IsSelected = true;
                        IVRSelection.Items.Add(IVRSelectionOption);
                    }
                }
                else
                {
                    ComboBoxItem IVRSelectionOption = new ComboBoxItem();
                    IVRSelectionOption.Content = "No option Avaliable";
                    IVRSelectionOption.IsSelected = true;
                    IVRSelection.Items.Add(IVRSelectionOption);
                }
                Grid.SetColumn(IVRSelection, 1);
                Grid.SetRow(IVRSelection, 0);

                grid.Children.Add(IVRSelection);
            }
            Button IVRButton = (Button)Calls.FindName("IVRCall_" + dialog._ID);
            if (IVRButton == null)
            {
                IVRButton = BuildCallButton(dialog._ID, CallButton.IVR, true, Visibility.Hidden);
                //IVRButton.Name = "IVRCall_" + dialog._ID;
                //RegisterName(IVRButton.Name, IVRButton);

                IVRButton.Visibility = Visibility.Visible;
                IVRButton.Click += SendBackToIVRClick;

                Grid.SetColumn(IVRButton, 2);
                Grid.SetRow(IVRButton, 0);

                grid.Children.Add(IVRButton);
            }

            //Adding IVR Option RJB Special Customization
            if (ivrOptions.IsExtendedParametersEnabled())
            {
                CheckBox MaleGenderCheckBox = (CheckBox)Calls.FindName("MaleGenderCheckBox_" + dialog._ID);
                if (MaleGenderCheckBox == null)
                {
                    MaleGenderCheckBox = new CheckBox();
                    MaleGenderCheckBox.Name = "MaleGenderCheckBox_" + dialog._ID;
                    RegisterName(MaleGenderCheckBox.Name, MaleGenderCheckBox);

                    MaleGenderCheckBox.Content = "Male Only";
                    MaleGenderCheckBox.Foreground = new SolidColorBrush(Colors.Gray);
                    MaleGenderCheckBox.IsChecked = true;
                    Grid.SetColumn(MaleGenderCheckBox, 0);
                    Grid.SetRow(MaleGenderCheckBox, 1);
                    grid.Children.Add(MaleGenderCheckBox);
                }

                StackPanel TransferTypeStackPanel = (StackPanel)Calls.FindName("TransferTypeStackPanel_" + dialog._ID);
                if (TransferTypeStackPanel == null)
                {
                    TransferTypeStackPanel = new StackPanel();
                    TransferTypeStackPanel.Name = "TransferTypeStackPanel_" + dialog._ID;
                    RegisterName(TransferTypeStackPanel.Name, TransferTypeStackPanel);

                    Grid.SetColumn(TransferTypeStackPanel, 1);
                    Grid.SetRow(TransferTypeStackPanel, 1);
                    Grid.SetColumnSpan(TransferTypeStackPanel, 2);

                    RadioButton InqueryRadioButton = new RadioButton();
                    InqueryRadioButton.Name = "InqueryRadioButton_" + dialog._ID;
                    RegisterName(InqueryRadioButton.Name, InqueryRadioButton);//Register Name
                    InqueryRadioButton.Content = "Inquery";
                    InqueryRadioButton.IsChecked = true;
                    InqueryRadioButton.Foreground = new SolidColorBrush(Colors.Gray);
                    TransferTypeStackPanel.Children.Add(InqueryRadioButton);

                    RadioButton ComplaintRadioButton = new RadioButton();
                    ComplaintRadioButton.Name = "ComplaintRadioButton_" + dialog._ID;
                    RegisterName(ComplaintRadioButton.Name, ComplaintRadioButton);//Register Name
                    ComplaintRadioButton.Content = "Complaint";
                    ComplaintRadioButton.Foreground = new SolidColorBrush(Colors.Gray);
                    TransferTypeStackPanel.Children.Add(ComplaintRadioButton);

                    RadioButton SupportRadioButton = new RadioButton();
                    SupportRadioButton.Name = "SupportRadioButton_" + dialog._ID;
                    RegisterName(SupportRadioButton.Name, SupportRadioButton);//Register Name
                    SupportRadioButton.Content = "Support";
                    SupportRadioButton.Foreground = new SolidColorBrush(Colors.Gray);
                    TransferTypeStackPanel.Children.Add(SupportRadioButton);

                    grid.Children.Add(TransferTypeStackPanel);
                }
            }
            return grid;
        }
        private Grid BuildRJBCallInformation(Dialog dialog)
        {
            Grid grid = (Grid)Calls.FindName("RJBCallInformation_" + dialog._ID);

            if (grid != null)
            {
                TextBlock eSegment = (TextBlock)Calls.FindName("RJBCallInformationSegment_" + dialog._ID);
                TextBlock ePOS = (TextBlock)Calls.FindName("RJBCallInformationPOS_" + dialog._ID);
                TextBlock eLanguage = (TextBlock)Calls.FindName("RJBCallInformationLanguage_" + dialog._ID);
                TextBlock eIvrOption = (TextBlock)Calls.FindName("RJBCallInformationIVR_" + dialog._ID);
                TextBlock eCIC = (TextBlock)Calls.FindName("RJBCallInformationSkillCIC_" + dialog._ID);
                TextBlock eDialedNumber = (TextBlock)Calls.FindName("RJBCallInformationDialedNumber_" + dialog._ID);
                try
                {
                    eSegment.Text = "Segment: " + ((CallVariable)dialog._MediaProperties._CallVariables[4])._Value;
                    ePOS.Text = "POS: " + ((CallVariable)dialog._MediaProperties._CallVariables[0])._Value;
                    eLanguage.Text = "Language: " + ((CallVariable)dialog._MediaProperties._CallVariables[1])._Value;
                    eIvrOption.Text = "IVR Option: " + ((CallVariable)dialog._MediaProperties._CallVariables[2])._Value;
                    eCIC.Text = "CIC: " + ((CallVariable)dialog._MediaProperties._CallVariables[3])._Value;
                    eDialedNumber.Text = "Dialed number: " + ((CallVariable)dialog._MediaProperties._CallVariables[6])._Value;
                }
                catch (Exception ex)
                {
                    if (TraceStatus.Equals("true"))
                    {
                        Trace.Write("CXConnect: Error in reading call variables during RJB call information section", ex.Message);
                        Trace.Write(ex);
                    }
                }
                return null;
            }
            grid = new Grid();
            grid.Name = "RJBCallInformation_" + dialog._ID;
            grid.Margin = new Thickness(0, 5, 0, 5);
            RegisterName(grid.Name, grid);
            StackPanel RJBCallInfoStackPanel = new StackPanel();

            TextBlock Segment = new TextBlock();
            TextBlock pos = new TextBlock();
            TextBlock language = new TextBlock();
            TextBlock ivrOption = new TextBlock();
            TextBlock CIC = new TextBlock();
            TextBlock dialedNumber = new TextBlock();
            //Button openCRMButton = new Button();

            Segment.Name = "RJBCallInformationSegment_" + dialog._ID;
            RegisterName(Segment.Name, Segment);

            Segment.Text = "Segment: ";
            Segment.HorizontalAlignment = HorizontalAlignment.Left;
            Segment.VerticalAlignment = VerticalAlignment.Center;
            Segment.Foreground = new SolidColorBrush(Colors.Gray);

            pos.Name = "RJBCallInformationPOS_" + dialog._ID;
            RegisterName(pos.Name, pos);

            pos.Text = "POS: ";
            pos.HorizontalAlignment = HorizontalAlignment.Left;
            pos.VerticalAlignment = VerticalAlignment.Center;
            pos.Foreground = new SolidColorBrush(Colors.Gray);

            language.Name = "RJBCallInformationLanguage_" + dialog._ID;
            RegisterName(language.Name, language);

            language.Text = "Language: ";
            language.HorizontalAlignment = HorizontalAlignment.Left;
            language.VerticalAlignment = VerticalAlignment.Center;
            language.Foreground = new SolidColorBrush(Colors.Gray);

            ivrOption.Name = "RJBCallInformationIVR_" + dialog._ID;
            RegisterName(ivrOption.Name, ivrOption);

            ivrOption.Text = "IVR: ";
            ivrOption.HorizontalAlignment = HorizontalAlignment.Left;
            ivrOption.VerticalAlignment = VerticalAlignment.Center;
            ivrOption.Foreground = new SolidColorBrush(Colors.Gray);

            CIC.Name = "RJBCallInformationCIC_" + dialog._ID;
            RegisterName(CIC.Name, CIC);

            CIC.Text = "CIC: ";
            CIC.HorizontalAlignment = HorizontalAlignment.Left;
            CIC.VerticalAlignment = VerticalAlignment.Center;
            CIC.Foreground = new SolidColorBrush(Colors.Gray);

            //Remove Open CRM button and open CRM in case of first call fetch
            //openCRMButton.Name = "RJBCallInformationCRMPopupButton_" + dialog._ID;
            //RegisterName(openCRMButton.Name, openCRMButton);

            //openCRMButton.Content = "Open CRM Page";
            //openCRMButton.HorizontalAlignment = HorizontalAlignment.Right;
            //openCRMButton.VerticalAlignment = VerticalAlignment.Center;
            //openCRMButton.Foreground = new SolidColorBrush(Colors.White);
            //openCRMButton.Background = new SolidColorBrush(Colors.Black);
            //openCRMButton.Padding = new Thickness(5, 2, 5, 2);
            //openCRMButton.FontWeight = FontWeights.Medium;
            //openCRMButton.Click += OpenCRMButtonClick;

            dialedNumber.Name = "RJBCallInformationDialedNumber_" + dialog._ID;
            RegisterName(dialedNumber.Name, dialedNumber);

            dialedNumber.Text = "Dialed number: ";
            dialedNumber.HorizontalAlignment = HorizontalAlignment.Left;
            dialedNumber.VerticalAlignment = VerticalAlignment.Center;
            dialedNumber.Foreground = new SolidColorBrush(Colors.Gray);

            Grid horizontalRowGrid = new Grid();

            horizontalRowGrid.Children.Add(Segment);
            //horizontalRowGrid.Children.Add(openCRMButton);

            RJBCallInfoStackPanel.Children.Add(horizontalRowGrid);
            RJBCallInfoStackPanel.Children.Add(CIC);
            RJBCallInfoStackPanel.Children.Add(pos);
            RJBCallInfoStackPanel.Children.Add(language);
            RJBCallInfoStackPanel.Children.Add(ivrOption);
            RJBCallInfoStackPanel.Children.Add(dialedNumber);

            try
            {
                Segment.Text = "Segment: " + ((CallVariable)dialog._MediaProperties._CallVariables[4])._Value;
                pos.Text = "POS: " + ((CallVariable)dialog._MediaProperties._CallVariables[0])._Value;
                language.Text = "Language: " + ((CallVariable)dialog._MediaProperties._CallVariables[1])._Value;
                ivrOption.Text = "IVR Option: " + ((CallVariable)dialog._MediaProperties._CallVariables[2])._Value;
                CIC.Text = "CIC: " + ((CallVariable)dialog._MediaProperties._CallVariables[3])._Value;
                dialedNumber.Text = "Dialed number: " + ((CallVariable)dialog._MediaProperties._CallVariables[6])._Value;
            }
            catch (Exception ex)
            {
                if (TraceStatus.Equals("true"))
                {
                    Trace.Write("CXConnect: Error in reading call variables during RJB call information section", ex.Message);
                    Trace.Write(ex);
                }
            }
            grid.Children.Add(RJBCallInfoStackPanel);


            return grid;
        }
        private Expander BuildCallVariablesExpander(Dialog dialog)
        {
            if (dialog == null)
                return null;
            Expander expander = (Expander)Calls.FindName("CallVariablesGroupBox_" + dialog._ID);
            if (expander != null)
            {
                for (int count = 0; count < 10; count++)
                {
                    Label labelInfo = (Label)Calls.FindName("CallVariable" + (count + 1) + "_" + dialog._ID);
                    if (labelInfo != null)
                        labelInfo.Content = ((CallVariable)dialog._MediaProperties._CallVariables[count])._Value;
                }
                return null;
            }

            expander = new Expander();
            expander.Name = "CallVariablesGroupBox_" + dialog._ID;
            RegisterName(expander.Name, expander);

            expander.Foreground = new SolidColorBrush(Colors.Gray);
            expander.Header = "Call Variables";

            ScrollViewer scroll = new ScrollViewer();
            scroll.Name = "CallVariablesScroll_" + dialog._ID;
            scroll.Margin = new Thickness(10, 10, 10, 10);

            Grid grid = new Grid();
            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(3, GridUnitType.Star);
            //ColumnDefinition col3 = new ColumnDefinition();
            //col3.Width = new GridLength(0.3, GridUnitType.Star);
            grid.ColumnDefinitions.Add(col1);
            grid.ColumnDefinitions.Add(col2);
            //grid.ColumnDefinitions.Add(col3);

            for (int count = 0; count < 10; count++)
                grid.RowDefinitions.Add(new RowDefinition());

            for (int count = 0; count < 10; count++)
            {
                Label label = new Label();
                label.Content = "Call Var " + (count + 1);
                label.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(label, count);
                Grid.SetColumn(label, 0);
                grid.Children.Add(label);
                try
                {
                    Label labelInfo = new Label();
                    labelInfo.Name = "CallVariable" + (count + 1) + "_" + dialog._ID;
                    RegisterName(labelInfo.Name, labelInfo);
                    labelInfo.Content = ((CallVariable)dialog._MediaProperties._CallVariables[count])._Value;
                    labelInfo.VerticalAlignment = VerticalAlignment.Center;
                    Grid.SetRow(labelInfo, count);
                    Grid.SetColumn(labelInfo, 1);
                    /*<Image Grid.Row="0" Grid.Column="2" x:Name="CallVariable1Image" Source="Images/copyData.png" Width="20" Height="20"  HorizontalAlignment="Center" VerticalAlignment="Center"/>*/

                    grid.Children.Add(labelInfo);
                }
                catch (Exception ex)
                {
                    if (TraceStatus.Equals("true"))
                    {
                        Trace.Write("CXConnect: Error in reading call variables during call variable Expander section", ex.Message);
                        Trace.Write(ex);
                    }

                }
            }

            scroll.Content = grid;
            expander.Content = scroll;
            return expander;
        }
        public void FireStartCallEvent(Dialog dialog, bool activityLog)
        {
            try
            {
                //List<LookupRequestItem> itemsList = new List<LookupRequestItem>();
                //itemsList.Add(new LookupRequestItem("AgentID", finAgent._agentInformation.AgentID));
                //itemsList.Add(new LookupRequestItem("AgentExt", finAgent._agentInformation.Extension));
                //itemsList.Add(new LookupRequestItem("FromAddress", dialog._FromAddress));
                //itemsList.Add(new LookupRequestItem("ToAddress", dialog._ToAddress));
                //itemsList.Add(new LookupRequestItem("DialNumber", dialog._MediaProperties._DialedNumber));
                //itemsList.Add(new LookupRequestItem("TrackLog", activityLog.ToString()));
                //itemsList.Add(new LookupRequestItem("CallStart", "true"));


                ////CIC Exist
                //if (((CallVariable)dialog._MediaProperties._CallVariables[3])._Value != null)
                //{
                //    //check the CIC length is 16 digits
                //    string CICFormatted = ((CallVariable)dialog._MediaProperties._CallVariables[3])._Value.Trim();

                //    while (CICFormatted.Length < 16)
                //        CICFormatted = "0" + CICFormatted;
                //    itemsList.Add(new LookupRequestItem("CallVar4", CICFormatted));
                //}
                //else
                //    itemsList.Add(new LookupRequestItem("CallVar4", "NA"));

                //Dispatcher.Invoke(() =>
                //{
                //    CtiLookupRequest data = new CtiLookupRequest(Guid.NewGuid(), base.ApplicationName, "phonecall", dialog._FromAddress, dialog._MediaProperties._DNIS);
                //    data.Items.AddRange(itemsList);
                //    FireRequestAction(new RequestActionEventArgs("*", CtiLookupRequest.CTILOOKUPACTIONNAME, GeneralFunctions.Serialize<CtiLookupRequest>(data)));
                //});

            }
            catch (Exception e)
            {
                if (TraceStatus.Equals("true"))
                {
                    System.Diagnostics.Trace.Write("CXConnect: Error during firing CRM event start" + e.Message);
                    System.Diagnostics.Trace.Write(e);
                }
                //Nothing to do in case of error. It will not popup. that is it.
            }
        }
        public void FireEndCallEvent(Dialog dialog)
        {
            try
            {
                //List<LookupRequestItem> itemsList = new List<LookupRequestItem>();
                //itemsList.Add(new LookupRequestItem("AgentID", finAgent._agentInformation.AgentID));
                //itemsList.Add(new LookupRequestItem("AgentExt", finAgent._agentInformation.Extension));
                //itemsList.Add(new LookupRequestItem("FromAddress", dialog._FromAddress));
                //itemsList.Add(new LookupRequestItem("ToAddress", dialog._ToAddress));
                //itemsList.Add(new LookupRequestItem("DialNumber", dialog._MediaProperties._DialedNumber));
                //itemsList.Add(new LookupRequestItem("TrackLog", "false"));
                //itemsList.Add(new LookupRequestItem("CallStart", "false"));

                ////CIC Exist
                //if (((CallVariable)dialog._MediaProperties._CallVariables[3])._Value != null)
                //{
                //    //check the CIC length is 16 digits
                //    string CICFormatted = ((CallVariable)dialog._MediaProperties._CallVariables[3])._Value;

                //    while (CICFormatted.Length < 16)
                //        CICFormatted = "0" + CICFormatted;
                //    itemsList.Add(new LookupRequestItem("CallVar4", CICFormatted));
                //}
                //else
                //    itemsList.Add(new LookupRequestItem("CallVar4", "NA"));

                //Dispatcher.Invoke(() =>
                //{
                //    CtiLookupRequest data = new CtiLookupRequest(Guid.NewGuid(), base.ApplicationName, "phonecall", dialog._FromAddress, dialog._MediaProperties._DNIS);
                //    data.Items.AddRange(itemsList);
                //    FireRequestAction(new RequestActionEventArgs("*", CtiLookupRequest.CTILOOKUPACTIONNAME, GeneralFunctions.Serialize<CtiLookupRequest>(data)));
                //});

            }
            catch (Exception e)
            {
                if (TraceStatus.Equals("true"))
                {
                    System.Diagnostics.Trace.Write("CXConnect: Error during firing CRM event end" + e.Message);
                    System.Diagnostics.Trace.Write(e);
                }

                //Nothing to do in case of error. It will not popup. that is it.
            }
        }
        public void FireDisconnectEvent()
        {
            LogMessage("Loading Screen Again as per FinAgent Request. seems XMPP was down");
            try
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => LoadScreen(ScreenName.LoginLoadingScreen)));
            }
            catch (Exception)
            {

            }
        }
        public void FireUSDEvent(Dialog dialog)
        {
            if (dialog == null)
                return;
            bool isCallStart = false;
            bool isCallEnd = false;

            if (dialog._DialogEvent != null && dialog._State != null)
            {
                if (dialog._DialogEvent.Equals("POST") && dialog._State.Equals("ALERTING")) // Call Ringing Event
                {
                    LogMessage("New Call started : Call From: " + dialog._FromAddress + " and Call To: " + dialog._ToAddress + " and dialog status is : " + dialog._State);
                    isCallStart = true;
                }
                else if (dialog._DialogEvent.Equals("DELETE") && dialog._State.Equals("ACTIVE")) // Call Terminated and call will be Transferred
                {
                    LogMessage("Call Termination event as you released the call : Call From: " + dialog._FromAddress + " and Call To: " + dialog._ToAddress);
                    isCallEnd = true;
                }
                else if (dialog._DialogEvent.Equals("DELETE") && dialog._State.Equals("DROPPED")) // Call Terminated and caller hangup
                {
                    LogMessage("Call Termination event and caller dropped the call : Call From: " + dialog._FromAddress + " and Call To: " + dialog._ToAddress);
                    isCallEnd = true;
                }
                else if (dialog._DialogEvent.Equals("DELETE")) // Call Terminated and caller hangup for any other reason
                {
                    LogMessage("Call Termination event  : Call From: " + dialog._FromAddress + " and Call To: " + dialog._ToAddress + ", Call State is: " + dialog._State);
                    isCallEnd = true;
                }
                else if (dialog._DialogEvent.Equals("RunningCall") && !dialog._State.Equals("DROPPED") && !dialog._State.Equals("FAILED")) // We found running call
                {
                    LogMessage("Call running event and call still active : Call From: " + dialog._FromAddress + " and Call To: " + dialog._ToAddress);
                    isCallStart = true;
                }
            }
            else
            {
                foreach (Dialog.Participant participant in dialog._Participants)
                {
                    if (participant._MediaAddress.Equals(finAgent._agentInformation.Extension)) //Checking my status
                    {
                        if (participant._State.Equals("DROPPED")) // call is not active , and this is call terminate event
                        {
                            LogMessage("We received message event without event discription. Seems system just started while call was active, we will terminate the call without firing end event");
                        }
                        else if (participant._State.Equals("INITIATING"))
                        {
                            LogMessage("We received message event without event discription. Seems system just started while call is active, your status is: " + participant._State);
                        }
                    }
                }
            }

            if (isCallStart)
            {
                try
                {
                    new Thread( () =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        if (CallAUDStart.Equals("true") || CallAUDStart.Equals("yes"))
                        {
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new System.Action(() =>
                            {
                                new AUDOption(finAgent, dialog, CTIWebServer).StartCallAUDRecord();
                            }));
                        }
                        if (LogCallStart.Equals("true") || LogCallStart.Equals("yes"))
                        {
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new System.Action(() =>
                            {
                                new ExtraServlets(finAgent, dialog, CTIWebServer).OnCallStartEvent();
                            }));
                        }

                        if (FireCallStartEvent.Equals("true") || FireCallStartEvent.Equals("yes"))
                            FireStartCallEvent(dialog, true);
                    }).Start();
                }
                catch (Exception e)
                {
                    if (TraceStatus.Equals("true"))
                    {
                        System.Diagnostics.Trace.Write("CXConnect: Error in call initiation" + e.Message);
                        System.Diagnostics.Trace.Write(e);
                    }
                    return;
                }
            }

            if (isCallEnd)
            {
                try
                {
                    new Thread( () =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        if (CallAUDEnd.Equals("true") || CallAUDEnd.Equals("yes"))
                        {
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new System.Action(() =>
                            {
                                new AUDOption(finAgent, dialog, CTIWebServer).EndCallAUDRecord();
                            }));
                        }
                        if (LogCallEnd.Equals("true") || LogCallEnd.Equals("yes"))
                        {
                            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, new System.Action(() =>
                            {
                                new ExtraServlets(finAgent, dialog, CTIWebServer).OnCallEndEvent();
                            }));
                        }
                        if (FireCallEndEvent.Equals("true") || FireCallEndEvent.Equals("yes"))
                            FireEndCallEvent(dialog);
                    }).Start();
                }
                catch (Exception e)
                {
                    if (TraceStatus.Equals("true"))
                    {
                        System.Diagnostics.Trace.Write("Error in call end" + e.Message);
                        System.Diagnostics.Trace.Write(e);
                    }
                    return;
                }
            }
        }
        public void TestEvent()
        {
            //Dictionary<string, string> eventParams = new Dictionary<string, string>();
            //eventParams.Add("FromAddress", "1072");
            //eventParams.Add("ToAddress", "1071");
            //eventParams.Add("DNIS", "1071");
            //eventParams.Add("DialNumber", "1071");
            //for (int counter = 0; counter < 10; counter++)
            //    eventParams.Add("CallVar" + (counter + 1), "Sample Data");
            //FireEvent("FinesseNewCall", eventParams);
        }


        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (finAgent == null)
                return;
            try
            {
                if (finAgent._agentInformation.Status.Equals("LOGOUT"))
                    Environment.Exit(0);
                else
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure, you want to close the application.. Please note that your status does not allow this, System will logout.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (finAgent.SignOut("LOGOUT", null))
                            Environment.Exit(0);
                        else
                        {
                            MessageBox.Show("We can not sign you out automatically.. Your request logout already sent to Finesse and it will happen soon. please wait", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            e.Cancel = true;
                        }
                    }
                    else
                        e.Cancel = true;

                }
            }
            catch (Exception)
            {

            }
        }
    }

}

