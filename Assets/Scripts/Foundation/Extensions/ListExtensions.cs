using System.Collections.Generic;

namespace DarkFrontier.Foundation.Extensions {
    public static class ListExtensions {
        public static bool AddUnique<T> (this List<T> list, T item) {
            if (list.Contains (item)) return false;
            list.Add (item);
            return true;
        }

        public static int RemoveAll<T> (this List<T> list, T item) {
            return list.RemoveAll (e => ReferenceEquals (e, item));
        }
    }
}
