using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstringSearch
{
    public class BoyerMooreAlgorithm : ISubstringSearch
    {
        private const int AlphabetSize = char.MaxValue + 1;

        public List<int> IndexesOf(string pattern, string text)
        {
            var result = new List<int>();

            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(text))
                return result;

            int m = pattern.Length;
            int n = text.Length;

            if (m > n) return result;

            int[] badChar = BuildBadCharTable(pattern);
            int[] goodSuffix = BuildGoodSuffixTable(pattern);

            int shift = 0;

            while (shift <= n - m)
            {
                int j = m - 1;

                while (j >= 0 && pattern[j] == text[shift + j])
                    j--;

                if (j < 0)
                {
                    result.Add(shift);
                    shift += goodSuffix[0];
                }
                else
                {
                    int badCharShift = j - badChar[text[shift + j]];
                    int goodSuffixShift = goodSuffix[j];

                    shift += Math.Max(1, Math.Max(badCharShift, goodSuffixShift));
                }
            }

            return result;
        }

        private int[] BuildBadCharTable(string pattern)
        {
            int[] table = new int[AlphabetSize];

            for (int i = 0; i < AlphabetSize; i++)
                table[i] = -1;

            for (int i = 0; i < pattern.Length; i++)
                table[pattern[i]] = i;

            return table;
        }

        private int[] BuildGoodSuffixTable(string pattern)
        {
            int m = pattern.Length;
            int[] goodSuffix = new int[m];
            int[] suffixes = new int[m];

            ComputeSuffixes(pattern, suffixes);

            for (int i = 0; i < m; i++)
                goodSuffix[i] = m;

            int j = 0;
            for (int i = m - 1; i >= 0; i--)
            {
                if (suffixes[i] == i + 1)
                {
                    for (; j < m - 1 - i; j++)
                    {
                        if (goodSuffix[j] == m)
                            goodSuffix[j] = m - 1 - i;
                    }
                }
            }

            for (int i = 0; i <= m - 2; i++)
                goodSuffix[m - 1 - suffixes[i]] = m - 1 - i;

            return goodSuffix;
        }

        private void ComputeSuffixes(string pattern, int[] suffixes)
        {
            int m = pattern.Length;
            suffixes[m - 1] = m;

            int g = m - 1;
            int f = 0;

            for (int i = m - 2; i >= 0; i--)
            {
                if (i > g && suffixes[i + m - 1 - f] < i - g)
                {
                    suffixes[i] = suffixes[i + m - 1 - f];
                }
                else
                {
                    if (i < g)
                        g = i;

                    f = i;

                    while (g >= 0 && pattern[g] == pattern[g + m - 1 - f])
                        g--;

                    suffixes[i] = f - g;
                }
            }
        }
    }
}
