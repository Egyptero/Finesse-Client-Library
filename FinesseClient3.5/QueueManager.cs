using FinesseClient.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace FinesseClient
{
    public class QueueManager : ObservableObject
    {
        private FinAgent _finAgent;
        private bool _stop;

        public Boolean Stop { get { return _stop; } set { _stop = value; OnPropertyChanged("Stop"); } }
        public QueueManager(FinAgent finAgent)
        {
            _finAgent = finAgent;
        }

        public void StartQueueMonitor()
        {
            while (!Stop)
            {
                ProcessMessages();
                Thread.Sleep(500); // wait 300 mili-seconds and recheck.
            }
        }
        public void ProcessMessages()
        {
            //while(_finAgent.MessageQueue.Count > 0)
            //{
            //    _finAgent.ExecuteMessage(_finAgent.MessageQueue.Dequeue() as XElement);
            //}
        }
    }
}
