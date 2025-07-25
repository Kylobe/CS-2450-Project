namespace UVSimClassLib;

public class TraversableRegister : Register
{
    public TraversableRegister? Next { get; set; } = null;
    public TraversableRegister? Prev { get; set; } = null;
    public TraversableRegister(string num) : base(num) { }
}