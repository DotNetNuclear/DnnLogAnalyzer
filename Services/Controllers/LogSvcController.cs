using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using DotNetNuke.Web.Api;
using DotNetNuclear.Modules.LogAnalyzer.Components;
using DotNetNuclear.Modules.LogAnalyzer.Models;

namespace DotNetNuclear.Modules.LogAnalyzer.Services.Controllers
{
    ///desktopmodules/dotnetnuclear.loganalyzer/api/logsvc/analyze
    public class LogSvcController : DnnApiController
    {
        public LogSvcController()
        {
        }

        /// <summary>
        /// API that returns Hello world
        /// </summary>
        /// <returns></returns>
        [HttpPost]  //[baseURL]/logsvc/analyze
        [ActionName("analyze")]
        [ValidateAntiForgeryToken]
        [SupportedModules(FeatureController.DESKTOPMODULE_NAME)]
        public HttpResponseMessage Analyze(LogServiceRequest req)
        {
            string taskId = req.taskId;
            var p = new LogAnalyzerHub();
            p.NotifyStart(taskId);
            Thread.Sleep(2000);

            LogViewModel vm = new LogViewModel();
            ILogItemRepository repo = new LogItemRepository();
            long totalLogLines = 0, lineIncrement = 0;

            FilterParams logFilter = new FilterParams();
            logFilter.Pattern = "HI"; //TODO get setting for regex pattern

            repo.DeleteAllItems(ActiveModule.ModuleID);
            foreach (string logFile in req.files)
            {
                totalLogLines += LogFileParser.GetLineCount(logFile);
            }
            lineIncrement = Convert.ToInt64(totalLogLines / 100);

            IAnalyzerNotifyer progressNotifyer = new AnalyzerNotifyer(totalLogLines, lineIncrement, taskId, p);

            var analyzer = new LogFileParser(progressNotifyer);

            foreach (string logFile in req.files)
            {
                var logItems = analyzer.GetEntries(logFile, logFilter);
                foreach (var li in logItems)
                {
                    li.ModuleId = ActiveModule.ModuleID;
                    li.Count = 1;
                    repo.InsertItem(li);
                }
            }
            // Final piece of work
            Thread.Sleep(1000);
            vm.ReportedItems = repo.GetRollupItems(ActiveModule.ModuleID).ToList();
            p.NotifyProgress(taskId, 100);
            Thread.Sleep(2000);
            p.NotifyEnd(taskId);

            return Request.CreateResponse(HttpStatusCode.OK, vm);
        }
    }

    public class LogServiceRequest
    {
        public List<string> files { get; set; }
        public string taskId { get; set; }
    }
}