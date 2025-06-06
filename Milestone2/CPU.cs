namespace Milestone2;

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
    public void Read()
    {

    }
    public void Write() 
    {

    }
    public void Load() 
    {

    }
    public void Store()
    {

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
        currentRegister.Next = MainMemory[int.Parse(currentRegister.SecondHalf)];
        return currentRegister;
    }
    public TraversableRegister BranchNeg(TraversableRegister currentRegister)
    {
        if (Accumulator.RegVal < 0) 
            currentRegister.Next = MainMemory[int.Parse(currentRegister.SecondHalf)];
        return currentRegister;
    }
    public TraversableRegister BranchZero(TraversableRegister currentRegister)
    {
        if (Accumulator.RegVal == 0)
            currentRegister.Next = MainMemory[int.Parse(currentRegister.SecondHalf)];
        return currentRegister;
    }
    public TraversableRegister Halt(TraversableRegister currentRegister)
    {
        currentRegister.Next = null;
        return currentRegister;
    }
}