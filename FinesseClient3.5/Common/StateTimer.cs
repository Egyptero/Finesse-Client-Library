using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace FinesseClient.Common
{
    public class StateTimer : ObservableObject
    {
        private string _timerLabel;
        private DateTime _stateStartTime;
        private string _prefixMessage;
        private DispatcherTimer _dispaterTimer;

        public String TimerLabel {get{ return _timerLabel; } set { _timerLabel = value; OnPropertyChanged("TimerLabel"); } }
        public StateTimer(DateTime stateStartTime, string prefixMessage)
        {
            _prefixMessage = prefixMessage;
            _dispaterTimer = new DispatcherTimer();
            _stateStartTime = stateStartTime;
            _dispaterTimer.Interval = new TimeSpan(0, 0, 1);
            _dispaterTimer.Tick += StateTimer_Tick;
            _dispaterTimer.Start();
        }
        public void ResetTimer(DateTime stateStartTime)
        {
            try
            {
                _dispaterTimer.Stop();
                _stateStartTime = stateStartTime;
                _dispaterTimer.Start();
            }catch(Exception)
            { }
        }
        public void StopTimer()
        {
            if (_dispaterTimer != null)
                _dispaterTimer.Stop();
        }

        private void StateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                TimeSpan timeSpan = TimeSpan.Zero;
                //if(DateTime.Now.CompareTo(_stateStartTime) >= 0)
                timeSpan = DateTime.Now.Subtract(_stateStartTime);


                string LabelContent = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");

                if (timeSpan.Days > 0)
                    LabelContent = timeSpan.Days.ToString("00") + "(days) " + timeSpan.Hours.ToString("00") + ":" + LabelContent;
                else if (timeSpan.Hours > 0 && timeSpan.Days == 0)
                    LabelContent = timeSpan.Hours.ToString("00") + ":" + LabelContent;
                TimerLabel = _prefixMessage + LabelContent;
            }catch(Exception)
            { }
        }
    }
}
