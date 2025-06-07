
namespace Milestone2;

public class Program
{
    static void Main(string[] args)
    {
        UVSim uvSim = new UVSim();
        string filePath = string.Empty;
        filePath = args.Length > 0 
            ? args[0] 
            : GetFilePath();
        uvSim.LoadFile(filePath); 
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
