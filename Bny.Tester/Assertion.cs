namespace Bny.Tester;

public readonly struct Assertion
{
    public bool Success { get; init; }
    public string Call { get; init; }
    public int LineNumber { get; init; }
    public string File { get; init; }

    public Assertion(bool success, string call, int lineNumber, string file)
    {
        Success = success;
        Call = call;
        LineNumber = lineNumber;
        File = file;
    }

    public override string ToString() => Success
        ? $"[\x1b[92msuccess\x1b[0m] \x1b[93m{Call}\x1b[0m"
        : $"[\x1b[91mfailure\x1b[0m] \x1b[93m{Call}\x1b[0m (in {File}:{LineNumber}::)";
}
