using System;
using System.Collections.Generic;
using O1IndexedCollections;
using O1IndexedCollections.Indexers;
using BenchmarkDotNet.Attributes;

namespace O1IndexedCollections.Benchmarks.IndexedArrays {
    public partial class IndexedArray {
        [MemoryDiagnoser]
        public partial class LongBenchmarks {
            private static readonly long DefaultStart = 0;

            [Params(0, 1, 100, 10_000, 1_000_000)]
            public int N { get; set; }
            private (long Key, int Value)[] entries = new (long, int)[0];
            private IndexedArray<long, int> populatedIndexedArray;
            private Dictionary<long, int> populatedDictionary;


            public LongBenchmarks() {
                var indexer = new LongIndexer.Base(DefaultStart, N);
                populatedIndexedArray = new IndexedArray<long, int>(indexer);
                populatedDictionary = new Dictionary<long, int>();
            }


            [GlobalSetup]
            public void GlobalSetup()
            {
                var indexer = new LongIndexer.Base(DefaultStart, N);
                populatedIndexedArray = new IndexedArray<long, int>(indexer);
                populatedDictionary = new Dictionary<long, int>();

                entries = new (long, int)[N];
                for (var i = 0; i < N; i++) {
                    entries[i] = (i, i + 900000);
                    populatedIndexedArray[i] = i + 900000;
                    populatedDictionary[i] = i + 900000;
                }
            }

            [Benchmark]
            public void IndexSet_DiscreteTimeSeries() {
                var indexer = new LongIndexer.Base(DefaultStart, N);
                var collection = new IndexedArray<long, int>(indexer);

                foreach (var entry in entries) {
                    collection[entry.Key] = entry.Value;
                }
            }

            [Benchmark]
            public void IndexSet_Dictionary() {
                var collection = new Dictionary<long, int>();

                foreach (var entry in entries) {
                    collection[entry.Key] = entry.Value;
                }
            }

            [Benchmark]
            public int IndexGet_DiscreteTimeSeries() {
                var value = 0;
                foreach (var entry in entries) {
                    value = populatedIndexedArray[entry.Key];
                }
                return value;
            }

            [Benchmark]
            public int IndexGet_Dictionary() {
                var value = 0;
                foreach (var entry in entries) {
                    value = populatedDictionary[entry.Key];
                }
                return value;
            }

            [Benchmark]
            public int Enumerate_DiscreteTimeSeries() {
                var value = 0;
                foreach (var entry in populatedIndexedArray) {
                    value = entry.Value;
                }
                return value;
            }

            [Benchmark]
            public int Enumerate_Dictionary() {
                var value = 0;
                foreach (var entry in populatedDictionary) {
                    value = entry.Value;
                }
                return value;
            }
        }
    }
}
