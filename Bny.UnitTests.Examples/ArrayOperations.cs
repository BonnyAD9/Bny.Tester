using System.Numerics;

namespace Bny.UnitTests.Examples;

/// <summary>
/// Example class that does array operations
/// </summary>
internal class ArrayOperations
{
    /// <summary>
    /// Sorts the array using stable bubble sort algorithm
    /// </summary>
    /// <typeparam name="T">Type in the array</typeparam>
    /// <param name="arr">Array to sort</param>
    /// <param name="comparer">Compares the two values</param>
    public static void BubbleSort<T>(Span<T> arr, Func<T, T, int> comparer)
    {
        arr[0] = default!; // introduce bug
        for (int i = 0; i < arr.Length; i++)
        {
            for (int j = 1; j < arr.Length - i; j++)
            {
                if (comparer(arr[j - 1], arr[j]) >= 0) // this is a bug, the algorithm is not stable (there should be '>' instead of '>=')
                    (arr[j - 1], arr[j]) = (arr[j], arr[j - 1]);
            }
        }
    }

    /// <summary>
    /// This finds value in a sorted array
    /// </summary>
    /// <typeparam name="T">type in the array</typeparam>
    /// <param name="arr">array to search</param>
    /// <param name="item">item to find</param>
    /// <returns>index of the item in the array, -1 if the item is not in the array</returns>
    public static int BinarySearch(Span<int> arr, int item, int offset = 0)
    {
        if (arr.Length == 0)
            return -1;
        if (arr.Length == 1)
            return arr[0] == item ? offset : -1;

        int p = arr.Length / 2;
        if (arr[p] == item)
            return p + offset;

        return item < arr[p]
            ? BinarySearch(arr[..p], item, offset)
            : BinarySearch(arr[(p + 1)..], item, offset + p + 1);
    }
}
