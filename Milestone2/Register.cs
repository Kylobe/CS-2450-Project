namespace Milestone2;

public class Register
{
    public string FirstHalf { get; set; } 
    public string SecondHalf { get; set; }
    private int regVal;
    public int RegVal
    {
        get
        {
            return regVal;
        }
        set
        {
            string valStr = Math.Abs(value).ToString().PadLeft(4, '0');
            FirstHalf = valStr.Substring(0, 2);
            SecondHalf = valStr.Substring(2, 2);
        }
    }
    public Register(string value)
    {
        RegVal = int.Parse(value);
    }
}