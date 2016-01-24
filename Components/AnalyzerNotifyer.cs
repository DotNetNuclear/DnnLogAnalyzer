﻿using Microsoft.AspNet.SignalR;
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
        private long _progressCurrent { get; set; }
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
            _progressCurrent = 0;
        }

        public void UpdateProgress()
        {
            lock(_progressLOCK)
            {
                int pi = Convert.ToInt32(_progressCurrent / ProgressIncrement);
                pi = (pi >= 99 ? 99 : pi);

                if (_currentPct != pi)
                {
                    _LogSignalRHub.NotifyProgress(CurrentHubTaskId, pi);
                    _currentPct = pi;
                    Thread.Sleep(25);  //Sleep for .025 sec in order for progress to display
                }
            }
        }

        public void IncrementCurrentProgress()
        {
            _progressCurrent++;
        }
    }
}