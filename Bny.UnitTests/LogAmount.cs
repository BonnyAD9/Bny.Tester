namespace Bny.UnitTests;

/// <summary>
/// Represents the amount of information to show
/// </summary>
public enum LogAmount
{
    /// <summary>
    /// Show detailed assertions only if the test fails
    /// </summary>
    Default,

    /// <summary>
    /// Don't show detailed assertions
    /// </summary>
    Minimal,

    /// <summary>
    /// Show all detailed assertions
    /// </summary>
    All,
}
