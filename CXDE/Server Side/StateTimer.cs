using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CXDE.Server_Side
{
    public class StateTimer
    {
        private TextBlock _TimerLabel { get; set; }
        private DateTime _StateStartTime { get; set; }

        private string _PrefixMessage { get; set; }
        private DispatcherTimer _DispaterTimer;
        public StateTimer(DateTime stateStartTime, TextBlock timerLabel, string prefixMessage)
        {
            _PrefixMessage = prefixMessage;
            _DispaterTimer = new DispatcherTimer();

            _TimerLabel = timerLabel;
            _StateStartTime = stateStartTime;
            _DispaterTimer.Interval = new TimeSpan(0, 0, 1);
            _DispaterTimer.Tick += StateTimer_Tick;
            _DispaterTimer.Start();
        }
        public void ResetTimer(DateTime stateStartTime)
        {
            _DispaterTimer.Stop();
            _StateStartTime = stateStartTime;
            _DispaterTimer.Start();
        }

        private void StateTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan timeSpan = DateTime.Now - _StateStartTime;

            string LabelContent = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");

            if (timeSpan.Days > 0)
                LabelContent = timeSpan.Days.ToString("00") + "(days) " + timeSpan.Hours.ToString("00") + ":" + LabelContent;
            else if (timeSpan.Hours > 0 && timeSpan.Days == 0)
                LabelContent = timeSpan.Hours.ToString("00") + ":" + LabelContent;
            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => _TimerLabel.Text = _PrefixMessage + LabelContent));
        }
    }
}
