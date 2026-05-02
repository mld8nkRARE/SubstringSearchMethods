using SubstringSearch;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace UnitTests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void SearchBagOfWordsOnAnnaTxt()
        {
            var algms = new List<ISubstringSearch>()
            {
                new BruteForceAlgorithm(),
                new RabinKarpAlgorithm(),
                new KMPAlgorithm(),
                new BoyerMooreAlgorithm()
            };
            string text;
            using (var sr = new StreamReader("anna.txt", Encoding.UTF8))
            {
                text = sr.ReadToEnd().ToLower();
            }

            int number = 100;
            Regex rg = new Regex(@"\w+");
            var bag = new HashSet<string>();
            var matches = rg.Matches(text);
            foreach (var match in matches)
            {
                bag.Add(match.ToString());
                if (bag.Count > number) break;
            }
            foreach (var pattern in bag)
            {
                var BF = new BruteForceAlgorithm();
                var expected = BF.IndexesOf(pattern, text);
                foreach (var algm in algms)
                {
                    var actual = algm.IndexesOf(pattern, text);
                    CollectionAssert.AreEqual(expected, actual);
                }
            }
        }

    }
}
