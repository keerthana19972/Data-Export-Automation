using DataExportAutomation.Interfaces; // Interface definition for Excel export service
using ClosedXML.Excel; // ClosedXML library for working with Excel files
using System.Data; // Provides access to DataTable for data handling

namespace DataExportAutomation.Services
{
    // Service class to handle exporting data to Excel using ClosedXML
    public class ClosedXmlExcelExportService : IExcelExportService
    {
        // Private field for logging service to log events and errors
        private readonly ILoggingService _loggingService;

        // Constructor that initializes the logging service through dependency injection
        public ClosedXmlExcelExportService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        // Method to export data from a DataTable to an Excel file at the specified file path
        public void ExportDataToExcel(DataTable dataTable, string filePath)
        {
            try
            {
                // Log the start of the export process
                _loggingService.LogInfo($"Exporting data to Excel file at: {filePath}");

                // Create a new Excel workbook using ClosedXML
                using (var workbook = new XLWorkbook())
                {
                    // Add a worksheet named "Data" and insert the DataTable as a table starting from cell (1, 1)
                    var worksheet = workbook.Worksheets.Add("Data");
                    worksheet.Cell(1, 1).InsertTable(dataTable);

                    // Save the workbook to the specified file path
                    workbook.SaveAs(filePath);
                }

                // Log successful completion of the export process
                _loggingService.LogInfo("Data exported successfully.");
            }
            catch (Exception ex)
            {
                // Log any exception that occurs during the export process
                _loggingService.LogError(ex, "An error occurred while exporting data to Excel.");
            }
        }
    }
}
