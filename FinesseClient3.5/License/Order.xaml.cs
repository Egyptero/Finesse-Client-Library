using FinesseClient.License.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace FinesseClient.License
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        public Order()
        {
            InitializeComponent();
        }

        public Order(AgentLicense agentLicense)
        {
            DataContext = agentLicense;
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.Name == "Developer")
                (DataContext as AgentLicense).Package = "Developer";
            if (radioButton.Name == "Basic")
                (DataContext as AgentLicense).Package = "Basic";
            if (radioButton.Name == "Enterprise")
                (DataContext as AgentLicense).Package = "Enterprise";
            if (radioButton.Name == "Unlimited")
                (DataContext as AgentLicense).Package = "Unlimited";

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
