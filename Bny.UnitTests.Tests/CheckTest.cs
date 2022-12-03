namespace Bny.UnitTests.Tests;

[UnitTest]
internal class CheckTest
{
    [UnitTest]
    public static void Test_AllNeighbours(Asserter a)
    {
        int value = Random.Shared.Next(ushort.MaxValue);
        var arr = TestData.Generate(100, value);

        a.Assert(Check.AllNeighbours(arr, (l, r) => l == r));

        arr[20] = value + Random.Shared.Next();

        a.Assert(!Check.AllNeighbours(arr, (l, r) => l != r));
    }

    [UnitTest]
    public static void Test_AnyNeighbour(Asserter a)
    {
        int value = Random.Shared.Next(ushort.MaxValue);
        var arr = TestData.Generate(100, i => i + value);

        a.Assert(!Check.AnyNeighbour(arr, (l, r) => l == r));

        arr[20] = 21 + value;

        a.Assert(Check.AnyNeighbour(arr, (l, r) => l == r));
    }

    [UnitTest]
    public static void Test_AreSame_Default(Asserter a)
    {
        int value = Random.Shared.Next(ushort.MaxValue);
        var arr = TestData.Generate(100, i => i + value);

        a.Assert(Check.AreSame(arr, arr));
        a.Assert(!Check.AreSame(arr[1..], arr[..^1]));
        a.Assert(!Check.AreSame(arr[..^1], arr));
        a.Assert(!Check.AreSame(arr, arr[..^1]));
    }

    [UnitTest]
    public static void TestAreSame_Selector(Asserter a)
    {
        int value = Random.Shared.Next(ushort.MaxValue);
        var arr = TestData.Generate(100, i => i + value);

        a.Assert( Check.AreSame(arr       , arr      , n => n % 7));
        a.Assert(!Check.AreSame(arr[1..  ], arr[..^1], n => n % 7));
        a.Assert(!Check.AreSame(arr[ ..^1], arr      , n => n % 7));
        a.Assert(!Check.AreSame(arr       , arr[..^1], n => n % 7));
        a.Assert( Check.AreSame(arr[7..  ], arr[..^7], n => n % 7));
    }

    [UnitTest]
    public static void TestAreSame_Comparer(Asserter a)
    {
        int value = Random.Shared.Next(ushort.MaxValue);
        var arr = TestData.Generate(100, i => i + value);

        a.Assert( Check.AreSame(arr       , arr      , (a, b) => a   == b));
        a.Assert(!Check.AreSame(arr[1..  ], arr[..^1], (a, b) => a   == b));
        a.Assert(!Check.AreSame(arr[ ..^1], arr      , (a, b) => a   == b));
        a.Assert(!Check.AreSame(arr       , arr[..^1], (a, b) => a   == b));
        a.Assert( Check.AreSame(arr[7..  ], arr[..^7], (a, b) => a%7 == b%7));

        var strArr = TestData.Generate(100, i => (i + value).ToString());

        a.Assert(Check.AreSame(arr, strArr, (a, b) => a.ToString() == b));
    }
}
