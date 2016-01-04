using Microsoft.AspNet.SignalR;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuclear.Modules.LogAnalyzer.Components
{
    public class AnalyzerNotifyer : IAnalyzerNotifyer
    {
        private object _progressLOCK;
        private int _currentPct = 0;
        private ILogAnalyzerHub _LogSignalRHub { get; set; }
        public long ProgressTotalCount { get; set; }
        public long ProgressIncrement { get; set; }
        public string CurrentHubTaskId { get; set; }

        public AnalyzerNotifyer(long total, long increment, string taskId, ILogAnalyzerHub hub)
        {
            _progressLOCK = new object();
            ProgressTotalCount = total;
            ProgressIncrement = increment;
            CurrentHubTaskId = taskId;
            _LogSignalRHub = hub;
        }

        public void UpdateProgress(long currentProgress)
        {
            lock(_progressLOCK)
            {
                int pi = Convert.ToInt32(currentProgress / ProgressIncrement);
                pi = (pi >= 99 ? 99 : pi);

                if (_currentPct != pi)
                {
                    _LogSignalRHub.NotifyProgress(CurrentHubTaskId, pi);
                    _currentPct = pi;
                    Thread.Sleep(50);  //Sleep for .05 sec in order for progress to display
                }
            }
        }
    }
}