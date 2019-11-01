using FinesseClient;
using FinesseClient.Common;
using FinesseClient.Model;
using FinesseClient.View;
using FinesseDE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace FinesseDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,FinView
    {
        private FinAgent _finAgent;
        public CXProperties _properties;
        private Window _loginWindow;
        private Window _loadingWindow;
        private Dictionary<IView,IModel> MVVM = new Dictionary<IView, IModel>();
        Binding _interactionSpaceBinding;
        private enum DialPadType { MakeCall,SSTransfer,ConsultTransfer,ConferenceCall,Keypad}
        //private List
        public MainWindow()
        {
            _finAgent = new FinAgent();
            DataContext = _finAgent;
            InitializeComponent();
            InitializeViews();
            _interactionSpaceBinding = new System.Windows.Data.Binding("IsInteractionSpace");
            _interactionSpaceBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            _interactionSpaceBinding.Converter = new System.Windows.Controls.BooleanToVisibilityConverter();
            _interactionSpaceBinding.Source = _finAgent;
        }
        public MainWindow(FinAgent finAgent, Window loginWindow,Window loadingWindow)
        {
            _finAgent = finAgent;
            _loginWindow = loginWindow;
            _loadingWindow = loadingWindow;
            _finAgent.FinView = this;
            _properties = (loginWindow as Login).properties;
            DataContext = _finAgent;
            InitializeComponent();
            InitializeViews();
            _interactionSpaceBinding = new System.Windows.Data.Binding("IsInteractionSpace");
            _interactionSpaceBinding.Mode = System.Windows.Data.BindingMode.TwoWay;
            _interactionSpaceBinding.Converter = new System.Windows.Controls.BooleanToVisibilityConverter();
            _interactionSpaceBinding.Source = _finAgent;
        }

        private void InitializeViews()
        {
            IView myMenuSpaceView = new MenuSpace();
            IModel myMenuSpaceModel = new MenuSpaceModel();
            myMenuSpaceModel.SetFinAgent(_finAgent);
            myMenuSpaceView.SetContext(myMenuSpaceModel,this);

            MVVM.Add(myMenuSpaceView, myMenuSpaceModel);
            LoadView(myMenuSpaceView);
            
        }
        private void LoadView(IView view)
        {
            switch (view.GetLocation())
            {
                case Screen.MainGrid:
                    MainGrid.Children.Add(view as UserControl);
                    break;
            }

        }
        private void UnLoadView(IView view)
        {
            switch (view.GetLocation())
            {
                case Screen.MainGrid:
                    MainGrid.Children.Remove(view as UserControl);
                    break;
            }

        }
        public void FireErrorMessage(string msg)
        {
            if (CurrentStatus != null)
                CurrentStatus.Text = "Error : " + msg;
        }

        public void FireLoadingMessage(string msg)
        {
            if(CurrentStatus != null)
                CurrentStatus.Text = "Event : (" + msg + ")";
        }

        public void FireLoadLoginScreen()
        {
            Hide();
            _loginWindow.Show();
            _finAgent.FinView = _loginWindow as FinView;
            Connected.Visibility = Visibility.Visible;
            Reconnect.Visibility = Visibility.Hidden;
        }

        public void FireNewEvent()
        {
            if (_finAgent.AgentInformation.MessageEvent == null)
                return;
            if (_finAgent.AgentInformation.MessageEvent.MessageType == null)
                return;
            if (_finAgent.AgentInformation.MessageEvent.MessageType.Equals("user"))
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => _finAgent.FireLoadingMessage("Agent status:" + _finAgent.AgentInformation.SelectedVoiceStatus.StatusLabel)));
                //System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => _finAgent.FireLoadingMessage("Agent status:"+_finAgent.AgentInformation.SelectedVoiceStatus.StatusLabel)));
            }
            else if (_finAgent.AgentInformation.MessageEvent.MessageType.Equals("call"))
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => _finAgent.FireLoadingMessage("Call Message")));
                //System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => _finAgent.FireLoadingMessage("Call Message")));
            }
            else if (_finAgent.AgentInformation.MessageEvent.MessageType.Equals("error"))
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => _finAgent.FireLoadingMessage("Error Message: " + _finAgent.AgentInformation.MessageEvent.ErrorMsg)));
                //System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() => _finAgent.FireLoadingMessage("Error Message: "+ _finAgent.AgentInformation.MessageEvent.ErrorMsg)));
            }

            foreach (IView view in MVVM.Keys)
                view.FireNewEvent();
        }

        public void FireReLoginEvent()
        {
            //Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
            //{
            //    _finAgent.LoadAgentInformation();
            //    _finAgent.LoadCallInformation();

            //}));
            //System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
            //{
            //    _finAgent.LoadAgentInformation();
            //    _finAgent.LoadCallInformation();
                
            //}));
            Connected.Visibility = Visibility.Visible;
            Reconnect.Visibility = Visibility.Hidden;
        }

        public void FireCallEvent(Dialog dialog)
        {
            //MessageBox.Show("ID:"+dialog.ID+", State:"+dialog.State);
            if (dialog == null)
                return;
            bool isCallStart = false;
            bool isCallEnd = false;

            if(dialog.DialogEvent != null && dialog.State != null)
            {
                if(dialog.DialogEvent.Equals("POST") && (dialog.State.Equals("ALERTING") || dialog.State.Equals("INITIATING"))) // Call Ringing Event
                {
                    _finAgent.FireLogMessage("New Call started : Call From: " + dialog.FromAddress + " and Call To: " + dialog.ToAddress + " and dialog status is : "+dialog.State);
                    isCallStart = true;
                }
                else if (dialog.DialogEvent.Equals("DELETE") && dialog.State.Equals("ACTIVE")) // Call Terminated and call will be Transferred
                {
                    _finAgent.FireLogMessage("Call Termination event as you released the call : Call From: " + dialog.FromAddress + " and Call To: " + dialog.ToAddress);
                    isCallEnd = true;
                }
                else if(dialog.DialogEvent.Equals("DELETE") && dialog.State.Equals("DROPPED")) // Call Terminated and caller hangup
                {
                    _finAgent.FireLogMessage("Call Termination event and caller dropped the call : Call From: "+dialog.FromAddress+" and Call To: "+dialog.ToAddress);
                    isCallEnd = true;
                }
                else if (dialog.DialogEvent.Equals("DELETE")) // Call Terminated and caller hangup
                {
                    _finAgent.FireLogMessage("Call Termination event  : Call From: " + dialog.FromAddress + " and Call To: " + dialog.ToAddress + ", Call State is: "+dialog.State);
                    isCallEnd = true;
                }
                else if (dialog.DialogEvent.Equals("RunningCall") && !dialog.State.Equals("DROPPED") && !dialog.State.Equals("FAILED")) // We found running call
                {
                    _finAgent.FireLogMessage("Call running event and call still active : Call From: " + dialog.FromAddress + " and Call To: " + dialog.ToAddress);
                    isCallStart = true;
                }
            } else
            {
               foreach(Dialog.Participant participant in dialog.Participants)
                {
                    if(participant.Me) //Checking my status
                    {
                        if (participant.State.Equals("DROPPED")) // call is not active , and this is call terminate event
                        {
                            _finAgent.FireLogMessage("We received message event without event discription. Seems system just started while call was active, we will terminate the call without firing end event");
                        }
                        else if (participant.State.Equals("INITIATING"))
                        {
                            _finAgent.FireLogMessage("We received message event without event discription. Seems system just started while call is active, your status is: "+participant.State);
                        }
                    }
                }
            }

            if(isCallEnd && _finAgent.AgentInformation.MessageEvent.MessageType.Equals("call"))
            {

                //new Thread(delegate ()
                //{
                    Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new System.Action(() =>
                    {
                        IView view = CheckView(dialog.ID);
                        if (view != null)
                            UnLoadView(view);
                    }));
                    //System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new System.Action(() =>
                    //{
                    //    IView view = CheckView(dialog.ID);
                    //    if (view != null)
                    //        UnLoadView(view);
                    //}));

                //}).Start();
                return;
            }

            if (isCallStart && _finAgent.AgentInformation.MessageEvent.MessageType.Equals("call"))
            {

                //new Thread(delegate ()
                //{
                    Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.ApplicationIdle, new System.Action(() =>
                    {
                        IView view = CheckView(dialog.ID);
                        if (view == null)
                        {
                            view = new InteractionSpace();
                            IModel myInteractionSpaceModel = new InteractionSpaceModel();
                            myInteractionSpaceModel.SetFinAgent(_finAgent);
                            myInteractionSpaceModel.SetDialogID(dialog.ID);
                            view.SetContext(myInteractionSpaceModel, this);

                            MVVM.Add(view, myInteractionSpaceModel);
                            LoadView(view);
                            _finAgent.IsInteractionSpace = true;
                        }
                    }));
                    //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle, new System.Action(() =>
                    //{
                    //    IView view = CheckView(dialog.ID);
                    //    if (view == null)
                    //    {
                    //        view = new InteractionSpace();
                    //        IModel myInteractionSpaceModel = new InteractionSpaceModel();
                    //        myInteractionSpaceModel.SetFinAgent(_finAgent);
                    //        myInteractionSpaceModel.SetDialogID(dialog.ID);
                    //        view.SetContext(myInteractionSpaceModel, this);

                    //        MVVM.Add(view, myInteractionSpaceModel);
                    //        LoadView(view);
                    //        _finAgent.IsInteractionSpace = true;
                    //    }
                    //}));
                //}).Start();
        }

            foreach (IView view in MVVM.Keys)
                view.FireCallEvent(dialog);
        }

        private void MenuItem_Selected(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            _finAgent.IsInteractionSpace = false;
            foreach (IView view in MVVM.Keys)
            {
                if (view is MenuSpace)
                {
                    _finAgent.IsMenuSpace = true;
                    MenuSpace menuSpace = view as MenuSpace;
                    menuSpace.MenuTabs.SelectedIndex = Int32.Parse(menuItem.Tag as String,0);
                }
            }
        }

        public void SetContext(IModel model,FinView finView)
        {
        }

        public Screen GetLocation()
        {
            return Screen.Toolbar;
        }

        private void DialPad_Click(object sender, RoutedEventArgs e)
        {
            

            Button button = sender as Button;
            if (button.Name.Equals("CloseDialPad"))
            {
                DialPad.Visibility = Visibility.Collapsed;
            }else if (button.Name.Equals("Dial"))
            {
                string dialedNumber = DialNumber.Text;
                if (dialedNumber == null || dialedNumber == "")
                {
                    DialNumber.Focus();
                    return;
                }
                if (Dial.Tag.Equals("MakeCall"))
                {
                    _finAgent.MakeCall(dialedNumber);

                }else if (Dial.Tag.Equals("SSTransfer"))
                {
                    _finAgent.SSTransferCall(DialPad.Tag as String, dialedNumber);
                }else if (Dial.Tag.Equals("ConsultTransfer"))
                {
                    _finAgent.ConsultCall(DialPad.Tag as String, dialedNumber);
                }
                else if (Dial.Tag.Equals("ConferenceCall"))
                {
                    _finAgent.ConferenceCall(DialPad.Tag as String, dialedNumber);
                }
                else if (Dial.Tag.Equals("Keypad"))
                {
                    _finAgent.KeypadSendDTMF(DialPad.Tag as String, dialedNumber);
                }
                DialPad.Visibility = Visibility.Collapsed;
            }
            else if (button.Name.Equals("Clear"))
            {
                DialNumber.Text = "";
            }else
            {
                DialNumber.Text += button.Content;
            }
            DialNumber.Focus();
        }
        private void OpenDialPad(string dialogID, DialPadType type)
        {
            switch (type)
            {
                case DialPadType.MakeCall:
                    Dial.Tag = "MakeCall";
                    Dial.Content = "Dial";
                    DialPad.Visibility = Visibility.Visible;
                    DialNumber.Focus();
                    break;
                case DialPadType.SSTransfer:
                    Dial.Tag = "SSTransfer";
                    Dial.Content = "Dial";
                    DialPad.Tag = dialogID;
                    DialPad.Visibility = Visibility.Visible;
                    DialNumber.Focus();
                    break;
                case DialPadType.ConsultTransfer:
                    Dial.Tag = "ConsultTransfer";
                    Dial.Content = "Dial";
                    DialPad.Tag = dialogID;
                    DialPad.Visibility = Visibility.Visible;
                    DialNumber.Focus();
                    break;
                case DialPadType.ConferenceCall:
                    Dial.Tag = "ConferenceCall";
                    Dial.Content = "Dial";
                    DialPad.Tag = dialogID;
                    DialPad.Visibility = Visibility.Visible;
                    DialNumber.Focus();
                    break;
                case DialPadType.Keypad:
                    Dial.Tag = "Keypad";
                    Dial.Content = "Send";
                    DialPad.Tag = dialogID;
                    DialPad.Visibility = Visibility.Visible;
                    DialNumber.Focus();
                    break;
            }
        }

        private void MakeCall_Click(object sender, RoutedEventArgs e)
        {
            OpenDialPad(null, DialPadType.MakeCall);
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Name.Equals("Answer"))
            {
                _finAgent.FireLoadingMessage("Answer Call");
                _finAgent.AnswerCall(button.Tag as String);
            }
            else if (button.Name.Equals("Transfer"))
            {
                _finAgent.FireLoadingMessage("Direct Transfer");
                OpenDialPad(button.Tag as String, DialPadType.SSTransfer); 
            }
            else if (button.Name.Equals("CTransfer"))
            {
                _finAgent.FireLoadingMessage("Transfer Call");
                _finAgent.TransferCall(button.Tag as String);
            }
            else if (button.Name.Equals("Conference"))
            {
                _finAgent.FireLoadingMessage("Conference Call");
                OpenDialPad(button.Tag as String, DialPadType.ConferenceCall);
            }
            else if (button.Name.Equals("Consult"))
            {
                _finAgent.FireLoadingMessage("Consult Call");
                OpenDialPad(button.Tag as String, DialPadType.ConsultTransfer);
            }
            else if (button.Name.Equals("Keypad"))
            {
                _finAgent.FireLoadingMessage("Keypad Clicked");
                OpenDialPad(button.Tag as String, DialPadType.Keypad);
            }
            else if (button.Name.Equals("Hold"))
            {
                _finAgent.FireLoadingMessage("Hold Call");
                _finAgent.HoldCall(button.Tag as String);
            }
            else if (button.Name.Equals("Resume"))
            {
                _finAgent.FireLoadingMessage("Resume Call");
                _finAgent.ResumeCall(button.Tag as String);
            }
            else if (button.Name.Equals("Release"))
            {
                _finAgent.FireLoadingMessage("Release Call");
                if (!_finAgent.ReleaseCall(button.Tag as String))
                {
                    Dialog dialog = _finAgent.FindDialog(button.Tag as String);
                    if(dialog != null)
                        Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => _finAgent.AgentInformation.Dialogs.Remove(dialog)));
                    //System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => _finAgent.AgentInformation.Dialogs.Remove(dialog)));
                }
            }

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_finAgent.AgentInformation.Status.Equals("LOGOUT"))
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
                        e.Cancel = true;
                    }
                }
                else
                    e.Cancel = true;

            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = "FGDP V2.281 - "+ _finAgent.AgentInformation.Name + " @ Extension( "+ _finAgent.AgentInformation.Extension + " )";
        }

        private void VoiceStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (StatusComboBox.SelectedItem == null)
                    return;
                _finAgent.FireLoadingMessage("Change status to:" + (StatusComboBox.SelectedItem as VoiceStatus).StatusLabel);
                _finAgent.ChangeAgentVoiceStatus(StatusComboBox.SelectedItem as VoiceStatus);
            }
            catch (Exception)
            {

            }
        }

        private void InteractionsToolBar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InteractionsToolBar.SelectedItem == null)
                return;
            Dialog dialog = InteractionsToolBar.SelectedItem as Dialog;

            //Now Remove Visibility for all
            foreach(IView view in MVVM.Keys)
            {
                if(view is InteractionSpace)
                {

                    if ((MVVM[view] as InteractionSpaceModel).DialogID.Equals(dialog.ID))
                    {
                        //(view as InteractionSpace).MySpace.Visibility = Visibility.Visible;
                        InteractionSpace myInteractionSpace = view as InteractionSpace;
                        myInteractionSpace.MySpace.SetBinding(UIElement.VisibilityProperty, _interactionSpaceBinding);
                        _finAgent.IsInteractionSpace = true;
                    }
                    else (view as InteractionSpace).MySpace.Visibility = Visibility.Hidden;
                }

            }

        }
        private IView CheckView(string dialogID)
        {
            IView outView = null;
            foreach (IView view in MVVM.Keys)
            {
                if (view is InteractionSpace)
                {

                    if ((MVVM[view] as InteractionSpaceModel).DialogID.Equals(dialogID))
                        outView = view;
                }

            }
            return outView;
        }

        public void FireLogMessage(string msg)
        {
            //In order to reduce memory size , we may need to clear log messages after a while.
            //_finAgent.LogMessages.Remove(msg);
            new LogWriter(msg);
        }

        public void FireDisconnectEvent()
        {
            Connected.Visibility = Visibility.Hidden;
            Reconnect.Visibility = Visibility.Visible;
        }

        public void FireQueueEvent(Queue queue)
        {
            FireLogMessage("New Message Event from Queue:"+queue.Name+" , and the agents ready:"+queue.AgentsReady+" ,  and the agents not ready:"+queue.AgentsNotReady);
        }
    }
}
