using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB.Architecture;

namespace RoomDataManager.Checkers
{
    /// <summary>
    /// Provides functionality to check whether a room meets the minimum area requirements for its type.
    /// </summary>
    /// <remarks>This checker uses a dictionary of minimum area thresholds for various room types. Room areas
    /// are converted from square feet to square meters before comparison. If a room's area is below the required
    /// minimum for its type, a corresponding issue is reported. This class is typically used to validate room sizes in
    /// building or architectural applications.</remarks>
    public class MinAreaChecker : IRoomChecker
    {
        private readonly Dictionary<string, double> _minAreas;

        public MinAreaChecker(Dictionary<string, double> minAreas)
        {
            _minAreas = minAreas;
        }
    
        /// <summary>
        /// Checks the specified room for compliance with minimum area requirements and identifies any size-related
        /// issues.
        /// </summary>
        /// <remarks>The method converts the room's area from square feet to square meters and compares it
        /// against predefined minimum area thresholds. If the room's area is below the required minimum, a RoomIssue
        /// with severity Error is added to the result list.</remarks>
        /// <param name="room">The room to be evaluated for area compliance. Must not be null.</param>
        /// <returns>A list of RoomIssue objects describing any area-related issues found with the specified room. The list is
        /// empty if no issues are detected.</returns>
        public List<RoomIssue> Check(Room room)
        {
            List<RoomIssue> issueList = new List<RoomIssue>();
            
            // area conversion: room.Area is in ft², multiply by 0.0929 to get m²
            double areaInMeters = Math.Round(room.Area * 0.0929,2);

            // Find and retrieve the first item of the dictionary whose name is contained in the roomName
            var match = _minAreas.FirstOrDefault(entry => room.Name.Contains(entry.Key, StringComparison.OrdinalIgnoreCase));
            if (match.Key != null && areaInMeters < match.Value)
            
                // if below threshold, add a new RoomIssue with IssueSeverity.Error
                issueList.Add(new RoomIssue(roomName: room.Name, description: "Room is too small", severity: IssueSeverity.ERROR));
            

            return issueList;
        }
    
    }

}
