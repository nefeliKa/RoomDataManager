using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
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
                using (Transaction t = new Transaction(doc, "name"))
                {
                    t.Start();
                    room.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)
                        .Set("Needs Review");
                    t.Commit();
                    report.WasUpdated = true;

                }

            }
            return report; 
        }

        internal static RoomReport TryWriteComment(Room room, Document doc, string value)
        {
            return new RoomReport(room);
        }
    }
}
