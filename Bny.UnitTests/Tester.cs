using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Bny.UnitTests;

/// <summary>
/// Class for unit testing
/// </summary>
public class Tester : IEnumerable<Assertion>
{
    /// <summary>
    /// The output for the results, stdout by default
    /// </summary>
    public TextWriter Out { get; set; } = Console.Out;
    internal Asserter Asserter { get; init; } = new();

    /// <summary>
    /// Name of the test group
    /// </summary>
    public string Name { get; init; }
    List<Assertion> Assertions { get; init; } = new();

    /// <summary>
    /// Determines whether the output should be formatted
    /// </summary>
    public bool Formatted { get; init; } = true;

    /// <summary>
    /// True if all tests passed
    /// </summary>
    public bool Success { get; private set; } = true;

    readonly string _success;
    readonly string _failure;
    readonly string _excepts;

    /// <summary>
    /// Creates new tester
    /// </summary>
    /// <param name="name">name of the test group</param>
    /// <param name="formatted">true if the output should be formatted, otherwise false</param>
    public Tester(string name, bool formatted = true)
    {
        Name = name;
        Formatted = formatted;

        _success = Formatted ? $"[{Color.Green}success{Color.Reset}]" : "[success]";
        _failure = Formatted ? $"[{Color.Red}failure{Color.Reset}]" : "[failure]";
        _excepts = Formatted ? $"[{Color.RedBg}{Color.Black}excepts{Color.Reset}] {Color.Yellow}" : "[excepts] ";
    }

    /// <summary>
    /// Executes the given test
    /// </summary>
    /// <param name="tf">Test to execute</param>
    /// <param name="logAmount">How much to log</param>
    /// <param name="caller">Name of the test, this is set automatically</param>
    /// <returns>True if all the assertions in the test passed</returns>
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

    /// <summary>
    /// Enumerates all the assertions
    /// </summary>
    /// <returns>Assertion enumerator</returns>
    public IEnumerator<Assertion> GetEnumerator() => Assertions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Assertions.GetEnumerator();

    /// <summary>
    /// Executes all the tests in the type
    /// </summary>
    /// <typeparam name="T">Type with the tests</typeparam>
    /// <param name="logAmount">How much to log</param>
    /// <param name="out">Test results output, null is stdout</param>
    /// <param name="formatted">True if the output should be formatted, otherwise false</param>
    /// <returns>Test with the summary</returns>
    public static Tester Test<T>(LogAmount logAmount = LogAmount.Default, TextWriter? @out = null, bool formatted = true)
        => Test(typeof(T), logAmount, @out, formatted);

    /// <summary>
    /// Executes all the tests in the type
    /// </summary>
    /// <param name="test">Type with the tests</param>
    /// <param name="logAmount">How much to log</param>
    /// <param name="out">Test results output, null is stdout</param>
    /// <param name="formatted">True if the output should be formatted, otherwise false</param>
    /// <returns></returns>
    public static Tester Test(Type test, LogAmount logAmount = LogAmount.Default, TextWriter? @out = null, bool formatted = true)
    {
        Tester t = new(test.Name, formatted);
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

    /// <summary>
    /// Executes all the tests in all the types with the UnitTestAttribute
    /// </summary>
    /// <param name="logAmount">How much to log</param>
    /// <param name="out">Test results output, null is stdout</param>
    /// <param name="formatted">True if the output should be formatted</param>
    /// <returns>Array of all the test results</returns>
    public static Tester[] TestAll(LogAmount logAmount = LogAmount.Default, TextWriter? @out = null, bool formatted = true)
    {
        var types = (from a in AppDomain.CurrentDomain.GetAssemblies() 
                     from t in a.GetTypes()
                     where t.GetCustomAttribute<UnitTestAttribute>() is not null
                     select t).ToList();

        var tests = new Tester[types.Count];
        @out ??= Console.Out;

        for (int i = 0; i < types.Count; ++i)
            tests[i] = Test(types[i], logAmount, @out, formatted);

        return tests;
    }
}
