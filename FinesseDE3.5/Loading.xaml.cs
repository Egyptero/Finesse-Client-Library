using FinesseClient;
using FinesseClient.Common;
using FinesseClient.Model;
using FinesseClient.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FinesseDE
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Window,FinView
    {
        private FinAgent _finAgent;
        private Login _loginWindow;
        private MainWindow _mainWindow;
        public Loading(FinAgent finAgent, Login loginWindow)
        {
            _finAgent = finAgent;
            _loginWindow = loginWindow;
            _finAgent.FinView = this;
            DataContext = _finAgent;
            InitializeComponent();
        }

        public void FireErrorMessage(string msg)
        {
            LoadingError.Content = msg;
        }

        public void FireNewEvent()
        {
        }

        public void FireLoadingMessage(string msg)
        {
        }

        public void FireReLoginEvent()
        {
            //Load Finesse Details
            //_finAgent.LoadAgentInformation();
            //_finAgent.LoadCallInformation();
            if (_mainWindow == null)
                _mainWindow = new MainWindow(_finAgent, _loginWindow, this);
            _finAgent.FinView = _mainWindow as FinView;
            Hide();
            _mainWindow.Show();
            _mainWindow.CurrentStatus.Text = "Agent logged in ...";

        }

        public void FireLoadLoginScreen()
        {
            Hide();
            _loginWindow.Show();
            _finAgent.FinView = _loginWindow as FinView;
        }

        public void FireCallEvent(Dialog dialog)
        {
        }

        public void SetContext(IModel model,FinView finView)
        {
        }

        public Screen GetLocation()
        {
            return Screen.Loading;
        }

        public void FireLogMessage(string msg)
        {
        }

        public void FireDisconnectEvent()
        {
        }

        public void FireQueueEvent(Queue queue)
        {
        }
    }
}
