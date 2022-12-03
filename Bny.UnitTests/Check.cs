using System.Collections;

namespace Bny.UnitTests;

/// <summary>
/// Static class for making checks on data
/// </summary>
public static class Check
{
    /// <summary>
    /// Checks whether all neighbouring elements satisfy the given condition
    /// </summary>
    /// <typeparam name="T">Type of elements in the collection</typeparam>
    /// <param name="collection">Collection to search</param>
    /// <param name="checker">The function that checks the elements</param>
    /// <returns>
    /// True if all neighbours satisfy the conditioin, otherwise false
    /// </returns>
    public static bool AllNeighbours<T>(
        IEnumerable<T>   collection,
        Func<T, T, bool> checker   )
    {
        var en = collection.GetEnumerator();
        if (!en.MoveNext())
            return true;

        T val = en.Current;
        while (en.MoveNext())
        {
            if (!checker(val, en.Current))
                return false;
            val = en.Current;
        }

        return true;
    }

    /// <summary>
    /// Checks whether at least one neighbouring pair of elements satisfies
    /// the condition
    /// </summary>
    /// <typeparam name="T">Type of the elements</typeparam>
    /// <param name="collection">Collection of the elements</param>
    /// <param name="checker">Condition that should be satisfied</param>
    /// <returns>
    /// True if at least one neighbouring pair satisfies the condition,
    /// otherwise false
    /// </returns>
    public static bool AnyNeighbour<T>(
        IEnumerable<T> collection,
        Func<T, T, bool> checker)
    {
        var en = collection.GetEnumerator();
        if (!en.MoveNext())
            return false;

        T val = en.Current;
        while (en.MoveNext())
        {
            if (checker(val, en.Current))
                return true;
            val = en.Current;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the two collections have the same number of
    /// elements and the elements are equal using the default equality
    /// comparer
    /// </summary>
    /// <param name="c1">The first collection</param>
    /// <param name="c2">The second collection</param>
    /// <returns>
    /// true if the lengths and each elements are the same, otherwise false
    /// </returns>
    public static bool AreSame(IEnumerable c1, IEnumerable c2)
    {
        var en1 = c1.GetEnumerator();
        var en2 = c2.GetEnumerator();

        while (en1.MoveNext())
        {
            if (!en2.MoveNext())
                return false;
            if (!en1.Current.Equals(en2.Current))
                return false;
        }

        return !en2.MoveNext();
    }

    /// <summary>
    /// Determines whether the two collections have the same number of
    /// elements and the selected elements are equal using the default
    /// equality comparer
    /// </summary>
    /// <typeparam name="T">Type of elements in the collection</typeparam>
    /// <param name="c1">First collection</param>
    /// <param name="c2">Second collection</param>
    /// <param name="selector">
    /// Selects the elements to compare from the collections
    /// </param>
    /// <returns>
    /// True if the lengths are same and the elements are equal,
    /// otherwise false
    /// </returns>
    public static bool AreSame<T>(
        IEnumerable<T>  c1      ,
        IEnumerable<T>  c2      ,
        Func<T, object> selector)
    {
        var en1 = c1.GetEnumerator();
        var en2 = c2.GetEnumerator();

        while (en1.MoveNext())
        {
            if (!en2.MoveNext())
                return false;
            if (!selector(en1.Current).Equals(selector(en2.Current)))
                return false;
        }

        return !en2.MoveNext();
    }

    /// <summary>
    /// Checks whether the two collections have the same number of elements
    /// and the elements are equal using the given comparer
    /// </summary>
    /// <typeparam name="T1">
    /// Type of elements in the first collection
    /// </typeparam>
    /// <typeparam name="T2">
    /// Type of elements in the second collection
    /// </typeparam>
    /// <param name="c1">The first collection</param>
    /// <param name="c2">The second collection</param>
    /// <param name="comparer">Comparer for the colleciton items</param>
    /// <returns>
    /// True if the lengths are the same and the elements are equal,
    /// otherwise false
    /// </returns>
    public static bool AreSame<T1, T2>(
        IEnumerable<T1>    c1      ,
        IEnumerable<T2>    c2      ,
        Func<T1, T2, bool> comparer)
    {
        var en1 = c1.GetEnumerator();
        var en2 = c2.GetEnumerator();

        while (en1.MoveNext())
        {
            if (!en2.MoveNext())
                return false;
            if (!comparer(en1.Current, en2.Current))
                return false;
        }

        return !en2.MoveNext();
    }
}
