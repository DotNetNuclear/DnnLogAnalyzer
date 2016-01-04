namespace DotNetNuclear.Modules.LogAnalyzer.Components
{
    public interface ILogAnalyzerHub
    {
        void NotifyEnd(string taskId);
        void NotifyProgress(string taskId, int percentage);
        void NotifyStart(string taskId);
    }
}