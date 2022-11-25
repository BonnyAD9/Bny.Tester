﻿
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
        t.Out = new StringWriter();

        a.Assert(t.Test(Test_Tester_Function1));
        a.Assert(!t.Test(Test_Tester_Function2));
        a.Assert(!t.Test(Test_Tester_Function3));
        a.Assert(t.Count() == 6);
        a.Assert(t.Where(p => p.Success).Count() == 3);
        a.Assert(t.Where(p => p.Call == "3 < 5").Count() == 2);
    }

    public static void Test_Tester_Function1(Asserter a)
    {
        a.Assert(3 < 5);
        a.Assert(4 < 5);
    }

    public static void Test_Tester_Function2(Asserter a)
    {
        a.Assert(3 < 5);
        a.Assert(9 < 5);
    }

    public static void Test_Tester_Function3(Asserter a)
    {
        a.Assert(3 > 5);
        a.Assert(9 < 5);
    }
}