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
using DotNetNuclear.Modules.LogAnalyzer.Components.Settings;

namespace DotNetNuclear.Modules.LogAnalyzer.Services.Controllers
{
    //[baseURL]=/desktopmodules/dotnetnuclear.loganalyzer/api
    public class LogSvcController : DnnApiController
    {
        ISettingsRepository _settingsRepo;

        public LogSvcController()
        {
        }

        /// <summary>
        /// API that starts the log analyzer process
        /// </summary>
        /// <returns></returns>
        [HttpPost]  //[baseURL]/logsvc/analyze
        [ActionName("analyze")]
        [ValidateAntiForgeryToken]
        [SupportedModules(FeatureController.DESKTOPMODULE_NAME)]
        public HttpResponseMessage Analyze(LogServiceRequest req)
        {
            _settingsRepo = new SettingsRepository(ActiveModule.ModuleID);

            long logItemCount = 0;
            string taskId = req.taskId;
            string logPath = FileUtils.GetDnnLogPath() + "\\";
            var p = new LogAnalyzerHub();
            p.NotifyStart(taskId);

            LogViewModel vm = new LogViewModel();
            ILogItemRepository repo = new LogItemRepository();
            long totalLogLines = 0, lineIncrement = 0;

            repo.DeleteAllItems(ActiveModule.ModuleID);
            foreach (string logFile in req.files)
            {
                totalLogLines += LogFileParser.GetLineCount(logPath + logFile);
            }
            lineIncrement = Convert.ToInt64(totalLogLines / 100);

            IAnalyzerNotifyer progressNotifyer = new AnalyzerNotifyer(totalLogLines, lineIncrement, taskId, p);

            try
            {
                var analyzer = new LogFileParser(progressNotifyer);

                foreach (string logFile in req.files)
                {
                    var logItems = analyzer.GetEntries(logPath + logFile, "log4net", _settingsRepo.LogAnalyzerRegex);
                    foreach (var li in logItems)
                    {
                        logItemCount++;
                        li.ModuleId = ActiveModule.ModuleID;
                        li.Count = 1;
                        repo.InsertItem(li);
                    }
                }

                if (logItemCount > 0)
                {
                    // Rollup results and produce report object
                    var reportItems = repo.GetRollupItems(ActiveModule.ModuleID).ToList();
                    vm.ReportedItems = reportItems.GroupBy(r => r.Level, r => r,
                               (key, g) => new LogItemCollection {
                                    Level = key,
                                    Items = g.Take(100).ToList()
                                }
                            ).ToList();
                    p.NotifyProgress(taskId, 100, string.Empty);
                }
                else
                {
                    p.NotifyProgress(taskId, -1, "Log files analyzed contain no entries.");
                }
            }
            catch (Exception ex)
            {
                p.NotifyProgress(taskId, -1, "An error occurred analyzing the log: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                p.NotifyEnd(taskId);
            }

            return Request.CreateResponse(HttpStatusCode.OK, vm);
        }
    }

    public class LogServiceRequest
    {
        public List<string> files { get; set; }
        public string taskId { get; set; }
    }
}