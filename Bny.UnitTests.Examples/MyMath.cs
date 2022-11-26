namespace Bny.UnitTests.Examples;

/// <summary>
/// Class with math operations
/// </summary>
internal class MyMath
{
    public static double IntPow(int x, int pow)
    {
        if (pow == 0)
            return 1;
        if (pow < 0)
            return 1 / IntPow(x, -pow);

        int r = x;
        for (int i = 1; i < pow; i++)
            r *= x;
        return r;
    }
}
