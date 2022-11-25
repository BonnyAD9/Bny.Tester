namespace Bny.UnitTests;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
public class UnitTestAttribute : Attribute { }
