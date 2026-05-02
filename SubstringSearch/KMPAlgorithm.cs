using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstringSearch
{
    public class KMPAlgorithm : ISubstringSearch
    {
        public List<int> IndexesOf(string pattern, string text)
        {
            List<int> result = new List<int>();
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(text))
                return result;

            int[] borders = FindBorders(pattern);
            int patternLen = pattern.Length;
            int textLen = text.Length;
            int compareIndex = 0;
            for (int i = 0; i < textLen; i++)
            {
                while (compareIndex > 0 && text[i] != pattern[compareIndex])
                {
                    compareIndex = borders[compareIndex - 1];
                }

                if (text[i] == pattern[compareIndex])
                {
                    compareIndex++;
                }

                if (compareIndex == patternLen)
                {
                    result.Add(i - patternLen + 1);
                    compareIndex = borders[patternLen - 1];
                }
            }
            
            return result;
        }

        private int[] FindBorders(string pattern)
        {
            int m = pattern.Length;
            int[] borders = new int[m];
            int borderLength = 0;

            for (int i = 1; i < m; i++)
            {
                while (borderLength > 0 && pattern[i] != pattern[borderLength])
                {
                    borderLength = borders[borderLength - 1];
                }

                if (pattern[i] == pattern[borderLength])
                {
                    borderLength++;
                }

                borders[i] = borderLength;
            }

            return borders;
        }
    }
}
