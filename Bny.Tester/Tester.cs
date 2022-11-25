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

    public bool Test(TestFunction tf, LogAmount logAmount = LogAmount.Default, [CallerArgumentExpression(nameof(tf))] string name = "")
    {
        Asserter.Clear(name);
        tf(Asserter);
        Assertions.AddRange(Asserter);

        Out.WriteLine(Asserter.Success
            ? $"[\x1b[92msuccess\x1b[0m] {Name}.{name}"
            : $"[\x1b[91mfailure\x1b[0m] {Name}.{name}"
        );

        if (logAmount == LogAmount.Minimal || (logAmount == LogAmount.Default && Asserter.Success))
            return Asserter.Success;

        foreach (var a in Asserter)
            Out.WriteLine($"  {a}");

        return Asserter.Success;
    }

    public IEnumerator<Assertion> GetEnumerator() => Assertions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Assertions.GetEnumerator();
}
