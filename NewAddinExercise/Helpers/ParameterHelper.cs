using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;


namespace RoomDataManager.Helpers
{
    internal static class ParameterHelper
    {
        internal static RoomReport TryWriteComment(Room room, Document doc)
        {
            RoomReport report = new RoomReport(room);

            if (report.commentWasEmpty)
            {
                using (Transaction t = new Transaction(doc, "Update Comments"))
                {
                    string commentMessage = "Needs Review";
                    t.Start();
                    room.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
                        .Set(commentMessage);
                    t.Commit();
                    report.WasUpdated = true;
                    report.Comment = commentMessage;

                }

            }
            return report; 
        }

        internal static RoomReport TryWriteComment(Room room, Document doc, string commentMessage)
        {
            RoomReport report = new RoomReport(room);

            if (report.commentWasEmpty)
            {
                using (Transaction t = new Transaction(doc, "Update Comments"))
                {
                    t.Start();
                    room.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
                        .Set(commentMessage);
                    t.Commit();
                    report.WasUpdated = true;
                    report.Comment = commentMessage;

                }

            }
            return report;
        }
    }
}
