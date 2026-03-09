using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;


namespace RoomDataManager.Helpers
{   
    internal static class RoomHelper
    {
        /// <summary>
        /// Generates a report of all rooms including each room's name, number and area
        /// </summary>
        /// <param name="rooms"> a list of Revit Architectural Room Elements </param>
        /// <returns>A string report</returns>
        internal static string GenerateReport(List<Room> rooms)
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
        /// <summary>
        /// This method checks if any elements have been selected and if so, it returns a list of all the room elements.
        /// </summary>
        /// <param name="uidoc"> A Revit UIDocument </param>
        /// <returns>A tuple of a List of Room elements and a string message</returns>
        internal static (List<Room> roomsList, string message) GetSelectedRooms(UIDocument uidoc)
        {
            Document doc = uidoc.Document; 
            // User picks the rooms himself
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();
            if (selectedIds.Count == 0)
            {
                return (new List<Room>(), "Please select some rooms before running this command");
            }

            List<Room> rooms = selectedIds
                                .Select(id => doc.GetElement(id))
                                .OfType<Room>()
                                .ToList();
            
            if (rooms.Count == 0)
            {
                return (new List<Room>(), "None of the selected elements are rooms");
            }

            return (rooms,"");

            }
         
    }
}
