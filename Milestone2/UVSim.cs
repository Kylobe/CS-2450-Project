namespace Milestone2;

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

    public void LoadFile(string filePath)
    {
        string[] numbers = File.ReadAllLines(filePath)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray();
        LoadArray(numbers);
    }

    public void LoadArray(string[] numbers)
    {
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
    public void Run()
    {
        TraversableRegister currentRegister = MainMemory[0];
        while (!Done)
        {
            switch (currentRegister.FirstHalf)
            {
                case "10":
                    CPU.Read(int.Parse(currentRegister.SecondHalf));
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