using Bny.UnitTests;

TestAll();

static void TestAll(TextWriter? @out = null, bool formatted = true)
{
    @out ??= Console.Out;
    Testr.Test<BasicTest>(@out: @out, formatted: formatted);
    Testr.Test<AttributeTest>(@out: @out, formatted: formatted);
}