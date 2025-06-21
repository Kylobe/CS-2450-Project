namespace UVSimClassLib;

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
            int curVal = value;
            while (Math.Abs(curVal) > 9999)
            {
                curVal -= 10000;
            }
            string valStr = Math.Abs(curVal).ToString().PadLeft(4, '0');
            FirstHalf = valStr.Substring(0, 2);
            SecondHalf = valStr.Substring(2, 2);
            regVal = curVal;
        }
    }
    public Register(string value)
    {
        RegVal = int.Parse(value);
    }
    public override string ToString()
    {
        return RegVal.ToString();
    }
}