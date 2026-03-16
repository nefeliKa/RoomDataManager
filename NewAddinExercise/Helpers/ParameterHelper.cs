using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;


namespace RoomDataManager.Helpers
{
    /// <summary>
    /// Provides helper methods for writing or updating the comment parameter of a room in a Revit document.
    /// </summary>
    /// <remarks>This class contains static methods to facilitate setting or appending comments on room
    /// elements, typically for review or reporting purposes. All methods return a RoomReport indicating the outcome of
    /// the operation. This class is intended for internal use and is not thread-safe.</remarks>
    internal static class ParameterHelper
    {
        /// <summary>
        /// Attempts to write a default comment to the specified room if its comment is currently empty, and returns a
        /// report describing the operation.
        /// </summary>
        /// <remarks>If the room's comment is empty, this method sets it to "Needs Review" and marks the
        /// report as updated. If the comment is not empty, no changes are made.</remarks>
        /// <param name="room">The room to which the comment may be written. Must not be null.</param>
        /// <param name="doc">The document context in which the room resides. Must not be null.</param>
        /// <returns>A RoomReport object indicating whether the comment was updated and containing the resulting comment value.</returns>
        internal static RoomReport TryWriteComment(Room room, Document doc)
        {
            RoomReport report = new RoomReport(room);

            if (report.CommentWasEmpty)
            {
                {
                    string commentMessage = "Needs Review";
                    room.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
                        .Set(commentMessage);
                    report.WasUpdated = true;
                    report.Comment = commentMessage;

                }

            }
            return report; 
        }

        /// <summary>
        /// Attempts to write a comment to the specified room if its comment parameter is currently empty.
        /// </summary>
        /// <remarks>If the room's comment parameter is not empty, no changes are made and the report
        /// reflects that the comment was not updated.</remarks>
        /// <param name="room">The room to which the comment will be written. Must not be null.</param>
        /// <param name="doc">The document context in which the room exists. Must not be null.</param>
        /// <param name="commentMessage">The comment message to write to the room. If the room's comment is empty, this value will be set as the new
        /// comment.</param>
        /// <returns>A RoomReport indicating whether the comment was updated and containing the resulting comment value.</returns>
        internal static RoomReport TryWriteComment(Room room, Document doc, string commentMessage)
        {
            RoomReport report = new RoomReport(room);

            if (report.CommentWasEmpty)
            {
                {
                    room.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
                        .Set(commentMessage);
                    report.WasUpdated = true;
                    report.Comment = commentMessage;

                }

            }

            return report;
        }

        /// <summary>
        /// Attempts to append a comment to the specified room and returns a report describing the operation.
        /// </summary>
        /// <param name="room">The room to which the comment will be added.</param>
        /// <param name="doc">The document context in which the room resides.</param>
        /// <param name="commentMessage">The comment text to append to the room's existing comments.</param>
        /// <param name="forceWrite">If set to <see langword="true"/>, the comment is written regardless of the current state; otherwise, no
        /// action is taken.</param>
        /// <returns>A <see cref="RoomReport"/> indicating whether the comment was updated and containing the resulting comment
        /// text.</returns>
        internal static RoomReport TryWriteComment(Room room, Document doc, string commentMessage, bool forceWrite)
        {
            RoomReport report = new RoomReport(room);

            if (forceWrite)
            {
                {
                    Parameter roomComment = room.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
                    string roomCommentString = roomComment.AsString() ?? string.Empty; 
                    roomComment.Set(roomCommentString + commentMessage);
                    report.WasUpdated = true;
                    report.Comment = roomCommentString + commentMessage;

                }

            }

            return report;
        }
    }
}
