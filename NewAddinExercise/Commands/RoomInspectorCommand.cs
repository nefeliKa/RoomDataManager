using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.Attributes;
using RoomDataManager.Helpers;

namespace RoomDataManager.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class RoomInspectorCommand : IExternalCommand
    {
        /// <summary>
        /// Executes the external command to generate and display a report for the rooms currently selected in the
        /// active Revit document.
        /// </summary>
        /// <remarks>The user must select one or more rooms in the active Revit document before running
        /// this command. If no rooms are selected, the command prompts the user to make a selection and does not
        /// proceed.</remarks>
        /// <param name="commandData">An object that provides contextual information about the current Revit application and active document,
        /// including access to the user interface.</param>
        /// <param name="message">A reference to a string that can be set to provide an error message to the user if the command fails.</param>
        /// <param name="elements">A set of elements that can be used to highlight or select elements in the Revit user interface. This
        /// parameter is not used by this command.</param>
        /// <returns>A value indicating whether the command executed successfully. Returns Result.Succeeded if a report is
        /// generated and displayed; otherwise, returns Result.Failed.</returns>
        public Result Execute(ExternalCommandData commandData, ref string message , ElementSet elements)
        {
            // Add the Revit docs
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // User picks the rooms himself
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();
            if (selectedIds.Count == 0)
            {
                TaskDialog.Show(title: "Room Inspector", mainInstruction: "Please select some rooms before runnign this command");
                return Result.Failed;
            }

            List<Room> rooms = selectedIds
                                .Select(id => doc.GetElement(id))
                                .OfType<Room>()
                                .ToList();
            if (rooms.Count == 0)
            {
                TaskDialog.Show(title: "Room Inspector", mainInstruction: "None of the selected elements are rooms");
                return Result.Failed;
            }

            string report = RoomHelper.GenerateReport(rooms: rooms); 
            TaskDialog.Show(title: "Room Inspector", mainInstruction: report);
            return Result.Succeeded;
        }
    }
}
