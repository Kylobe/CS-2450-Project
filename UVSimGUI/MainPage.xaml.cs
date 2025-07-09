using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using CommunityToolkit.Maui.Storage;
using UVSimClassLib;
#if MACCATALYST
using AppKit;
using Foundation;
#endif

namespace UVSimGUI;

public partial class MainPage : ContentPage
{
    public ObservableCollection<FileDisplay> Files { get; set; } = new();
    const string DefaultFileName = "CustomBasicML.txt";
    bool Compiled = false;
    UVSim UVSim = new UVSim();
    private ThemeColors Theme;
    private FileDisplay? _activeFile;
    private string? _folderPath;

    public MainPage()
    {
        InitializeComponent();
        Theme = ThemeColors.Load();
        BindingContext = this;
        Resources["PrimaryColor"] = Theme.Primary;
        Resources["OffColor"] = Theme.Off;
    }
    private async void OnRenameClicked(object sender, EventArgs e)
    {
        try
        {
            string? newName = await DisplayPromptAsync("Rename File", "Enter new file name (with .txt):");
            if (string.IsNullOrEmpty(newName)) return;
            string oldPath = Path.Combine(_folderPath, _activeFile.FileName);
            string newPath = Path.Combine(_folderPath, newName);

            if (File.Exists(oldPath))
            {
                File.Move(oldPath, newPath);
                _activeFile.FileName = newName;
                _activeFile.FullPath = newPath;
                FileExplorerView.ItemsSource = null;
                FileExplorerView.ItemsSource = Files;
                AddToConsole($"Renamed to: {newPath}", Colors.Black);
            }
            else
            {
                AddToConsole("Original file not found.", Colors.Orange);
            }
        }
        catch (Exception ex)
        {
            AddToConsole(ex.Message, Colors.Red);
        }
    }
    private async void OnLoadFolderClicked(object sender, EventArgs e)
    {
        try
        {
            var pickResult = await FolderPicker.Default.PickAsync(CancellationToken.None);
            if (!pickResult.IsSuccessful)
            {
                AddToConsole($"Folder pick cancelled or failed: {pickResult.Exception?.Message}", Colors.Orange);
                return;
            }

            _folderPath = pickResult.Folder.Path;

            var txtFiles = Directory.GetFiles(_folderPath, "*.txt", SearchOption.TopDirectoryOnly);

            if (txtFiles.Length == 0)
            {
                AddToConsole("No .txt files found in the selected folder.", Colors.Orange);
                return;
            }

            Files.Clear();

            foreach (var filePath in txtFiles)
            {
                Files.Add(new FileDisplay(new FileResult(filePath)));
            }

            AddToConsole($"Loaded {txtFiles.Length} file(s) from {_folderPath}.", Colors.Black);
            Compiled = false;
        }
        catch (Exception ex)
        {
            AddToConsole(ex.Message, Colors.Red);
        }
    }
    private async void OnFileSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;
        if (_activeFile != null) _activeFile.FileText = InstructionsEditor.Text;
        _activeFile = e.CurrentSelection[0] as FileDisplay;
        if (_activeFile is null) return;

