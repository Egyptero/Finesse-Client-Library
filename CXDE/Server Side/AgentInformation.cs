using System.Collections;

namespace CXDE.Server_Side
{
    public class AgentInformation
    {
        public string ActiveSite { get; set; }
        public string AgentID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Extension { get; set; }
        public string DomainA { get; set; }
        public string DomainB { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string PendingStatus { get; set; }
        public string StateChangeTime { get; set; }
        public string ReasonCodeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamID { get; set; }
        public string TeamName { get; set; }
        public MessageEvent _MessageEvent { get; set; }
        public ReasonCode _ReasonCode { get; set; }
        public Setting _Setting { get; set; }
        public ArrayList Teams { get; set; }
        public ArrayList Dialogs { get; set; }
        public ArrayList Roles { get; set; }
        public ArrayList NotReadyReasonCodeList { get; set; }
        public ArrayList LogoutReasonCodeList { get; set; }
        public string XmppPort { get; set; }// = "7071";
        public string XmppURL { get; set; }// = "/http-bind/";
        public string HttpPort { get; set; }// = "80";
        public string HttpURL { get; set; }// = "/finesse";
        public string XmppConnectionType { get; set; }
        public string HttpConnectionType { get; set; }
        public bool Ssl { get; set; }

        public class Team
        {
            public string URI { get; set; }
            public string ID { get; set; }
            public string Name { get; set; }
        }

        public class ReasonCode
        {
            public string Category { get; set; }
            public string URI { get; set; }
            public string Code { get; set; }
            public string Label { get; set; }
            public string ForAll { get; set; }
            public string ID { get; set; }
        }

        public class Setting
        {
            public string WrapUpOnIncoming { get; set; }
        }
    }
}
