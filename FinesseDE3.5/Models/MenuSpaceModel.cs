using FinesseClient;
using FinesseClient.Common;
using FinesseClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinesseDE.Models
{
    class MenuSpaceModel : ObservableObject,IModel
    {
        private FinAgent _finAgent;
        private string _dialogID;

        public FinAgent FinAgent { get { return _finAgent; } set { _finAgent = value; OnPropertyChanged("FinAgent"); } }
        public String DialogID { get { return _dialogID; } set { _dialogID = value; OnPropertyChanged("DialogID"); } }

        public void SetDialogID(string dialogID)
        {
            DialogID = dialogID;
        }

        public void SetFinAgent(FinAgent finAgent)
        {
            FinAgent = finAgent;
        }
    }
}
