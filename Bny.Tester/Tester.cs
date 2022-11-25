using System.Runtime.CompilerServices;

namespace Bny.Tester;

public class Tester
{
    public TextWriter Out { get; set; } = Console.Out;
    public Asserter Asserter { get; init; } = new();
    public string Name { get; init; }

    public Tester(string name)
    {
        Name = name;
    }

    public bool Test(TestFunction tf, [CallerArgumentExpression(nameof(tf))] string name = "")
    {
        Asserter.Clear();
        tf(Asserter);

        if (Asserter.Success)
        {
            Out.WriteLine($"[\x1b[92msuccess\x1b[0m] {Name}.{name}");
            return true;
        }

        Out.WriteLine($"[\x1b[91mfailure\x1b[0m] {Name}.{name}");
        foreach (var a in Asserter)
            Out.WriteLine($"  {a}");

        return false;
    }
}
