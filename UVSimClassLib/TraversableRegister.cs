namespace UVSimClassLib;

public class TraversableRegister : Register
{
    public TraversableRegister? Next { get; set; } = null;
    public TraversableRegister? Prev { get; set; } = null;
    public TraversableRegister(string num) : base(num) { }
    public override string ToString()
    {
        if (Next != null)
        {
            return Prev.RegVal.ToString() + "<-" + base.ToString() + "->" + Next.RegVal.ToString();
        }
        return base.ToString() + "->Null";
    }
}