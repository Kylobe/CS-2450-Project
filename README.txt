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
- Support for both 6 digit BasicML instruction and conversion from 4 digit to 6 digit instruction

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
- Enter or paste up to 250 BasicML instructions.
- Editor automatically shows line numbers for clarity.
- Instructions are validated and compiled into memory.

File Explorer:
- Located on the right side of the GUI.
- Lists all `.txt` files in a selected folder.
- Click a file to load its contents into the editor.
- Any changes made in a file editor will be temporarily saved while switching between files.
  If you need the changes to be saved permenantly, just be sure to click 'write'. 
- Files can also be renamed (feature available if wired).

Buttons and Their Functions:
----------------------------
Apply Theme:  Sets new theme colors based on two HEX input fields.
Toggle:       Toggles the theme primary and secondary colors by inverting their hex.
File:         Opens a file picker to load a `.txt` file into the editor.
Folder:       Opens a file picker to load a folder with '.txt' files autoloaded into the editor.
Rename:       Rename the file that you have selected.
Write:        Saves all currently loaded and edited files back to disk.
Compile:      Parses and validates instructions, loading them into memory.
Run:          Executes instructions line by line. Output appears in the console.

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

RGB Hex values to use:
--------------------
Primary                 Secondary
Blue - #007BFF          Light Gray - #F8F9FA
Red - #DC3545           Light Red - #F8D7DA
Green - #28A745         Light Green - #D3F9D8
Yellow - #FFC107        Light Yellow - #FFF3CD
Orange - #FD7E14        Light Teal - #C3FAE8
Purple - #6F42C1        Light Purple - #E0BBE4
Teal - #20C997          Gray - #6C757D

------------------------------------------------------

4 digit file support
--------------------
We do support 4 digit files. When you compile your program, the editor will update the 
4 digit instructions to 0XXXX format. This way we can still expect the same 3 digit 
instructions as when we have six digits without changing the numeric value. 

This means 1002 will be changed to 01002 so that '010' can be seen as a command, but
still allow 1002 to be the numeric value in any mathmatic operations. 

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
