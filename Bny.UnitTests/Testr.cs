using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Bny.UnitTests;

public class Testr : IEnumerable<Assertion>
{
    public TextWriter Out { get; set; } = Console.Out;
    public Asserter Asserter { get; init; } = new();
    public string Name { get; init; }
    List<Assertion> Assertions { get; init; } = new();
    public bool Formatted { get; init; } = true;
    public bool Success { get; private set; } = true;

    readonly string _success;
    readonly string _failure;
    readonly string _excepts;

    public Testr(string name, bool formatted = true)
    {
        Name = name;
        Formatted = formatted;

        _success = Formatted ? "[\u001b[92msuccess\u001b[0m]" : "[success]";
        _failure = Formatted ? "[\u001b[91mfailure\u001b[0m]" : "[failure]";
        _excepts = Formatted ? "[\u001b[101m\u001b[30mexcepts\u001b[0m] \u001b[93m" : "[excepts] ";
    }

    public bool Test(TestFunction tf, LogAmount logAmount = LogAmount.Default, [CallerArgumentExpression(nameof(tf))] string caller = "")
    {
        Asserter.Clear(caller);

        bool exs = true;
        try
        {
            tf(Asserter);

            Out.WriteLine(Asserter.Success
                ? $"{_success} {Name}.{caller}"
                : $"{_failure} {Name}.{caller}"
            );
        }
        catch (Exception ex)
        {
            Out.WriteLine($"{_excepts}{ex.GetType().Name}{(Formatted ? "\x1b[0m" : "")} {Name}.{caller}");
            var frame = new StackTrace(ex).GetFrame(0);
            Asserter.Assert(false, ex.GetType().Name, frame is null ? 0 : frame.GetFileLineNumber(), frame?.GetFileName() ?? "");
            exs = false;
        }

        bool success = Asserter.Success && exs;
        Success &= success;

        Assertions.AddRange(Asserter);

        if (logAmount == LogAmount.Minimal || (logAmount == LogAmount.Default && success))
            return success;

        foreach (var a in Asserter)
            Out.WriteLine($"  {a.ToString(Formatted ? "F" : null, null)}");

        return success;
    }

    public IEnumerator<Assertion> GetEnumerator() => Assertions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Assertions.GetEnumerator();

    public static Testr Test<T>(LogAmount logAmount = LogAmount.Default, TextWriter? @out = null, bool formatted = true)
        => Test(typeof(T), logAmount, @out, formatted);

    public static Testr Test(Type test, LogAmount logAmount = LogAmount.Default, TextWriter? @out = null, bool formatted = true)
    {
        Testr t = new(test.Name, formatted);
        if (@out is not null)
            t.Out = @out;

        string format = formatted ? "[\x1b[101m\x1b[30merrored\x1b[0m] \x1b[93{0}\x1b[0m invalid signature" : "[errored] {0} invalid sigmature";

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
                t.Out.WriteLine(format, m.Name);
                t.Assertions.Add(new(false, m.Name, 0, ""));
                t.Success = false;
                continue;
            }

            t.Test(tf, logAmount, caller: m.Name);
        }

        return t;
    }
}
