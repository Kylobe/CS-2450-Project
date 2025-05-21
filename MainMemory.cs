using System.IO;


public class MainMemory
{
    public int[] words;
    public string[] commands;
    void MainMemory(string commandFile)
    {
        string[] lines = File.ReadAllLines(commandFile);
        foreach (string line in lines)
        {
            int i = 0;
            string curStr = "";
            foreach (char character in line)
            {
                if (!character == "+" && !character == "-")
                {
                    if (i <= 2)
                    {
                        curStr += character;
                    }
                    else if (i == 2)
                    {
                        curStr += "|";
                    }
                }
            }
        }
    }
}
