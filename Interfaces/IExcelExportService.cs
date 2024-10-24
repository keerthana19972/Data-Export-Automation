using System.Data; // Provides access to the DataTable class

namespace DataExportAutomation.Interfaces // Define a suitable namespace for interfaces
{
    // Interface to abstract the Excel export functionality
    public interface IExcelExportService
    {
        // Method to export data from a DataTable to an Excel file
        // Parameters:
        //   dataTable: The data to be exported in the form of a DataTable
        //   filePath: The file path where the Excel file will be saved
        void ExportDataToExcel(DataTable dataTable, string filePath);
    }
}
