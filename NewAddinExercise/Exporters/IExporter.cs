using RoomDataManager.Helpers;

namespace RoomDataManager.Exporters
{   
    /// <summary>
    /// Exporter Interface
    /// </summary>
    public interface IExporter
    {
        /// <summary>
        /// Export method to be implemented in all classes that Inherit the Exporter Interface
        /// </summary>
        /// <param name="roomReports"> A list of RoomReport instances</param>
        void Export(List<RoomReport> roomReports);
    }
}
