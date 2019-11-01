using FinesseClient.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinesseClient.Model
{
    public class Dialog : ObservableObject
    {
        #region Fields
        private string _associatedDialogUri;
        private string _fromAddress;
        private string _toAddress;
        private string _id;
        private MediaPropertiesClass _mediaProperties;
        private string _mediaType;
        private string _state;
        private string _uri;
        private ObservableCollection<Participant> _participants;
        private string _dialogEvent;
        private FinAgent _finAgent;
        private StateTimer _dialogStateTimer;
        #endregion

        #region Constructor
        private Dialog()
        {

        }
        public Dialog(FinAgent finAgent)
        {
            _finAgent = finAgent;
        }
        #endregion

        #region Properties
        public StateTimer DialogStateTimer { get { return _dialogStateTimer; } set { _dialogStateTimer = value; OnPropertyChanged("DialogStateTimer"); } }
        public String AssociatedDialogUri { get { return _associatedDialogUri; } set { _associatedDialogUri = value; OnPropertyChanged("AssociatedDialogUri"); } }
        public String FromAddress { get { return _fromAddress; } set { _fromAddress = value; OnPropertyChanged("FromAddress"); OnPropertyChanged("Header"); } }
        public String ID { get { return _id; } set { _id = value; OnPropertyChanged("ID"); } }
        public MediaPropertiesClass MediaProperties { get { return _mediaProperties; } set { _mediaProperties = value; OnPropertyChanged("MediaProperties"); } }
        public String MediaType { get { return _mediaType; } set { _mediaType = value; OnPropertyChanged("MediaType"); } }
        public String State { get { return _state; } set { _state = value; OnPropertyChanged("State"); } }
        public String ToAddress { get { return _toAddress; } set { _toAddress = value; OnPropertyChanged("ToAddress"); OnPropertyChanged("Header"); } }
        public String URI { get { return _uri; } set { _uri = value; OnPropertyChanged("URI"); } }
        public ObservableCollection<Participant> Participants { get { return _participants; } set { _participants = value; OnPropertyChanged("Participants");
                OnPropertyChanged("Answer");
                OnPropertyChanged("Conference");
                OnPropertyChanged("Consult");
                OnPropertyChanged("CTransfer");
                OnPropertyChanged("Hold");
                OnPropertyChanged("MakeCall");
                OnPropertyChanged("Release");
                OnPropertyChanged("Resume");
                OnPropertyChanged("SendDTMF");
                OnPropertyChanged("Transfer");
            } }
        public String DialogEvent { get { return _dialogEvent; } set { _dialogEvent = value; OnPropertyChanged("DialogEvent"); } }
        public String Header { get
            {
                string outCome = string.Empty;
                if(MediaType.Equals("Voice"))
                {
                    if (FromAddress.Equals(_finAgent.AgentInformation.Extension)) // Outgoing call
                        outCome = "Out Call to " + ToAddress +" ("+State+") ";
                    else if (ToAddress.Equals(_finAgent.AgentInformation.Extension)) //Incoming call
                        outCome = "In Call from " + FromAddress + " (" + State + ") ";
                }
                return outCome;
            } }
        public Boolean Answer { get
            {
                foreach(Participant participant in Participants)
                {
                    if (participant.MediaAddress.Equals(_finAgent.AgentInformation.Extension))
                    { //My Extension Details
                        foreach(String action in participant.Actions)
                        {
                            if (action.Equals("ANSWER"))
                                return true;
                        }
                    }
                }
                return false;
            }
        }
        public Boolean Release
        {
            get
            {
                foreach (Participant participant in Participants)
                {
                    if (participant.MediaAddress.Equals(_finAgent.AgentInformation.Extension))
                    { //My Extension Details
                        foreach (String action in participant.Actions)
                        {
                            if (action.Equals("DROP"))
                                return true;
                        }
                    }
                }
                return false;
            }
        }
        public Boolean CTransfer
        {
            get
            {
                foreach (Participant participant in Participants)
                {
                    if (participant.MediaAddress.Equals(_finAgent.AgentInformation.Extension))
                    { //My Extension Details
                        foreach (String action in participant.Actions)
                        {
                            if (action.Equals("TRANSFER"))
                                return true;
                        }
                    }
                }
                return false;
            }
        }
        public Boolean Transfer
        {
            get
            {
                foreach (Participant participant in Participants)
                {
                    if (participant.MediaAddress.Equals(_finAgent.AgentInformation.Extension))
                    { //My Extension Details
                        foreach (String action in participant.Actions)
                        {
                            if (action.Equals("TRANSFER_SST"))
                                return true;
                        }
                    }
                }
                return false;
            }
        }
        public Boolean Consult
        {
            get
            {
                foreach (Participant participant in Participants)
                {
                    if (participant.MediaAddress.Equals(_finAgent.AgentInformation.Extension))
                    { //My Extension Details
                        foreach (String action in participant.Actions)
                        {
                            if (action.Equals("CONSULT_CALL"))
                                return true;
                        }
                    }
                }
                return false;
            }
        }
        public Boolean Conference
        {
            get
            {
                foreach (Participant participant in Participants)
                {
                    if (participant.MediaAddress.Equals(_finAgent.AgentInformation.Extension))
                    { //My Extension Details
                        foreach (String action in participant.Actions)
                        {
                            if (action.Equals("CONFERENCE"))
                                return true;
                        }
                    }
                }
                return false;
            }
        }
        public Boolean Hold
        {
            get
            {
                foreach (Participant participant in Participants)
                {
                    if (participant.MediaAddress.Equals(_finAgent.AgentInformation.Extension))
                    { //My Extension Details
                        foreach (String action in participant.Actions)
                        {
                            if (action.Equals("HOLD"))
                                return true;
                        }
                    }
                }
                return false;
            }
        }
        public Boolean Resume
        {
            get
            {
                foreach (Participant participant in Participants)
                {
                    if (participant.MediaAddress.Equals(_finAgent.AgentInformation.Extension))
                    { //My Extension Details
                        foreach (String action in participant.Actions)
                        {
                            if (action.Equals("RETRIEVE"))
                                return true;
                        }
                    }
                }
                return false;
            }
        }
        public Boolean SendDTMF
        {
            get
            {
                foreach (Participant participant in Participants)
                {
                    if (participant.MediaAddress.Equals(_finAgent.AgentInformation.Extension))
                    { //My Extension Details
                        foreach (String action in participant.Actions)
                        {
                            if (action.Equals("SEND_DTMF"))
                                return true;
                        }
                    }
                }
                return false;
            }
        }
        public Boolean MakeCall
        {
            get
            {
                foreach (Participant participant in Participants)
                {
                    if (participant.MediaAddress.Equals(_finAgent.AgentInformation.Extension))
                    { //My Extension Details
                        foreach (String action in participant.Actions)
                        {
                            if (action.Equals("MAKE_CALL"))
                                return true;
                        }
                    }
                }
                return false;
            }
        }
        #endregion

        #region Inner Class
        public class MediaPropertiesClass : ObservableObject
        {
            #region Fields
            private string _mediaID;
            private string _dnis;
            private string _callType;
            private string _dialedNumber;
            private string _outboundClassification;
            private ObservableCollection<CallVariableClass> _callVariables;
            #endregion

            #region Properties
            public String MediaID { get { return _mediaID; } set { _mediaID = value; OnPropertyChanged("MediaID"); } }
            public String DNIS { get { return _dnis; } set { _dnis = value; OnPropertyChanged("DNIS"); } }
            public String CallType { get { return _callType; } set { _callType = value; OnPropertyChanged("CallType"); } }
            public String DialedNumber { get { return _dialedNumber; } set { _dialedNumber = value; OnPropertyChanged("DialedNumber"); } }
            public String OutboundClassification { get { return _outboundClassification; } set { _outboundClassification = value; OnPropertyChanged("OutboundClassification"); } }
            public ObservableCollection<CallVariableClass> CallVariables { get { return _callVariables; } set { _callVariables = value; OnPropertyChanged("CallVariables"); } }
            #endregion
            public class CallVariableClass : ObservableObject
            {
                #region Fields
                private string _name;
                private string _value;
                #endregion

                #region Properties
                public String Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
                public String Value { get { return _value; } set { _value = value; OnPropertyChanged("Value"); } }
                #endregion
            }
        }

        public class Participant : ObservableObject
        {
            #region Fields
            private ObservableCollection<String> _actions;
            private string _mediaAddress;
            private string _mediaAddressType;
            private string _startTime;
            private string _state;
            private string _stateCause;
            private string _stateChangeTime;
            private bool _me;
            #endregion

            #region Properties
            public ObservableCollection<String> Actions { get { return _actions; } set { _actions = value; OnPropertyChanged("Actions"); } }
            public String MediaAddress { get { return _mediaAddress; } set { _mediaAddress = value; OnPropertyChanged("MediaAddress"); } }
            public String MediaAddressType { get { return _mediaAddressType; } set { _mediaAddressType = value; OnPropertyChanged("MediaAddressType"); } }
            public String StartTime { get { return _startTime; } set { _startTime = value; OnPropertyChanged("StartTime"); } }
            public String State { get { return _state; } set { _state = value; OnPropertyChanged("State"); } }
            public String StateCause { get { return _stateCause; } set { _stateCause = value; OnPropertyChanged("StateCause"); } }
            public String StateChangeTime { get { return _stateChangeTime; } set { _stateChangeTime = value; OnPropertyChanged("StateChangeTime"); } }
            public Boolean Me { get { return _me; } set { _me = value; OnPropertyChanged("Me"); } }
            #endregion
        }
        #endregion
    }
}
