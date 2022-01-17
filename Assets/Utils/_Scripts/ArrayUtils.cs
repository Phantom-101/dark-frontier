using System;

namespace DarkFrontier.Utils
{
    public static class ArrayUtils
    {
        public static T[] Copy<T>(this T[] array)
        {
            var l = array.Length;
            var ret = new T[l];
            for(var i = 0; i < l; i++)
            {
                ret[i] = array[i];
            }
            return ret;
        }
        
        public static TNew[] Copy<TOld, TNew>(this TOld[] array) where TOld : TNew
        {
            var l = array.Length;
            var ret = new TNew[l];
            for(var i = 0; i < l; i++)
            {
                ret[i] = array[i];
            }
            return ret;
        }
        
        public static T[] Concat<T>(T[] a, T[] b)
        {
            var c = new T[a.Length + b.Length];
            Array.Copy(a, 0, c, 0, a.Length);
            Array.Copy(b, 0, c, a.Length, b.Length);
            return c;
        }
    }
}
