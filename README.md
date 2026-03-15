# Room Data Manager

A Revit 2026 add-in for inspecting, editing, and exporting room data.

---

## Requirements

- Autodesk Revit 2026
- Windows 10 or later
- .NET 8

---

## Installation

1. Download the latest release and unzip it.
2. Copy the following files into your Revit add-ins folder:

```
C:\Users\<YourName>\AppData\Roaming\Autodesk\Revit\Addins\2026\
```

Files to copy:
- `NewAddinExercise.dll`
- `NewAddinExercise.addin`

3. Launch Revit. A new tab called **Room Data Manager** will appear in the ribbon.

> To find your AppData folder, press `Win + R`, type `%appdata%`, and press Enter.

---

## Features

### Room Inspector
Select one or more rooms in the model and click this button to see a report of their name, number, area, and comments.

### Rooms by Floor
Displays a list of all rooms on the currently active floor plan view, with their key data.

### Parameter Writer
Selects rooms and writes a default comment ("Pending Review") to any room that has an empty comment field. Rooms that already have a comment are skipped.

### CSV Exporter
Exports room data (name, number, area, comments) to a `.csv` file saved on your Desktop. The file is timestamped so each export is kept separately.

### BIM Checker
Checks all selected rooms for common data issues:
- Missing name, number, or comment
- Area below the expected minimum for the room type

Any issues found are written directly into the room's comment field in Revit, so they are visible in the model.

---

## Troubleshooting

**The "Room Data Manager" tab does not appear in Revit**
- Check that both `NewAddinExercise.dll` and `NewAddinExercise.addin` are in the correct folder.
- Make sure you are using Revit 2026. The add-in will not load in other versions.

**A button does nothing when clicked**
- Make sure you have a floor plan view open and active before using the add-in.
- Some buttons require you to select rooms first. Select the rooms in the model, then click the button.

**The CSV file does not appear on my Desktop**
- Check that Revit has permission to write files to your Desktop. This can be blocked by company IT policies.
- Look for a file named `RoomExport_<timestamp>.csv` on your Desktop.

**An error appears when loading the add-in**
- A log file called `log.txt` is saved to your Desktop when something goes wrong at startup. Send this file to your administrator for help.

---

## Icon Credits

Icons used in this add-in are sourced from [Flaticon](https://www.flaticon.com):

- **BIM Checker** — [Bim icons created by Freepik - Flaticon](https://www.flaticon.com/free-icons/bim)
- **Rooms by Floor** — [Analysis icons created by RaftelDesign - Flaticon](https://www.flaticon.com/free-icons/analysis)
- **Room Inspector** — [Big data icons created by xnimrodx - Flaticon](https://www.flaticon.com/free-icons/big-data)
- **CSV Exporter** — [Storage icons created by Freepik - Flaticon](https://www.flaticon.com/free-icons/storage)
- **Parameter Writer** — [Computer icons created by vectorsmarket15 - Flaticon](https://www.flaticon.com/free-icons/computer)
