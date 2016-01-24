using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuclear.Modules.LogAnalyzer.Components
{
    public interface IAnalyzerNotifyer
    {
        long ProgressTotalCount { get; set; }
        long ProgressIncrement { get; set; }
        string CurrentHubTaskId { get; set; }
        void UpdateProgress();
        void IncrementCurrentProgress();
    }
}