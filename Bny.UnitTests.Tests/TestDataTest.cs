namespace Bny.UnitTests.Tests;

[UnitTest]
internal class TestDataTest
{
    [UnitTest]
    public static void Test_FillValue(Asserter a)
    {
        int value = Random.Shared.Next();

        int[] arr = new int[100];
        TestData.Fill(arr, value);

        bool isAll4 = true;
        foreach (var i in arr)
            isAll4 &= i == value;

        a.Assert(isAll4);
    }

    [UnitTest]
    public static void Test_FillFiller(Asserter a)
    {
        int value = Random.Shared.Next();

        int[] arr = new int[100];
        TestData.Fill(arr, i => i + value);

        bool isAllIndexPlusValue = true;
        for (int i = 0; i < arr.Length; ++i)
            isAllIndexPlusValue = arr[i] == i + value;

        a.Assert(isAllIndexPlusValue);
    }

    [UnitTest]
    public static void Test_GenerateValue(Asserter a)
    {
        int value = Random.Shared.Next();

        var arr = TestData.Generate(100, value);

        bool isAll4 = true;
        foreach (var i in arr)
            isAll4 &= i == value;

        a.Assert(isAll4);
    }

    [UnitTest]
    public static void Test_GenerateFiller(Asserter a)
    {
        int value = Random.Shared.Next();

        var arr = TestData.Generate(100, i => i + value);

        bool isAllIndexPlusValue = true;
        for (int i = 0; i < arr.Length; ++i)
            isAllIndexPlusValue = arr[i] == i + value;

        a.Assert(isAllIndexPlusValue);
    }

    [UnitTest]
    public static void Test_FillRngInt_Seed(Asserter a)
    {
        int seed = Random.Shared.Next();
        int min = Random.Shared.Next(short.MinValue, short.MaxValue);
        int max = min + Random.Shared.Next();
        
        var arr1 = new int[100];
        var arr2 = new int[100];

        TestData.FillRngInt<int>(arr1, seed, min, max);
        TestData.FillRngInt<int>(arr2, seed, min, max);

        bool isInRange          = true ;
        bool isNotAllSame       = false;
        bool sameSeedSameValues = true ;

        for (int i = 0; i < arr1.Length; ++i)
        {
            isInRange &= arr1[i] >= min && arr1[i] < max;
            isNotAllSame |= i > 1 && arr1[i - 1] != arr1[i];
            sameSeedSameValues &= arr1[i] == arr2[i];
        }

        a.Assert(isInRange);
        a.Assert(isNotAllSame);
        a.Assert(sameSeedSameValues);
    }

    [UnitTest]
    public static void Test_FillRngInt_NoSeed(Asserter a)
    {
        int min = Random.Shared.Next(short.MinValue, short.MaxValue);
        int max = min + Random.Shared.Next();

        var arr1 = new int[100];
        var arr2 = new int[100];

        TestData.FillRngInt<int>(arr1, min: min, max: max);
        TestData.FillRngInt<int>(arr2, min: min, max: max);

        bool isInRange             = true ;
        bool isNotAllSame          = false;
        bool noSeedDefferentValues = true ;

        for (int i = 0; i < arr1.Length; ++i)
        {
            isInRange &= arr1[i] >= min && arr1[i] < max;
            isNotAllSame |= i > 1 && arr1[i - 1] != arr1[i];
            noSeedDefferentValues &= arr1[i] != arr2[i];
        }

        a.Assert(isInRange);
        a.Assert(isNotAllSame);
        a.Assert(noSeedDefferentValues);
    }

    [UnitTest]
    public static void Test_FillRngFloat_Seed(Asserter a)
    {
        int seed = Random.Shared.Next();
        double min =
            (Random.Shared.NextDouble() - 0.5) * 2.0 * ushort.MaxValue;
        double max = min + Random.Shared.NextDouble() * ushort.MaxValue;

        var arr1 = new float[100];
        var arr2 = new float[100];

        TestData.FillRngFloat<float>(arr1, seed, min, max);
        TestData.FillRngFloat<float>(arr2, seed, min, max);

        bool isInRange          = true ;
        bool isNotAllSame       = false;
        bool sameSeedSameValues = true ;

        for (int i = 0; i < arr1.Length; ++i)
        {
            isInRange &= arr1[i] >= min && arr1[i] < max;
            isNotAllSame |= i > 1 && arr1[i - 1] != arr1[i];
            sameSeedSameValues &= arr1[i] == arr2[i];
        }

        a.Assert(isInRange);
        a.Assert(isNotAllSame);
        a.Assert(sameSeedSameValues);
    }

