using System.Collections;
using System.Diagnostics;
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

        bool exs = true;
        try
        {
            tf(Asserter);

            Out.WriteLine(Asserter.Success
                ? $"[\x1b[92msuccess\x1b[0m] {Name}.{name}"
                : $"[\x1b[91mfailure\x1b[0m] {Name}.{name}"
            );
        }
        catch (Exception ex)
        {
            Out.WriteLine($"[\x1b[101m\x1b[30mexcepts\x1b[0m] \x1b[93m{ex.GetType().Name}\x1b[0m {Name}.{name}");
            var frame = new StackTrace(ex).GetFrame(0);
            Asserter.Assert(false, ex.GetType().Name, frame is null ? -1 : frame.GetFileLineNumber(), frame?.GetFileName() ?? "");
            exs = false;
        }

        Assertions.AddRange(Asserter);

        if (logAmount == LogAmount.Minimal || (logAmount == LogAmount.Default && Asserter.Success && exs))
            return Asserter.Success && exs;

        foreach (var a in Asserter)
            Out.WriteLine($"  {a}");

        return Asserter.Success && exs;
    }

    public IEnumerator<Assertion> GetEnumerator() => Assertions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Assertions.GetEnumerator();
}
