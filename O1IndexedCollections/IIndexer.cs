using System.Collections.Generic;

namespace O1IndexedCollections {
    public interface IIndexer<TKey> : IEnumerable<TKey> {

    }

    public class IIndexer {
        public interface Base<TKey> : IIndexer<TKey> {
            uint GetOrAdd(TKey key);
        }

        public interface Nested<TKey> : IIndexer<TKey> {
            uint GetOrAdd(uint prior, TKey key);
        }
    }
}
