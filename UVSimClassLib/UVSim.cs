using System;
using System.IO;
using System.Linq;
using Microsoft.Maui.Controls;

namespace UVSimClassLib
{
    public class UVSim
    {
        public TraversableRegister[] MainMemory = new TraversableRegister[100];
        public Register Accumulator = new Register("0000");
        private CPU CPU { get; set; }
        private bool Done { get; set; } = false;

        public UVSim()
        {
            CPU = new CPU(MainMemory, Accumulator);
        }

        public bool LoadFile(string filePath, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                string fileContent = File.ReadAllText(filePath);
                string[] lines = fileContent
                    .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

                LoadArray(lines);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        private bool CheckString(string line, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                int lineVal = int.Parse(line);
                if (Math.Abs(lineVal) <= 9999)
                {
                    return true;
                }
                else
                {
                    errorMessage = line + " must contain 4 or less digits";
                    return false;
                }
            }
            catch (Exception)
            {
                errorMessage = line + " must contain only numerical values";
                return false;
            }
        }

        public void LoadArray(string[] lines)
        {
            if (lines == null || lines.Length == 0)
                throw new Exception("Cannot load empty data");

            string[] numbers = new string[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    string checkErrorMessage;
                    if (CheckString(lines[i], out checkErrorMessage))
                    {
                        numbers[i] = lines[i].Trim();
                    }
                    else
                    {
                        int errorLine = i + 1;
                        string errorMessage = "Error on line: " + errorLine + " " + checkErrorMessage + "\n";
                        throw new FormatException(errorMessage);
                    }
                }
            }

            for (int i = 0; i < MainMemory.Length; i++)
            {
                if (i >= numbers.Length)
                    MainMemory[i] = new TraversableRegister("0000");
                else
                    MainMemory[i] = new TraversableRegister(numbers[i]);
            }

            for (int i = 0; i < MainMemory.Length; i++)
            {
                if (i < MainMemory.Length - 1)
                {
                    MainMemory[i].Next = MainMemory[i + 1];
                }
                if (i != 0)
                {
                    MainMemory[i].Prev = MainMemory[i - 1];
                }
                else
                {
                    TraversableRegister head = new TraversableRegister("0000");
                    MainMemory[i].Prev = head;
                    head.Next = MainMemory[i];
                }
            }
        }

        public void Run(VerticalStackLayout mockConsole)
        {
            TraversableRegister currentRegister = MainMemory[0];
            while (!Done)
            {
                switch (currentRegister.FirstHalf)
                {
                    case "10":
                        CPU.Read(int.Parse(currentRegister.SecondHalf), mockConsole);
                        break;
                    case "11":
                        CPU.Write(int.Parse(currentRegister.SecondHalf));
                        break;
                    case "20":
                        CPU.Load(int.Parse(currentRegister.SecondHalf));
                        break;
                    case "21":
                        CPU.Store(int.Parse(currentRegister.SecondHalf));
                        break;
                    case "30":
                        CPU.Add(int.Parse(currentRegister.SecondHalf));
                        break;
                    case "31":
                        CPU.Subtract(int.Parse(currentRegister.SecondHalf));
                        break;
                    case "32":
                        CPU.Divide(int.Parse(currentRegister.SecondHalf));
                        break;
                    case "33":
                        CPU.Multiply(int.Parse(currentRegister.SecondHalf));
                        break;
                    case "40":
                        currentRegister = CPU.Branch(currentRegister);
                        break;
                    case "41":
                        currentRegister = CPU.BranchNeg(currentRegister);
                        break;
                    case "42":
                        currentRegister = CPU.BranchZero(currentRegister);
                        break;
                    case "43":
                        currentRegister = CPU.Halt(currentRegister);
                        break;
                }

                if (currentRegister.Next is null)
                {
                    Done = true;
                }
                else
                {
                    currentRegister = currentRegister.Next;
                }
            }
        }
    }
}
