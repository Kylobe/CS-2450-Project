using System;
using System.IO;
using System.Linq;
using Microsoft.Maui.Controls;

namespace UVSimClassLib
{
    public class UVSim
    {
        public TraversableRegister[] MainMemory = new TraversableRegister[100];
        public Register Accumulator = new Register("000000");
        private CPU CPU { get; set; }
        private bool Done { get; set; } = false;
        private int MaxRegister { get; } = 249;

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
                if (Math.Abs(lineVal) <= 999999)
                {
                    return true;
                }
                else
                {
                    errorMessage = line + " must contain 6 or less digits";
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
                    MainMemory[i] = new TraversableRegister("000000");
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
                    TraversableRegister head = new TraversableRegister("000000");
                    MainMemory[i].Prev = head;
                    head.Next = MainMemory[i];
                }
            }
        }

        public async Task Run(VerticalStackLayout mockConsole)
        {
            TraversableRegister currentRegister = MainMemory[0];
            Done = false;
            while (!Done)
            {
                Console.Write(currentRegister.FirstHalf);
                switch (currentRegister.FirstHalf)
            {
                case "010":
                    if (int.Parse(currentRegister.SecondHalf) <= MaxRegister)
                        await CPU.Read(int.Parse(currentRegister.SecondHalf), mockConsole);
                    break;
                case "011":
                    if (int.Parse(currentRegister.SecondHalf) <= MaxRegister)
                        CPU.Write(int.Parse(currentRegister.SecondHalf), mockConsole);
                    break;
                case "020":
                    if (int.Parse(currentRegister.SecondHalf) <= MaxRegister)
                        CPU.Load(int.Parse(currentRegister.SecondHalf));
                    break;
                case "021":
                    if (int.Parse(currentRegister.SecondHalf) <= MaxRegister)
                        CPU.Store(int.Parse(currentRegister.SecondHalf));
                    break;
                case "030":
                    if (int.Parse(currentRegister.SecondHalf) <= MaxRegister)
                        CPU.Add(int.Parse(currentRegister.SecondHalf));
                    break;
                case "031":
                    if (int.Parse(currentRegister.SecondHalf) <= MaxRegister)
                        CPU.Subtract(int.Parse(currentRegister.SecondHalf));
                    break;
                case "032":
                    if (int.Parse(currentRegister.SecondHalf) <= MaxRegister)
                        CPU.Divide(int.Parse(currentRegister.SecondHalf));
                    break;
                case "033":
                    if (int.Parse(currentRegister.SecondHalf) <= MaxRegister)
                        CPU.Multiply(int.Parse(currentRegister.SecondHalf));
                    break;
                case "040":
                    if (int.Parse(currentRegister.SecondHalf) <= MaxRegister)
                        currentRegister = CPU.Branch(currentRegister);
                    break;
                case "041":
                    if (int.Parse(currentRegister.SecondHalf) <= MaxRegister)
                        currentRegister = CPU.BranchNeg(currentRegister);
                    break;
                case "042":
                    if (int.Parse(currentRegister.SecondHalf) <= MaxRegister)
                        currentRegister = CPU.BranchZero(currentRegister);
                    break;
                case "043":
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
