using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;


namespace RoomDataManager.Helpers
{
    internal class RoomReport
    {
        private readonly string _roomName; 
        private readonly string _roomNumber; 
        private readonly double _roomArea; 
        private readonly Room _room;

        public bool commentWasEmpty { get; }
        public bool WasUpdated { get; internal set; }

        public RoomReport(Room room)
        {
            _roomName = room.Name;
            _roomNumber = room.Number;
            _roomArea = room.Area;
            _room = room;

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
            string roomName = $"Name: {_roomName}";
            string roomNumber = $"Number: {_roomNumber}";
            string roomArea = $"Area: {Math.Round((_roomArea * 0.0929), 2)} m²";
            string report = String.Join(" | ", new[] { roomName, roomNumber, roomArea });
            return report; 
        }

        public static string MakeReport(List<RoomReport> reportList)
        {
            string reportAllRooms = string.Empty;
            foreach (RoomReport rep in reportList)
            {
                string roomName = $"Name: {rep._roomName}";
                string roomNumber = $"Number: {rep._roomNumber}";
                string roomArea = $"Area: {Math.Round((rep._roomArea * 0.0929), 2)} m²";
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
                summary = $"\n{_roomName}'s comment section was empty.";


            }
            else
            {
                summary = $"\n{_roomName}'s comment section was already filled.";
            }

            if (WasUpdated)
            {
                summary = summary + $"\n{_roomName}'s comment section was updated with 'Needs Review'."; 
            }
            else
            {
                summary = summary + $"\n{_roomName}'s comment section was not updated."; 
            }

            return summary;
        }

    }



}
