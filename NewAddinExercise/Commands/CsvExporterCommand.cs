using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using RoomDataManager.Helpers;
using RoomDataManager.Exporters;

namespace RoomDataManager.Commands
{
    /// <summary>
    /// Exports data for all selected rooms to a timestamped CSV file saved to the user's Desktop.
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    internal class CsvExporterCommand : IExternalCommand
    {
        /// <summary>
        /// Collects selected rooms, builds a report for each, and writes the result to a CSV file.
        /// </summary>
        /// <param name="commandData">Provides access to the active Revit application and document.</param>
        /// <param name="message">Set to an error description if the command fails.</param>
        /// <param name="elements">Not used by this command.</param>
        /// <returns>Result.Succeeded if the export completes; Result.Failed if no rooms are selected.</returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            
            (List<Room> roomList,string selectionMessage) result = RoomHelper.GetSelectedRooms(uidoc:uidoc);
            
            if (result.roomList.Count == 0 || !string.IsNullOrEmpty(result.selectionMessage))
            {
                TaskDialog.Show(title: "CSV Exporter", mainInstruction: result.selectionMessage);
                return Result.Failed;
            }


            // Create report of each room
            List<RoomReport> roomReports = result.roomList
                                                  .Select(r => new RoomReport(r))
                                                  .ToList();

            
            
            // Get folder path
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //Export to CSV
            IExporter exporter = new CsvExporter(folderPath);
            exporter.Export(roomReports: roomReports);

            // Return a message to the Revit screen 
            TaskDialog dialog = new TaskDialog(title: "CSV Exporter");
            dialog.MainInstruction = "Export complete.";
            dialog.MainContent = $"File saved to: {folderPath}";
            dialog.Show();

            return Result.Succeeded;
        }
    }

}
