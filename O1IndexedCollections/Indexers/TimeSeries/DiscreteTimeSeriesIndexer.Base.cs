using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace O1IndexedCollections.Indexers.TimeSeries {
    public partial class DiscreteTimeSeriesIndexer {
        // todo - base indexers can actually grow (allowing larger values should be easy, smaller is much more involved) - consider implementing
        public class Base : IIndexer.Base<DateTime>, IIndexLibrary<DateTime> {
            public DateTime StartTime { get; }
            public DateTime EndTime { get; }
            public Resolution Resolution { get; }
            private readonly int numberOfInsignificantBits;

            public IIndexer.Base<DateTime> Indexer1 => this;
            public IEnumerable<(DateTime, uint)> Indexes => this.Select(key => (key, (uint)GetIndex(StartTime.Ticks, key.Ticks, numberOfInsignificantBits)));


            public Base(DateTime startTime, DateTime endTime, Resolution resolution) {
                StartTime = startTime;
                EndTime = endTime;
                Resolution = resolution;
                numberOfInsignificantBits = (int)Math.Log(resolution.Ticks, 2);
            }


            public uint GetOrAdd(DateTime key) {// todo consider checked/unchecked overloads for alignment and out of bounds
                return (uint)GetIndex(StartTime.Ticks, key.Ticks, numberOfInsignificantBits);
            }

            public bool TryGet(DateTime key, out uint index) {//todo handle out of bounds, unaligned etc
                index = (uint)GetIndex(StartTime.Ticks, key.Ticks, numberOfInsignificantBits);
                return true;
            }

            public IEnumerator<DateTime> GetEnumerator() {
                for (var key = StartTime; key <= EndTime; key = key.AddTicks(Resolution.Ticks)) {
                    yield return key;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public DateTime GetKeyFromIndex(int index) => new DateTime(Resolution.Align(index << numberOfInsignificantBits) + StartTime.Ticks);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static long GetIndex(long startTicks, long endTicks, int numberOfInsignificantBits)
                => (endTicks - startTicks) >> numberOfInsignificantBits;
        }
    }
}
