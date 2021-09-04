using System;
using System.Collections.Generic;

namespace O1IndexedCollections {
    public static class IndexedArrayExtensions {
        public static IEnumerable<TValue> GetValues<TValue>(this IIndexedArray<TValue> array) {
            for (var i = 0; i < array.ValueArray.Length; i++) {
                if (array.HasValueArray[i]) yield return array.ValueArray[i];
            }
        }

        public static void Clear<TValue>(this IIndexedArray<TValue> array) {
            Array.Clear(array.ValueArray, 0, array.ValueArray.Length);
            Array.Clear(array.HasValueArray, 0, array.HasValueArray.Length);
        }

        public static void Set<TKey1, TValue>(this IIndexedArray<TKey1, TValue> array, TKey1 key1, TValue value) {
            var index = array.IndexLibrary.GetOrAdd(key1);

            if (index >= array.ValueArray.Length) {
                array.Resize((int)(index * 2) + 1);
            }

            array.ValueArray[index] = value;
            array.HasValueArray[index] = true;
        }

        public static void Set<TKey1, TKey2, TValue>(this IIndexedArray<TKey1, TKey2, TValue> array, TKey1 key1, TKey2 key2, TValue value) {
            var index = array.IndexLibrary.GetOrAdd(key1, key2);

            if (index >= array.ValueArray.Length) {
                array.Resize((int)(index * 2) + 1);
            }

            array.ValueArray[index] = value;
            array.HasValueArray[index] = true;
        }

        public static TValue Get<TKey1, TValue>(this IIndexedArray<TKey1, TValue> array, TKey1 key1) {
            var index = array.IndexLibrary.GetOrAdd(key1);
            return array.ValueArray[index];
        }

        public static TValue Get<TKey1, TKey2, TValue>(this IIndexedArray<TKey1, TKey2, TValue> array, TKey1 key1, TKey2 key2) {
            var index = array.IndexLibrary.GetOrAdd(key1, key2);
            return array.ValueArray[index];
        }
    }
}
