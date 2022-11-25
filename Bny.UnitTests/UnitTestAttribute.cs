namespace Bny.UnitTests;

/// <summary>
/// Marks types with tests / tests within the types
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class UnitTestAttribute : Attribute { }
