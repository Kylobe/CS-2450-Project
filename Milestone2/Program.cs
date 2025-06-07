
namespace Milestone2;

public class Program
{
    static void Main(string[] args)
    {
        UVSim uvSim = new UVSim();
        string filePath = string.Empty;
        /*
        filePath = args.Length > 0 
            ? args[0] 
            : GetFilePath();
        */
        uvSim.LoadArray(["1050", "1150", "2050", "3108", "2150", "4207", "4001", "4300", "0001"]); 
        uvSim.Run();
        Console.WriteLine("End of Program\nPress any key to exit...");
        Console.ReadKey();
    }
    static string GetFilePath()
    {
        string filePath = Console.ReadLine() ?? string.Empty;
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File {filePath} does not exist or cannot be accessed!");
            filePath = GetFilePath();
        }
        return filePath;
    }
}
