using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using RoomDataManager.Helpers;

namespace RoomDataManager.Commands
{
    /// <summary>
    /// Displays a data report for all rooms found in the currently active floor plan view.
    /// </summary>
    [Transaction(TransactionMode.ReadOnly)]
    public class RoomsByFloorCommand : IExternalCommand
    {
        /// <summary>
        /// 
        /// Executes the command to generate a report of rooms in the active floor plan view.
        /// </summary>
        /// <remarks>The method checks for an active UI document and an active view, ensuring the view is
        /// a plan with an associated level before generating the report. If any of these conditions are not met, an
        /// appropriate error message is set.</remarks>
        /// <param name="commandData">The data associated with the external command, providing access to the application and active UI document.</param>
        /// <param name="message">A reference to a string that will contain error messages if the command fails.</param>
        /// <param name="elements">A collection of elements that the command can operate on, though it is not used in this method.</param>
        /// <returns>A Result indicating the success or failure of the command execution.</returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {   
            // Try to get the UIDocument , return null otherwise
            var uidoc = commandData?.Application?.ActiveUIDocument; // ? evaluates and returns null otherwise
            if (uidoc == null) { message = "No active UI document."; return Result.Failed; }
            
            // Try to get the current open document 
            var doc = uidoc.Document;
            var activeView = uidoc.ActiveView; 
            if (activeView == null) { message = "No active view."; return Result.Failed;  }
            
            // Return null if Active View is not a ViewPlan 
            var planView = activeView as ViewPlan; 

            // Ensure view is a plan (has a level)
            if (planView?.GenLevel == null)
            {
                message = $"Active view '{activeView.Name}' has no associated level.Switch to floor plan view";
                return Result.Failed; 
            }

            // Get all rooms of current view
            List<Room> rooms = new FilteredElementCollector(document: doc, viewId: planView.Id)
                            .OfClass(typeof(SpatialElement))
                            .WhereElementIsNotElementType()
                            .OfType<Room>()
                            .ToList();

            if (rooms.Count == 0)
            {
                TaskDialog.Show(title:"Rooms By FLoor Inspector", mainInstruction: "No rooms found in the current selection");
                return Result.Failed;
            }

            // Create report for rooms
            string report = RoomHelper.GenerateReport(rooms);

            // Show the report in a Revit pop up window
            TaskDialog.Show(title: "Rooms By Floor Inspector", mainInstruction: report); 

            return Result.Succeeded; 
        }



    }
}
