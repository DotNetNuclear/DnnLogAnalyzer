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
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace DotNetNuclear.Modules.LogAnalyzer.Components
{
    public class LogAnalyzerHub : Hub, ILogAnalyzerHub
    {
        #region Hub base Overrides

        public override Task OnConnected()
        {
            //Console.WriteLine("[{0}] Client '{1}' connected.", DateTime.Now.ToString("dd-mm-yyyy hh:MM:ss"), Context.ConnectionId);
            return base.OnConnected();
        }

        #endregion

        public void NotifyStart(string taskId)
        {
            string userId = Guid.NewGuid().ToString();
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<LogAnalyzerHub>();
            hubContext.Clients.All.procStart(taskId, userId);
        }
        public void NotifyProgress(string taskId, int percentage, string errorMsg)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<LogAnalyzerHub>();
            hubContext.Clients.All.progress(taskId, percentage, errorMsg);
        }
        public void NotifyEnd(string taskId)
        {
            var hubcontext = GlobalHost.ConnectionManager.GetHubContext<LogAnalyzerHub>();
            hubcontext.Clients.All.procComplete(taskId);
        }
    }
}