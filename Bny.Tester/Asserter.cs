using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bny.Tester;

public class Asserter : IEnumerable<Assertion>
{
    List<Assertion> Assertions { get; } = new();

    public bool Success { get; private set; } = true;

    internal string Caller { get; private set; } = "";

    [EditorBrowsable]
    public void Assert(bool assertion,
        [CallerArgumentExpression(nameof(assertion))] string call = "",
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string file = "")
    {
        Assertions.Add(new(assertion, call, lineNumber, file) { Caller = Caller });
        Success &= assertion;
    }


    internal void Clear(string newCaller)
    {
        Caller = newCaller;
        Assertions.Clear();
        Success = true;
    }

    public IEnumerator<Assertion> GetEnumerator() => Assertions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Assertions.GetEnumerator();
}
