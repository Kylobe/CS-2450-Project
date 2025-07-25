﻿using System;
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
    private ConsoleManager consoleManager;
    private const int MaxLines = 250;
    private bool _isUpdating = false;
    private bool _isScrolling = false;
    private double _lastScrollPosition = 0;
    private ThemeColors Theme;
    private FileDisplay? _activeFile;
    private string? _folderPath;

    public MainPage()
    {
        InitializeComponent();
        consoleManager = new ConsoleManager(MockConsole, ConsoleScrollView);
        Theme = ThemeColors.Load();
        BindingContext = this;
        PopulateLineNum();
        EditorScrollView.Scrolled += (sender, e) =>
        {
            if (!_isScrolling && Math.Abs(e.ScrollY - _lastScrollPosition) > 0.1)
            {
                _isScrolling = true;
                LineNumberScrollView.ScrollToAsync(0, e.ScrollY, false);
                _lastScrollPosition = e.ScrollY;
                _isScrolling = false;
            }
        };
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
                consoleManager.AddToConsole($"Renamed to: {newPath}", Colors.Black);
            }
            else
            {
                consoleManager.AddToConsole("Original file not found.", Colors.Orange);
            }
        }
        catch (Exception ex)
        {
            consoleManager.AddToConsole(ex.Message, Colors.Red);
        }
    }
    private async void OnLoadFolderClicked(object sender, EventArgs e)
    {
        try
        {
            var pickResult = await FolderPicker.Default.PickAsync(CancellationToken.None);
            if (!pickResult.IsSuccessful)
            {
                consoleManager.AddToConsole($"Folder pick cancelled or failed: {pickResult.Exception?.Message}", Colors.Orange);
                return;
            }

            _folderPath = pickResult.Folder.Path;

            var txtFiles = Directory.GetFiles(_folderPath, "*.txt", SearchOption.TopDirectoryOnly);

            if (txtFiles.Length == 0)
            {
                consoleManager.AddToConsole("No .txt files found in the selected folder.", Colors.Orange);
                return;
            }

            Files.Clear();

            foreach (var filePath in txtFiles)
            {
                Files.Add(new FileDisplay(new FileResult(filePath)));
            }

            consoleManager.AddToConsole($"Loaded {txtFiles.Length} file(s) from {_folderPath}.", Colors.Black);
            Compiled = false;
        }
        catch (Exception ex)
        {
            consoleManager.AddToConsole(ex.Message, Colors.Red);
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
                _activeFile.FileText = InstructionsEditor.Text;
            }
            Compiled = false;
        }
        catch (Exception ex)
        {
            consoleManager.AddToConsole(ex.Message, Colors.Red);
        }
    }

    private void PopulateLineNum()
    {
        LineNumberLabel.Text = string.Join(Environment.NewLine, Enumerable.Range(0, MaxLines).Select(i => i.ToString("D2")));
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
                FileDisplay loadedFile = new FileDisplay(result);
                loadedFile.FileText = InstructionsEditor.Text;
                _activeFile = loadedFile;
                Files.Add(loadedFile);
                consoleManager.AddToConsole($"Added file: {result.FileName}", Colors.Yellow);
                Compiled = false;
            }
        }
        catch (Exception ex)
        {
            consoleManager.AddToConsole(ex.Message, Colors.Red);
        }
    }

    private void OnEditorChanged(object sender, TextChangedEventArgs e)
    {
        if (_isUpdating) return;
    
        _isUpdating = true;
        try
        {
            Compiled = false;
            string[] lines = InstructionsEditor.Text?.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None) ?? Array.Empty<string>();

            if (lines.Length > MaxLines)
            {
                string newText = string.Join(Environment.NewLine, lines.Take(MaxLines));
                InstructionsEditor.Text = newText;
                InstructionsEditor.CursorPosition = newText.Length;
            }
        }
        finally
        {
            _isUpdating = false;
        }
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
            consoleManager.AddToConsole(ex.Message, Colors.Red);
        }
    }
    private async Task SaveSilentlyAsync(string folderPath, string fileName, string text)
    {
        Directory.CreateDirectory(folderPath);                     // make sure it exists
        var fullPath = Path.Combine(folderPath, fileName);

        await File.WriteAllTextAsync(fullPath, text);
        consoleManager.AddToConsole($"Saved to: {fullPath}", Colors.Black);
    }
    private async void OnCompileClicked(object sender, EventArgs e)
    {
        try
        {
            string[] lines = InstructionsEditor.Text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            UVSim.LoadArray(lines);
            InstructionsEditor.Text = String.Join(Environment.NewLine, UVSim.MainMemory.Select(i => i.ToString()));
            consoleManager.AddToConsole("Compiled!", Colors.Black);
            Compiled = true;
        }
        catch (Exception ex)
        {
            consoleManager.AddToConsole(ex.Message, Colors.Red);
        }
    }

    private async void OnRunClicked(object sender, EventArgs e)
    {
        try
        {
            if (!Compiled)
                throw new Exception("Please compile before run");

            await UVSim.Run(consoleManager);
            consoleManager.AddToConsole("Run Success!", Colors.Black);
        }
        catch (Exception ex)
        {
            consoleManager.AddToConsole(ex.Message, Colors.Red);
        }
    }

    private void UpdateTheme(string primaryHex, string offHex)
    {
        if (!ThemeColors.IsValidHex(primaryHex) || !ThemeColors.IsValidHex(offHex))
        {
            Theme.PrimaryHex = "#4C721D";
            Theme.OffHex = "#FFFFFF";
            consoleManager.AddToConsole("⚠ Invalid HEX code. Reverting to default theme (UVU green/white).", Colors.Orange);
        }
        else
        {
            Theme.PrimaryHex = primaryHex;
            Theme.OffHex = offHex;
            consoleManager.AddToConsole($"Theme updated to {primaryHex} / {offHex}", Colors.LightGreen);
        }

        Theme.Save();

        Resources["PrimaryColor"] = Theme.Primary;
        Resources["OffColor"] = Theme.Off;
    }


    private void OnThemeToggled(object sender, ToggledEventArgs e)
    {
        if (!ThemeColors.IsValidHex(Theme.PrimaryHex) || !ThemeColors.IsValidHex(Theme.OffHex))
        {
            consoleManager.AddToConsole("⚠ Cannot invert theme: Invalid HEX codes. Reverting to default.", Colors.Red);
            UpdateTheme("#4C721D", "#FFFFFF");
            return;
        }
        string primaryHex = InverseColor(Theme.PrimaryHex);
        string offHex = InverseColor(Theme.OffHex);
        UpdateTheme(primaryHex, offHex);
    }

    private string InverseColor(string hex)
    {
        hex = hex.Substring(1);

        var r = 255 - Convert.ToInt32(hex.Substring(0, 2), 16);
        var g = 255 - Convert.ToInt32(hex.Substring(2, 2), 16);
        var b = 255 - Convert.ToInt32(hex.Substring(4, 2), 16);

        return $"#{r:X2}{g:X2}{b:X2}";
    }
    private void OnApplyThemeClicked(object sender, EventArgs e)
    {
        string primaryHex = PrimaryColorEntry.Text?.Trim();
        string offHex = OffColorEntry.Text?.Trim();

        if (!string.IsNullOrWhiteSpace(primaryHex) && !string.IsNullOrWhiteSpace(offHex))
        {
            try
            {
                UpdateTheme(primaryHex, offHex);
            }
            catch (Exception ex)
            {
                consoleManager.AddToConsole($"Invalid color format: {ex.Message}", Colors.Red);
            }
        }
        else
        {
            consoleManager.AddToConsole("Please enter both primary and off colors.", Colors.Orange);
        }
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
