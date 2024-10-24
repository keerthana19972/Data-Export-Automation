using System.Data; // Provides access to DataTable and database-related interfaces
using DataExportAutomation.Interfaces; // Import custom interfaces for database connection and logging

namespace DataExportAutomation.Services
{
    // Service class for handling PostgreSQL database operations
    public class PostgresDatabaseService : IDatabaseService
    {
        // Private fields for database connection and logging services
        private readonly IDatabaseConnection _databaseConnection;
        private readonly ILoggingService _loggingService;

        // Constructor to initialize the database connection and logging service via dependency injection
        public PostgresDatabaseService(IDatabaseConnection databaseConnection, ILoggingService loggingService)
        {
            _databaseConnection = databaseConnection;
            _loggingService = loggingService;
        }

        // Method to retrieve data from the PostgreSQL database and return it as a DataTable
        public DataTable RetrieveData()
        {
            // Log the start of the data retrieval process
            _loggingService.LogInfo("Retrieve data from the database...");

            try
            {
                // Open the database connection
                _databaseConnection.Open();

                // Create a command using the open connection
                using var cmd = _databaseConnection.CreateCommand();
                
                // SQL command text to call the PostgreSQL function 'GetCountryStateCityData'
                cmd.CommandText = "SELECT * FROM GetCountryStateCityData()";

                // Execute the query and load the result into a DataTable
                using var reader = cmd.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);

                // Log successful retrieval of data
                _loggingService.LogInfo("Data successfully retrieved from the database.");
                
                return dataTable; // Return the populated DataTable
            }
            catch (Exception ex)
            {
                // Log the error along with the exception details
                _loggingService.LogError(ex, "An error occurred while retrieving data.");
                throw; // Re-throw the exception to propagate it further
            }
            finally
            {
                // Ensure the database connection is closed, even if an exception occurs
                _databaseConnection.Close();
            }
        }
    }
}
