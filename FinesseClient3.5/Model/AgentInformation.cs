using FinesseClient.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;

namespace FinesseClient.Model
{
    public class AgentInformation : ObservableObject
    {
        #region Fields
        private string _activeSite;
//        private string _loginID;
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
        private MTObservableCollection<Team> _teams;
        private MTObservableCollection<Dialog> _dialogs;
        private MTObservableCollection<Queue> _queues;
        private MTObservableCollection<String> _roles;
        private MTObservableCollection<ReasonCodeClass> _notReadyReasonCodeList;
        private MTObservableCollection<ReasonCodeClass> _logoutReasonCodeList;
        private MTObservableCollection<VoiceStatus> _voiceStatusList;
        private VoiceStatus _selectedVoiceStatus;
        private Dialog _selectedDialog;
        private bool _makeCallVisibile;
        private Object _reservedObject;
        private int _xmppKeepAliveInterval = -1;
        #endregion

        #region Properties
        public String ActiveSite { get { return _activeSite; } set { _activeSite = value; } }
 //       public String LoginID { get { return _loginID; } set { _loginID = value; OnPropertyChanged("LoginID"); } }
        public String AgentID { get { return _agentID; } set { _agentID = value; OnPropertyChanged("AgentID"); } }
        public String UserName { get { return _userName; } set { _userName = value; OnPropertyChanged("UserName"); } }
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
        public String Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        public String Status { get { return _status; } set { _status = value; OnPropertyChanged("Status"); } }
        public String PendingStatus { get { return _pendingStatus; } set { _pendingStatus = value; OnPropertyChanged("PendingStatus"); } }
        public String StateChangeTime { get { return _stateChangeTime; } set { _stateChangeTime = value; OnPropertyChanged("StateChangeTime"); } }
        public String ReasonCodeId { get { return _reasonCodeId; } set { _reasonCodeId = value; OnPropertyChanged("ReasonCodeId"); } }
        public String FirstName { get { return _firstName; } set { _firstName = value; OnPropertyChanged("FirstName"); } }
        public String LastName { get { return _lastName; } set { _lastName = value; OnPropertyChanged("LastName"); } }
        public String TeamID { get { return _teamId; } set { _teamId = value; OnPropertyChanged("TeamID"); } }
        public String TeamName { get { return _teamName; } set { _teamName = value; OnPropertyChanged("TeamName"); } }
        public MessageEvent MessageEvent { get { return _messageEvent; } set { _messageEvent = value; OnPropertyChanged("MessageEvent"); } }
        public ReasonCodeClass ReasonCode { get { return _reasonCode; } set { _reasonCode = value; OnPropertyChanged("ReasonCode"); } }
        public SettingClass Setting { get { return _setting; } set { _setting = value; OnPropertyChanged("Setting"); } }
        public MTObservableCollection<Team> Teams { get { return _teams; } set { _teams = value; OnPropertyChanged("Teams"); } }
        public MTObservableCollection<Dialog> Dialogs { get { return _dialogs; } set { _dialogs = value; OnPropertyChanged("Dialogs"); } }
        public MTObservableCollection<Queue> Queues { get { return _queues; } set { _queues = value; OnPropertyChanged("Queues"); } }
        public MTObservableCollection<String> Roles { get { return _roles; } set { _roles = value; OnPropertyChanged("Roles"); } }
        public MTObservableCollection<ReasonCodeClass> NotReadyReasonCodeList { get { return _notReadyReasonCodeList; } set { _notReadyReasonCodeList = value; OnPropertyChanged("NotReadyReasonCodeList"); } }
        public MTObservableCollection<ReasonCodeClass> LogoutReasonCodeList { get { return _logoutReasonCodeList; } set { _logoutReasonCodeList = value; OnPropertyChanged("LogoutReasonCodeList"); } }
        public MTObservableCollection<VoiceStatus> VoiceStatusList { get { return _voiceStatusList; } set { _voiceStatusList = value; OnPropertyChanged("VoiceStatusList"); } }
        public VoiceStatus SelectedVoiceStatus { get { return _selectedVoiceStatus; } set { _selectedVoiceStatus = value; OnPropertyChanged("SelectedVoiceStatus"); }}
        public Dialog SelectedDialog { get { return _selectedDialog; } set { _selectedDialog = value; OnPropertyChanged("SelectedDialog"); OnPropertyChanged("Dialogs"); } }
        public Boolean MakeCallVisible {
            get {
                if (SelectedDialog != null)
                    return _makeCallVisibile || SelectedDialog.MakeCall;
                else
                    return _makeCallVisibile;
            } set { _makeCallVisibile = value; OnPropertyChanged("MakeCallVisible"); } }
        public Object ReservedObject { get { return _reservedObject; } set { _reservedObject = value; OnPropertyChanged("ReservedObject"); } }
        public int XmppKeepAliveInterval { get { return _xmppKeepAliveInterval; } set { _xmppKeepAliveInterval = value; OnPropertyChanged("XmppKeepAliveInterval"); } }
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
