using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using O1IndexedCollections;
using O1IndexedCollections.Indexers.TimeSeries;

namespace O1IndexedCollections.IndexedArrays
{
    [TestFixture]
    public partial class IndexedArrayTests {
        [TestFixture]
        public class DiscreteTimeSeries {
            private static readonly DateTime DefaultStart = new DateTime(2000, 01, 01);
            private static readonly DateTime DefaultEnd = new DateTime(2001, 01, 01);
            private static readonly Resolution DefaultResolution = new Resolution(TimeSpan.FromMinutes(10).Ticks);

            [Test]
            public void IndexGet_ReturnsSetValue_GivenStartValueSet() {
                var indexer = new DiscreteTimeSeriesIndexer.Base(DefaultStart, DefaultEnd, DefaultResolution);
                var array = new IndexedArray<DateTime, int>(indexer);
                var inputValue = 3;


                array[DefaultStart] = inputValue;
                var result = array[DefaultStart];


                Assert.AreEqual(inputValue, result);
            }

            [Test]
            public void IndexGet_ReturnsSetValue_GivenEndValueSet() {
                var indexer = new DiscreteTimeSeriesIndexer.Base(DefaultStart, DefaultEnd, DefaultResolution);
                var array = new IndexedArray<DateTime, int>(indexer);
                var inputValue = 5;


                array[DefaultEnd] = inputValue;
                var result = array[DefaultEnd];


                Assert.AreEqual(inputValue, result);
            }

            [Test]
            public void IndexGet_ReturnsSetValues_GivenAllValuesSet() {
                var indexer = new DiscreteTimeSeriesIndexer.Base(DefaultStart, DefaultEnd, DefaultResolution);
                var array = new IndexedArray<DateTime, int>(indexer);


                var inputValue = 0;
                var inputValues = new List<int>();
                for (var key = DefaultStart; key <= DefaultEnd; key = key.AddTicks(DefaultResolution.Ticks)) {
                    inputValues.Add(inputValue);
                    array[key] = inputValue;
                    inputValue++;
                }

                var results = new List<int>();
                for (var key = DefaultStart; key <= DefaultEnd; key = key.AddTicks(DefaultResolution.Ticks)) {
                    results.Add(
                        array[key]
                    );
                }


                Assert.AreEqual(inputValues, results);
            }

            // ToList takes a different code path via CopyTo when called on an ICollection vs an IEnumerable,
            // it is important to test both seperately
            [Test]
            public void Enumerate_YieldsAllEntries_GivenAllValuesSet() {
                var indexer = new DiscreteTimeSeriesIndexer.Base(DefaultStart, DefaultEnd, DefaultResolution);
                var array = new IndexedArray<DateTime, int>(indexer);


                var inputValue = 0;
                var inputValues = new List<KeyValuePair<DateTime, int>>();
                for (var key = DefaultStart; key <= DefaultEnd; key = key.AddTicks(DefaultResolution.Ticks)) {
                    inputValues.Add(new KeyValuePair<DateTime, int>(key, inputValue));
                    array[key] = inputValue;
                    inputValue++;
                }

                var results = array.Select(r => r).ToList();


                Assert.AreEqual(inputValues, results);
            }

            // ToList takes a different code path via CopyTo when called on an ICollection vs an IEnumerable,
            // it is important to test both seperately
            [Test]
            public void ToList_ReturnsAllEntries_GivenAllValuesSet() {
                var indexer = new DiscreteTimeSeriesIndexer.Base(DefaultStart, DefaultEnd, DefaultResolution);
                var array = new IndexedArray<DateTime, int>(indexer);


                var inputValue = 0;
                var inputValues = new List<KeyValuePair<DateTime, int>>();
                for (var key = DefaultStart; key <= DefaultEnd; key = key.AddTicks(DefaultResolution.Ticks)) {
                    inputValues.Add(new KeyValuePair<DateTime, int>(key, inputValue));
                    array[key] = inputValue;
                    inputValue++;
                }

                var results = array.ToList();


                Assert.AreEqual(inputValues, results);
            }
        }
    }
}
