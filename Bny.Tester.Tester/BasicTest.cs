
using Bny.Tester;

class BasicTest
{
    public static void TestAll()
    {
        Tester t = new(nameof(BasicTest));
        t.Test(Test_Tester);
    }

    public static void Test_Tester(Asserter a)
    {
        Tester t = new(nameof(Test_Tester));
        StringWriter sw = new();
        t.Out = sw;

        a.Assert(t.Test(Function1));
        a.Assert(!t.Test(Function2));
        a.Assert(!t.Test(Function3));
        a.Assert(!t.Test(Function4));
        a.Assert(t.Count() == 7);
        a.Assert(t.Where(p => p.Success).Count() == 3);
        a.Assert(t.Where(p => p.Call == "3 < 5").Count() == 2);
        a.Assert(t.Where(p => p.Call == "ArrayTypeMismatchException").Count() == 1);

        var laArr = Enum.GetValues<LogAmount>().Select(p =>
        {
            Tester t = new("Test_Tester_LogAmount");
            StringWriter sw = new();
            t.Out = sw;

            t.Test(Function1, p);
            t.Test(Function2, p);
            t.Test(Function3, p);
            t.Test(Function4, p);

            return (p, sw.ToString().Length);
        }).ToArray();

        int minlen = laArr.First(p => p.p == LogAmount.Minimal).Length;
        int deflen = laArr.First(p => p.p == LogAmount.Default).Length;
        int alllen = laArr.First(p => p.p == LogAmount.All).Length;

        a.Assert(minlen < deflen);
        a.Assert(deflen < alllen);

        static void Function1(Asserter a)
        {
            a.Assert(3 < 5);
            a.Assert(4 < 5);
        }

        static void Function2(Asserter a)
        {
            a.Assert(3 < 5);
            a.Assert(9 < 5);
        }

        static void Function3(Asserter a)
        {
            a.Assert(3 > 5);
            a.Assert(9 < 5);
        }

        static void Function4(Asserter a)
        {
            throw new ArrayTypeMismatchException();
        }
    }
}