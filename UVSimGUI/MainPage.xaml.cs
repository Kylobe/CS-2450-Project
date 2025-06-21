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
            Label newLabel = new Label 
            { 
                Text = ex.Message,
                // You can add other properties if needed
                TextColor = Colors.Red,
                FontSize = 14
            };
            MockConsole.Children.Add(newLabel);
        }
    }

    private void OnEditorChanged(object sender, TextChangedEventArgs e)
    {
        Compiled = false;
    }
    private void OnWriteClicked(object sender, EventArgs e)
    {
        // Call a write method
    }
    private async void OnCompileClicked(object sender, EventArgs e)
    {
        Console.WriteLine("Compiling...");
        try
        {
            string[] lines = InstructionsEditor.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            UVSim.LoadArray(lines);
            Compiled = true;
            Console.WriteLine("Compiled!");
            Label newLabel = new Label 
            { 
                Text = "Compile Success!",
                // You can add other properties if needed
                TextColor = Colors.White,
                FontSize = 14
            };
            MockConsole.Children.Add(newLabel);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Label newLabel = new Label 
            { 
                Text = ex.Message,
                TextColor = Colors.Red,
                FontSize = 14
            };
            MockConsole.Children.Add(newLabel);
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
            Label newLabel = new Label 
            { 
                Text = "Run Success!",
                // You can add other properties if needed
                TextColor = Colors.White,
                FontSize = 14
            };
            MockConsole.Children.Add(newLabel);
        }
        catch(Exception ex)
        {
            Label newLabel = new Label 
            { 
                Text = ex.Message,
                // You can add other properties if needed
                TextColor = Colors.Red,
                FontSize = 14
            };
            MockConsole.Add(newLabel);
        }
    }
}