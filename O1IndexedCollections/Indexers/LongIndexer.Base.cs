using System;
using System.Collections;
using System.Collections.Generic;

namespace O1IndexedCollections.Indexers {
    public partial class LongIndexer {
        public class Base : IIndexer.Base<long>, IIndexLibrary<long> {
            public long Minimum { get; }
            public long Maximum { get; }
            public IIndexer.Base<long> Indexer1 => this;

            private uint nextNewIndex = 0;
            private readonly uint?[] indexes;

            public IEnumerable<(long, uint)> Indexes {
                get {
                    for (var key = Minimum; key <= Maximum; key++) {
                        var index = indexes[key];
                        if (index.HasValue) {
                            yield return (key, index.Value);
                        }
                    }
                }
            }


            public Base(long min, long max) {
                if (max - min > int.MaxValue) throw new NotImplementedException("Capacities greater than int.MaxValue are not currently supported.");
                Minimum = min;
                Maximum = max;
                indexes = new uint?[max - min + 1];
            }


            public uint GetOrAdd(long key) {
                var internalIndex = (int)(key - Minimum);
                var index = indexes[internalIndex];

                if (!index.HasValue) {
                    indexes[internalIndex] = nextNewIndex;
                    index = nextNewIndex;
                    nextNewIndex++; // this is not deterministic, consider project name rethink, revist after implementing non-base indexer
                }

                return index.Value;
            }

            public bool TryGet(long key, out uint index) {
                index = 0;
                if (key < Minimum) return false;
                if (key > Maximum) return false;

                var internalIndex = (int)(key - Minimum);
                var existingIndex = indexes[internalIndex];
                if (!existingIndex.HasValue) return false;

                index = existingIndex.Value;
                return true;
            }

            public IEnumerator<long> GetEnumerator() {
                for (var key = Minimum; key <= Maximum; key++) {
                    if (indexes[key].HasValue) {
                        yield return key;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
