
namespace Milestone2;

public class Program
{
    static void Main()
    {
        UVSim uvSim = new UVSim();
        uvSim.LoadFile("test1.txt"); // load file name
        uvSim.Run();
        Console.WriteLine("End of Program\nPress any key to exit...");
        Console.ReadKey();
    }
}
