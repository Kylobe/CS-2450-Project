using Milestone2;
using Xunit.Abstractions;

namespace xunitTest;

public class ProfessorUnitTests
{
    [Fact]
    public void ReadTest()
    {
        UVSim uvSim = new UVSim();
        Console.SetIn(new StringReader("7777\n"));
        string[] numbers = ["1008","4300"];
        uvSim.LoadArray(numbers);
        uvSim.Run();
        Assert.Equal(7777, uvSim.MainMemory[8].RegVal);
    }
}