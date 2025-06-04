
public class CPU 
{
    private MainMemory memory;
    private int Counter;

    public CPU(MainMemory mem)
    {
        memory = mem;
        Counter = 0;
    }
    //Ask for Input, Store in the memory location of the last 2 digits following the "+10xx" 
    public void Read(int address)
    {
        //Get Input
        Console.Write($"Enter a value for memory location {address:D2}: ");
        int input;

        //Repeatedly ask if input is invalid
        while (!int.TryParse(Console.ReadLine(), out input))
        {
            Console.Write("Enter a VALID Memory Integer: ");
        }

        //Store Input (input) in memory (address)
        memory.SetWord(address, input);

        //Increment
        Counter++;
    }
    //Output value of the address after "+11xx"
    public void Write(int address)
    {
        //Get Value at (address) & Output
        int value = memory.GetWord(address);
        Console.WriteLine($"Output from memory {address:D2}: {value}");

        //Increment
        Counter++;
    }
    public void Load() 
    {

    }
    public void Store()
    {

    }
    public void Add() 
    {

    }
    public void Subtract()
    {

    }
    public void Divide()
    {

    }
    public void Multiply()
    {

    }
    public void Branch()
    {

    }
    public void BranchNeg()
    {

    }
    public void BranchZero()
    {

    }
    public void Halt()
    {

    }
}

