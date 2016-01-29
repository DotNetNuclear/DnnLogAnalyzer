/*
' Copyright (c) 2016 DotNetNuclear LLC
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
            vm.FilesToAnalyze = new List<FileListItem>();
            try
            {
                string logPath = FileUtils.GetDnnLogPath();
                string[] logFileList = System.IO.Directory.GetFiles(logPath);
                Array.Sort(logFileList, StringComparer.InvariantCulture);
                foreach (string s in logFileList.Reverse())
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(s);
                    if (f.FullName.EndsWith(".log.resources"))
                    {
                        if (f.Length > 0)
                        {
                            vm.FilesToAnalyze.Add(new FileListItem
                            {
                                FileSize = FileUtils.FormatFileSize((double)f.Length),
                                Name = f.Name
                            });
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
            }
            return View(vm);
        }

    }
}