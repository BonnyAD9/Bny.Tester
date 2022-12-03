using System.Numerics;

namespace Bny.UnitTests;

/// <summary>
/// Class for generating data for unit tests
/// </summary>
public static class TestData
{
    /// <summary>
    /// Fills a span with the given value
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    /// <param name="span">Span to fill</param>
    /// <param name="value">Value to fill the span with</param>
    public static void Fill<T>(Span<T> span, T value)
    {
        for (int i = 0; i < span.Length; ++i)
            span[i] = value;
    }

    /// <summary>
    /// Fills a span with values supplied by a filler function
    /// </summary>
    /// <typeparam name="T">Type of the data in the span</typeparam>
    /// <param name="span">Span to fill</param>
    /// <param name="filler">
    /// Fill function, gets index of the data in the span
    /// and returns the data to add to the span
    /// </param>
    public static void Fill<T>(Span<T> span, Func<int, T> filler)
    {
        for (int i = 0; i < span.Length; ++i)
            span[i] = filler(i);
    }

    /// <summary>
    /// Creates array of the given type and fills it with the given value
    /// </summary>
    /// <typeparam name="T">Type of the values in the array</typeparam>
    /// <param name="length">Size of the array</param>
    /// <param name="value">Value to fill the array with</param>
    /// <returns>New array filled with the value</returns>
    public static T[] Generate<T>(int length, T value)
    {
        T[] arr = new T[length];
        Fill(arr, value);
        return arr;
    }

    /// <summary>
    /// Creates array of the given type and fills it with values supplied by a
    /// filler function
    /// </summary>
    /// <typeparam name="T">Type of the values in the array</typeparam>
    /// <param name="length">Size of the array</param>
    /// <param name="filler">
    /// Fill function, gets index of the fata in the
    /// array and returns the data to add to the array
    /// </param>
    /// <returns>New array filled with the filler function</returns>
    public static T[] Generate<T>(int length, Func<int, T> filler)
    {
        T[] arr = new T[length];
        Fill(arr, filler);
        return arr;
    }

    /// <summary>
    /// Fills a span with integer numbers in the given range
    /// </summary>
    /// <typeparam name="T">Type of the number in the span</typeparam>
    /// <param name="span">Span to fill</param>
    /// <param name="seed">
    /// Random number generator seed, 0 for random seed
    /// </param>
    /// <param name="min">Lower boundry of numbers to generate</param>
    /// <param name="max">Upper boundry of numbers to generate</param>
    public static void FillRngInt<T>(
        Span<T> span                ,
        int     seed = 0            ,
        long    min  = 0            ,
        long    max  = long.MaxValue)
        where T : INumberBase<T>, IMinMaxValue<T>
    {
        if (max == long.MaxValue)
            max = long.CreateTruncating(T.MaxValue);
        Random r = seed == 0 ? Random.Shared : new Random(seed);
        for (int i = 0; i < span.Length; ++i)
            span[i] = T.CreateTruncating(r.NextInt64(min, max));
    }

    /// <summary>
    /// Fills span with floating point numbers int the given range
    /// </summary>
    /// <typeparam name="T">Type of the values in the span</typeparam>
    /// <param name="span">Span to fill</param>
    /// <param name="seed">
    /// Seed of the random generator, 0 means random seed
    /// </param>
    /// <param name="min">Lower boundary of the numbers to generate</param>
    /// <param name="max">Upper boundary of the numbers to generate</param>
    public static void FillRngFloat<T>(
        Span<T> span    ,
        int     seed = 0,
        double  min  = 0,
        double  max  = 1) where T : INumberBase<T>
    {
        Random r = seed == 0 ? Random.Shared : new Random(seed);
        var range = Math.Abs(max - min);
        for (int i = 0; i < span.Length; ++i)
            span[i] = T.CreateTruncating(r.NextDouble() * range + min);
    }

    /// <summary>
    /// Generates array of random integer values in the given range
    /// </summary>
    /// <typeparam name="T">Type of the values to generate</typeparam>
    /// <param name="length">Size of the array</param>
    /// <param name="seed">Random number generator seed, 0 for random</param>
    /// <param name="min">Lower boundary of the generated values</param>
    /// <param name="max">Upper boundary of the generated values</param>
    /// <returns>New array with random integers</returns>
    public static T[] GenerateRngInt<T>(
        int  length              ,
        int  seed = 0            ,
        long min  = 0            ,
        long max  = long.MaxValue) where T : INumberBase<T>, IMinMaxValue<T>
    {
        T[] arr = new T[length];
        FillRngInt<T>(arr, seed, min, max);
        return arr;
    }

    /// <summary>
    /// Generates array of random floating point values in the given range
    /// </summary>
    /// <typeparam name="T">Type of the values to generate</typeparam>
    /// <param name="length">Size of the array</param>
    /// <param name="seed">Random number generator seed, 0 for random</param>
    /// <param name="min">Lower boundary of the generated values</param>
    /// <param name="max">Upper boundary of the generated values</param>
    /// <returns>new array with random floating point values</returns>
    public static T[] GenerateRngFloat<T>(
        int    length  ,
        int    seed = 0,
        double min  = 0,
        double max  = 1) where T : INumberBase<T>
    {
        T[] arr = new T[length];
        FillRngFloat<T>(arr, seed, min, max);
        return arr;
    }
}
