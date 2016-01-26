/*
' Copyright (c) 2016 DotNetNuclear LLC
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/
using System;
using System.Collections;
using DotNetNuke.Entities.Modules;

namespace DotNetNuclear.Modules.LogAnalyzer.Components.Settings
{
    /// <summary>
    /// Implementation class for module settings data
    /// </summary>
    public class SettingsRepository : ISettingsRepository
    {
        /// <summary>
        /// </summary>
        public static bool SettingsChanged = false;
        private ModuleController _controller;
        private int _moduleId;

        /// <summary>
        /// </summary>
        public SettingsRepository(int moduleId)
        {
            _controller = new ModuleController();
            _moduleId = moduleId;
        }

        #region setting methods

        /// <summary>
        /// </summary>
        public T ReadSetting<T>(string settingName, T defaultValue)
        {
            Hashtable settings = _controller.GetModule(_moduleId, DotNetNuke.Common.Utilities.Null.NullInteger, true).ModuleSettings;

            T ret = default(T);

            if (settings.ContainsKey(settingName))
            {
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                try
                {
                    ret = (T)tc.ConvertFrom(settings[settingName]);
                }
                catch
                {
                    ret = defaultValue;
                }
            }
            else
                ret = defaultValue;

            return ret;
        }

        /// <summary>
        /// </summary>
        public void WriteSetting(string settingName, string value)
        {
            _controller.UpdateModuleSetting(_moduleId, settingName, value);
            SettingsChanged = true;
        }

        #endregion

        #region public properties

        /// <summary>
        /// </summary>
        public string LogAnalyzerRegex
        {
            get { return ReadSetting<string>("LogAnalyzerRegex", @"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}),(\d{3})(\s)\[(.*?)\]\[(.*?)\]\[(.*?)\](\s)(.*?)\-(\s+)(.*)"); }
            set { WriteSetting("LogAnalyzerRegex", value); }
        }

        #endregion

    }
}