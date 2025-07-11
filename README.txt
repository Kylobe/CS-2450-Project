UVSim Milestone 4
=================

Overview:
---------
UVSim is a virtual machine simulator built for educational use. It executes programs written in BasicML (Basic Machine Language) and visually simulates memory, instructions, and CPU execution via a clean, modern GUI built using .NET MAUI.

This version includes:
- Graphical editor for BasicML programs
- Real-time console for simulated I/O
- Visual file explorer for .txt programs
- File loading, editing, renaming, and saving
- Customizable theme support with persistent config

IMPORTANT:
----------
 This application is optimized and tested for **Windows only**.
While MAUI supports multiple platforms, the current implementation and features have been tailored specifically for Windows performance, styling, and file handling.

------------------------------------------------------

How to Run the Program:
-----------------------
Requirements:
- .NET 8.0 SDK (https://dotnet.microsoft.com/download)
- Visual Studio 2022 or newer with the .NET MAUI workload installed

Supported Platform:
- Windows 10 or later

Steps to Run:
1. Clone the repository:
   git clone https://github.com/YOUR-REPO/UVSim.git

2. Open the solution (UVSim.sln) in Visual Studio.

3. Select "Windows Machine" as the target platform.

4. Build and run the project.

------------------------------------------------------

GUI Workflow and Controls:
--------------------------

Instructions Editor:
- Enter or paste up to 100 BasicML instructions.
- Editor automatically shows line numbers for clarity.
- Instructions are validated and compiled into memory.

File Explorer:
- Located on the right side of the GUI.
- Lists all `.txt` files in a selected folder.
- Click a file to load its contents into the editor.
- Files can also be renamed (feature available if wired).

Buttons and Their Functions:
----------------------------
Load:         Opens a file picker to load a `.txt` file into the editor.
Write:        Saves all currently loaded and edited files back to disk.
Compile:      Parses and validates instructions, loading them into memory.
Run:          Executes instructions line by line. Output appears in the console.
Apply Theme:  Sets new theme colors based on two HEX input fields.

------------------------------------------------------

User Input:
-----------
When a READ instruction is executed:
- A text field appears in the console.
- Type an integer and press ENTER to submit.
- Invalid input will show an error message below the input field.

------------------------------------------------------

Theme Customization:
--------------------
You can customize the appearance of the simulator by setting two colors:

Primary Color: Affects header bars, console background, and major highlights.
Off Color:     Affects button backgrounds and supporting areas.

To apply a theme:
1. Enter valid HEX codes into the theme fields (e.g., #4C721D, #FFFFFF).
2. Click the "Apply Theme" button.
3. Optionally use the toggle to invert the theme.

Themes are saved to disk and persist between sessions.

------------------------------------------------------

Project Structure:
------------------

UVSimClassLib/
- CPU.cs: Handles execution of each instruction.
- Register.cs: Stores and formats values using a four-digit word model.
- TraversableRegister.cs: Enables linked navigation of memory.
- UVSim.cs: Runs the fetch-decode-execute loop and manages memory setup.

UVSimGUI/
- MainPage.xaml: Defines the visual layout of the GUI.
- MainPage.xaml.cs: Handles user interaction, file operations, execution control.
- ThemeColors.cs: Loads and saves persistent theme settings.
- App.xaml, AppShell.xaml: Set up the MAUI application shell and routes.

------------------------------------------------------

Example Instruction File (example.txt):
---------------------------------------
1007   ; READ a value into memory address 07  
2007   ; LOAD value from address 07 into accumulator  
3008   ; ADD value at address 08  
2109   ; STORE result into address 09  
1109   ; PRINT value at address 09  
4300   ; HALT program  

------------------------------------------------------

Team Members:
-------------
- Daniel Urling  
- Michael Findlay  
- Traedon Harris
