using System;
using System.Collections.Generic;
using O1IndexedCollections;
using O1IndexedCollections.Indexers.TimeSeries;
using BenchmarkDotNet.Attributes;

namespace O1IndexedCollections.Benchmarks.IndexedArrays {
    public partial class IndexedArray {
        [MemoryDiagnoser]
        public partial class DiscreteTimeSeriesBenchmarks {
            private static readonly DateTime DefaultStart = new DateTime(2000, 01, 01);
            private static readonly DateTime DefaultEnd = new DateTime(2001, 01, 01);
            private static readonly Resolution DefaultResolution = new Resolution(TimeSpan.FromMinutes(10).Ticks);

            [Params(0, 1, 100, 10_000)]
            public int N { get; set; }
            private (DateTime Key, int Value)[] entries = new (DateTime, int)[0];
            private IndexedArray<DateTime, int> populatedIndexedArray;
            private Dictionary<DateTime, int> populatedDictionary;


            public DiscreteTimeSeriesBenchmarks() {
                var indexer = new DiscreteTimeSeriesIndexer.Base(DefaultStart, DefaultEnd, DefaultResolution);
                populatedIndexedArray = new IndexedArray<DateTime, int>(indexer);
                populatedDictionary = new Dictionary<DateTime, int>();
            }


            [GlobalSetup]
            public void GlobalSetup()
            {
                entries = new (DateTime, int)[N];
                for (var i = 0; i < N; i++) {
                    var key = DefaultStart.AddMinutes(i * 10);
                    entries[i] = (key, i);
                    populatedIndexedArray[key] = i;
                    populatedDictionary[key] = i;
                }
            }

            [Benchmark]
            public void IndexSet_DiscreteTimeSeries() {
                var indexer = new DiscreteTimeSeriesIndexer.Base(DefaultStart, DefaultEnd, DefaultResolution);
                var collection = new IndexedArray<DateTime, int>(indexer);

                foreach (var entry in entries) {
                    collection[entry.Key] = entry.Value;
                }
            }

            [Benchmark]
            public void IndexSet_Dictionary() {
                var collection = new Dictionary<DateTime, int>();

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
