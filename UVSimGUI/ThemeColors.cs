using Microsoft.Maui.Graphics;
using Microsoft.Maui.Storage;
using System.IO;

namespace UVSimGUI;

public class ThemeColors
{
    public string PrimaryHex { get; set; } = "#4C721D";
    public string OffHex { get; set; } = "#FFFFFF";

    public Color Primary => IsValidHex(PrimaryHex) ? Color.FromArgb(PrimaryHex) : Colors.Green;
    public Color Off => IsValidHex(OffHex) ? Color.FromArgb(OffHex) : Colors.White;

    private static string ConfigPath =>
        Path.Combine(FileSystem.AppDataDirectory, "theme.txt");

    public void Save()
    {
        File.WriteAllText(ConfigPath, $"{PrimaryHex}\n{OffHex}");
    }

    public static ThemeColors Load()
    {
        try
        {
            if (File.Exists(ConfigPath))
            {
                var lines = File.ReadAllLines(ConfigPath);
                return new ThemeColors
                {
                    PrimaryHex = lines.ElementAtOrDefault(0)?.Trim() ?? "#4C721D",
                    OffHex = lines.ElementAtOrDefault(1)?.Trim() ?? "#FFFFFF"
                };
            }
        }
        catch
        {
            return new ThemeColors
            {
                PrimaryHex = "#4C721D",
                OffHex = "#FFFFFF"
            };
        }

        return new ThemeColors();
    }

    public static bool IsValidHex(string hex)
    {
        return !string.IsNullOrWhiteSpace(hex) &&
               hex.StartsWith("#") &&
               (hex.Length == 7 || hex.Length == 9) &&
               int.TryParse(hex[1..], System.Globalization.NumberStyles.HexNumber, null, out _);
    }
}

