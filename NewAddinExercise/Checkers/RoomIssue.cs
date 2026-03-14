using System;
using System.Threading.Tasks;


namespace RoomDataManager.Checkers
{
    /// <summary>
    /// Enum representing the Severity of an Issue
    /// </summary>
    public enum IssueSeverity 
    {
        WARNING, 
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
        public string RoomName { get; }
        public string Description { get; }
        public IssueSeverity Severity { get; }

        public RoomIssue(string roomName, string description, IssueSeverity severity)
        {
            RoomName = roomName;
            Description = description;
            Severity = severity; 
        }

        public override string ToString()
        {
            // return a readable summary, e.g. "[Error] Kitchen: area too small"
            string summary = $"[{Severity}] {RoomName} : {Description}";
            return summary;
        }
    }
}
