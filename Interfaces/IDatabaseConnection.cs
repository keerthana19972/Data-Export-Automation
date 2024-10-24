using System.Data; // Provides access to database-related interfaces like IDbCommand

namespace DataExportAutomation.Interfaces // Define a suitable namespace for interfaces
{
    // Interface to abstract the database connection functionality
    public interface IDatabaseConnection
    {
        // Method to open the database connection
        void Open();

        // Method to close the database connection
        void Close();

        // Method to create and return a database command (SQL command)
        IDbCommand CreateCommand();
    }
}

