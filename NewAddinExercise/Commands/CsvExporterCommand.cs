using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using RoomDataManager.Helpers;
using RoomDataManager.Exporters;
using RoomDataManager.Factories;


namespace RoomDataManager.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class CsvExporterCommand : IExternalCommand
    {
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
            List<RoomReport> roomReports = result.roomList.Select(r => ParameterHelper.TryWriteComment(r, uidoc.Document, "Pending Review"))
                                                          .ToList();

            
            
            // Get folder path
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //Export to CSV
            IExporter exporter = ExporterFactory.Create("csv", folderPath);
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
