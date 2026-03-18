using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;


namespace RoomDataManager.Helpers
{
    public class RoomReport
    {
        public string Name { get; }
        public string Number { get; }
        public double Area { get; }
        public string Comment { get; internal set; }
        public bool CommentWasEmpty { get; }
        public bool WasUpdated { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the RoomReport class using the specified room data.
        /// </summary>
        /// <remarks>The area is converted from square feet to square meters and rounded to two decimal
        /// places. The comment is retrieved from the room's parameters and defaults to an empty string if not
        /// present.</remarks>
        /// <param name="room">The room object containing the name, number, area, and comment information to be used for the report. Cannot
        /// be null.</param>
        public RoomReport(Room room)
        {
            Name = room.Name;
            Number = room.Number;
            Area = Math.Round(UnitUtils.ConvertFromInternalUnits(room.Area, UnitTypeId.SquareMeters), 2);
            Comment = room.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)?.AsString() ?? string.Empty;
            CommentWasEmpty = ReadCommentIsEmpty();


        }

        /// <summary>
        /// Determines whether the comment is null or empty.
        /// </summary>
        /// <returns>true if the comment is null or empty; otherwise, false.</returns>
        private bool ReadCommentIsEmpty()
        {
            return string.IsNullOrEmpty(Comment);
        }

        /// <summary>
        /// Generates a formatted report containing the room's name, number, and area.
        /// </summary>
        /// <remarks>The report includes the room's name, number, and area in square meters, separated by
        /// vertical bars. This method does not validate the underlying property values; ensure that the properties are
        /// set appropriately before calling.</remarks>
        /// <returns>A string representing the room's details, formatted as "Name: {Name} | Number: {Number} | Area: {Area} m²".</returns>
        public string MakeReport()
        {
            string roomName = $"Name: {Name}";
            string roomNumber = $"Number: {Number}";
            string roomArea = $"Area: {Area} m²";
            string report = String.Join(" | ", new[] { roomName, roomNumber, roomArea });
            return report; 
        }

        /// <summary>
        /// Generates a formatted report string containing details for each room in the specified list.
        /// </summary>
        /// <remarks>The report includes the name, number, area, and status of each room. The status is
        /// determined by the RoomReport object's ToString() method. The method does not validate the contents of the
        /// RoomReport objects.</remarks>
        /// <param name="reportList">A list of RoomReport objects representing the rooms to include in the report. Cannot be null.</param>
        /// <returns>A string containing the formatted report for all rooms. Each room's details are separated by a newline.
        /// Returns an empty string if the list is empty.</returns>
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

        /// <summary>
        /// Returns a formatted string describing the state of the comment section for the current instance.
        /// </summary>
        /// <remarks>The returned string includes the name associated with the comment section and details
        /// about its update status. This method is useful for logging or displaying the comment section's state in a
        /// human-readable format.</remarks>
        /// <returns>A string containing information about whether the comment section was empty, whether it was already filled,
        /// and whether it was updated with 'Needs Review'.</returns>
        public override string ToString()
        {
            string summary = String.Empty;
            if (CommentWasEmpty) 
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
