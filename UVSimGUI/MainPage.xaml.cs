using System;
using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;

namespace UVSimGUI;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    private void OnLoadClicked(object sender, EventArgs e)
    {
        // Implement file loading logic
    }

    private void OnWriteClicked(object sender, EventArgs e)
    {
        // Implement file writing logic from instructions
    }

    private void OnCompileClicked(object sender, EventArgs e)
    {
        // Call your compile method
    }

    private void OnRunClicked(object sender, EventArgs e)
    {
        // Call your run method
    }

    private void OnSaveInstructionsClicked(object sender, EventArgs e)
    {
        // Save instructions if needed
    }
}