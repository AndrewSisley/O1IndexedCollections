using BenchmarkDotNet.Running;

namespace O1IndexedCollections.Benchmarks {
    public class Program {
        static void Main(string[] args) => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
