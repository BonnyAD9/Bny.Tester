namespace Bny.UnitTests.Tests;

[UnitTest]
public class AttributeTest
{
    [UnitTest]
    public static void Test_UnitTestAttribute(Asserter a)
    {
        Tester t = Tester.Test<TestClass>(@out: new StringWriter(), formatted: false);

        a.Assert(t.Count() == 7);
        a.Assert(t.Count(p => p.Success) == 3);
        a.Assert(t.Count(p => p.Call == "3 < 5") == 2);
        a.Assert(t.Count(p => p.Call == "ArrayTypeMismatchException") == 1);

        var laArr = Enum.GetValues<LogAmount>().Select(p =>
        {
            StringWriter sw = new();
            Tester.Test<TestClass>(p, sw, formatted: false);
            return (p, sw.ToString().Length);
        }).ToArray();

        int minlen = laArr.First(p => p.p == LogAmount.Minimal).Length;
        int deflen = laArr.First(p => p.p == LogAmount.Default).Length;
        int alllen = laArr.First(p => p.p == LogAmount.All).Length;

        a.Assert(minlen < deflen);
        a.Assert(deflen < alllen);

        t = Tester.Test<WrongTestClass>(@out: new StringWriter(), formatted: false);

        a.Assert(t.Count(p => p.Call == "WrongTestFunction") == 1);
        a.Assert(!t.Success);
    }

    class TestClass
    {
        [UnitTest]
        public static void Function1(Asserter a)
        {
            a.Assert(3 < 5);
            a.Assert(4 < 5);
        }

        [UnitTest]
        public static void Function2(Asserter a)
        {
            a.Assert(3 < 5);
            a.Assert(9 < 5);
        }

        [UnitTest]
        public static void Function3(Asserter a)
        {
            a.Assert(3 > 5);
            a.Assert(9 < 5);
        }

        [UnitTest]
        public static void Function4(Asserter a)
        {
            throw new ArrayTypeMismatchException();
        }
    }

    class WrongTestClass
    {
        [UnitTest]
        public static void WrongTestFunction() { }
    }
}


