using SubstringSearch;
using System.Diagnostics;
using System.Text;

class Program
{
    static void Main()
    {
        string pattern = "анна";

        string text = File.ReadAllText("anna.txt", Encoding.UTF8);

        var algorithms = new List<ISubstringSearch>
        {
            new BruteForceAlgorithm(),
            new RabinKarpAlgorithm(),
            new KMPAlgorithm(),
            new BoyerMooreAlgorithm()
        };

        foreach (var alg in algorithms)
        {
            var sw = Stopwatch.StartNew();
            var result = alg.IndexesOf(pattern, text);
            sw.Stop();

            Console.WriteLine($"{alg.GetType().Name}:");
            Console.WriteLine($"Найдено: {result.Count}");
            Console.WriteLine($"Время: {sw.ElapsedMilliseconds} ms");
            Console.WriteLine();
        }
    }
}