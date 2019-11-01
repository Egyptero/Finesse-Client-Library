using FinesseClient.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinesseClient.Model
{
    public class Queue : ObservableObject
    {
        #region Fields
        private string _uri;
        private string _name;
        private string _callsInQueue;
        private string _startTimeOfLongestCallInQueue;
        private string _agentsReady;
        private string _agentsNotReady;
        private string _agentsTalkingInbound;
        private string _agentsTalkingOutbound;
        private string _agentsTalkingInternal;
        private string _agentsWrapUpNotReady;
        private string _agentsWrapUpReady;
        private FinAgent _finAgent;
        #endregion

        #region Constructor
        public Queue()
        {

        }
        public Queue(FinAgent finAgent)
        {
            _finAgent = finAgent;
        }
        #endregion

        #region Properties
        public String Uri { get { return _uri; } set { _uri = value; OnPropertyChanged("Uri"); } }
        public String Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }
        public String CallsInQueue { get { return _callsInQueue; } set { if (!value.Equals("-1")) _callsInQueue = value; else _callsInQueue = "-"; OnPropertyChanged("CallsInQueue"); } }
        public String StartTimeOfLongestCallInQueue { get { return _startTimeOfLongestCallInQueue; } set { if (!value.Equals("-1")) _startTimeOfLongestCallInQueue = value; else _startTimeOfLongestCallInQueue = "-"; OnPropertyChanged("StartTimeOfLongestCallInQueue"); } }
        public String AgentsReady { get { return _agentsReady; } set { if (!value.Equals("-1")) _agentsReady = value; else _agentsReady = "-"; OnPropertyChanged("AgentsReady"); } }
        public String AgentsNotReady { get { return _agentsNotReady; } set { if (!value.Equals("-1")) _agentsNotReady = value; else _agentsNotReady = "-"; OnPropertyChanged("AgentsNotReady"); } }
        public string AgentsTalkingInbound { get { return _agentsTalkingInbound; } set { if (!value.Equals("-1")) _agentsTalkingInbound = value; else _agentsTalkingInbound = "-"; OnPropertyChanged("AgentsTalkingInbound"); } }
        public string AgentsTalkingOutbound { get { return _agentsTalkingOutbound; } set { if (!value.Equals("-1")) _agentsTalkingOutbound = value; else _agentsTalkingOutbound = "-"; OnPropertyChanged("AgentsTalkingOutbound"); } }
        public string AgentsTalkingInternal { get { return _agentsTalkingInternal; } set { if (!value.Equals("-1")) _agentsTalkingInternal = value; else _agentsTalkingInternal = "-"; OnPropertyChanged("AgentsTalkingInternal"); } }
        public string AgentsWrapUpNotReady { get { return _agentsWrapUpNotReady; } set { if (!value.Equals("-1")) _agentsWrapUpNotReady = value; else _agentsWrapUpNotReady = "-"; OnPropertyChanged("AgentsWrapUpNotReady"); } }
        public string AgentsWrapUpReady { get { return _agentsWrapUpReady; } set { if (!value.Equals("-1")) _agentsWrapUpReady = value; else _agentsWrapUpReady = "-"; OnPropertyChanged("AgentsWrapUpReady"); } }
        #endregion
    }
}
