using System; // Provides basic functionality for the program
using System.Data; // For working with DataTables and database operations
using Microsoft.Extensions.Configuration; // For loading configuration from files
using DataExportAutomation.Services; // Custom services for database and Excel export
using DataExportAutomation.Interfaces; // Interfaces for logging and database services
using NLog; // For logging
namespace DataExportAutomation // Define a suitable namespace for the program
{
    // Declare the Program class as static to prevent instantiation
    public static class Program
    {
        // Private fields for logging, database, and Excel export services
        private static ILoggingService _loggingService = new NLogLoggingService();
        private static IDatabaseService? _databaseService; // Nullable database service
        private static IExcelExportService? _excelExportService; // Nullable Excel export service

        // Entry point of the application
        static void Main(string[] args)
        {
            try
            {
                // Load NLog configuration from the file
                string nlogConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "nlog.config");
                LogManager.Setup().LoadConfigurationFromFile(nlogConfigPath); // Set up NLog from config file
                LogManager.Configuration.Variables["internalLogFile"] = "nlog-internal.log"; // Set internal log file

                _loggingService.LogInfo("NLog configuration loaded successfully."); // Log successful loading
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("NLog configuration file not found.");
                _loggingService.LogError(ex, "NLog configuration file not found."); // Log error if file not found
                return; // Stop execution if logging is not configured properly
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "Error loading NLog configuration."); // Log generic configuration error
                return;
            }

            IConfigurationRoot config;
            try
            {
                // Load application configuration from appsettings.json
                config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build(); // Build the configuration object
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Configuration file not found.");
                _loggingService.LogError(ex, "Configuration file not found."); // Log error if appsettings.json is missing
                return;
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "Error loading configuration."); // Log generic configuration error
                return;
            }

            try
            {
                // Retrieve connection string from configuration
                string connectionString = config.GetConnectionString("DefaultConnection")
                                           ?? throw new InvalidOperationException("Connection string is null.");
                _loggingService.LogInfo("Connection string loaded successfully."); // Log success

                // Initialize database and Excel export services
                IDatabaseConnection dbConnection = new PostgresConnection(connectionString); // Create PostgreSQL connection
                _databaseService = new PostgresDatabaseService(dbConnection, _loggingService); // Pass database connection to service
                _excelExportService = new ClosedXmlExcelExportService(_loggingService); // Initialize Excel export service
            }
            catch (InvalidOperationException ex)
            {
                _loggingService.LogError(ex, "Invalid connection string."); // Log if connection string is invalid
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "Error initializing services."); // Log generic initialization error
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }

            try
            {
                _loggingService.LogInfo("Starting data export process..."); // Log process start

                // Retrieve data from PostgreSQL database using a stored procedure
                DataTable dataTable = _databaseService.RetrieveData();

                if (dataTable.Rows.Count > 0)
                {
                    _loggingService.LogInfo($"Retrieved {dataTable.Rows.Count} rows from the database."); // Log row count
                                                                                                          // Export data to Excel
                    string filePath = GetExportFilePath(); // Generate the Excel file path
                    _excelExportService.ExportDataToExcel(dataTable, filePath); // Export data to Excel
                    _loggingService.LogInfo("Data Export Automation process completed successfully."); // Log success
                }
                else
                {
                    _loggingService.LogWarning("No data returned from the stored procedure."); // Log warning if no data
                }
            }
            catch (DataException ex)
            {
                _loggingService.LogError(ex, "Database error occurred."); // Log database-related errors
                Console.WriteLine($"Database Error: {ex.Message}");
            }
            catch (IOException ex)
            {
                _loggingService.LogError(ex, "File I/O error occurred."); // Log file-related errors
                Console.WriteLine($"File Error: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                _loggingService.LogError(ex, "File access error occurred."); // Log file permission errors
                Console.WriteLine($"Access Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "An error occurred during the data export process."); // Log any other errors
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Ensure NLog is flushed and shut down to write all logs
                LogManager.Shutdown();
            }
        }

        // Method to construct the export file path
        private static string GetExportFilePath()
        {
            try
            {
                // Generate a timestamped filename for the exported Excel file
                string timestamp = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
                string fileName = $"GetCountryStateCityData_{timestamp}.xlsx";
                // return Path.Combine(@"D:\OneDrive - ERP Logic LLC\Document", fileName); // Dynamic path if needed
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName); // Save to desktop
            }
            catch (Exception ex)
            {
                _loggingService.LogError(ex, "Error constructing file path."); // Log file path construction errors
                throw; // Re-throw to handle in calling method
            }
        }
    }
}
