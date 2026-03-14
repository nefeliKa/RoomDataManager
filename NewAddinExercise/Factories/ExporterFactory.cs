using RoomDataManager.Exporters;

namespace RoomDataManager.Factories
{
    /// <summary>
    /// Provides factory methods for creating exporter instances based on a specified format.
    /// </summary>
    /// <remarks>Use this class to obtain an exporter that supports a given format, such as CSV. The factory
    /// maintains a registry of supported formats and returns an appropriate exporter instance. This class cannot be
    /// inherited.</remarks>
    public sealed class ExporterFactory
    {
        private static readonly Dictionary<string, Func<string, IExporter>> _registry
            = new Dictionary<string, Func<string, IExporter>>(StringComparer.OrdinalIgnoreCase)
            {
                { "csv", folderPath => new CsvExporter(folderPath) }
            };

        /// <summary>
        /// Creates an exporter instance for the specified format and output folder.
        /// </summary>
        /// <remarks>Use this method to obtain an exporter for a particular format. The returned exporter
        /// will write output to the specified folder. Supported formats depend on the registry configuration.</remarks>
        /// <param name="format">The export format to use. Must be a supported format registered in the exporter registry.</param>
        /// <param name="folderPath">The path to the folder where exported files will be written. Must be a valid directory path.</param>
        /// <returns>An exporter instance that supports the specified format and writes to the given folder.</returns>
        /// <exception cref="ArgumentException">Thrown if the specified format is not supported or not registered.</exception>
        public static IExporter Create(string format, string folderPath)
        {
            // TryGetValue on _registry
            if (_registry.TryGetValue(format, out var factory)) // out var factory is where the found function gets stored
            {
                return factory(folderPath);
            }
            throw new ArgumentException($"Unknown export format: {format}");
        }
    }
}
