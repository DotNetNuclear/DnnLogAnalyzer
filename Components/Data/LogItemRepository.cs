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
using DotNetNuke.Data;
using DotNetNuclear.Modules.LogAnalyzer.Models;

namespace DotNetNuclear.Modules.LogAnalyzer.Components
{
    public class LogItemRepository : ILogItemRepository
    {
        public void InsertItem(LogItem t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LogItem>();
                rep.Insert((LogItem)t);
            }
        }

        public void DeleteAllItems(int moduleId)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LogItem>();
                rep.Delete("WHERE ModuleId=@0", moduleId);
            }
        }

        public void DeleteItem(LogItem t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LogItem>();
                rep.Delete((LogItem)t);
            }
        }

        public IEnumerable<LogItemCore> GetRollupItems(int moduleId)
        {
            IEnumerable<LogItemCore> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                t = ctx.ExecuteQuery<LogItemCore>(System.Data.CommandType.StoredProcedure, "DNNuclear_LogAnalyzer_RollupLogs", moduleId);
            }
            return t;
        }

        public LogItem GetItem(int itemId, int moduleId)
        {
            LogItem t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LogItem>();
                t = rep.GetById(itemId, moduleId);
            }
            return t;
        }
    }
}