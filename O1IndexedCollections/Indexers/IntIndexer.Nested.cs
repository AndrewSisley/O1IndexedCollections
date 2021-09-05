using System;
using System.Collections;
using System.Collections.Generic;

namespace O1IndexedCollections.Indexers {
    public partial class IntIndexer {
        public class Nested : IIndexer.Nested<int> {
            public int Minimum { get; }
            public int Maximum { get; }

            private uint nextNewIndex = 0;
            private uint?[] indexes;


            public Nested(int min, int max) {
                Minimum = min;
                Maximum = max;
                indexes = new uint?[max - min];
            }


            public uint GetOrAdd(uint prior, int key) {//todo - overflow safety, consider unchecked overloads
                var internalIndex = (prior * (Maximum - Minimum)) + key - Minimum;

                if (internalIndex >= indexes.Length) {
                    Array.Resize(ref indexes, (int)(internalIndex * 2) + 1);
                }

                var index = indexes[internalIndex];
                if (!index.HasValue) {
                    indexes[internalIndex] = nextNewIndex;
                    index = nextNewIndex;
                    nextNewIndex++;
                }

                return nextNewIndex;
            }

            public IEnumerator<int> GetEnumerator() {
                for (var index = Minimum; index <= Maximum; index++) {
                    if (indexes[index].HasValue) {
                        yield return index;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
