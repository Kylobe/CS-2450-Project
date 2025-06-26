UVSim Milestone 3
=================

Description:
------------
This Project simulates Memory & Processor functions. It will take BasicML instructions in a .txt file.
The Program GUI allows Reading and Writing of .txt instructions. Saving and Running in a local console.

📁 UVSimClassLib
- CPU.cs (Handles Instruction Execution & I/O)
- Register.cs (Parses and stores instruction values)
- TraversableRegister.cs (Uses Next & Prev pointers for Traversal.) 
- UVSim.cs (Constrols the instruction cycle (Fetch -> Decode -> Execute))

THIS IS A MAC OS PROGRAM ONLY
-----------------------------
 **How to run the Program**

1. Requirements
The following must be installed in order to run the program:

- .NET SDK 6.0 or later

2. Download the proper .zip file for your OS

3. Open terminal and go to downloads directory

4. Unzip file and enter file

> unzip <file-name>.zip && cd <file-name>

5. Grant permissions to run executable file

> chmod +x UVSimGUI

6. Run UVSimGUI 

> ./UVSimGUI <full path to file>

You can also leave file path blank and in that case you will then be asked to input the path to your instructions file (ex: text1.txt)

The Program will then execute the instructions line by line as written.

===================
Team Members:
- Corbin Beus
- Daniel Urling
- Michael Findlay
- Traedon Harris
