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
using DotNetNuclear.Modules.LogAnalyzer.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetNuclear.Modules.LogAnalyzer.Components
{
    public class LogFileParser
    {
        private IAnalyzerNotifyer _progress;
        private const string Separator = "[---]";

        public LogFileParser(IAnalyzerNotifyer notifyer)
        {
            _progress = notifyer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<LogItem> GetEntries(string dataSource, string loggerName, string logPattern)
        {
            if (String.IsNullOrEmpty(dataSource))
                throw new ArgumentNullException("dataSource");

            //if (String.IsNullOrEmpty(logPattern))
            //    logPattern = FileParserRegex;

            FileInfo file = new FileInfo(dataSource);
            if (!file.Exists)
                throw new FileNotFoundException("file not found", dataSource);

            List<LogItem> result = new List<LogItem>();

            using (StreamReader reader = file.OpenText())
            {
                string line;
                int logNum = 0;
                int classColonIdx;
                bool firstMatch = false;
                bool foundThrowable = false;
                LogItem entry = new LogItem();
                StringBuilder msg = new StringBuilder();

                while ((line = reader.ReadLine()) != null)
                {
                    _progress.IncrementCurrentProgress();
                    string message;
                    Regex matchLineRegEx = new Regex(logPattern);
                    Match m = matchLineRegEx.Match(line);
                    if (m.Success)
                    {
                        if (entry.Id > 0)
                        {
                            entry.Logger = loggerName;
                            entry.File = file.Name;
                            entry.Message = msg.ToString();
                            result.Add(entry);
                            entry = new LogItem();
                        }
                        msg.Clear();
                        logNum++;
                        foundThrowable = false;
                        firstMatch = true;
                        DateTime logDate = DateTime.Parse(m.Groups[1].Value); // matches '2008-01-17 20:10:54'
                        logDate = logDate.AddMilliseconds(Convert.ToDouble(m.Groups[2].Value)); // the third group is the milliseconds from the date
                        entry.Id = logNum;
                        entry.TimeStamp = logDate;
                        entry.MachineName = m.Groups[4].Value.Trim();
                        entry.Thread = m.Groups[5].Value.Trim();
                        entry.Level = m.Groups[6].Value.Trim();
                        entry.Class = m.Groups[8].Value.Trim();
                        message = m.Groups[10].Value.Trim();
                        classColonIdx = message.IndexOf(':');
                        if (classColonIdx > 0)
                        {
                            foundThrowable = true;
                            entry.Throwable = message.Substring(0, classColonIdx).Trim();
                            msg.Append(message.Substring(classColonIdx + 1).Trim());
                        }
                    }
                    else
                    {
                        message = line;
                        if (firstMatch && !(message.StartsWith("LOG:") || message.StartsWith("WRN:")))
                        {
                            if (!foundThrowable)
                            {
                                classColonIdx = message.IndexOf(':');
                                if (classColonIdx > 0)
                                {
                                    foundThrowable = true;
                                    entry.Throwable = message.Substring(0, classColonIdx).Trim();
                                    msg.Append(message.Substring(classColonIdx + 1).Trim());
                                }
                            }
                            else
                            {
                                msg.Append(message);
                            }
                        }
                    }
                    if (_progress != null) { _progress.UpdateProgress(); }
                }
                // Add last entry
                if (entry.Id > 0)
                {
                    entry.Logger = loggerName;
                    entry.File = file.Name;
                    entry.Message = msg.ToString();
                    result.Add(entry);
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static long GetLineCount(string dataSource)
        {
            long lineCount = 0;
            try
            {
                lineCount = File.ReadLines(dataSource).Count();
            }
            catch
            {
                lineCount = 0;
            }
            return lineCount;
        }
    }
}