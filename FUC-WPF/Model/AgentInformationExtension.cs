using FinesseClient.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUC_WPF.Model
{
    public class AgentInformationExtension : ObservableObject
    {
        #region fields
        private string _ctiWeb;
        private bool _logCallStart;
        private bool _logCallEnd;
        private bool _logCallTransfer;
        private bool _callAUDStart;
        private bool _callAUDEnd;
        private bool _fireCallStartEvent;
        private bool _fireCallEndEvent;
        private bool _configureDetails;
        #endregion

        #region properties
        public String CtiWeb { get { return _ctiWeb; } set { _ctiWeb = value; OnPropertyChanged("CtiWeb"); } }
        public Boolean LogCallStart { get { return _logCallStart; } set { _logCallStart = value; OnPropertyChanged("LogCallStart"); } }
        public Boolean LogCallEnd { get { return _logCallEnd; } set { _logCallEnd = value; OnPropertyChanged("LogCallEnd"); } }
        public Boolean LogCallTransfer { get { return _logCallTransfer; } set { _logCallTransfer = value; OnPropertyChanged("LogCallTransfer"); } }
        public Boolean CallAUDStart { get { return _callAUDStart; } set { _callAUDStart = value; OnPropertyChanged("CallAUDStart"); } }
        public Boolean CallAUDEnd { get { return _callAUDEnd; } set { _callAUDEnd = value; OnPropertyChanged("CallAUDEnd"); } }
        public Boolean FireCallStartEvent { get { return _fireCallStartEvent; } set { _fireCallStartEvent = value; OnPropertyChanged("FireCallStartEvent"); } }
        public Boolean FireCallEndEvent { get { return _fireCallEndEvent; } set { _fireCallEndEvent = value; OnPropertyChanged("FireCallEndEvent"); } }
        public Boolean ConfigureDetails { get { return _configureDetails; } set { _configureDetails = value; OnPropertyChanged("ConfigureDetails"); } }
        #endregion
    }
}
