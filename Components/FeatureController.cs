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
using DotNetNuke.Entities.Controllers;
using DotNetNuke.Entities.Modules;
using System;

namespace DotNetNuclear.Modules.LogAnalyzer.Components
{
    public class FeatureController
    {
        public const string DESKTOPMODULE_NAME = "DotNetNuclear.LogAnalyzer";
        public const string DESKTOPMODULE_FRIENDLYNAME = "Log Analyzer";

        #region IUpgradeable Interface

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="Version">The current version of the module</param>
        /// -----------------------------------------------------------------------------
        public string UpgradeModule(string Version)
        {
            // Any version changes, check for friendly url exceptions
            string ret = string.Empty;

            // Do this for a specific version upgrade
            switch (Version)
            {
                case "01.00.00":
                    break;
            }
            return ret;
        }

        #endregion

        public static string AddHostSettingUrlRewiteException()
        {
            string newAUMExp = string.Empty;
            var hostSettings = HostController.Instance.GetSettingsDictionary();

            if (hostSettings.ContainsKey("AUM_DoNotRewriteRegEx"))
            {
                bool signalrExp = hostSettings["AUM_DoNotRewriteRegEx"].IndexOf("/signalr", System.StringComparison.OrdinalIgnoreCase) >= 0;
                if (!signalrExp)
                {
                    newAUMExp = hostSettings["AUM_DoNotRewriteRegEx"] + "|/signalr";
                }
            }
            else
            {
                newAUMExp = @"/desktopmodules/|/providers|/linkclick\.aspx|/signalr";
            }

            if (!string.IsNullOrEmpty(newAUMExp))
            {
                HostController.Instance.Update("AUM_DoNotRewriteRegEx", newAUMExp, true);
            }

            return newAUMExp;
        }
    }
}
