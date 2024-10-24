using DataExportAutomation.Interfaces; // Interface definition for logging service
using NLog; // NLog library for logging

namespace DataExportAutomation.Services
{
    /// <summary>
    /// NLogLoggingService is an implementation of the ILoggingService interface, using NLog for logging functionality.
    /// </summary>
    public class NLogLoggingService : ILoggingService
    {
        // NLog Logger instance for logging
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor to initialize NLog with a specific configuration file.
        /// </summary>
        /// <param name="configFilePath">The file path for NLog configuration.</param>
        public NLogLoggingService(string configFilePath)
        {
            // Load NLog configuration from the specified file
            LogManager.Setup().LoadConfigurationFromFile(configFilePath);
            // Set the internal log file variable (used for internal NLog logging, such as errors in the logging framework)
            LogManager.Configuration.Variables["internalLogFile"] = "nlog-internal.log";
        }

        /// <summary>
        /// Default constructor that initializes the logger without any specific configuration file.
        /// </summary>
        public NLogLoggingService()
        {
            // Re-initializing the logger instance in case no file path is provided
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The information message to log.</param>
        public void LogInfo(string message)
        {
            _logger.Info(message); // Log the message at Info level
        }

        /// <summary>
        /// Logs an error message along with an exception.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="message">The error message to log.</param>
        public void LogError(Exception ex, string message)
        {
            _logger.Error(ex, message); // Log the exception and message at Error level
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public void LogWarning(string message)
        {
            _logger.Warn(message); // Log the message at Warn level
        }

        /// <summary>
        /// Logs a debug message, useful during development for troubleshooting.
        /// </summary>
        /// <param name="message">The debug message to log.</param>
        public void LogDebug(string message)
        {
            _logger.Debug(message); // Log the message at Debug level
        }

        /// <summary>
        /// Logs a trace message, providing detailed information about the application's execution flow.
        /// </summary>
        /// <param name="message">The trace message to log.</param>
        public void LogTrace(string message)
        {
            _logger.Trace(message); // Log the message at Trace level
        }
    }
}
