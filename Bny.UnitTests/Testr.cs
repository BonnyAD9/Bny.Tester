using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Bny.UnitTests;

public class Testr : IEnumerable<Assertion>
{
    public TextWriter Out { get; set; } = Console.Out;
    public Asserter Asserter { get; init; } = new();
    public string Name { get; init; }
    List<Assertion> Assertions { get; init; } = new();

    public Testr(string name)
    {
        Name = name;
    }

    public bool Test(TestFunction tf, LogAmount logAmount = LogAmount.Default, [CallerArgumentExpression(nameof(tf))] string caller = "")
    {
        Asserter.Clear(caller);

        bool exs = true;
        try
        {
            tf(Asserter);

            Out.WriteLine(Asserter.Success
                ? $"[\x1b[92msuccess\x1b[0m] {Name}.{caller}"
                : $"[\x1b[91mfailure\x1b[0m] {Name}.{caller}"
            );
        }
        catch (Exception ex)
        {
            Out.WriteLine($"[\x1b[101m\x1b[30mexcepts\x1b[0m] \x1b[93m{ex.GetType().Name}\x1b[0m {Name}.{caller}");
            var frame = new StackTrace(ex).GetFrame(0);
            Asserter.Assert(false, ex.GetType().Name, frame is null ? 0 : frame.GetFileLineNumber(), frame?.GetFileName() ?? "");
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

    public static Testr Test<T>(LogAmount logAmount = LogAmount.Default, TextWriter? @out = null)
        => Test(typeof(T), logAmount, @out);

    public static Testr Test(Type test, LogAmount logAmount = LogAmount.Default, TextWriter? @out = null)
    {
        Testr t = new(test.Name);
        if (@out is not null)
            t.Out = @out;

        const BindingFlags bf = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        foreach (var m in test.GetMethods(bf).Where(p => p.GetCustomAttribute<UnitTestAttribute>() is not null))
        {
            TestFunction tf;
            try
            {
                tf = m.CreateDelegate<TestFunction>();
            }
            catch
            {
                t.Out.WriteLine($"[\x1b[101m\x1b[30merrored\x1b[0m] \x1b[93{m.Name}\x1b[0m invalid signature");
                t.Assertions.Add(new(false, m.Name, 0, ""));
                continue;
            }

            t.Test(tf, logAmount, caller: m.Name);
        }

        return t;
    }
}
