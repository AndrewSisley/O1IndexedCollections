using System.Collections.Generic;

namespace O1IndexedCollections {
    public interface IIndexedArray {
        IIndexLibrary IndexLibrary { get; }
        void Resize(int newSize);
    }

    public interface IIndexedArray<TValue> : IIndexedArray {
        TValue[] ValueArray { get; }
        bool[] HasValueArray { get; }
    }

    public interface IIndexedArray<TKey1, TValue> : IIndexedArray<TValue>, IDictionary<TKey1, TValue> {
        new IIndexLibrary<TKey1> IndexLibrary { get; }
        new IEnumerable<TKey1> Keys { get; }
    }

    public interface IIndexedArray<TKey1, TKey2, TValue> : IIndexedArray<TValue>, IDictionary<(TKey1, TKey2), TValue> {
        new IIndexLibrary<TKey1, TKey2> IndexLibrary { get; }
        new IEnumerable<(TKey1, TKey2)> Keys { get; }
    }
}
