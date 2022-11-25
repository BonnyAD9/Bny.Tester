namespace Bny.UnitTests;

public readonly struct Assertion : IFormattable
{
    public bool Success { get; init; }
    public string Call { get; init; }
    public int LineNumber { get; init; }
    public string File { get; init; }
    public string Caller { get; init; } = "";

    public Assertion(bool success, string call, int lineNumber, string file)
    {
        Success = success;
        Call = call;
        LineNumber = lineNumber;
        File = file;
    }

    public override string ToString() => Success
        ? $"[success] {Call}"
        : $"[failure] {Call} (in {File}:{LineNumber}::)";

    public string ToString(string? format, IFormatProvider? formatProvider) => format switch
    {
        "F" => Success
            ? $"[\x1b[92msuccess\x1b[0m] \x1b[93m{Call}\x1b[0m"
            : $"[\x1b[91mfailure\x1b[0m] \x1b[93m{Call}\x1b[0m (in {File}:{LineNumber}::)",
        _ => ToString()
    };
}
