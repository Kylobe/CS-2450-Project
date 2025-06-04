namespace Milestone2;

public class UVSim
{
    private TraversableRegister[] MainMemory = new TraversableRegister[100];
    private Register Accumulator = new Register("0000");
    private CPU CPU { get; set; } 
    public bool Done { get; set; } = false;
    public UVSim()
    {
        CPU = new CPU(MainMemory, Accumulator);
    }

    public void Load(string filePath)
    {
        string[] numbers = File.ReadAllLines(filePath)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray();
        for (int i = 0; i < MainMemory.Length; i++)
        {
            if (i >= numbers.Length)
                MainMemory[i] = new TraversableRegister("0000");
            else
                MainMemory[i] = new TraversableRegister(numbers[i]);

        }
    }

    public void Run()
    {
        int i = 0;
        while (!Done)
        {
            TraversableRegister currentRegister = MainMemory[i];
            currentRegister.Next = MainMemory[i++];
            switch (currentRegister.FirstHalf)
            {
                case "10":
                    // CPU.Read(SecondHalf);
                    break;
                case "11":
                    // CPU.Write(SecondHalf);
                    break;
                case "20":
                    // CPU.Load(SecondHalf, accumulator);
                    break;
                case "21":
                    // CPU.Store(SecondHalf, accumulator);
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
                default:
                    throw new Exception("Unknown register " + currentRegister.FirstHalf);
            }
            
            if (currentRegister.Next is null)
            {
                Done = true;
            }
            i = Array.IndexOf(MainMemory, currentRegister);
            currentRegister = currentRegister.Next;
            i++;
        }
    }
}