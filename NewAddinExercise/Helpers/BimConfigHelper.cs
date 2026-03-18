using System.Collections.Generic;
using System.IO;


namespace RoomDataManager.Helpers
{
    internal static class BimConfigHelper
    {
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
