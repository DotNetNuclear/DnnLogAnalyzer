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
using DotNetNuke.ComponentModel.DataAnnotations;

namespace DotNetNuclear.Modules.LogAnalyzer.Models
{
    [TableName("DNNuclear_LogAnalyzer_LogItem")]
    //setup the primary key for table
    [PrimaryKey("Id", AutoIncrement = true)]
    //scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    [Scope("ModuleId")]
    public class LogItem : LogItemCore
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Logger { get; set; }
        public string Thread { get; set; }
        public string MachineName { get; set; }
        public string File { get; set; }

        /// <summary>
        /// LevelIndex
        /// </summary>
        [IgnoreColumn]
        public LevelIndex LevelIndex { get; set; }

        /// <summary>
        /// Level Property
        /// </summary>
        public override string Level
        {
            get { return _level; }
            set
            {
                if (value != _level)
                {
                    _level = value;
                    assignLevelIndex(_level);
                }
            }
        }

        #region Privates

        private string _level;

        private void assignLevelIndex(string level)
        {
            string ul = !String.IsNullOrWhiteSpace(level) ? level.Trim().ToUpper() : string.Empty;
            switch (ul)
            {
                case "DEBUG":
                    LevelIndex = LevelIndex.DEBUG;
                    break;
                case "INFO":
                    LevelIndex = LevelIndex.INFO;
                    break;
                case "WARN":
                    LevelIndex = LevelIndex.WARN;
                    break;
                case "ERROR":
                    LevelIndex = LevelIndex.ERROR;
                    break;
                case "FATAL":
                    LevelIndex = LevelIndex.FATAL;
                    break;
                default:
                    LevelIndex = LevelIndex.NONE;
                    break;
            }
        }

        #endregion
    }

    public enum LevelIndex
    {
        NONE = 0,
        DEBUG = 1,
        INFO = 2,
        WARN = 3,
        ERROR = 4,
        FATAL = 5
    }
}