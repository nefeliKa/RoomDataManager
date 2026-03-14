using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace RoomDataManager.Checkers
{   
    /// <summary>
    /// Provides functionality to check a room for required parameters and identify any missing values.
    /// </summary>
    /// <remarks>This class implements the IRoomChecker interface to validate that essential parameters of a
    /// room are present. It returns a list of issues indicating which parameters are missing, enabling corrective
    /// actions. Use this class when you need to ensure that rooms meet minimum data requirements before further
    /// processing.</remarks>
    internal class RequiredParamsChecker : IRoomChecker
    {
        /// <summary>
        /// Checks the specified room for missing essential parameters and returns a list of issues found.
        /// </summary>
        /// <remarks>This method verifies the presence of required parameters in the room object,
        /// including its name, number, and comments. It adds a warning issue for each missing parameter.</remarks>
        /// <param name="room">The room to be checked for parameter completeness. This parameter must not be null.</param>
        /// <returns>A list of RoomIssue objects detailing any missing parameters in the specified room. The list will be empty
        /// if no issues are found.</returns>
        public List<RoomIssue> Check(Room room )
        {
            List<RoomIssue> issues = new();

            Dictionary<string, string> roomData = new()
            {
                [$"{nameof(room.Name)}"] = room.Name,
                [$"{nameof(room.Number)}"] = room.Number,
                [$"Comment"] = room.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)?.AsString() ?? string.Empty

            };

            foreach (var roomParameter in roomData)
            {
                if (string.IsNullOrEmpty(roomParameter.Value))
                    issues.Add(new RoomIssue(roomName: room.Name, description: $"Parameter {roomParameter.Key} is missing", severity: IssueSeverity.WARNING));

            }


            return issues;
        }
    }
}
