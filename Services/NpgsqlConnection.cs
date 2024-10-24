using System.Data; // Provides access to IDbCommand and other database-related interfaces
using DataExportAutomation.Interfaces; // Interface definition for database connection
using Npgsql; // Npgsql is a .NET library for PostgreSQL database access

namespace DataExportAutomation.Services
{
    // Service class for managing PostgreSQL database connections
    public class PostgresConnection : IDatabaseConnection
    {
        // Private field for NpgsqlConnection to manage the PostgreSQL connection
        private readonly NpgsqlConnection _connection;

        // Constructor to initialize the PostgreSQL connection using a connection string
        public PostgresConnection(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
        }

        // Opens the PostgreSQL database connection
        public void Open()
        {
            _connection.Open();
        }

        // Closes the PostgreSQL database connection
        public void Close()
        {
            _connection.Close();
        }

        // Creates and returns a PostgreSQL command for executing queries
        // Implements the IDatabaseConnection interface method
        public IDbCommand CreateCommand()
        {
            // Return a command object associated with the open connection
            return _connection.CreateCommand();
        }
    }
}
