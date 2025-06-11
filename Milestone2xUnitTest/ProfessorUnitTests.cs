using Milestone2;
using Xunit.Abstractions;

namespace xunitTest;

public class ProfessorUnitTests
{
    UVSim UVSim { get; } = new UVSim();
    [Theory]
    [InlineData("1008","4300")]
    [InlineData("0000","1008","0000","4300")]
    public void ReadTest_Success(params string[] numbers)
    {
        Console.SetIn(new StringReader("7777\n"));
        UVSim.LoadArray(numbers);
        UVSim.Run();
        Assert.Equal(7777, UVSim.MainMemory[8].RegVal);
    }

    [Theory]
    [InlineData("7777","1104", "4300","0000","0000","7777")]
    [InlineData("8888","1103","4300","0000","8888","0000")]
    public void WriteTest_Success(string targetResult, params string[] numbers)
    {
        var sw = new StringWriter();
        Console.SetOut(sw);
        UVSim.LoadArray(numbers);
        UVSim.Run();
        string result = sw.ToString();
        Assert.Contains(targetResult, result);
    }
    [Theory]
    [InlineData(4,"2004", "4300","0000","0000","7777")]
    [InlineData(3,"2003","4300","0000","8888","0000")]
    public void LoadTest_Success(int targetLocation, params string[] numbers)
    {
        UVSim.LoadArray(numbers);
        Assert.NotEqual(UVSim.MainMemory[targetLocation].RegVal, UVSim.Accumulator.RegVal);
        UVSim.Run();
        Assert.Equal(UVSim.MainMemory[targetLocation].RegVal, UVSim.Accumulator.RegVal);
    }
    [Theory]
    [InlineData(4,"2104", "4300","0000","0000","0000")]
    [InlineData(3,"2103","4300","0000","0000","0000")]
    public void StoreTest_Success(int targetLocation, params string[] numbers)
    {
        UVSim.Accumulator.RegVal = 7777;
        UVSim.LoadArray(numbers);
        Assert.NotEqual(UVSim.MainMemory[targetLocation].RegVal, UVSim.Accumulator.RegVal);
        UVSim.Run();
        Assert.Equal(UVSim.MainMemory[targetLocation].RegVal, UVSim.Accumulator.RegVal);
    }
    [Fact]
    public void CoordinatedOperationsOrderedTest_Success()
    {
        int targetLocation1 = 5;
        int targetLocation2 = 6;
        string[] numbers = ["1005", "1105", "2005", "2106", "4300"];
        Console.SetIn(new StringReader("7777\n"));
        
        var sw = new StringWriter();
        Console.SetOut(sw);
        
        UVSim.LoadArray(numbers);
        UVSim.Run();
        string result = sw.ToString();
        // command 10 should put 7777 in the target location
        Assert.Equal(7777, UVSim.MainMemory[targetLocation1].RegVal);
        // command 10 and 11 should give us 7777 in the console output
        Assert.Contains("7777", result);
        // command 10 and 20 should put 7777 in the accumulator
        Assert.Equal(7777, UVSim.Accumulator.RegVal);
        // command 10 and 20 and 21 should get 7777 in the second target location
        Assert.Equal(7777, UVSim.MainMemory[targetLocation2].RegVal);
    }
    [Fact]
    public void CoordinatedOperationsReversedOrderTest_Success()
    {
        int targetLocation1 = 89;
        int targetLocation2 = 0;
        string[] numbers = ["2189", "2088", "1189", "1000", "4300"];
        UVSim.Accumulator.RegVal = 7777;
        
        Console.SetIn(new StringReader("0000\n"));
        
        var sw = new StringWriter();
        Console.SetOut(sw);
        
        UVSim.LoadArray(numbers);
        UVSim.Run();
        string result = sw.ToString();
        // command 21 should put 7777 in the target location
        Assert.Equal(7777, UVSim.MainMemory[targetLocation1].RegVal);
        // command 20 should clear the accumulator due to referencing an empty location
        Assert.Equal(0, UVSim.Accumulator.RegVal);
        // command 21 and 11 should put 7777 in the console output
        Assert.Contains("7777", result);
        // command 10 should clear the second target location
        Assert.Equal(0, UVSim.MainMemory[targetLocation2].RegVal);
    }
}