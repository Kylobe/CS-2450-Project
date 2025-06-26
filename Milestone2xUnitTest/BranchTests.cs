using Milestone2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xunitTest;

public class BranchTests
{
    [Theory]
    [InlineData("4002", "4300", "2004", "4300", "0005")]
    public void Branch_Success(params string[] numbers)
    {
        UVSim uvSim = new UVSim();
        uvSim.LoadArray(numbers);
        uvSim.Run();
        Assert.Equal(5, uvSim.Accumulator.RegVal);
    }
    [Theory]
    [InlineData("2004", "4103", "2005", "4300", "-1000", "1000")]  //Tests when the accumulator is negative.
    [InlineData("2004", "4103", "2005", "4300", "1000", "-1000")]  //Tests when the accumulator is positive.
    [InlineData("2004", "4103", "2005", "4300", "0000", "-1000")]  //Tests when the accumulator is zero.
    public void BranchNeg_Success(params string[] numbers)
    {
        UVSim uvSim = new UVSim();
        uvSim.LoadArray(numbers);
        uvSim.Run();
        Assert.Equal(-1000, uvSim.Accumulator.RegVal);
    }
    [Theory]
    [InlineData("2004", "4203", "2005", "4300", "-1000", "0000")]  //Tests when the accumulator is negative.
    [InlineData("2004", "4203", "2005", "4300", "1000", "0000")]   //Tests when the accumulator is positive.
    [InlineData("2004", "4203", "2005", "4300", "0000", "-1000")]  //Tests when the accumulator is zero.
    public void BranchZero_Success(params string[] numbers)
    {
        UVSim uvSim = new UVSim();
        uvSim.LoadArray(numbers);
        uvSim.Run();
        Assert.Equal(0, uvSim.Accumulator.RegVal);
    }

    [Theory]
    [InlineData("2002","4300","2003", "0000")]
    public void Halt_Success(params string[] numbers)
    {
        UVSim uvSim = new UVSim();
        uvSim.LoadArray(numbers);
        uvSim.Run();
        Assert.Equal(2003, uvSim.Accumulator.RegVal);
    }
}

