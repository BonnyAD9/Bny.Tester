﻿namespace Bny.UnitTests.Examples;

/// <summary>
/// This example shows how to mark tests in class and run them for each class
/// </summary>
internal class Example2
{
    public static void Run()
    {
        // Run the tests on the classes
        Tester.Test<ArrayOperationsTests>();
        Tester.Test<MyMathTests>();
    }

    /// <summary>
    /// Tests the ArrayOperations
    /// </summary>
    class ArrayOperationsTests
    {
        // tests whether the function sorts
        [UnitTest] // Mark functions that should be tested with the attribute
        static void Test_BubbleSort_Sort(Asserter a)
        {
            // Generate test data
            int[] array = TestData.GenerateRngInt<int>(100);
            int[] copy = array.ToArray();

            // run the function
            ArrayOperations.BubbleSort<int>(array, (a, b) => a - b);

            // thest whether the array is sorted
            a.Assert(Check.AllNeighbours(array, (a, b) => a < b));

            // test whether the array contains the same elements
            bool hasAll = true;
            foreach (var i in copy)
                hasAll &= array.Count(p => p == i) == copy.Count(p => p == i);

            a.Assert(hasAll);
        }

        // tests whether the data is sorted stably
        [UnitTest]
        static void Test_BubbleSort_Stable(Asserter a)
        {
            (int Num, char Char)[] arr = new[]
            {
                (1, 'a'), (6, 'b'), (10, 'c'), (2, 'd'),
                (5, 'e'), (6, 'f'), (9 , 'g'), (2, 'h'),
            };

            // run the function
            ArrayOperations.BubbleSort<(int Num, char Char)>(
                arr                    ,
                (a, b) => a.Num - b.Num);

            a.Assert(arr[1].Char == 'd' && arr[2].Char == 'h');
            a.Assert(arr[5].Char == 'b' && arr[6].Char == 'f');
        }

        // tests binary search
        [UnitTest]
        static void Test_BinarySearch(Asserter a)
        {
            int[] arr = TestData.Generate(100, i => i);

            a.Assert(ArrayOperations.BinarySearch(arr, 5) == 5);
            a.Assert(ArrayOperations.BinarySearch(arr, 125) == -1);
        }
    }

    /// <summary>
    /// Tests the MyMath
    /// </summary>
    class MyMathTests
    {
        // Tests the IntPow
        [UnitTest]
        static void Test_Pow(Asserter a)
        {
            a.Assert(MyMath.IntPow(0, 5) == 0);
            a.Assert(MyMath.IntPow(2, 3) == 8);
            a.Assert(MyMath.IntPow(5, 0) == 1);
            a.Assert(MyMath.IntPow(4, -2) == 0.0625);
        }
    }
}
