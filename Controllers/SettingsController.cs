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
using System.Web.Mvc;
using DotNetNuke.Collections;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Security;
using DotNetNuclear.Modules.LogAnalyzer.Components.Settings;

namespace DotNetNuclear.Modules.LogAnalyzer.Controllers
{
    /// <summary>
    /// The Settings Controller manages the modules settings
    /// </summary>
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    [DnnHandleError]
    public class SettingsController : DnnController
    {
        ISettingsRepository _settingsRepo;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Settings()
        {
            _settingsRepo = new SettingsRepository(ActiveModule.ModuleID);
            var settings = new Models.Settings
            {
                LogAnalyzerRegex = _settingsRepo.LogAnalyzerRegex
            };

            return View(settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Settings(Models.Settings settings)
        {
            _settingsRepo = new SettingsRepository(ActiveModule.ModuleID);
            _settingsRepo.LogAnalyzerRegex = settings.LogAnalyzerRegex;
            //ModuleContext.Configuration.ModuleSettings["DNNuclear_LogAnalyzer_ParserRegEx"] = settings.LogAnalyzerRegex;

            return RedirectToDefaultRoute();
        }
    }
}