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
    /// Interaction logic for InteractionSpace.xaml
    /// </summary>
    public partial class InteractionSpace : UserControl ,IView
    {
        private FinAgent _finAgent;
        public InteractionSpace()
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

        public void SetContext(IModel model,FinView finView)
        {
            _finAgent = (model as InteractionSpaceModel).FinAgent;
            DataContext = _finAgent;
            InitializeComponent();
        }

        public void FireReLoginEvent()
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
