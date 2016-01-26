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
using System.Web;

namespace DotNetNuclear.Modules.LogAnalyzer.Models
{
    public class FilterParams
    {
        public DateTime? Date { get; set; }

        public int Level { get; set; }

        public string Thread { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string Pattern { get; set; }

        public override string ToString()
        {
            return String.Format(
                "Date: {0}, Level: {1}, Thread: {2}, Logger: {3}, Message: {4}",
                this.Date, this.Level, this.Thread, this.Logger, this.Message);
        }
    }
}