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
using DotNetNuclear.Modules.LogAnalyzer.Components;

namespace DotNetNuclear.Modules.LogAnalyzer.Models
{
    public class LogViewModel
    {
        public List<FileListItem> FilesToAnalyze { get; set; }
        public List<LogItemCore> ReportedItems { get; set; }
    }
}