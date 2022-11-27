using Bny.UnitTests.Examples;
using Bny.UnitTests;

// The main 3 ways of using the library
Console.WriteLine("Example 1:");
Example1.Run();
Console.WriteLine("Example 2:");
Example2.Run();
Console.WriteLine("Example 3:");
Example3.Run();

/* Standard output:
Example 1:
[failure] ArrayOperationsTests.Test_BubbleSort_Sort
  [success] Check.AllNeighbours(array, (a, b) => a < b)
  [failure] hasAll (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example1.cs:45::)
[failure] ArrayOperationsTests.Test_BubbleSort_Stable
  [failure] arr[1].Char == 'd' && arr[2].Char == 'h' (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example1.cs:56::)
  [failure] arr[5].Char == 'b' && arr[6].Char == 'f' (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example1.cs:57::)
[success] ArrayOperationsTests.Test_BinarySearch
[success] MyMathTests.Test_Pow
Example 2:
[failure] ArrayOperationsTests.Test_BubbleSort_Sort
  [success] Check.AllNeighbours(array, (a, b) => a < b)
  [failure] hasAll (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example2.cs:39::)
[failure] ArrayOperationsTests.Test_BubbleSort_Stable
  [failure] arr[1].Char == 'd' && arr[2].Char == 'h' (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example2.cs:51::)
  [failure] arr[5].Char == 'b' && arr[6].Char == 'f' (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example2.cs:52::)
[success] ArrayOperationsTests.Test_BinarySearch
[success] MyMathTests.Test_Pow
Example 3:
[failure] ArrayOperationsTests.Test_BubbleSort_Sort
  [success] Check.AllNeighbours(array, (a, b) => a < b)
  [failure] hasAll (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example3.cs:39::)
[failure] ArrayOperationsTests.Test_BubbleSort_Stable
  [failure] arr[1].Char == 'd' && arr[2].Char == 'h' (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example3.cs:51::)
  [failure] arr[5].Char == 'b' && arr[6].Char == 'f' (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example3.cs:52::)
[success] ArrayOperationsTests.Test_BinarySearch
[success] MyMathTests.Test_Pow
 */

// all the following examples will use the code in the Example3

// How the logAmount parameter changes the behaviour
// logAmount changes the amount of information printed to the output

// LogAmount.Minimal logs only the individual test functions
Console.WriteLine("logAmount = LogAmount.Minimal:");
Tester.TestAll(logAmount: LogAmount.Minimal);

// LogAmount.Default logs individual asserts for function only if at least one of the asserts fails
Console.WriteLine("logAmount = LogAmount.Default:");
Tester.TestAll(logAmount: LogAmount.Default);

// LofAmount.All logs all individual asserts
Console.WriteLine("logAmount = LogAmount.All:");
Tester.TestAll(logAmount: LogAmount.All);

/* Standard output:
logAmount = LogAmount.Minimal:
[failure] ArrayOperationsTests.Test_BubbleSort_Sort
[failure] ArrayOperationsTests.Test_BubbleSort_Stable
[success] ArrayOperationsTests.Test_BinarySearch
[success] MyMathTests.Test_Pow
logAmount = LogAmount.Default:
[failure] ArrayOperationsTests.Test_BubbleSort_Sort
  [success] Check.AllNeighbours(array, (a, b) => a < b)
  [failure] hasAll (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example3.cs:39::)
[failure] ArrayOperationsTests.Test_BubbleSort_Stable
  [failure] arr[1].Char == 'd' && arr[2].Char == 'h' (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example3.cs:51::)
  [failure] arr[5].Char == 'b' && arr[6].Char == 'f' (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example3.cs:52::)
[success] ArrayOperationsTests.Test_BinarySearch
[success] MyMathTests.Test_Pow
logAmount = LogAmount.All:
[failure] ArrayOperationsTests.Test_BubbleSort_Sort
  [success] Check.AllNeighbours(array, (a, b) => a < b)
  [failure] hasAll (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example3.cs:39::)
[failure] ArrayOperationsTests.Test_BubbleSort_Stable
  [failure] arr[1].Char == 'd' && arr[2].Char == 'h' (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example3.cs:51::)
  [failure] arr[5].Char == 'b' && arr[6].Char == 'f' (in X:\Files\Programming\C#\Bny.UnitTests\Bny.UnitTests.Examples\Example3.cs:52::)
[success] ArrayOperationsTests.Test_BinarySearch
  [success] ArrayOperations.BinarySearch(arr, 5) == 5
  [success] ArrayOperations.BinarySearch(arr, 125) == -1
[success] MyMathTests.Test_Pow
  [success] MyMath.IntPow(0, 5) == 0
  [success] MyMath.IntPow(2, 3) == 8
  [success] MyMath.IntPow(5, 0) == 1
  [success] MyMath.IntPow(4, -2) == 0.0625
 */

// The parameter @out sets the output stream

// The parameter formatting determines whether the output is written in ANSI colors, or unformatted, this is true by default

Console.WriteLine("Formatted:");
Tester.TestAll(formatted: true);
Console.WriteLine("Unformatted:");
Tester.TestAll(formatted: false);

// See the differences in the README