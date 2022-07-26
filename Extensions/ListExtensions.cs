using System;
using System.Collections.Generic;
using System.Linq;

namespace AssetSizeDetector
{
    public static class ListExtensions
    {
        public static void SortedInsert<T>(this IList<T> collection, T orderableItem, Comparison<T> comparer)
        {
            var index = collection.FindOptimalIndexInSortedCollection(orderableItem, comparer.Invoke);
            collection.Insert(index, orderableItem);
        }
        
        public static void SortedInsert<T>(this IList<T> collection, T orderableItem, IComparer<T> comparer)
        {
            var index = collection.FindOptimalIndexInSortedCollection(orderableItem, comparer.Compare);
            collection.Insert(index, orderableItem);
        }

        private static int FindOptimalIndexInSortedCollection<T>(this IList<T> collection, T orderableItem, Func<T, T, int> comparer)
        {
            for (var index = collection.Count - 1; index >= 0; index--)
            {
                var item = collection[index];
                if (comparer.Invoke(orderableItem, item) > 0)
                {
                    return index + 1;
                }
            }
            return 0;
        }

        public static List<T> Clone<T>(this List<T> list)
        {
            var array = new T[list.Count];
            list.CopyTo(array);
            return array.ToList();
        }
    }
}