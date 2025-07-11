UVSim Milestone 4
=================

##  Overview

UVSim is a virtual machine simulator designed for educational use. It executes BasicML (Basic Machine Language) programs and visually simulates memory, instructions, and execution cycles via a user-friendly GUI built with .NET MAUI.

This version includes:
- A graphical editor for BasicML programs
- A real-time console
- File loading, editing, and saving capabilities
- Customizable color themes
- Visual file explorer for .txt programs
- Simulated input/output (I/O)

---

##  How to Run the Program

###  Requirements
- .NET 8.0 SDK  
- Visual Studio 2022+ with MAUI workload installed

###  Supported Platforms
-  macOS
-  Windows
-  iOS & Android (via simulator/emulator)

###  Steps to Launch
1. Clone or download the repository.
2. Open the solution in **Visual Studio**.
3. Select the correct platform (MacCatalyst, Windows, Android, etc.).
4. Build and run the project.

---

##  GUI Workflow

###  File Explorer
- Located on the right side.
- Lists `.txt` files in a selected folder.
- Click a file to load it into the **Instructions Editor**.
- Use the `Load Folder` option to populate the list.

###  Instructions Editor
- Enter or edit BasicML instructions (max 100 lines).
- Line numbers displayed for easy reference.
- Changes trigger recompilation.

###  Buttons and Features

| Button         | Purpose                                                                 |
|----------------|-------------------------------------------------------------------------|
| `Load`         | Opens a file picker to load a `.txt` file into the editor.              |
| `Write`        | Saves all loaded file tabs to disk (including edits).                   |
| `Compile`      | Parses and loads the instructions into virtual memory.                  |
| `Run`          | Executes instructions line-by-line and shows output in the console.     |
| `Apply Theme`  | Updates the GUI color scheme using two HEX fields.                      |

---

##  User Input

Some instructions (like `READ`) will prompt for input in the console:

- A text entry field will appear under `Console`.
- **Type an integer and press Enter** to submit.
- Invalid input will display an error below.

---

##  Themes

Users can customize the color scheme:
- `Primary Color`: Used for headers, console background, etc.
- `Off Color`: Used for buttons and text areas.

Input HEX values and click `Apply Theme`.

You can also toggle an **inverse mode** that switches foreground and background colors.

---

##  Files Overview

###  UVSimClassLib

- `CPU.cs`: Executes individual instructions.
- `Register.cs`: Basic register model with string parsing.
- `TraversableRegister.cs`: Linked-memory style structure for navigation.
- `UVSim.cs`: Controls the instruction cycle and handles compilation.

###  UVSimGUI

- `MainPage.xaml`: Defines the GUI layout and styling.
- `MainPage.xaml.cs`: Implements event logic and user interactions.
- `ThemeColors.cs`: Manages persistent theming and config loading/saving.
- `AppShell.xaml`: Shell configuration (routes & navigation).

---

##  Team Members
 
- Daniel Urling  
- Michael Findlay  
- Traedon Harris
