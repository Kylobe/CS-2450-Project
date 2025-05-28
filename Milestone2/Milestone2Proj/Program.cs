namespace Milestone2;
class Program
{
    static void Main(string[] args)
    {
        UVSim sim = new UVSim();
        sim.Load();
        sim.Run();
    }
}
