namespace UVSimClassLib;

public class CPU
{
    TraversableRegister[] MainMemory { get; set; }
    public Register Accumulator { get; set; }

    public CPU(TraversableRegister[] mainMemory, Register accumulator)
    {
        MainMemory = mainMemory;
        Accumulator = accumulator;
    }
    // all commands
    public async Task Read(int memoryAddress, VerticalStackLayout mockConsole)
    {
        int input = await GetUserInputAsync(mockConsole);
        //Store in memory 
        MainMemory[memoryAddress].RegVal = input;

    }
    public void Write(int memoryAddress, VerticalStackLayout mockConsole)
    {
        //Output Value of the given address
        int value = MainMemory[memoryAddress].RegVal;
        Label newLabel = new Label 
        { 
            Text = $"Value of memory {memoryAddress:D2}: {value}",
            TextColor = Colors.White,
            FontSize = 14
        };
        mockConsole.Children.Add(newLabel);
    }
    public void Load(int memoryAddress) 
    {
        Accumulator.RegVal = MainMemory[memoryAddress].RegVal;
    }
    public void Store(int memoryAddress)
    {
        MainMemory[memoryAddress].RegVal = Accumulator.RegVal;
    }
    public void Add(int memoryAddress)
    {
        int newVal = Accumulator.RegVal + MainMemory[memoryAddress].RegVal;
        Accumulator.RegVal = newVal;
    }
    public void Subtract(int memoryAddress)
    {
        int newVal = Accumulator.RegVal - MainMemory[memoryAddress].RegVal;
        Accumulator.RegVal = newVal;        
    }
    public void Divide(int memoryAddress)
    {
        int newVal = Accumulator.RegVal / MainMemory[memoryAddress].RegVal;
        Accumulator.RegVal = newVal;
    }
    public void Multiply(int memoryAddress)
    {
        int newVal = Accumulator.RegVal * MainMemory[memoryAddress].RegVal;
        Accumulator.RegVal = newVal;
    }
    public TraversableRegister Branch(TraversableRegister currentRegister)
    {
        TraversableRegister jumpedRegister = MainMemory[int.Parse(currentRegister.SecondHalf)];
        if (jumpedRegister != null && jumpedRegister.Prev != null)
        {
            currentRegister = jumpedRegister.Prev;
        }
        return currentRegister;
    }
    public TraversableRegister BranchNeg(TraversableRegister currentRegister)
    {
        if (Accumulator.RegVal < 0)
        {
            TraversableRegister jumpedRegister = MainMemory[int.Parse(currentRegister.SecondHalf)];
            if (jumpedRegister != null && jumpedRegister.Prev != null)
            {
                currentRegister = jumpedRegister.Prev;
            }
        }
        return currentRegister;
    }
    public TraversableRegister BranchZero(TraversableRegister currentRegister)
    {
        if (Accumulator.RegVal == 0)
        {
            TraversableRegister jumpedRegister = MainMemory[int.Parse(currentRegister.SecondHalf)];
            if (jumpedRegister != null && jumpedRegister.Prev != null)
            {
                currentRegister = jumpedRegister.Prev;
            }
        }
        return currentRegister;
    }
    public TraversableRegister Halt(TraversableRegister currentRegister)
    {
        currentRegister.Next = null;
        return currentRegister;
    }

    private async Task<int> GetUserInputAsync(VerticalStackLayout mockConsole)
    {
        var tcs = new TaskCompletionSource<int>();

        Entry inputEntry = new Entry()
        {
            FontSize = 16,
            Placeholder = "waiting for user input",
        };
        
        mockConsole.Children.Add(inputEntry);
        
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