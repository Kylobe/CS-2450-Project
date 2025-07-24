using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVSimClassLib
{
    public class ConsoleManager
    {
        private VerticalStackLayout mockConsole;
        private ScrollView consoleScrollView;
        public ConsoleManager(VerticalStackLayout mockConsole, ScrollView consoleScrollView)
        {
            this.mockConsole = mockConsole;
            this.consoleScrollView = consoleScrollView;
        }
        public async void AddToConsole(string message, Color textColor)
        {
            Label newLabel = new Label
            {
                Text = message,
                TextColor = textColor,
                FontSize = 14
            };
            mockConsole.Add(newLabel);
            await Task.Delay(50);
            await consoleScrollView.ScrollToAsync(mockConsole, ScrollToPosition.End, false);
        }
        public async Task<int> GetUserInputAsync()
        {
            var tcs = new TaskCompletionSource<int>();

            Entry inputEntry = new Entry()
            {
                FontSize = 16,
                Placeholder = "waiting for user input",
            };

            mockConsole.Children.Add(inputEntry);
            if (consoleScrollView.Height < mockConsole.Height)
            {
                await Task.Delay(50);
                await consoleScrollView.ScrollToAsync(inputEntry, ScrollToPosition.End, false);
            }

            inputEntry.Completed += (sender, e) =>
            {
                if (int.TryParse(inputEntry.Text, out int result))
                {
                    tcs.TrySetResult(result);
                }
                else
                {
                    var errorLabel = new Label
                    {
                        Text = "Invalid input. Please enter an integer.",
                        TextColor = Colors.Red,
                        FontSize = 14
                    };
                    mockConsole.Children.Add(errorLabel);
                }
            };

            int input = await tcs.Task;

            return input;
        }
    }
}
