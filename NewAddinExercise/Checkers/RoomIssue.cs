using System;
using System.Threading.Tasks;


namespace RoomDataManager.Checkers
{
    /// <summary>
    /// Represents the severity level of a room issue.
    /// </summary>
    public enum IssueSeverity
    {
        /// <summary>A non-critical issue that should be reviewed but does not block the model.</summary>
        WARNING,
        /// <summary>A critical issue that indicates the room does not meet minimum requirements.</summary>
        ERROR
    }

    /// <summary>
    /// Represents an issue associated with a specific room, including its name, description, and severity level.
    /// </summary>
    /// <remarks>Use this class to encapsulate information about problems or concerns identified in rooms. The
    /// severity level can be used to prioritize issues for resolution. Instances of this class are immutable and
    /// provide a readable summary via the overridden ToString method.</remarks>
    public sealed class RoomIssue
    {
        /// <summary>The name of the room the issue was found in.</summary>
        public string RoomName { get; }
        /// <summary>A human-readable description of the issue.</summary>
        public string Description { get; }
        /// <summary>The severity of the issue.</summary>
        public IssueSeverity Severity { get; }

        /// <summary>
        /// Initializes a new issue for the specified room.
        /// </summary>
        /// <param name="roomName">The name of the room.</param>
        /// <param name="description">A description of the problem found.</param>
        /// <param name="severity">The severity level of the issue.</param>
        public RoomIssue(string roomName, string description, IssueSeverity severity)
        {
            RoomName = roomName;
            Description = description;
            Severity = severity;
        }

        /// <summary>
        /// Returns a formatted string summarising the issue, e.g. <c>[ERROR] Kitchen : area too small</c>.
        /// </summary>
        public override string ToString()
        {
            // return a readable summary, e.g. "[Error] Kitchen: area too small"
            string summary = $"[{Severity}] {RoomName} : {Description}";
            return summary;
        }
    }
}
