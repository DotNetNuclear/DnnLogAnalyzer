namespace DotNetNuclear.Modules.LogAnalyzer.Components
{
    public interface ILogAnalyzerHub
    {
        void NotifyEnd(string taskId);
        void NotifyProgress(string taskId, int percentage, string errorMsg);
        void NotifyStart(string taskId);
    }
}