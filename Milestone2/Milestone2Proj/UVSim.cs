namespace Milestone2;

public class UVSim
{
    public CPU CPU { get; set; } = new CPU();
    public void Load()
    {
        // read file into Main memory
    }

    public void Run()
    {
        for (int i = 0; i < MainMemory.Length; i++)
        {
            Register currentRegister = MainMemory[i] ?? new Register();
            if (currentRegister.FirstHalf == "10")
            {
                // CPU.Read(SecondHalf);
            }

            if (currentRegister.FirstHalf == "11")
            {
                // CPU.Write(SecondHalf);
            }

            if (currentRegister.FirstHalf == "20")
            {
                // CPU.Load(SecondHalf, accumulator);
            }

            if (currentRegister.FirstHalf == "21")
            {
                // CPU.Store(SecondHalf, accumulator);
            }

            if (currentRegister.FirstHalf == "30")
            {
                CPU.Add(int.Parse(currentRegister.SecondHalf));
            }

            if (currentRegister.FirstHalf == "31")
            {
                CPU.Subtract(int.Parse(currentRegister.SecondHalf));
            }

            if (currentRegister.FirstHalf == "32")
            {
                CPU.Divide(int.Parse(currentRegister.SecondHalf));
            }

            if (currentRegister.FirstHalf == "33")
            {
                CPU.Multiply(int.Parse(currentRegister.SecondHalf));
            }

            if (currentRegister.FirstHalf == "40")
            {
                // CPU.Branch(SecondHalf);
            }

            if (currentRegister.FirstHalf == "41")
            {
                // CPU.BranchNeg(SecondHalf, accumulator);
            }

            if (currentRegister.FirstHalf == "42")
            {
                // CPU.BranchZero(SecondHalf, accumulator);
            }

            if (currentRegister.FirstHalf == "43")
            {
                // CPU.Halt();
            }
        }
    }
    static Main[] ()
    {
        Run();
    }
}