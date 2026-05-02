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
            int[] goodSuffixTable = BuildGoodSuffixTable(pattern);

            int shift = 0;

            while (shift <= textLength - patternLength)
            {
                int comparisonIndex = patternLength - 1;

                while (comparisonIndex >= 0 && pattern[comparisonIndex] == text[shift + comparisonIndex])
                    comparisonIndex--;

                if (comparisonIndex < 0)
                {
                    result.Add(shift);
                    shift += goodSuffixTable[0];
                }
                else
                {
                    int badCharShift = comparisonIndex - badCharTable[text[shift + comparisonIndex]];
                    int goodSuffixShift = goodSuffixTable[comparisonIndex];

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
            int patternLength = pattern.Length;
            int[] goodSuffix = new int[patternLength];
            int[] suffixes = new int[patternLength];

            ComputeSuffixes(pattern, suffixes);

            for (int i = 0; i < patternLength; i++)
                goodSuffix[i] = patternLength;

            int suffixIndex = 0;
            for (int i = patternLength - 1; i >= 0; i--)
            {
                if (suffixes[i] == i + 1)
                {
                    for (; suffixIndex < patternLength - 1 - i; suffixIndex++)
                    {
                        if (goodSuffix[suffixIndex] == patternLength)
                            goodSuffix[suffixIndex] = patternLength - 1 - i;
                    }
                }
            }

            for (int i = 0; i <= patternLength - 2; i++)
                goodSuffix[patternLength - 1 - suffixes[i]] = patternLength - 1 - i;

            return goodSuffix;
        }

        private void ComputeSuffixes(string pattern, int[] suffixes)
        {
            int patternLength = pattern.Length;
            suffixes[patternLength - 1] = patternLength;

            int matchEnd = patternLength - 1;
            int matchStart = 0;                    

            for (int currentIndex = patternLength - 2; currentIndex >= 0; currentIndex--)
            {
                if (currentIndex > matchEnd && suffixes[currentIndex + patternLength - 1 - matchStart] < currentIndex - matchEnd)
                {
                    suffixes[currentIndex] = suffixes[currentIndex + patternLength - 1 - matchStart];
                }
                else
                {
                    if (currentIndex < matchEnd)
                        matchEnd = currentIndex;

                    matchStart = currentIndex;

                    while (matchEnd >= 0 && pattern[matchEnd] == pattern[matchEnd + patternLength - 1 - matchStart])
                        matchEnd--;

                    suffixes[currentIndex] = matchStart - matchEnd;
                }
            }
        }
    }
}