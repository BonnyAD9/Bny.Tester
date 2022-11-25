namespace Bny.UnitTests;

/// <summary>
/// Represents assertion and stores additional information about it
/// </summary>
public readonly struct Assertion : IFormattable
{
    /// <summary>
    /// Assertion value
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// What was asserted
    /// </summary>
    public string Call { get; init; }

    /// <summary>
    /// Line number of the assertion, 0 if unknown
    /// </summary>
    public int LineNumber { get; init; }

    /// <summary>
    /// File in which the assertion is, empty if unknown
    /// </summary>
    public string File { get; init; }

    /// <summary>
    /// Function where it is asserted
    /// </summary>
    public string Caller { get; init; } = "";

    /// <summary>
    /// Creates the assertion with its metadata
    /// </summary>
    /// <param name="success">The assertion value</param>
    /// <param name="call">What was asserted</param>
    /// <param name="lineNumber">Line number of the assertion, 0 if unknown</param>
    /// <param name="file">File in which the assertion is, empty if unknown</param>
    public Assertion(bool success, string call, int lineNumber, string file)
    {
        Success = success;
        Call = call;
        LineNumber = lineNumber;
        File = file;
    }

    /// <summary>
    /// Returns unformatted assertion summary
    /// </summary>
    /// <returns>Unformatted assertion summary</returns>
    public override string ToString() => Success
        ? $"[success] {Call}"
        : $"[failure] {Call} (in {File}:{LineNumber}::)";

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider) => format switch
    {
        "F" => Success
            ? $"[{Color.Green}success{Color.Reset}] {Color.Yellow}{Call}{Color.Reset}"
            : $"[{Color.Red}failure{Color.Reset}] {Color.Yellow}{Call}{Color.Reset} (in {File}:{LineNumber}::)",
        _ => ToString()
    };
}