        try
        {
            if (_activeFile.FileText != null)
            {
                InstructionsEditor.Text = _activeFile.FileText;
            }
            else
            {
                InstructionsEditor.Text = await File.ReadAllTextAsync(_activeFile.FullPath);
            }
            Compiled = false;
        }
        catch (Exception ex)
        {
            AddToConsole(ex.Message, Colors.Red);
        }
    }

    private async void OnLoadClicked(object sender, EventArgs e)
    {
        var customFileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { "public.plain-text" } },
                { DevicePlatform.Android, new[] { "text/plain" } },
                { DevicePlatform.WinUI, new[] { ".txt" } },
                { DevicePlatform.Tizen, new[] { "text/plain" } },
                { DevicePlatform.macOS, new[] { "public.plain-text" } },
                { DevicePlatform.MacCatalyst, new[] { "public.plain-text" } }
            });

        PickOptions options = new()
        {
            PickerTitle = "Please select a text file",
            FileTypes = customFileType,
        };
        try
        {
            FileResult result = await FilePicker.Default.PickAsync(options);
            if (result != null && result.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                using var stream = await result.OpenReadAsync();
                using var reader = new StreamReader(stream);
                InstructionsEditor.Text = await reader.ReadToEndAsync();
                Files.Add(new FileDisplay(result));
                AddToConsole($"Added file: {result.FileName}", Colors.Black);
                Compiled = false;
            }
        }
        catch (Exception ex)
        {
            AddToConsole(ex.Message, Colors.Red);
        }
    }

    private void OnEditorChanged(object sender, TextChangedEventArgs e)
    {
        Compiled = false;
    }
    private async void OnWriteClicked(object sender, EventArgs e)
    {
        try
        {
            _activeFile.FileText = InstructionsEditor.Text;
            foreach (FileDisplay file in Files)
            {
                await SaveSilentlyAsync(_folderPath, file.FileName, file.FileText);
            }
        }
        catch (Exception ex)
        {
            AddToConsole(ex.Message, Colors.Red);
        }
    }
    private async Task SaveSilentlyAsync(string folderPath, string fileName, string text)
    {
        Directory.CreateDirectory(folderPath);                     // make sure it exists
        var fullPath = Path.Combine(folderPath, fileName);

        await File.WriteAllTextAsync(fullPath, text);
        AddToConsole($"Saved to: {fullPath}", Colors.Black);
    }
    private async void OnCompileClicked(object sender, EventArgs e)
    {
        try
        {
            string[] lines = InstructionsEditor.Text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            UVSim.LoadArray(lines);
            Compiled = true;
            AddToConsole("Compiled!", Colors.Black);
        }
        catch (Exception ex)
        {
            AddToConsole(ex.Message, Colors.Red);
        }
    }

    private async void OnRunClicked(object sender, EventArgs e)
    {
        try
        {
            if (!Compiled)
                throw new Exception("Please compile before run");

            await UVSim.Run(MockConsole);
            AddToConsole("Run Success!", Colors.Black);
        }
        catch (Exception ex)
        {
            AddToConsole(ex.Message, Colors.Red);
        }
    }

    private void UpdateTheme(string primaryHex, string offHex)
    {
        Theme.PrimaryHex = primaryHex;
        Theme.OffHex = offHex;
        Theme.Save();

        Resources["PrimaryColor"] = Theme.Primary;
        Resources["OffColor"] = Theme.Off;

        AddToConsole($"Theme updated to {primaryHex} / {offHex}", Colors.LightGreen);
    }

    private void OnApplyThemeClicked(object sender, EventArgs e)
    {
        string primaryHex = PrimaryColorEntry.Text?.Trim();
        string offHex = OffColorEntry.Text?.Trim();
        UpdateTheme(primaryHex, offHex);

        if (!string.IsNullOrWhiteSpace(primaryHex) && !string.IsNullOrWhiteSpace(offHex))
        {
            try
            {
                UpdateTheme(primaryHex, offHex);
            }
            catch (Exception ex)
            {
                AddToConsole($"Invalid color format: {ex.Message}", Colors.Red);
            }
        }
        else
        {
            AddToConsole("Please enter both primary and off colors.", Colors.Orange);
        }
    }

    private void AddToConsole(string message, Color textColor)
    {
        Label newLabel = new Label
        {
            Text = message,
            TextColor = Colors.Black,
            FontSize = 14
        };
        MockConsole.Add(newLabel);
    }
}

public class FileDisplay
{
    public string FileName { get; set; }
    public string FullPath { get; set; }
    public string? FileText { get; set; }

    public FileDisplay(FileResult file)
    {
        FileName = file.FileName;
        FullPath = file.FullPath;
    }
}
