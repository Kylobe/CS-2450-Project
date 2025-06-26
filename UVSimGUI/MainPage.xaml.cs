using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Foundation;
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
    public ObservableCollection<FileResult> Files = new ObservableCollection<FileResult>();
    const string DefaultFileName = "CustomBasicML.txt";
    bool Compiled = false;
    UVSim UVSim = new UVSim();
    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnLoadClicked(object sender, EventArgs e)
    {
        var customFileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { "public.plain-text" } },        // UTType for plain text
                { DevicePlatform.Android, new[] { "text/plain" } },           // MIME type
                { DevicePlatform.WinUI, new[] { ".txt" } },                   // File extension
                { DevicePlatform.Tizen, new[] { "text/plain" } },             // MIME type
                { DevicePlatform.macOS, new[] { "public.plain-text" } },      // UTType
                { DevicePlatform.MacCatalyst, new[] { "public.plain-text" } } // Critical addition for MacCatalyst
            });

        PickOptions options = new()
        {
            PickerTitle = "Please select a text file",
            FileTypes = customFileType,
        };
        try
        {
            FileResult result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                if (result.FileName.EndsWith("txt", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream = await result.OpenReadAsync();
                    Files.Add(result);
                    using var reader = new StreamReader(stream);
                    InstructionsEditor.Text = await reader.ReadToEndAsync();
                    Compiled = false;
                }
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
        Console.WriteLine("Compiling...");
        try
        {
            string[] lines = InstructionsEditor.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
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
            {
                throw new Exception("Please compile before run");
            }
            UVSim.Run(MockConsole);
            AddToConsole("Run Success!", Colors.White);
        }
        catch(Exception ex)
        {
            AddToConsole(ex.Message, Colors.Red);
        }
    }

    private void AddToConsole(string message, Color textColor)
    {
        Label newLabel = new Label 
        { 
            Text = message,
            // You can add other properties if needed
            TextColor = textColor,
            FontSize = 14
        };
        MockConsole.Add(newLabel);
    }
}