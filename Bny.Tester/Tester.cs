using System.Collections;
using System.Runtime.CompilerServices;

namespace Bny.Tester;

public class Tester : IEnumerable<Assertion>
{
    public TextWriter Out { get; set; } = Console.Out;
    public Asserter Asserter { get; init; } = new();
    public string Name { get; init; }
    List<Assertion> Assertions { get; init; } = new();

    public Tester(string name)
    {
        Name = name;
    }

    public bool Test(TestFunction tf, [CallerArgumentExpression(nameof(tf))] string name = "")
    {
        Asserter.Clear(name);
        tf(Asserter);
        Assertions.AddRange(Asserter);

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

    public IEnumerator<Assertion> GetEnumerator() => Assertions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Assertions.GetEnumerator();
}
