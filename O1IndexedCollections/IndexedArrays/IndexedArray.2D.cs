using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace O1IndexedCollections {
    public class IndexedArray<TKey1, TKey2, TValue> : IIndexedArray<TKey1, TKey2, TValue> {
        public IIndexLibrary<TKey1, TKey2> IndexLibrary { get; }
        IIndexLibrary IIndexedArray.IndexLibrary => IndexLibrary;

        private TValue[] valueArray;
        private bool[] hasValueArray;
        public TValue[] ValueArray => valueArray;
        public bool[] HasValueArray => hasValueArray;


        public IndexedArray(IIndexLibrary<TKey1, TKey2> indexLibrary) {
            this.IndexLibrary = indexLibrary;
            valueArray = new TValue[0];
            hasValueArray = new bool[0];
        }


        public TValue this[(TKey1, TKey2) key] { // check dictionary behaviour and ensure this matches
            get => this.Get(key.Item1, key.Item2);
            set => this.Set(key.Item1, key.Item2, value);
        }

        public IEnumerable<(TKey1, TKey2)> Keys => IndexLibrary;
        ICollection<(TKey1, TKey2)> IDictionary<(TKey1, TKey2), TValue>.Keys => Keys.ToList();

        ICollection<TValue> IDictionary<(TKey1, TKey2), TValue>.Values => this.GetValues().ToList();

        public int Count => this.GetValues().Count();

        public bool IsReadOnly => false;

        public void Add((TKey1, TKey2) key, TValue value) {
            var index = this.IndexLibrary.GetOrAdd(key.Item1, key.Item2);
            if (HasValueArray[index]) throw new System.Exception();// todo - check dictionary exception type and message

            this.Set(key.Item1, key.Item2, value);
        }

        public void Add(KeyValuePair<(TKey1, TKey2), TValue> item) {
            var index = this.IndexLibrary.GetOrAdd(item.Key.Item1, item.Key.Item2);
            if (HasValueArray[index]) throw new System.Exception();// todo - check dictionary exception type and message

            this.Set(item.Key.Item1, item.Key.Item2, item.Value);
        }

        public void Clear() => this.Clear<TValue>();

        public bool Contains(KeyValuePair<(TKey1, TKey2), TValue> item) {
            if (!this.IndexLibrary.TryGet(item.Key.Item1, item.Key.Item2, out var index)) return false;
            if (index >= HasValueArray.Length) return false;
            if (!HasValueArray[index]) return false;

            return Equals(item.Value, ValueArray[index]);
        }

        public bool ContainsKey((TKey1, TKey2) key) {
            if (!this.IndexLibrary.TryGet(key.Item1, key.Item2, out var index)) return false;
            if (index >= HasValueArray.Length) return false;

            return HasValueArray[index];
        }

        public void CopyTo(KeyValuePair<(TKey1, TKey2), TValue>[] array, int arrayIndex) {//todo - match array/dictionary exception behaviour
            if (arrayIndex + Count > array.Length) throw new ArgumentException();

            var index = arrayIndex;
            foreach (var kvp in this) {
                array[index] = new KeyValuePair<(TKey1, TKey2), TValue>((kvp.Key.Item1, kvp.Key.Item2), kvp.Value);
            }
        }

        public IEnumerator<KeyValuePair<(TKey1, TKey2), TValue>> GetEnumerator() {
            foreach (var item in IndexLibrary.Indexes) {
                if (item.Item3 >= HasValueArray.Length) yield break;

                if (HasValueArray[item.Item3]) {
                    yield return new KeyValuePair<(TKey1, TKey2), TValue>((item.Item1, item.Item2), ValueArray[item.Item3]);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Remove((TKey1, TKey2) key) {
            if (!IndexLibrary.TryGet(key.Item1, key.Item2, out var index)) {
                return false;
            }

            ValueArray[index] = default!;
            var result = HasValueArray[index];
            HasValueArray[index] = false;

            return result;
        }

        public bool Remove(KeyValuePair<(TKey1, TKey2), TValue> item) {
            if (!IndexLibrary.TryGet(item.Key.Item1, item.Key.Item2, out var index)) {
                return false;
            }

            var result = HasValueArray[index] && Equals(ValueArray[index], item.Value);
            ValueArray[index] = default!;
            HasValueArray[index] = false;

            return result;
        }

        public bool TryGetValue((TKey1, TKey2) key, out TValue value) {
            if (!IndexLibrary.TryGet(key.Item1, key.Item2, out var index)) {
                value = default!;
                return false;
            }

            if (index >= HasValueArray.Length) {
                value = default!;
                return false;
            }

            value = ValueArray[index];
            return HasValueArray[index];
        }

        public void Resize(int newSize) {
            Array.Resize(ref valueArray, newSize);
            Array.Resize(ref hasValueArray, newSize);
        }
    }
}
