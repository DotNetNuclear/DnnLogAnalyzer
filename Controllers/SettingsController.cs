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
using System.Web.Mvc;
using DotNetNuke.Collections;
using DotNetNuke.Web.Mvc.Framework.Controllers;

using DotNetNuclear.Modules.LogAnalyzer.Models;

namespace DotNetNuclear.Modules.LogAnalyzer.Controllers
{
    /// <summary>
    /// The Settings Controller manages the modules settings
    /// </summary>
    public class SettingsController : DnnController
    {
        /// <summary>
        /// SettingsController Constructor
        /// </summary>
        public SettingsController()
        {
        }

        /// <summary>
        /// The Index action renders the default Settings View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var settings = new Settings
            {
                LogAnalyzerRegex = @"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}),(\d{3})(\s)\[(.*?)\]\[(.*?)\]\[(.*?)\](\s)(.*?)\-(\s+)(.*)",
                ModuleId = base.ActiveModule.ModuleID
            };
            return View(settings);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection collection)
        {
            return View();
        }
    }
}