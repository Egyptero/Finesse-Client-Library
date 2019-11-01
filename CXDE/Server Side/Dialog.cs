using System.Collections;

namespace CXDE.Server_Side
{
    public class Dialog
    {
        public string _AssociatedDialogUri { get; set; }
        public string _FromAddress { get; set; }
        public string _ID { get; set; }
        public MediaProperties _MediaProperties { get; set; }
        public string _MediaType { get; set; }
        public string _State { get; set; }
        public string _ToAddress { get; set; }
        public string _URI { get; set; }
        public ArrayList _Participants { get; set; }
        public string _DialogEvent { get; set; }
        public class MediaProperties
        {
            public string _MediaID { get; set; }
            public string _DNIS { get; set; }
            public string _CallType { get; set; }
            public string _DialedNumber { get; set; }
            public string _OutboundClassification { get; set; }
            public ArrayList _CallVariables { get; set; }
            public class CallVariable
            {
                public string _Name { get; set; }
                public string _Value { get; set; }

            }
        }

        public class Participant
        {
            public ArrayList _Actions { get; set; }
            public string _MediaAddress { get; set; }
            public string _MediaAddressType { get; set; }
            public string _StartTime { get; set; }
            public string _State { get; set; }
            public string _StateCause { get; set; }
            public string _StateChangeTime { get; set; }
        }
    }
}
