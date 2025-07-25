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
            while (Math.Abs(curVal) > 999999)
            {
                curVal -= 1000000;
            }
            string valStr = Math.Abs(curVal).ToString();
            FirstHalf = valStr.Substring(0, valStr.Length / 2).PadLeft(3,'0');
            SecondHalf = valStr.Substring(valStr.Length / 2, valStr.Length / 2).PadLeft(3, '0');
            regVal = curVal;
        }
    }
    public Register(string value)
    {
        RegVal = int.Parse(value);
    }
    public override string ToString()
    {
        if (RegVal > 0)
        {
            return "+" + FirstHalf + SecondHalf;
        }
        return "-" + FirstHalf + SecondHalf;
    }
}