using Autodesk.Revit.UI;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;



namespace RoomDataManager
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Create a custom tab in the Revit ribbon, then add a panel inside it
            application.CreateRibbonTab(tabName: "Room Data Manager");
            RibbonPanel panel = application.CreateRibbonPanel(tabName:"Room Data Manager", panelName:"Room Tools");

            // Each dictionary holds the data needed to create one ribbon button:
            // "name" = display label, "classname" = command class, "resource" = icon filename, "description" = tooltip
            List<Dictionary<string, string>> buttonInputs = new()
            {
               new(){ ["name"] = "Room Inspector", ["classname"] = "RoomInspectorCommand", ["resource"] = "RoomInspector.png", ["description"] = "Inspects selected rooms and returns a list of data including Name, Number, Area(m2)"},
               new(){ ["name"] = "Inspect By Floor", ["classname"] = "RoomsByFloorCommand", ["resource"] = "RoomInspectorByFloor.png", ["description"] = "Automatically inspects all rooms in the active floor/level and returns a list of data including Name, Number, Area(m2) "},
               new(){ ["name"] = "Room Reporter", ["classname"] = "ParameterWriterCommand", ["resource"] = "RoomReporter.png", ["description"] = "Inspects selected rooms. Updates the comment section if it is empty. Returns a list of data including Name, Number, Area(m2) and status of Comment section."},
               new(){ ["name"] = "CSV Exporter", ["classname"] = "CsvExporterCommand", ["resource"] = "ToCSV.png", ["description"] = "Exports a CSV report on selected rooms which includes Name, Number, Area(m2), Comments, and if the comment was empty."},
               new(){ ["name"] = "BIM Checker", ["classname"] = "BimCheckerCommand", ["resource"] = "bim.png", ["description"] = "Simple BIM standards checker that validates rooms are within required parameters"},
            };

            // Assembly = the compiled .dll of this add-in. We need it to access embedded resources.
            Assembly asm = Assembly.GetExecutingAssembly();


            foreach (Dictionary<string,string> buttonData in buttonInputs)
            {
                try {
                // GetManifestResourceStream finds an embedded file inside the .dll by its full resource name
                // (namespace.folder.filename). Returns null if not found — the ! suppresses the compiler
                // null warning, but the try/catch will handle it at runtime if the stream is null.
                Stream iconStream = asm.GetManifestResourceStream("RoomDataManager.Resources."+ buttonData["resource"])!;

                // BitmapImage must be configured between BeginInit() and EndInit().
                // All properties (source, size) must be set in that window before the image loads.
                BitmapImage icon = new BitmapImage();
                icon.BeginInit();
                icon.StreamSource = iconStream;
                icon.DecodePixelWidth = 32;  // Revit ribbon expects 32x32 for LargeImage
                icon.DecodePixelHeight = 32;
                icon.EndInit();

                // PushButtonData defines the button configuration before it is added to the panel.
                // name = internal ID, text = label shown in ribbon, assemblyName = this .dll, className = full command class path
                PushButtonData button = new PushButtonData(name: $"{buttonData["name"].Trim()}",
                                                            text: $"{buttonData["name"]}",
                                                            assemblyName: Assembly.GetExecutingAssembly().Location,
                                                            className: "RoomDataManager.Commands."+ buttonData["classname"]);

                // AddItem registers the button in the panel and returns a RibbonItem.
                // We cast it to PushButton so we can set icon and tooltip on it.
                PushButton newPushButton = (PushButton)panel.AddItem(button);
                newPushButton.ToolTip = buttonData["description"];
                newPushButton.LargeImage = icon;
                }
                catch (Exception e)
                {
                    // If a button fails to load (e.g. missing icon), log the error and continue.
                    // This prevents one bad button from crashing the entire add-in on startup.
                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string logFilePath = Path.Combine(desktopPath, "log.txt");
                    File.AppendAllText(logFilePath, $"[{buttonData["name"]}] {e.Message}\n");
                }
            }

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