    [UnitTest]
    public static void Test_FillRngFloat_NoSeed(Asserter a)
    {
        double min =
            (Random.Shared.NextDouble() - 0.5) * 2.0 * ushort.MaxValue;
        double max = min + Random.Shared.NextDouble() * ushort.MaxValue;

        var arr1 = new float[100];
        var arr2 = new float[100];

        TestData.FillRngFloat<float>(arr1, min: min, max: max);
        TestData.FillRngFloat<float>(arr2, min: min, max: max);

        bool isInRange             = true ;
        bool isNotAllSame          = false;
        bool noSeedDifferentValues = true ;

        for (int i = 0; i < arr1.Length; ++i)
        {
            isInRange &= arr1[i] >= min && arr1[i] < max;
            isNotAllSame |= i > 1 && arr1[i - 1] != arr1[i];
            noSeedDifferentValues &= arr1[i] != arr2[i];
        }

        a.Assert(isInRange);
        a.Assert(isNotAllSame);
        a.Assert(noSeedDifferentValues);
    }

    [UnitTest]
    public static void Test_GenerateRngInt_Seed(Asserter a)
    {
        int seed = Random.Shared.Next();
        int min = Random.Shared.Next(short.MinValue, short.MaxValue);
        int max = min + Random.Shared.Next();
        
        var arr1 = TestData.GenerateRngInt<int>(100, seed, min, max);
        var arr2 = TestData.GenerateRngInt<int>(100, seed, min, max);

        bool isInRange          = true ;
        bool isNotAllSame       = false;
        bool sameSeedSameValues = true ;

        for (int i = 0; i < arr1.Length; ++i)
        {
            isInRange &= arr1[i] >= min && arr1[i] < max;
            isNotAllSame |= i > 1 && arr1[i - 1] != arr1[i];
            sameSeedSameValues &= arr1[i] == arr2[i];
        }

        a.Assert(isInRange);
        a.Assert(isNotAllSame);
        a.Assert(sameSeedSameValues);
    }

    [UnitTest]
    public static void Test_GenerateRngInt_NoSeed(Asserter a)
    {
        int min = Random.Shared.Next(short.MinValue, short.MaxValue);
        int max = min + Random.Shared.Next();

        var arr1 = TestData.GenerateRngInt<int>(100, min: min, max: max);
        var arr2 = TestData.GenerateRngInt<int>(100, min: min, max: max);

        bool isInRange             = true ;
        bool isNotAllSame          = false;
        bool noSeedDefferentValues = true ;

        for (int i = 0; i < arr1.Length; ++i)
        {
            isInRange &= arr1[i] >= min && arr1[i] < max;
            isNotAllSame |= i > 1 && arr1[i - 1] != arr1[i];
            noSeedDefferentValues &= arr1[i] != arr2[i];
        }

        a.Assert(isInRange);
        a.Assert(isNotAllSame);
        a.Assert(noSeedDefferentValues);
    }

    [UnitTest]
    public static void Test_GenerateRngFloat_Seed(Asserter a)
    {
        int seed = Random.Shared.Next();
        double min = (Random.Shared.NextDouble() - 0.5) * 2.0 * ushort.MaxValue;
        double max = min + Random.Shared.NextDouble() * ushort.MaxValue;

        var arr1 = TestData.GenerateRngFloat<float>(100, seed, min, max);
        var arr2 = TestData.GenerateRngFloat<float>(100, seed, min, max);

        bool isInRange          = true ;
        bool isNotAllSame       = false;
        bool sameSeedSameValues = true ;

        for (int i = 0; i < arr1.Length; ++i)
        {
            isInRange &= arr1[i] >= min && arr1[i] < max;
            isNotAllSame |= i > 1 && arr1[i - 1] != arr1[i];
            sameSeedSameValues &= arr1[i] == arr2[i];
        }

        a.Assert(isInRange);
        a.Assert(isNotAllSame);
        a.Assert(sameSeedSameValues);
    }

    [UnitTest]
    public static void Test_GenerateRngFloat_NoSeed(Asserter a)
    {
        double min = (Random.Shared.NextDouble() - 0.5) * 2.0 * ushort.MaxValue;
        double max = min + Random.Shared.NextDouble() * ushort.MaxValue;

        var arr1 = TestData.GenerateRngFloat<float>(100, min: min, max: max);
        var arr2 = TestData.GenerateRngFloat<float>(100, min: min, max: max);


        bool isInRange             = true ;
        bool isNotAllSame          = false;
        bool noSeedDifferentValues = true ;

        for (int i = 0; i < arr1.Length; ++i)
        {
            isInRange &= arr1[i] >= min && arr1[i] < max;
            isNotAllSame |= i > 1 && arr1[i - 1] != arr1[i];
            noSeedDifferentValues &= arr1[i] != arr2[i];
        }

        a.Assert(isInRange);
        a.Assert(isNotAllSame);
        a.Assert(noSeedDifferentValues);
    }
}
