using System.Collections.Generic;

namespace O1IndexedCollections {
    public interface IIndexLibrary {

    }

    public interface IIndexLibrary<TKey1> : IIndexLibrary, IEnumerable<TKey1> {
        IIndexer.Base<TKey1> Indexer1 { get; }
        IEnumerable<(TKey1, uint)> Indexes { get; }
        uint GetOrAdd(TKey1 key1);
        bool TryGet(TKey1 key1, out uint index);
    }

    public interface IIndexLibrary<TKey1, TKey2> : IIndexLibrary, IEnumerable<(TKey1, TKey2)> {
        IIndexer.Base<TKey1> Indexer1 { get; }
        IIndexer.Nested<TKey2> Indexer2 { get; }
        IEnumerable<(TKey1, TKey2, uint)> Indexes { get; }
        uint GetOrAdd(TKey1 key1, TKey2 key2);
        bool TryGet(TKey1 key1, TKey2 key2, out uint index);
    }
}
