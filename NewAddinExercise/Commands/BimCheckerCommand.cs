using System.IO;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using RoomDataManager.Checkers;
using RoomDataManager.Helpers;

namespace RoomDataManager.Commands
{
    /// <summary>
    /// Implements a Revit external command that validates selected rooms against predefined criteria and reports any
    /// issues found.
    /// </summary>
    /// <remarks>This command checks selected rooms for minimum area requirements and required parameters. If
    /// issues are detected, they are reported in a dialog and comments are added to the respective rooms. Any
    /// exceptions encountered during execution are logged to a file on the user's desktop. Use this command to ensure
    /// room data integrity within a Revit project.</remarks>
    [Transaction(TransactionMode.Manual)]
    internal class BimCheckerCommand : IExternalCommand
    {
        /// <summary>
        /// Executes the BIM checker command, validating selected rooms against minimum area requirements and required
        /// parameters, and reports any issues found.
        /// </summary>
        /// <remarks>This method checks selected rooms for compliance with minimum area and required
        /// parameter criteria. If issues are detected, they are reported to the user and written as comments to the
        /// affected rooms. If no issues are found, a success message is displayed. In the event of an exception, the
        /// error is logged to a file on the user's desktop.</remarks>
        /// <param name="commandData">The external command data providing application context and access to the active user interface document.</param>
        /// <param name="message">A reference to a string used to return error messages or status information to the user.</param>
        /// <param name="elements">A set of elements that may be affected by the command execution. This parameter is not utilized in this
        /// method.</param>
        /// <returns>A Result value indicating whether the command executed successfully or failed.</returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Step 1: get selected rooms (reuse RoomHelper.GetSelectedRooms)
            var results = RoomHelper.GetSelectedRooms(uidoc);
            List<Room> rooms = results.roomsList;
            string errorMessage = results.message;

            if (rooms.Count == 0)
            {
                TaskDialog.Show("BIM Checker", errorMessage); 
                return Result.Succeeded;
            }

            // Step 2: build a RoomCheckerRunner with both checkers.
            Dictionary<string, double> minimumAreaPerRoomType = new()
            {
                ["Kitchen"] = 8.0,
                ["Bedroom"] = 10.0,
                ["Bathroom"] = 4.0,
                ["Livingroom"] = 15.0
            };

            MinAreaChecker minChecker = new MinAreaChecker(minimumAreaPerRoomType);
            RequiredParamsChecker paramChecker = new RequiredParamsChecker();
            var checkers = new List<IRoomChecker> { minChecker, paramChecker }; 


            try
            {
                // Step 3: runner.RunAll(rooms) → collect issues
                List<RoomIssue> issues = new RoomCheckerRunner(checkers: checkers).RunAll(rooms:rooms); 

                // Step 4: if no issues → show "All checks passed."
                if (issues.Count == 0)
                {
                    TaskDialog.Show(title: "BIM Checker", mainInstruction: "All checks passed.");
                    return Result.Succeeded;
                }

                // Step 5: Update the comment based on the existing issues. 
                foreach (Room room in rooms)
                {
                    IEnumerable<RoomIssue> roomIssues =  issues.Where(issue => issue.RoomName == room.Name);
                    if (roomIssues.Any())
                    {
                        string roomIssuesComment = string.Join("\n", roomIssues.Select(i => i.Description));
                        ParameterHelper.TryWriteComment(room: room, doc: doc, commentMessage: roomIssuesComment, forceWrite: true);
                    }

                }

                // Otherwise build a report string from issues and show in TaskDialog
                var allissues = issues.Select(i => i.ToString()).ToList();
                
                TaskDialog dialog = new TaskDialog(title: "BIM Checker");
                dialog.MainInstruction = "There are issues"; 
                dialog.MainContent = string.Join("\n", allissues);
                dialog.Show();
                return Result.Succeeded; 

            }
            catch (Exception e)
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string logFilePath = Path.Combine(desktopPath, "log.txt");
                File.AppendAllText(logFilePath, $"{e.Message}\n");
                return Result.Failed;
            }

        }

    }
}
