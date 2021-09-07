namespace O1IndexedCollections.Indexers.TimeSeries {
    public struct Resolution {
        public readonly long Ticks;

        public Resolution(long ticks) {
            Ticks = ticks;
        }
    }

    public static class ResolutionExtensions {
        public static long Align(this Resolution resolution, long ticks) => (ticks / resolution.Ticks) * resolution.Ticks;
    }
}
