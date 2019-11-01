using FinesseClient;
using FinesseClient.Common;
using FinesseClient.Model;
using FinesseClient.View;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,FinView
    {
        public MainWindow()
        {
            FinAgent finaAgent = new FinAgent();
            finaAgent.TraceStatus = false;
            finaAgent.SaveLog = true;
            finaAgent.FinView = this;
            DataContext = finaAgent;
            //finaAgent.AgentInformation.A
           
            InitializeComponent();
        }

        public void FireCallEvent(Dialog dialog)
        {
            
            if (dialog.State.Equals("ALERTING"))
            {

            }else if (dialog.State.Equals("DROPPED"))
            {

            }
            //throw new NotImplementedException();
        }

        public void FireErrorMessage(string msg)
        {
            //throw new NotImplementedException();
            MessageBox.Show("FireErrorMessage>>" + msg);
        }

        public void FireLoadingMessage(string msg)
        {
           MessageBox.Show("FireLoadingMessage>>" + msg);
            //throw new NotImplementedException();
        }

        public void FireLoadLoginScreen()
        {
            Login.Visibility = Visibility.Visible;
            Main.Visibility = Visibility.Hidden;
            MessageBox.Show("FireLoadLoginScreen>>");
            //throw new NotImplementedException();
        }

        public void FireNewEvent()
        {
            FinAgent finAgent = DataContext as FinAgent;
            if (finAgent.AgentInformation.MessageEvent != null)
            {
                if (finAgent.AgentInformation.MessageEvent.MessageType.Equals("user"))
                {
                   // MessageBox.Show(finAgent.AgentInformation.MessageEvent.ToString());

                }else if (finAgent.AgentInformation.MessageEvent.MessageType.Equals("call"))
                {
                    //MessageBox.Show(finAgent.AgentInformation.MessageEvent.ToString());

                }
                else if (finAgent.AgentInformation.MessageEvent.MessageType.Equals("error"))
                {
                    MessageBox.Show(finAgent.AgentInformation.MessageEvent.ErrorMsg);

                }
                //throw new NotImplementedException();
            }
        }

        public void FireReLoginEvent()
        {
            FinAgent finAgent = DataContext as FinAgent;

            finAgent.LoadAgentInformation();
            finAgent.LoadCallInformation();
            Login.Visibility = Visibility.Hidden;
            Main.Visibility = Visibility.Visible;
            //throw new NotImplementedException();
            MessageBox.Show("FireReLoginEvent>>");
        }

        public Screen GetLocation()
        {
            return Screen.MainGrid;
            //throw new NotImplementedException();
        }

        public void SetContext(IModel model, FinView finView)
        {
            //throw new NotImplementedException();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            FinAgent finAgent = DataContext as FinAgent;
            finAgent.AgentInformation.Password = password.Text;
            //finAgent.AgentInformation.HTTPPort = "8082";

            if (finAgent.SignIn())
            {
             //  finAgent.LoadAgentInformation();
              // finAgent.LoadCallInformation();
               // Login.Visibility = Visibility.Hidden;
                //Main.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Erro in Login");
            }
       
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string dialogID = button.Tag.ToString();
            FinAgent finAgent = DataContext as FinAgent;
            if (finAgent.AnswerCall(dialogID))
            {

            }else
            {
                MessageBox.Show("Erro in call answer");
            }

        }

        private void Release_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string dialogID = button.Tag.ToString();
            FinAgent finAgent = DataContext as FinAgent;
            if (finAgent.ReleaseCall(dialogID))
            {

            }
            else
            {
                MessageBox.Show("Erro in Release answer");
            }

        }

        private void Hold_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string dialogID = button.Tag.ToString();
            FinAgent finAgent = DataContext as FinAgent;
            if (finAgent.HoldCall(dialogID))
            {

            }
            else
            {
                MessageBox.Show("Erro in Hold Call");
            }
        }

        private void Status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VoiceStatus vs =  Status.SelectedItem as VoiceStatus;
            if (vs == null)
                return;
            FinAgent fin = DataContext as FinAgent;
            fin.ChangeAgentVoiceStatus(vs);


        }

        public void FireLogMessage(string msg)
        {

            //throw new NotImplementedException();
        }

        public void FireDisconnectEvent()
        {
            Login.Visibility = Visibility.Hidden;
            Main.Visibility = Visibility.Hidden;

            MessageBox.Show("Disconnected>>");
            //throw new NotImplementedException();
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string dialogID = button.Tag.ToString();
            FinAgent finAgent = DataContext as FinAgent;
            if (finAgent.ResumeCall(dialogID))
            {

            }
            else
            {
                MessageBox.Show("Erro in Hold Call");
            }
        }
    }
}
