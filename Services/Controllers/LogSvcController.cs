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
        /// API that returns Hello world
        /// </summary>
        /// <returns></returns>
        [HttpPost]  //[baseURL]/logsvc/analyze
        [ActionName("analyze")]
        [ValidateAntiForgeryToken]
        [SupportedModules(FeatureController.DESKTOPMODULE_NAME)]
        public HttpResponseMessage Analyze(LogServiceRequest req)
        {
            _settingsRepo = new SettingsRepository(ActiveModule.ModuleID);

            string taskId = req.taskId;
            string logPath = FileUtils.GetDnnLogPath() + "\\";
            var p = new LogAnalyzerHub();
            p.NotifyStart(taskId);
            Thread.Sleep(50);

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
                        li.ModuleId = ActiveModule.ModuleID;
                        li.Count = 1;
                        repo.InsertItem(li);
                    }
                }
                // Final piece of work
                Thread.Sleep(50);
                vm.ReportedItems = repo.GetRollupItems(ActiveModule.ModuleID).ToList();
                p.NotifyProgress(taskId, 100, string.Empty);
            }
            catch (Exception ex)
            {
                p.NotifyProgress(taskId, -1, "An error occurred analyzing the log: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                Thread.Sleep(500);
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