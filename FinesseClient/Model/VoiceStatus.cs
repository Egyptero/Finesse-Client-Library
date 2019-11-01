using FinesseClient.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FinesseClient.Model
{
    public class VoiceStatus:ObservableObject
    {
        #region Fields
        private string _status;
        private string _reasonCode;
        private string _pendingStatus;
        private string _statusLabel;
        private bool _selected;
        private string _iconName;
        #endregion

        #region Properties
        public String Status { get { return _status; } set { _status = value; OnPropertyChanged("Status"); } }
        public String ReasonCode { get { return _reasonCode; } set { _reasonCode = value; OnPropertyChanged("ReasonCode"); } }
        public String PendingStatus { get { return _pendingStatus; } set { _pendingStatus = value; OnPropertyChanged("PendingStatus"); } }
        public String StatusLabel { get { return _statusLabel; } set { _statusLabel = value; OnPropertyChanged("StatusLabel"); } }
        public Boolean Selected { get { return _selected; } set { _selected = value; OnPropertyChanged("Selected"); } }
        public BitmapImage StatusImage { get { return new BitmapImage(new Uri("Images/"+_iconName+".png", UriKind.Relative)); } }
        public String IconName { get { return _iconName; } set { _iconName = value; OnPropertyChanged("IconName"); OnPropertyChanged("StatusImage"); } }
        #endregion
    }
}
