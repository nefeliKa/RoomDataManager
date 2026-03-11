using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;


namespace RoomDataManager.Helpers
{
    public class RoomReport
    {
        private readonly Room _room;

        public string Name { get; }
        public string Number { get; }
        public double Area { get; }
        public string Comment { get; internal set; }
        public bool commentWasEmpty { get; }
        public bool WasUpdated { get; internal set; }

        public RoomReport(Room room)
        {
            Name = room.Name;
            Number = room.Number;
            Area = Math.Round(room.Area*0.0929,2);
            _room = room;
            Comment = room.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)?.AsString() ?? string.Empty;
            commentWasEmpty = ReadCommentIsEmpty();


        }

        private bool ReadCommentIsEmpty()
        {
            Parameter commentParam = _room.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
            string value = commentParam.AsString();
            return string.IsNullOrEmpty(value);

        }

        public string MakeReport()
        {
            string roomName = $"Name: {Name}";
            string roomNumber = $"Number: {Number}";
            string roomArea = $"Area: {Area} m²";
            string report = String.Join(" | ", new[] { roomName, roomNumber, roomArea });
            return report; 
        }

        public static string MakeReport(List<RoomReport> reportList)
        {
            string reportAllRooms = string.Empty;
            foreach (RoomReport rep in reportList)
            {
                string roomName = $"Name: {rep.Name}";
                string roomNumber = $"Number: {rep.Number}";
                string roomArea = $"Area: {rep.Area} m²";
                string roomStatus = $"Status: {rep.ToString()}";
                string report = String.Join(" | ", new[] { roomName, roomNumber, roomArea, roomStatus });
                reportAllRooms += $"\n{report}";
            }

            return reportAllRooms;
        }

        public override string ToString()
        {
            string summary = String.Empty;
            if (commentWasEmpty) 
            {
                summary = $"\n{Name}'s comment section was empty.";


            }
            else
            {
                summary = $"\n{Name}'s comment section was already filled.";
            }

            if (WasUpdated)
            {
                summary = summary + $"\n{Name}'s comment section was updated with 'Needs Review'."; 
            }
            else
            {
                summary = summary + $"\n{Name}'s comment section was not updated."; 
            }

            return summary;
        }

    }



}
