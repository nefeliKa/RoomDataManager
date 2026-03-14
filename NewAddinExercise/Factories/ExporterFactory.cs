using RoomDataManager.Exporters;

namespace RoomDataManager.Factories
{
    public sealed class ExporterFactory
    {
        private static readonly Dictionary<string, Func<string, IExporter>> _registry
            = new Dictionary<string, Func<string, IExporter>>(StringComparer.OrdinalIgnoreCase)
            {
                { "csv", folderPath => new CsvExporter(folderPath) }
            };

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
