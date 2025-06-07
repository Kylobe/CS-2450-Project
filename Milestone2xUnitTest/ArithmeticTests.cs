using Milestone2;
using Xunit.Abstractions;

namespace xunitTest;

public class ArithmeticTests
{
    [Theory]
    [InlineData("1050", "1051", "2050", "3051", "4300")]
    [InlineData("0000", "1050", "0000", "1051", "0000", "2050", "0000", "3051", "0000", "4300")]
    public void AddTest_Success(params string[] numbers)
    {
        UVSim uvSim = new UVSim();
        Console.SetIn(new StringReader("10\n3"));
        uvSim.LoadArray(numbers);
        uvSim.Run();
        Assert.Equal(13, uvSim.Accumulator.RegVal);
    }
    [Theory]
    [InlineData("1050", "1051", "2050", "3051", "4300")]
    [InlineData("0000", "1050", "0000", "1051", "0000", "2050", "0000", "3051", "0000", "4300")]
    public void AddOverFlowTest_Success(params string[] numbers)
    {
        UVSim uvSim = new UVSim();
        Console.SetIn(new StringReader("9999\n9999"));
        uvSim.LoadArray(numbers);
        uvSim.Run();
        Assert.Equal(1999, uvSim.Accumulator.RegVal);
    }
    [Theory]
    [InlineData("1050", "1051", "2050", "3151", "4300")]
    [InlineData("0000", "1050", "0000", "1051", "0000", "2050", "0000", "3151", "0000", "4300")]
    public void SubtractTest_Success(params string[] numbers)
    {
        UVSim uvSim = new UVSim();
        Console.SetIn(new StringReader("10\n3"));
        uvSim.LoadArray(numbers);
        uvSim.Run();
        Assert.Equal(7, uvSim.Accumulator.RegVal);
    }
    [Theory]
    [InlineData("1050", "1051", "2050", "3151", "4300")]
    [InlineData("0000", "1050", "0000", "1051", "0000", "2050", "0000", "3151", "0000", "4300")]
    public void SubtractOverFlowTest_Success(params string[] numbers)
    {
        UVSim uvSim = new UVSim();
        Console.SetIn(new StringReader("9999\n-9999"));
        uvSim.LoadArray(numbers);
        uvSim.Run();
        Assert.Equal(1999, uvSim.Accumulator.RegVal);
    }
    [Theory]
    [InlineData("1050", "1051", "2050", "3351", "4300")]
    [InlineData("0000", "1050", "0000", "1051", "0000", "2050", "0000", "3351", "0000", "4300")]
    public void MultiplyTest_Success(params string[] numbers)
    {
        UVSim uvSim = new UVSim();
        Console.SetIn(new StringReader("10\n3"));
        uvSim.LoadArray(numbers);
        uvSim.Run();
        Assert.Equal(30, uvSim.Accumulator.RegVal);
    }
    [Theory]
    [InlineData("1050", "1051", "2050", "3351", "4300")]
    [InlineData("0000", "1050", "0000", "1051", "0000", "2050", "0000", "3351", "0000", "4300")]
    public void MultiplyOverFlowTest_Success(params string[] numbers)
    {
        UVSim uvSim = new UVSim();
        Console.SetIn(new StringReader("9999\n9999"));
        uvSim.LoadArray(numbers);
        uvSim.Run();
        Assert.Equal(9998, uvSim.Accumulator.RegVal);
    }
    [Theory]
    [InlineData("1050", "1051", "2050", "3251", "4300")]
    [InlineData("0000", "1050", "0000", "1051", "0000", "2050", "0000", "3251", "0000", "4300")]
    public void DivideTest_Success(params string[] numbers)
    {
        UVSim uvSim = new UVSim();
        Console.SetIn(new StringReader("10\n3"));
        uvSim.LoadArray(numbers);
        uvSim.Run();
        Assert.Equal(3, uvSim.Accumulator.RegVal);
    }
}

