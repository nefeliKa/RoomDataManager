using System;
using RoomDataManager.Helpers;

namespace RoomDataManager.Exporters
{
    /// <summary>
    /// A public abstract class that inherits from the Exporter Interface. 
    /// Receives a string field FolderPath 
    /// </summary>
    public abstract class BaseExporter : IExporter
    {
        /// <summary>The absolute path to the folder where exported files will be written.</summary>
        protected readonly string folderPath;

        /// <summary>
        /// Initialises the exporter with the target output folder path. 
        /// </summary>
        /// <param name="folderPath"> A path to the output folder as a string </param>
        public BaseExporter(string folderPath)
        {
            this.folderPath = folderPath;
        }

        /// <summary>
        /// Generates the distinct name of a file based on the datetime stamp 
        /// </summary>
        /// <param name="prefix">A string name of the file (e.g. "ReportFile") </param>
        /// <param name="extension">An type extension of the file (e.g. "csv", "txt") </param>
        /// <returns></returns>
        protected string GetTimestampedFileName(string prefix, string extension)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string time = DateTime.Now.ToString("HHmmss");
            string dateTime = prefix + "_" + date + "_" + time + "." + extension;
            return dateTime;
        }

        /// <summary>
        /// Abstract export method to implemented by subclasses
        /// </summary>
        /// <param name="roomReports"> A list of RoomReport instances</param>
        public abstract void Export(List<RoomReport> roomReports); 
    }
}
