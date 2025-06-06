
namespace Milestone2;

public class Program
{
    static void Main()
    {
        UVSim uvSim = new UVSim();
        uvSim.LoadArray(["1008", "4300"]); // load file name
        uvSim.Run();
        Console.WriteLine("End of Program\nPress any key to exit...");
        Console.ReadKey();
    }
}
