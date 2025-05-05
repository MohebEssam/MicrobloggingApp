using MicrobloggingApp.Infrastructure.Interfacses;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MicrobloggingApp.Infrastructure.Services
{
    public class EventViewerLogger : IExceptionLogger
    {
        private readonly ILogger<EventViewerLogger> _logger;
        private const string SourceName = "MicrobloggingApp";
        private const string LogName = "Application";

        public EventViewerLogger(ILogger<EventViewerLogger> logger)
        {
            _logger = logger;

            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, LogName);
            }
        }

        public void Log(Exception ex, string context = null)
        {
            string message = $"{DateTime.UtcNow}: {(context ?? "Exception")} - {ex}";

            // Log to Event Viewer
            try
            {
                EventLog.WriteEntry(SourceName, message, EventLogEntryType.Error);
            }
            catch (Exception eventLogEx)
            {
                _logger.LogError(eventLogEx, "Failed to write to Event Viewer.");
            }

            // Also log to .NET ILogger (useful for App Insights or other sinks)
            _logger.LogError(ex, message);
        }
    }
}
