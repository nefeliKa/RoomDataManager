using Autodesk.Revit.UI;
using System.Reflection; 


namespace RoomDataManager
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            RibbonPanel panel = application.CreateRibbonPanel("Room Data Manager");
            PushButtonData roomInspector_button = new PushButtonData(name:"RoomInspector", text: "Room Inspector", assemblyName: Assembly.GetExecutingAssembly().Location, className: "RoomDataManager.Commands.RoomInspectorCommand");
            PushButtonData autoselect_roomInspector_button = new PushButtonData(name: "InspectByFloor", text: "InspectByFloor", assemblyName: Assembly.GetExecutingAssembly().Location, className: "RoomDataManager.Commands.RoomsByFloorCommand");
            PushButtonData roomReporter_button = new PushButtonData(name: "RoomReporter", text: "Room Reporter", assemblyName: Assembly.GetExecutingAssembly().Location, className: "RoomDataManager.Commands.ParameterWriterCommand");
            panel.AddItem(roomInspector_button);
            panel.AddItem(autoselect_roomInspector_button);
            panel.AddItem(roomReporter_button);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}