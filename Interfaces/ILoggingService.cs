namespace DataExportAutomation.Interfaces // Define a suitable namespace for interfaces
{
    // Interface to abstract the logging functionality
    public interface ILoggingService
    {
        // Method to log informational messages
        // Parameters:
        //   message: The information message to log
        void LogInfo(string message);

        // Method to log error messages, including exception details
        // Parameters:
        //   ex: The exception object that contains the error details
        //   message: A custom error message to provide context
        void LogError(Exception ex, string message);

        // Method to log warning messages
        // Parameters:
        //   message: The warning message to log
        void LogWarning(string message);
    }
}
