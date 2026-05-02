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

            int patternLength = pattern.Length;
            int textLength = text.Length;

            if (patternLength > textLength) return result;

            int[] badCharTable = BuildBadCharTable(pattern);

            int shift = 0;

            while (shift <= textLength - patternLength)
            {
                int comparisonIndex = patternLength - 1;

                while (comparisonIndex >= 0 && pattern[comparisonIndex] == text[shift + comparisonIndex])
                    comparisonIndex--;

                if (comparisonIndex < 0)
                {
                    result.Add(shift);
                }
                else
                {
                    int badCharShift = comparisonIndex - badCharTable[text[shift + comparisonIndex]];
                    shift += Math.Max(1, badCharShift);
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
    }
}