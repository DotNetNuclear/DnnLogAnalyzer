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
using System.Web;

namespace DotNetNuclear.Modules.LogAnalyzer.Components
{
    public class FileUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetDnnLogPath()
        {
            return System.Web.Hosting.HostingEnvironment.MapPath("~/portals/_default/Logs");
        }

        /// <summary>
        /// Format the file size to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatFileSize(double value)
        {
            string[] suffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            for (int i = 0; i < suffixes.Length; i++)
            {
                if (value <= (Math.Pow(1024, i + 1)))
                {
                    return threeNonZeroDigits(value / Math.Pow(1024, i)) + " " + suffixes[i];
                }
            }

            return threeNonZeroDigits(value /
                Math.Pow(1024, suffixes.Length - 1)) + " " + suffixes[suffixes.Length - 1];
        }

        private static string threeNonZeroDigits(double value)
        {
            if (value >= 100)
            {
                // No digits after the decimal.
                return value.ToString("0,0");
            }
            else if (value >= 10)
            {
                // One digit after the decimal.
                return value.ToString("0.0");
            }
            else
            {
                // Two digits after the decimal.
                return value.ToString("0.00");
            }
        }

    }
}