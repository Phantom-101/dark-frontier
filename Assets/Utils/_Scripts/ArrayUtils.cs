using System;

namespace DarkFrontier
{
    public class ArrayUtils
    {
        public static T[] Concat<T>(T[] a, T[] b)
        {
            var c = new T[a.Length + b.Length];
            Array.Copy(a, 0, c, 0, a.Length);
            Array.Copy(b, 0, c, a.Length, b.Length);
            return c;
        }
    }
}
