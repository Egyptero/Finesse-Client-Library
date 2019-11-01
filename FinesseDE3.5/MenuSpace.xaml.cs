using FinesseClient;
using FinesseClient.Common;
using FinesseClient.Model;
using FinesseClient.View;
using FinesseDE.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinesseDE
{
    /// <summary>
    /// Interaction logic for MenuSpace.xaml
    /// </summary>
    public partial class MenuSpace : UserControl,IView
    {
        private FinAgent _finAgent;
        public MenuSpace()
        {
        }

        public void FireNewEvent()
        {
        }

        public void FireCallEvent(Dialog dialog)
        {
        }

        public Screen GetLocation()
        {
            return Screen.MainGrid;
        }

        public void SetContext(IModel model, FinView finView)
        {
            _finAgent = (model as MenuSpaceModel).FinAgent;
            DataContext = _finAgent;
            InitializeComponent();
        }

        private void CloseMenuSpace_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as FinAgent).IsMenuSpace = false;
        }

        public void FireReLoginEvent()
        {
        }

        public void FireDisconnectEvent()
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            Button button = sender as Button;
            if (button.Content.Equals("Save"))
            {
                CXProperties themeProperties = (CXProperties)Application.Current.Resources["CXProperties"];
                if (themeProperties != null)
                {
                    themeProperties.Save();
                    _finAgent.FireLogMessage("Theme setting was saved successfully");
                }
                else
                    _finAgent.FireErrorMessage("Can not save theme setting");

            }
            else if (button.Content.Equals("Cancel"))
            {
                CXProperties themeProperties = (CXProperties)Application.Current.Resources["CXProperties"];
                if (themeProperties != null)
                    themeProperties.reload();

            }
        }

        private void ClearLog_Click(object sender, RoutedEventArgs e)
        {
            if(_finAgent != null)
                _finAgent.LogMessages.Clear();
        }

        public void FireQueueEvent(Queue queue)
        {
        }
    }
}
