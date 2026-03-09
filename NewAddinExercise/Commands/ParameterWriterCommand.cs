using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using RoomDataManager.Helpers; 



namespace RoomDataManager.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class ParameterWriterCommand : IExternalCommand
    {   
        /// <summary>
        /// Generates a report for each room, checking if comments exist in the Parameters.
        /// If comments do not exist, updates the parameter.
        /// </summary>
        /// <param name = "commandData" > An object that provides contextual information about the current Revit application and active document,
        /// including access to the user interface.</param>
        /// <param name="message">A reference to a string that can be set to provide an error message to the user if the command fails.</param>
        /// <param name="elements">A set of elements that can be used to highlight or select elements in the Revit user interface. This
        /// parameter is not used by this command.</param>
        /// <returns>
        /// A string report of each room involving its name, number, area and status
        /// </returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Get rooms (either hand selected or by floor) 
            var results = RoomHelper.GetSelectedRooms(uidoc);
            if (results.roomsList.Count == 0 || !string.IsNullOrEmpty(results.message))
            {
                TaskDialog.Show(title: "Error Message", mainInstruction: results.message);
                return Result.Failed; 
            }

            List<RoomReport> roomReportList = new List<RoomReport>();
            // for each room make a report 
            foreach (Room room in results.roomsList)
            {
                try
                {
                    RoomReport report = ParameterHelper.TryWriteComment(room, doc);
                    roomReportList.Add(report);
                }

                catch (Exception e)
                {
                    File.AppendAllText(path: "log.txt", $"{e}");
                }

            }

            // compile and show the report in task dialog
            string allRoomReport = RoomReport.MakeReport(roomReportList);
            TaskDialog.Show(title: "Room Reporter", mainInstruction: allRoomReport);

            return Result.Succeeded;
        }
    }
}

