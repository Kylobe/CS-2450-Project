namespace UVSimClassLib;

public class CPU
{
    TraversableRegister[] MainMemory { get; set; }
    public Register Accumulator { get; set; }

    public CPU(TraversableRegister[] mainMemory, Register accumulator)
    {
        MainMemory = mainMemory;
        Accumulator = accumulator;
    }
    // all commands
    public void Read(int memoryAddress)
    {
        int input;
        //Ask for input
        Console.Write($"Type value for memory location {memoryAddress:D2}: ");
        while (!int.TryParse(Console.ReadLine(), out input))
        {
            Console.Write("Type an Integer: ");
        }
        //Store in memory 
        MainMemory[memoryAddress].RegVal = input;

    }
    public void Write(int memoryAddress)
    {
        //Output Value of the given address
        int value = MainMemory[memoryAddress].RegVal;
        Console.WriteLine($"Value of memory {memoryAddress:D2}: {value}");

    }
    public void Load(int memoryAddress) 
    {
        Accumulator.RegVal = MainMemory[memoryAddress].RegVal;
    }
    public void Store(int memoryAddress)
    {
        MainMemory[memoryAddress].RegVal = Accumulator.RegVal;
    }
    public void Add(int memoryAddress)
    {
        int newVal = Accumulator.RegVal + MainMemory[memoryAddress].RegVal;
        Accumulator.RegVal = newVal;
    }
    public void Subtract(int memoryAddress)
    {
        int newVal = Accumulator.RegVal - MainMemory[memoryAddress].RegVal;
        Accumulator.RegVal = newVal;        
    }
    public void Divide(int memoryAddress)
    {
        int newVal = Accumulator.RegVal / MainMemory[memoryAddress].RegVal;
        Accumulator.RegVal = newVal;
    }
    public void Multiply(int memoryAddress)
    {
        int newVal = Accumulator.RegVal * MainMemory[memoryAddress].RegVal;
        Accumulator.RegVal = newVal;
    }
    public TraversableRegister Branch(TraversableRegister currentRegister)
    {
        TraversableRegister jumpedRegister = MainMemory[int.Parse(currentRegister.SecondHalf)];
        if (jumpedRegister != null && jumpedRegister.Prev != null)
        {
            currentRegister = jumpedRegister.Prev;
        }
        return currentRegister;
    }
    public TraversableRegister BranchNeg(TraversableRegister currentRegister)
    {
        if (Accumulator.RegVal < 0)
        {
            TraversableRegister jumpedRegister = MainMemory[int.Parse(currentRegister.SecondHalf)];
            if (jumpedRegister != null && jumpedRegister.Prev != null)
            {
                currentRegister = jumpedRegister.Prev;
            }
        }
        return currentRegister;
    }
    public TraversableRegister BranchZero(TraversableRegister currentRegister)
    {
        if (Accumulator.RegVal == 0)
        {
            TraversableRegister jumpedRegister = MainMemory[int.Parse(currentRegister.SecondHalf)];
            if (jumpedRegister != null && jumpedRegister.Prev != null)
            {
                currentRegister = jumpedRegister.Prev;
            }
        }
        return currentRegister;
    }
    public TraversableRegister Halt(TraversableRegister currentRegister)
    {
        currentRegister.Next = null;
        return currentRegister;
    }
}