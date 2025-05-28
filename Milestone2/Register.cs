namespace Milestone2;

public class Register
{
    public string FirstHalf { get; set; } 
    public string SecondHalf { get; set; } 
    public int Value { get; set; }
    public Register(string value = "0000")
    {
        Value = int.Parse(value);
        FirstHalf = String.Join("",value[..1]);
        SecondHalf = String.Join("",value[2..]);
    }
}