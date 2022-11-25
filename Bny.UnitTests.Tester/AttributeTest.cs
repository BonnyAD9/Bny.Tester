using Bny.UnitTests;

class AttributeTest
{
    [UnitTest]
    public static void Test_UnitTestAttribute(Asserter a)
    {
        Testr t = Testr.Test<TestClass>(@out: new StringWriter());

        a.Assert(t.Count() == 7);
        a.Assert(t.Where(p => p.Success).Count() == 3);
        a.Assert(t.Where(p => p.Call == "3 < 5").Count() == 2);
        a.Assert(t.Where(p => p.Call == "ArrayTypeMismatchException").Count() == 1);

        var laArr = Enum.GetValues<LogAmount>().Select(p =>
        {
            StringWriter sw = new();
            Testr.Test<TestClass>(p, sw);
            return (p, sw.ToString().Length);
        }).ToArray();

        int minlen = laArr.First(p => p.p == LogAmount.Minimal).Length;
        int deflen = laArr.First(p => p.p == LogAmount.Default).Length;
        int alllen = laArr.First(p => p.p == LogAmount.All).Length;

        a.Assert(minlen < deflen);
        a.Assert(deflen < alllen);

        t = Testr.Test<WrongTestClass>(@out: new StringWriter());

        a.Assert(t.Count(p => p.Call == "WrongTestFunction") == 1);
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
        public void WrongTestFunction() { }
    }
}


