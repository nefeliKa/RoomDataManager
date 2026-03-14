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
        List<RoomIssue> Check(Room room);
    }
}
