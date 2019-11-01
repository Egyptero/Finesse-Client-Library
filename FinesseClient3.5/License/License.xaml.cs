using FinesseClient.License.Model;
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

namespace FinesseClient.License
{
    /// <summary>
    /// Interaction logic for License.xaml
    /// </summary>
    public partial class License : Window
    {
        public License()
        {
            AgentLicense agentLicense = new AgentLicense();
            this.DataContext = agentLicense;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
