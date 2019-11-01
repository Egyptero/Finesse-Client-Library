using FinesseClient.Common;
using FinesseClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUCX_WPF.Model
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
        private string _dialNumber;
        private bool _agentStatusDisabled;
        private MTObservableCollection<Dialog> _recent = new MTObservableCollection<Dialog>();
        private MTObservableCollection<string> _connectionType = new MTObservableCollection<string>();
        #endregion

        #region properties
        public Boolean AgentStatusDisabled { get { return _agentStatusDisabled; } set { _agentStatusDisabled = value; OnPropertyChanged("AgentStatusDisabled"); OnPropertyChanged("AgentStatusEnabled"); } }
        public Boolean AgentStatusEnabled { get { return !_agentStatusDisabled; } }
        public String CtiWeb { get { return _ctiWeb; } set { _ctiWeb = value; OnPropertyChanged("CtiWeb"); } }
        public Boolean LogCallStart { get { return _logCallStart; } set { _logCallStart = value; OnPropertyChanged("LogCallStart"); } }
        public Boolean LogCallEnd { get { return _logCallEnd; } set { _logCallEnd = value; OnPropertyChanged("LogCallEnd"); } }
        public Boolean LogCallTransfer { get { return _logCallTransfer; } set { _logCallTransfer = value; OnPropertyChanged("LogCallTransfer"); } }
        public Boolean CallAUDStart { get { return _callAUDStart; } set { _callAUDStart = value; OnPropertyChanged("CallAUDStart"); } }
        public Boolean CallAUDEnd { get { return _callAUDEnd; } set { _callAUDEnd = value; OnPropertyChanged("CallAUDEnd"); } }
        public Boolean FireCallStartEvent { get { return _fireCallStartEvent; } set { _fireCallStartEvent = value; OnPropertyChanged("FireCallStartEvent"); } }
        public Boolean FireCallEndEvent { get { return _fireCallEndEvent; } set { _fireCallEndEvent = value; OnPropertyChanged("FireCallEndEvent"); } }
        public Boolean ConfigureDetails { get { return _configureDetails; } set { _configureDetails = value; OnPropertyChanged("ConfigureDetails"); } }
        public String DialNumber { get { return _dialNumber; } set { _dialNumber = value; OnPropertyChanged("DialNumber"); } }
        public MTObservableCollection<Dialog> Recent { get { return _recent; } set { _recent = value; OnPropertyChanged("Recent"); } }
        public MTObservableCollection<string> ConnectionType { get { return _connectionType; } set { _connectionType = value; OnPropertyChanged("ConnectionType"); } }
        #endregion

        public AgentInformationExtension()
        {
            ConnectionType.Add("Ssl3");
            ConnectionType.Add("Tls");
            ConnectionType.Add("Tls11");
            ConnectionType.Add("Tls12");
        }
    }
}
