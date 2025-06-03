namespace Milestone2;

public class TraversableRegister : Register
{
    public TraversableRegister? Next { get; set; } = null;
    public TraversableRegister(string num) : base(num) { }
}