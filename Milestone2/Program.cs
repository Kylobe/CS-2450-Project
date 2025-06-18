
namespace Milestone2;

public class Program
{
    static void Main(string[] args)
    {
        UVSim uvSim = new UVSim();
        string errorMessage;
        string filePath = string.Empty;
        filePath = args.Length > 0
            ? args[0]
            : GetFilePath() ?? string.Empty;
        bool going = true;
        while (going)
        {
            if (uvSim.LoadFile(filePath, out errorMessage))
            {
                going = false;
            }
            else
            {
                Console.Write(errorMessage);
                filePath = GetFilePath();
            }
        }
        uvSim.Run();
        Console.WriteLine("End of Program\nPress any key to exit...");
        Console.ReadKey();
    }
    static string GetFilePath()
    {
        Console.WriteLine("Please enter a file path: ");
        string filePath = Console.ReadLine() ?? string.Empty;
        bool going = true;
        while (going)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File {filePath} does not exist or cannot be accessed!");
                filePath = GetFilePath();
            }
            else
            {
                going = false;
            }
        }
        return filePath;
    }
}
