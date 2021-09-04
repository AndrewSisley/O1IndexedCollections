using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using O1IndexedCollections.Indexers.TimeSeries;

namespace O1IndexedCollections.Indexers.TimeSeries.Tests
{
    [TestFixture]
    public partial class DiscreteTimeSeriesIndexerTests {
        [TestFixture]
        public class Base
        {
            private static readonly DateTime DefaultStart = new DateTime(2000, 01, 01);
            private static readonly DateTime DefaultEnd = new DateTime(2001, 01, 01);
            private static readonly Resolution DefaultResolution = new Resolution(TimeSpan.FromMinutes(10).Ticks);

            [Test]
            public void GetOrAdd_ReturnsDistinctIndexes_GivenKeys()
            {
                var indexer = new DiscreteTimeSeriesIndexer.Base(DefaultStart, DefaultEnd, DefaultResolution);


                var result = new List<uint>();
                for (var key = DefaultStart; key <= DefaultEnd; key = key.AddTicks(DefaultResolution.Ticks)) {
                    result.Add(
                        indexer.GetOrAdd(key)
                    );
                }


                var recievedIndexes = new HashSet<uint>(result.Count);
                result.ForEach(r => Assert.True(recievedIndexes.Add(r)));
            }

            [Test]
            public void GetOrAdd_ReturnsReproducableIndexes_GivenKeys()
            {
                var indexer = new DiscreteTimeSeriesIndexer.Base(DefaultStart, DefaultEnd, DefaultResolution);


                var result1 = new List<uint>();
                for (var key = DefaultStart; key <= DefaultEnd; key = key.AddTicks(DefaultResolution.Ticks)) {
                    result1.Add(
                        indexer.GetOrAdd(key)
                    );
                }
                var result2 = new List<uint>();
                for (var key = DefaultStart; key <= DefaultEnd; key = key.AddTicks(DefaultResolution.Ticks)) {
                    result2.Add(
                        indexer.GetOrAdd(key)
                    );
                }


                Assert.AreEqual(result1, result2);
            }

            [Test]
            public void Enumerate_YieldsAllKeys() {
                var indexer = new DiscreteTimeSeriesIndexer.Base(DefaultStart, DefaultEnd, DefaultResolution);
                var expectedValues = new List<DateTime>();
                for (var key = DefaultStart; key <= DefaultEnd; key = key.AddTicks(DefaultResolution.Ticks)) {
                    expectedValues.Add(key);
                }


                var result = indexer.ToList();


                Assert.AreEqual(expectedValues, result);
            }

            [Test]
            public void Indexes_YieldsAllKeysAndDistinctIndexes() {
                var indexer = new DiscreteTimeSeriesIndexer.Base(DefaultStart, DefaultEnd, DefaultResolution);
                var expectedValues = new List<DateTime>();
                for (var key = DefaultStart; key <= DefaultEnd; key = key.AddTicks(DefaultResolution.Ticks)) {
                    expectedValues.Add(key);
                }


                var result = indexer.Indexes.ToList();


                Assert.AreEqual(expectedValues, result.Select(i => i.Item1).ToList());
                var recievedIndexes = new HashSet<uint>(result.Count);
                result.Select(i => i.Item2).ToList().ForEach(r => Assert.True(recievedIndexes.Add(r)));
            }
        }
    }
}
