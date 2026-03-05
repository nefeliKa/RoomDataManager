using Autodesk.Revit.DB.Architecture;


namespace RoomDataManager.Helpers
{   
    public static class RoomHelper
    {
        /// <summary>
        /// Generates a report of all rooms including each room's name, number and area
        /// </summary>
        /// <param name="rooms"> a list of Revit Architectural Room Elements </param>
        /// <returns>A string report</returns>
        public static string GenerateReport(List<Room> rooms)
        {
            if (rooms.Count == 0)
            {
                return string.Empty;
            }

            try 
            {
                List<string> roomsData = new List<string>();
                foreach (Room room in rooms)
                {
                    string roomName = $"Name: {room.Name}";
                    string roomNumber = $"Number: {room.Number}";
                    string roomArea = $"Area: {Math.Round((room.Area * 0.0929), 2)} m²";
                    string roomData = String.Join("|", new[] { roomName, roomNumber, roomArea });

                    roomsData.Add(roomData);
                }
                string report = String.Join("\n", roomsData);
                return report;
            }
            catch (Exception e)
            {
                File.AppendAllText(path: "log.txt", contents: $"{e}");
                return string.Empty;
            }

            }

    }
}
