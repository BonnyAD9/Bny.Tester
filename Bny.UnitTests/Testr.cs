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

        _success = Formatted ? $"[{Color.Green}success{Color.Reset}]" : "[success]";
        _failure = Formatted ? $"[{Color.Red}failure{Color.Reset}]" : "[failure]";
        _excepts = Formatted ? $"[{Color.RedBg}{Color.Black}excepts{Color.Reset}] {Color.Yellow}" : "[excepts] ";
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
            Out.WriteLine($"{_excepts}{ex.GetType().Name}{(Formatted ? Color.Reset : "")} {Name}.{caller}");
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

        string format = formatted ? $"[{Color.RedBg}{Color.Black}errored{Color.Reset}] {Color.Yellow}{{0}}{Color.Reset} invalid signature" : "[errored] {0} invalid sigmature";

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

    public static Testr[] TestAll(LogAmount logAmount = LogAmount.Default, TextWriter? @out = null, bool formatted = true)
    {
        var types = (from a in AppDomain.CurrentDomain.GetAssemblies() 
                     from t in a.GetTypes()
                     where t.GetCustomAttribute<UnitTestAttribute>() is not null
                     select t).ToList();

        var tests = new Testr[types.Count];
        @out ??= Console.Out;

        for (int i = 0; i < types.Count; ++i)
            tests[i] = Test(types[i], logAmount, @out, formatted);

        return tests;
    }
}
