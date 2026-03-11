using RoomDataManager.Helpers;
using System.Collections.Generic; 
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;


namespace RoomDataManager.Exporters
{
    /// <summary>
    /// An Exporter Class that exports to CSV 
    /// </summary>
    public class CsvExporter : BaseExporter 
    {
        public CsvExporter(string folderPath) : base(folderPath) // constructor passes the folder path to be stored at the abstract methods folderpath field
        {
        }
        /// <summary>
        /// Exports a CSV file with information retreived from a RoomReport file
        /// </summary>
        /// <param name="roomReports"> A list of RoomReport instances</param>
        public override void Export(List<RoomReport> roomReports)
        {
            string csvFileName = GetTimestampedFileName("RoomExport", "csv"); //Create file name
            string filePath = Path.Combine(folderPath, csvFileName); // Create file path
            string separatorCharacter = ",";
            var properties = typeof(RoomReport).GetProperties(); // get all public properties of RoomReport as Property Info objects
            string[] headers = properties.Select(p => p.Name).ToArray(); //Get the name of each property and place into array  
            headers = new[] { "Index" }.Concat(headers).ToArray(); // add index to the headers
            string csvHeaderText = string.Join(separatorCharacter, headers); 

            try
            {   // <using> allows writer to close automatically when block ends
                using (StreamWriter fwriter = new StreamWriter(filePath, true))// true opens the file in append mode (adds to existing file rather than overwriting) 
                {
                    fwriter.WriteLine(csvHeaderText); // write header row
                    int idx = 1; 

                    foreach (RoomReport rep in roomReports)
                    {
                        string[] propertyValues = properties.Select(p => FormatValue(p.GetValue(rep))).ToArray(); // Get property values and format them 
                        propertyValues = new[] { idx.ToString() }.Concat(propertyValues).ToArray(); // place index
                        fwriter.WriteLine(string.Join(separatorCharacter, propertyValues)); 
                        idx ++;
                    }
                }
            }
            catch (Exception e)
            {
                
                File.WriteAllText("log.txt", e.ToString());
            }
        }

        /// <summary>
        /// Method that ensures decimal separators are regarded as separators from the system
        /// </summary>
        /// <param name="value"> Any object</param>
        /// <returns> A formated string or a null string</returns>
        private string FormatValue(object? value)
        {
            if (value is IFormattable f) // If IFormattable store it as f 
                // InvarianCulture forces a period as the decimal separator regarldess of the system's locale settings.
                return f.ToString(null, CultureInfo.InvariantCulture);

            //  - ?.ToString() — converts to string, but if value is null, returns null instead of crashing
            // - ?? string.Empty — if the result is null, use "" instead
            return value?.ToString() ?? string.Empty; 
        }
    }
}
