using System.Text.RegularExpressions;

namespace UVSimClassLib;

public class Register
{
    public string FirstHalf { get; set; } 
    public string SecondHalf { get; set; }
    private int regVal;
    private string valStr;
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
            regVal = curVal;
            valStr = FirstHalf + SecondHalf;
        }
    }
    public Register(string value)
    {
        value = Regex.Replace(value, @"[+-]", "");
        FirstHalf = value.Substring(0, (value.Length / 2) + (value.Length % 2) ).PadLeft(3,'0');
        SecondHalf = value.Substring((value.Length / 2) + (value.Length % 2) , value.Length / 2 );
        RegVal = int.Parse(value);
    }
    public override string ToString()
    {
        if (RegVal < 0)
        {
            return "-" + valStr;
        }
        return "+" + valStr;
    }
}