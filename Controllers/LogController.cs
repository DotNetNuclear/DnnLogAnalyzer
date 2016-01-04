/*
' Copyright (c) 2015 DotNetNuclear LLC
' http://www.dotnetnuclear.com
' All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DotNetNuke.Common;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;

using DotNetNuclear.Modules.LogAnalyzer.Models;
using DotNetNuclear.Modules.LogAnalyzer.Components;
using DotNetNuke.Entities.Users;

namespace DotNetNuclear.Modules.LogAnalyzer.Controllers
{
    public class LogController : DnnController
    {
        //private const string _Log4NetPattern = "%date [%property{log4net:HostName}][Thread:%thread][%level] %logger - %message%newline";

        /// <summary>
        /// </summary>
        public LogController()
        {
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            LogViewModel vm = new LogViewModel();
            vm.FilesToAnalyze = new List<System.IO.FileInfo>();
            try
            {
                string logPath = ControllerContext.HttpContext.Server.MapPath("~/portals/_default/Logs");
                string[] logFileList = System.IO.Directory.GetFiles(logPath);
                Array.Sort(logFileList, StringComparer.InvariantCulture);
                foreach (string s in logFileList.Reverse())
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(s);
                    if (f.FullName.EndsWith(".log.resources"))
                    {
                        vm.FilesToAnalyze.Add(f);
                    }
                }
            }
            catch (Exception exc)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
            }
            return View(vm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        //public ActionResult Index(FormCollection collection)
        //{
        //    int moduleId = ModuleContext.ModuleId;
        //    LogViewModel vm = new LogViewModel();
        //    string logFile = collection["dnnuclear$fileSelector"];
        //    var analyzer = new FileEntriesProvider();
        //    FilterParams logFilter = new FilterParams();
        //    logFilter.Pattern = _Log4NetPattern;

        //    ILogItemRepository repo = new LogItemRepository();
        //    var logItems = analyzer.GetEntries(logFile, logFilter);

        //    repo.DeleteAllItems(moduleId);
        //    foreach(var li in logItems)
        //    {
        //        li.ModuleId = moduleId;
        //        li.Count = 1;
        //        repo.InsertItem(li);
        //    }
        //    vm.ReportedItems = repo.GetRollupItems(moduleId).ToList();
        //    return View(vm);
        //}

    }
}