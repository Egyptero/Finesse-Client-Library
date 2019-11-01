using FinesseClient.Common;
using FUC_WPF.ARB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUC_WPF.Model
{
    public class DialogExtension : ObservableObject
    {
        #region fields
        private bool _dialPadStatus;
        private bool _updateRoutingData;
        private string _dialNumber;
        private bool _disabled;
        private bool _isMaleOnly = true;
        private bool _isInquiry = true;
        private string _message;
        private IVROptions _ivrOptions;
        private ObservableCollection<String> _transferIVRMenu;
        private string _selectedIVROption;
        #endregion
        #region properties
        public IVROptions IVROptions { get { return _ivrOptions; } set { _ivrOptions = value; OnPropertyChanged("IVROptions"); } }
        public Boolean DialPadStatus { get { return _dialPadStatus; } set { _dialPadStatus = value; OnPropertyChanged("DialPadStatus"); } }
        public String SelectedIVROption { get { return _selectedIVROption; } set { _selectedIVROption = value; OnPropertyChanged("SelectedIVROption"); } }
        public ObservableCollection<String> TransferIVRMenu { get { return _transferIVRMenu; } set { _transferIVRMenu = value; OnPropertyChanged("TransferIVRMenu"); } }
        public Boolean IsMaleOnly { get { return _isMaleOnly; } set { _isMaleOnly = value; OnPropertyChanged("IsMaleOnly"); } }
        public Boolean IsInquiry { get { return _isInquiry; } set { _isInquiry = value; OnPropertyChanged("IsInquiry"); } }
        public Boolean IsComplaint { get { return !_isInquiry; } set { _isInquiry = !value; OnPropertyChanged("IsComplaint"); } }
        public Boolean UpdateRoutingData { get { return _updateRoutingData; } set { _updateRoutingData = value; OnPropertyChanged("UpdateRoutingData"); } }
        public String DialNumber { get { return _dialNumber; } set { _dialNumber = value; OnPropertyChanged("DialNumber"); } }
        public Boolean Disabled { get { return _disabled; } set { _disabled = value; OnPropertyChanged("Disabled"); OnPropertyChanged("Enabled"); } }
        public Boolean Enabled { get { return !_disabled; } }
        public String Message { get { return _message; } set { _message = value; OnPropertyChanged("Message"); } }

        internal void LoadTransferOptions()
        {
            if (IVROptions == null)
                return;
            string[] options = IVROptions.GetTransferOptions();
            if (TransferIVRMenu == null)
                TransferIVRMenu = new ObservableCollection<string>();
            foreach (string option in options)
                if(!TransferIVRMenu.Contains(option))
                    TransferIVRMenu.Add(option);

            if (SelectedIVROption == null && options.Length > 0)
                SelectedIVROption = options[0];
            OnPropertyChanged("TransferIVRMenu");
        }
        #endregion
    }
}
