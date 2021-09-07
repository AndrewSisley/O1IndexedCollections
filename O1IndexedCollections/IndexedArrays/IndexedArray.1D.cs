using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace O1IndexedCollections {
    public class IndexedArray<TKey1, TValue> : IIndexedArray<TKey1, TValue> {
        public IIndexLibrary<TKey1> IndexLibrary { get; }
        IIndexLibrary IIndexedArray.IndexLibrary => IndexLibrary;

        private TValue[] valueArray;
        private bool[] hasValueArray;
        public TValue[] ValueArray => valueArray;
        public bool[] HasValueArray => hasValueArray;


        public IndexedArray(IIndexLibrary<TKey1> indexLibrary) {
            this.IndexLibrary = indexLibrary;
            valueArray = new TValue[0];
            hasValueArray = new bool[0];
        }


        public TValue this[TKey1 key] { // check dictionary behaviour and ensure this matches
            get => this.Get(key);
            set => this.Set(key, value);
        }

        public IEnumerable<TKey1> Keys => IndexLibrary;
        ICollection<TKey1> IDictionary<TKey1, TValue>.Keys => Keys.ToList();

        ICollection<TValue> IDictionary<TKey1, TValue>.Values => this.GetValues().ToList();

        public int Count => this.GetValues().Count();

        public bool IsReadOnly => false;

        public void Add(TKey1 key, TValue value) {
            var index = this.IndexLibrary.GetOrAdd(key);
            if (HasValueArray[index]) throw new System.Exception();// todo - check dictionary exception type and message

            this.Set(key, value);
        }

        public void Add(KeyValuePair<TKey1, TValue> item) {
            var index = this.IndexLibrary.GetOrAdd(item.Key);
            if (HasValueArray[index]) throw new System.Exception();// todo - check dictionary exception type and message

            this.Set(item.Key, item.Value);
        }

        public void Clear() => this.Clear<TValue>();

        public bool Contains(KeyValuePair<TKey1, TValue> item) {
            if (!this.IndexLibrary.TryGet(item.Key, out var index)) return false;
            if (index >= HasValueArray.Length) return false;
            if (!HasValueArray[index]) return false;

            return Equals(item.Value, ValueArray[index]);
        }

        public bool ContainsKey(TKey1 key) {
            if (!this.IndexLibrary.TryGet(key, out var index)) return false;
            if (index >= HasValueArray.Length) return false;

            return HasValueArray[index];
        }

        public void CopyTo(KeyValuePair<TKey1, TValue>[] array, int arrayIndex) {//todo - match array/dictionary exception behaviour
            if (arrayIndex + Count > array.Length) throw new ArgumentException();

            var index = arrayIndex;
            foreach (var kvp in this) {
                array[index] = kvp;
                index++;
            }
        }

        public IEnumerator<KeyValuePair<TKey1, TValue>> GetEnumerator() {
            for (var i = 0; i < ValueArray.Length; i++) {
                if (HasValueArray[i]) {
                    yield return new KeyValuePair<TKey1, TValue>(IndexLibrary.GetKeyFromIndex(i), ValueArray[i]);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Remove(TKey1 key) {
            if (!IndexLibrary.TryGet(key, out var index)) {
                return false;
            }

            ValueArray[index] = default!;
            var result = HasValueArray[index];
            HasValueArray[index] = false;

            return result;
        }

        public bool Remove(KeyValuePair<TKey1, TValue> item) {
            if (!IndexLibrary.TryGet(item.Key, out var index)) {
                return false;
            }

            var result = HasValueArray[index] && Equals(ValueArray[index], item.Value);
            ValueArray[index] = default!;
            HasValueArray[index] = false;

            return result;
        }

        public bool TryGetValue(TKey1 key, out TValue value) {
            if (!IndexLibrary.TryGet(key, out var index)) {
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
