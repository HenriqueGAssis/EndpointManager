namespace EndpointManager.Service
{
    public interface ILoggerService
    {
        void LogError(string message);
        void LogError(string message, Exception exception);
        void LogInformation(string message);
        void LogInformation(string message, Exception exception);
        void LogWarning(string message);
        void LogWarning(string message, Exception exception);
    }
}