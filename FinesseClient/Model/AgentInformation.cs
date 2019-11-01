using FinesseClient.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FinesseClient.Model
{
    public class AgentInformation : ObservableObject
    {
        #region Fields
        private string _activeSite;
        private string _agentID;
        private string _userName;
        private string _password;
        private string _extension;
        private string _domainA;
        private string _domainB;
        private string _name;
        private string _status;
        private string _pendingStatus;
        private string _stateChangeTime;
        private string _reasonCodeId;
        private string _firstName;
        private string _lastName;
        private string _teamId;
        private string _teamName;
        private string _xmppPort;// = "7071";
        private string _xmppURL;// = "/http-bind/";
        private string _httpPort;// = "80";
        private string _httpURL;// = "/finesse";
        private string _xmppConnectionType;
        private string _httpConnectionType;
        private bool _ssl = false;
        private MessageEvent _messageEvent;
        private ReasonCodeClass _reasonCode;
        private SettingClass _setting;
        private ObservableCollection<Team> _teams;
        private ObservableCollection<Dialog> _dialogs;
        private ObservableCollection<String> _roles;
        private ObservableCollection<ReasonCodeClass> _notReadyReasonCodeList;
        private ObservableCollection<ReasonCodeClass> _logoutReasonCodeList;
        private ObservableCollection<VoiceStatus> _voiceStatusList;
        private VoiceStatus _selectedVoiceStatus;
        private Dialog _selectedDialog;
        private bool _makeCallVisibile;
        #endregion

        #region Properties
        public String ActiveSite { get { return _activeSite; } set { _activeSite = value; } }
        public String AgentID { get { return _agentID; } set { _agentID = value; OnPropertyChanged("AgentID"); } }
        public String UserName { get { return _userName; } set { _userName = value; } }
        public String Password { get { return _password; } set { _password = value; OnPropertyChanged("Password"); } }
        public String Extension { get { return _extension; } set { _extension = value; OnPropertyChanged("Extension"); } }
        public String DomainA { get { return _domainA; } set { _domainA = value; OnPropertyChanged("DomainA"); } }
        public String DomainB { get { return _domainB; } set { _domainB = value; OnPropertyChanged("DomainB"); } }
        public String XMPPPort { get { return _xmppPort; } set { _xmppPort = value; OnPropertyChanged("XMPPPort"); } }
        public String HTTPPort { get { return _httpPort; } set { _httpPort = value; OnPropertyChanged("HTTPPort"); } }
        public String XMPPURL { get { return _xmppURL; } set { _xmppURL = value; OnPropertyChanged("XMPPURL"); } }
        public String HTTPURL { get { return _httpURL; } set { _httpURL = value; OnPropertyChanged("HTTPURL"); } }
        public String XMPPConnectionType { get { return _xmppConnectionType; } set { _xmppConnectionType = value; OnPropertyChanged("XMPPConnectionType"); } }
        public String HTTPConnectionType { get { return _httpConnectionType; } set { _httpConnectionType = value; OnPropertyChanged("HTTPConnectionType"); } }
        public Boolean SSL { get { return _ssl; } set { _ssl = value; OnPropertyChanged("SSL"); } }
        public String Name { get { return _name; } set { _name = value; } }
        public String Status { get { return _status; } set { _status = value; } }
        public String PendingStatus { get { return _pendingStatus; } set { _pendingStatus = value; } }
        public String StateChangeTime { get { return _stateChangeTime; } set { _stateChangeTime = value; } }
        public String ReasonCodeId { get { return _reasonCodeId; } set { _reasonCodeId = value; } }
        public String FirstName { get { return _firstName; } set { _firstName = value; } }
        public String LastName { get { return _lastName; } set { _lastName = value; } }
        public String TeamID { get { return _teamId; } set { _teamId = value; } }
        public String TeamName { get { return _teamName; } set { _teamName = value; } }
        public MessageEvent MessageEvent { get { return _messageEvent; } set { _messageEvent = value; } }
        public ReasonCodeClass ReasonCode { get { return _reasonCode; } set { _reasonCode = value; } }
        public SettingClass Setting { get { return _setting; } set { _setting = value; } }
        public ObservableCollection<Team> Teams { get { return _teams; } set { _teams = value; OnPropertyChanged("Teams"); } }
        public ObservableCollection<Dialog> Dialogs { get { return _dialogs; } set { _dialogs = value; OnPropertyChanged("Dialogs"); } }
        public ObservableCollection<String> Roles { get { return _roles; } set { _roles = value; OnPropertyChanged("Roles"); } }
        public ObservableCollection<ReasonCodeClass> NotReadyReasonCodeList { get { return _notReadyReasonCodeList; } set { _notReadyReasonCodeList = value; OnPropertyChanged("NotReadyReasonCodeList"); } }
        public ObservableCollection<ReasonCodeClass> LogoutReasonCodeList { get { return _logoutReasonCodeList; } set { _logoutReasonCodeList = value; OnPropertyChanged("LogoutReasonCodeList"); } }
        public ObservableCollection<VoiceStatus> VoiceStatusList { get { return _voiceStatusList; } set { _voiceStatusList = value; OnPropertyChanged("VoiceStatusList"); } }
        public VoiceStatus SelectedVoiceStatus { get { return _selectedVoiceStatus; } set { _selectedVoiceStatus = value; OnPropertyChanged("SelectedVoiceStatus"); }}
        public Dialog SelectedDialog { get { return _selectedDialog; } set { _selectedDialog = value; OnPropertyChanged("SelectedDialog"); } }
        public Boolean MakeCallVisible {
            get {
                if (SelectedDialog != null)
                    return _makeCallVisibile || SelectedDialog.MakeCall;
                else
                    return _makeCallVisibile;
            } set { _makeCallVisibile = value; OnPropertyChanged("MakeCallVisible"); } }
        #endregion

        #region Inner Classes
        public class Team : ObservableObject
        {
            #region Fields
            private string _uri;
            private string _id;
            private string _name;
            #endregion

            #region Properties
            public String URI { get { return _uri; } set { _uri = value; } }
            public String ID { get { return _id; } set { _id = value; } }
            public String Name { get { return _name; } set { _name = value; } }
            #endregion
        }

        public class ReasonCodeClass : ObservableObject
        {
            #region Fields;
            private string _category;
            private string _uri;
            private string _code;
            private string _label;
            private string _forAll;
            private string _id;
            #endregion

            #region Properties
            public string Category { get { return _category; } set { _category = value; } }
            public string URI { get { return _uri; } set { _uri = value; } }
            public string Code { get { return _code; } set { _code = value; } }
            public string Label { get { return _label; } set { _label = value; } }
            public string ForAll { get { return _forAll; } set { _forAll = value; } }
            public string ID { get { return _id; } set { _id = value; } }
            #endregion
        }

        public class SettingClass : ObservableObject
        {
            #region Fields
            private string _wrapUpOnIncoming;
            #endregion

            #region Properties
            public String WrapUpOnIncoming { get { return _wrapUpOnIncoming; } set { _wrapUpOnIncoming = value; } }
            #endregion
        }
        #endregion
    }
}
