using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Bny.UnitTests;

/// <summary>
/// Asserts statements and saves the results
/// </summary>
public class Asserter : IEnumerable<Assertion>
{
    List<Assertion> Assertions { get; } = new();

    /// <summary>
    /// Shows whether the assert was true
    /// </summary>
    public bool Success { get; private set; } = true;

    internal string Caller { get; private set; } = "";

    /// <summary>
    /// Asserts the given statment and saves info about the assertion
    /// </summary>
    /// <param name="assertion">The assertion</param>
    /// <param name="call">The assertion string, this is set automatically</param>
    /// <param name="lineNumber">Line number of the assertion, this is set automatically</param>
    /// <param name="file">File in which the assertion is, this is set automatically</param>
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

    /// <summary>
    /// Gets the enumerator to enumerate the assertions
    /// </summary>
    /// <returns>Enumerator for the assertions</returns>
    public IEnumerator<Assertion> GetEnumerator() => Assertions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Assertions.GetEnumerator();
}
