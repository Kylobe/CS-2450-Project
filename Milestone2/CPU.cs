namespace Milestone2;

public class CPU
{
    Register[] MainMemory { get; set; } = new Register[100];
    public Register Accumulator { get; set; } = new Register();
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
        int newVal = Accumulator.RegVal + MainMemory[memoryAddress].RegVal;
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
    public void Branch()
    {

    }
    public void BranchNeg()
    {

    }
    public void BranchZero()
    {

    }
    public void Halt()
    {

    }
}