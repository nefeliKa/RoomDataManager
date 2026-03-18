using System.Collections.Generic;
using System.IO;


namespace RoomDataManager.Helpers
{
    /// <summary>
    /// Provides helper methods for loading BIM configuration data, such as minimum area requirements, from a
    /// configuration file.
    /// </summary>
    /// <remarks>This class is intended for internal use within the application and is not intended to be used
    /// directly by external consumers.</remarks>
    internal static class BimConfigHelper
    {
        /// <summary>
        /// Loads minimum area requirements from a CSV file and returns the results along with any warnings encountered
        /// during loading.
        /// </summary>
        /// <remarks>The method attempts to read a CSV file from the user's application data directory. If
        /// the file does not exist, default area requirements are returned. Warnings are provided for missing files,
        /// empty files, or lines that cannot be parsed. The method does not throw exceptions for missing or malformed
        /// files, but instead reports issues via the warnings list.</remarks>
        /// <returns>A tuple containing a dictionary of area requirements keyed by room type, and a list of warning messages
        /// describing any issues encountered while loading the data. If the CSV file is missing or empty, the
        /// dictionary may contain default values or be empty, and warnings will describe the condition.</returns>
        internal static (Dictionary<string, double>areas, List<string> warnings) Load()
        {
            List<string> warningMessages= new() {};
            string root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string csvPath = Path.Combine(root, "Autodesk", "Revit", "Addins", "2026", "bim_requirements.csv");
            if (!Path.Exists(csvPath))
            {
                warningMessages.Add("Path didn't exist. Returned default checkers.");
                return (new()
                {
                    // Minimum area requirements based on Neufert
                    ["Kitchen"] = 5.0,
                    ["Bedroom"] = 8.0,
                    ["Bathroom"] = 4.0,
                    ["Livingroom"] = 14.0
                }, warningMessages);
            }

            string[] lines = File.ReadAllLines(path: csvPath);
            Dictionary<string, double> result = new(); 
            if (lines.Length == 0)
            {
                warningMessages.Add("CSV file is empty.");
                return (result, warningMessages);
            }

            foreach (string line in lines)
            {
                if (!line.Contains(','))
                {
                    warningMessages.Add("File contains lines with no comma separators");
                    continue;
                }
                string[] parts = line.Split(',');
                string key = parts[0].Trim();

                if (!double.TryParse(parts[1].Trim(), out double value)) 
                { 
                    warningMessages.Add($"Could not parse value on line: {line}"); 
                    continue; 
                }

                result[key] = value;
            }
            return (result, warningMessages); 
        }

    }
}
