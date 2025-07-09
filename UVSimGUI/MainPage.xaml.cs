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
using Microsoft.Maui.Storage;
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
    private const int MaxLines = 100;
    private bool _isUpdating = false;
    private bool _isScrolling = false;
    private double _lastScrollPosition = 0;
    public MainPage()
    {
        InitializeComponent();
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
    }

    private void PopulateLineNum()
    {
        LineNumberLabel.Text = string.Join(Environment.NewLine, Enumerable.Range(1, MaxLines));
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
                AddToConsole($"Added file: {result.FileName}", Colors.Yellow);
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
    private void OnWriteClicked(object sender, EventArgs e)
    {
        string fileName = DefaultFileName;
        string appDocumentsPath = FileSystem.AppDataDirectory;
        string fullPath = Path.Combine(appDocumentsPath, fileName);

        try
        {
            File.WriteAllText(fullPath, InstructionsEditor.Text);
            AddToConsole($"Editor successfully written to {fullPath}", Colors.White);
        }
        catch (Exception ex)
        {
            AddToConsole(ex.Message, Colors.Red);
        }
    }
    private async void OnCompileClicked(object sender, EventArgs e)
    {
        try
        {
            string[] lines = InstructionsEditor.Text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            UVSim.LoadArray(lines);
            Compiled = true;
            AddToConsole("Compiled!", Colors.White);
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
            AddToConsole("Run Success!", Colors.White);
        }
        catch (Exception ex)
        {
            AddToConsole(ex.Message, Colors.Red);
        }
    }

    private void AddToConsole(string message, Color textColor)
    {
        Label newLabel = new Label
        {
            Text = message,
            TextColor = textColor,
            FontSize = 14
        };
        MockConsole.Add(newLabel);
    }
}

public class FileDisplay
{
    public string FileName { get; set; }
    public string FullPath { get; set; }

    public FileDisplay(FileResult file)
    {
        FileName = file.FileName;
        FullPath = file.FullPath;
    }
}
