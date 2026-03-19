using System;
using Autodesk.Revit.DB.Architecture;

namespace RoomDataManager.Checkers
{
    /// <summary>
    /// Defines a contract for checking the state of a room and identifying any issues present.
    /// </summary>
    /// <remarks>Implementations of this interface should provide specific logic for checking room conditions
    /// and returning a list of identified issues. This interface is intended for use in scenarios where room validation
    /// is necessary, such as in hotel management or facility maintenance systems.</remarks>
    public interface IRoomChecker
    {
        /// <summary>
        /// Checks the specified room for issues and returns a list of any problems found.
        /// </summary>
        /// <param name="room">The room to validate.</param>
        /// <returns>A list of <see cref="RoomIssue"/> objects. Empty if no issues are found.</returns>
        List<RoomIssue> Check(Room room);
    }
}
