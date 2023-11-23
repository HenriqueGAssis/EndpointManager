using Microsoft.Extensions.Logging;

namespace EndpointManager.Service
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogInformation(string message, Exception exception)
        {
            _logger.LogInformation(message, exception);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogWarning(string message, Exception exception)
        {
            _logger.LogWarning(message, exception);
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        public void LogError(string message, Exception exception)
        {
            _logger.LogError(message, exception);
        }
    }
}
