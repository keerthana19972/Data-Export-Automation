using System.Data; // Provides access to the DataTable class and other database-related components

namespace DataExportAutomation.Interfaces // Define a suitable namespace for interfaces
{
    // Interface to abstract the database service functionality
    public interface IDatabaseService
    {
        // Method to retrieve data from the database
        // Returns a DataTable containing the retrieved data
        DataTable RetrieveData();
    }
}

