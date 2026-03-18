using System;
using System.Collections.Generic;
using Autodesk.Revit.DB.Architecture;


namespace RoomDataManager.Checkers
{
    /// <summary>
    /// Aggregates room issues by running a series of room checks using the provided list of checkers.
    /// </summary>
    /// <remarks>This class coordinates multiple implementations of the IRoomChecker interface to evaluate a
    /// collection of rooms. It collects and returns all issues identified by each checker for each room. If no rooms
    /// are provided or the list of checkers is null, the runner returns an empty list of issues. This class is intended
    /// for internal use and is not thread-safe.</remarks>
    public class RoomCheckerRunner
    {
        private readonly List<IRoomChecker> _checkers;

        /// <summary>
        /// Initializes the runner with the list of checkers to apply to each room.
        /// </summary>
        /// <param name="checkers">The checkers to run. Each will be applied to every room in <see cref="RunAll"/>.</param>
        public RoomCheckerRunner(List<IRoomChecker> checkers)
        {
            _checkers = checkers;
        }

        /// <summary>
        /// Evaluates the specified rooms using all configured checkers and returns any issues found.
        /// </summary>
        /// <remarks>Each room is assessed by all checkers in the current configuration. The method does
        /// not perform any checks if the checkers collection is uninitialized.</remarks>
        /// <param name="rooms">A list of rooms to be checked for issues. The list must not be empty; otherwise, no checks are performed.</param>
        /// <returns>A list of issues detected in the provided rooms. The list is empty if no issues are found or if the input
        /// list is empty.</returns>
        public List<RoomIssue> RunAll(List<Room> rooms)
        {
            List<RoomIssue> issues = new();

            if (rooms.Count == 0)
                return issues; 

            foreach (Room room in rooms)
            {
                foreach (IRoomChecker checker in _checkers)
                {
                    List<RoomIssue> roomIssues = checker.Check(room);
                    issues.AddRange(roomIssues);
                }
            }


            return issues;
        }



    }

}
