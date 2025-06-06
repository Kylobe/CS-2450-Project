using Milestone2;
using Xunit.Abstractions;

namespace xunitTest;

public class ArithmeticTests
{
    [Fact]
    public void AddTest_Success()
    {
        TraversableRegister[] mainMemory = new TraversableRegister[100];
        Register accumulator = new Register("0000");
        for (int i = 0; i < mainMemory.Length; i++)
        {
            mainMemory[i] = new TraversableRegister("0000");
        }
        CPU cpu = new CPU(mainMemory, accumulator);
        mainMemory[0].RegVal = 1;
        accumulator.RegVal = 1;
        cpu.Add(0);
        Console.WriteLine("Testing Add Function");
        Assert.Equal(2, accumulator.RegVal);
    }
    [Fact]
    public void AddOverFlowTest_Success()
    {
        TraversableRegister[] mainMemory = new TraversableRegister[100];
        Register accumulator = new Register("0000");
        for (int i = 0; i < mainMemory.Length; i++)
        {
            mainMemory[i] = new TraversableRegister("0000");
        }
        CPU cpu = new CPU(mainMemory, accumulator);
        mainMemory[0].RegVal = 5000;
        accumulator.RegVal = 5000;
        cpu.Add(0);
        Assert.Equal(1000, accumulator.RegVal);
    }
    [Fact]
    public void SubtractTest_Success()
    {
        TraversableRegister[] mainMemory = new TraversableRegister[100];
        Register accumulator = new Register("0000");
        for (int i = 0; i < mainMemory.Length; i++)
        {
            mainMemory[i] = new TraversableRegister("0000");
        }
        CPU cpu = new CPU(mainMemory, accumulator);
        mainMemory[0].RegVal = 1;
        accumulator.RegVal = 10;
        cpu.Subtract(0);
        Assert.Equal(9, accumulator.RegVal);
    }
    [Fact]
    public void SubtractOverFlowTest_Success()
    {
        TraversableRegister[] mainMemory = new TraversableRegister[100];
        Register accumulator = new Register("0000");
        for (int i = 0; i < mainMemory.Length; i++)
        {
            mainMemory[i] = new TraversableRegister("0000");
        }
        CPU cpu = new CPU(mainMemory, accumulator);
        mainMemory[0].RegVal = -5000;
        accumulator.RegVal = 5000;
        cpu.Subtract(0);
        Assert.Equal(1000, accumulator.RegVal);
    }
    [Fact]
    public void MultiplyTest_Success()
    {
        TraversableRegister[] mainMemory = new TraversableRegister[100];
        Register accumulator = new Register("0000");
        for (int i = 0; i < mainMemory.Length; i++)
        {
            mainMemory[i] = new TraversableRegister("0000");
        }
        CPU cpu = new CPU(mainMemory, accumulator);
        mainMemory[0].RegVal = 2;
        accumulator.RegVal = 10;
        cpu.Multiply(0);
        Assert.Equal(20, accumulator.RegVal);
    }
    [Fact]
    public void MultiplyOverFlowTest_Success()
    {
        TraversableRegister[] mainMemory = new TraversableRegister[100];
        Register accumulator = new Register("0000");
        for (int i = 0; i < mainMemory.Length; i++)
        {
            mainMemory[i] = new TraversableRegister("0000");
        }
        CPU cpu = new CPU(mainMemory, accumulator);
        mainMemory[0].RegVal = 2;
        accumulator.RegVal = 5000;
        cpu.Multiply(0);
        Assert.Equal(1000, accumulator.RegVal);
    }
    [Fact]
    public void DivideTest_Success()
    {
        TraversableRegister[] mainMemory = new TraversableRegister[100];
        Register accumulator = new Register("0000");
        for (int i = 0; i < mainMemory.Length; i++)
        {
            mainMemory[i] = new TraversableRegister("0000");
        }
        CPU cpu = new CPU(mainMemory, accumulator);
        mainMemory[0].RegVal = 2;
        accumulator.RegVal = 10;
        cpu.Divide(0);
        Assert.Equal(5, accumulator.RegVal);
    }
}

