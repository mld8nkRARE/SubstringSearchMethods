using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstringSearch
{
    public class RabinKarpAlgorithm : ISubstringSearch
    {

        public List<int> IndexesOf(string pattern, string text)
        {
            var result = new List<int>();
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
                return result;


            const int alphabetSize = 256;
            const int mod = 35317;

            long patternHash = 0;
            long textHash = 0;
            long firstIndexHash = 1; //p^m-1

            int patternLen = pattern.Length;
            int maxIndex = text.Length - patternLen;

            for (int i = 0; i < patternLen; i++)
            {
                patternHash = (patternHash * alphabetSize + pattern[i]) % mod;
                textHash = (textHash * alphabetSize + text[i]) % mod;

                if (i < patternLen - 1)
                    firstIndexHash = (firstIndexHash * alphabetSize) % mod;
            }

            for (int i = 0; i <= maxIndex; i++)
            {

                if (patternHash == textHash)
                {
                    bool match = true;
                    for (int j = 0; j < patternLen; j++)
                    {
                        if (pattern[j] != text[i + j])
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                        result.Add(i);
                }

                if (i < maxIndex)
                {
                    textHash = ((textHash - text[i] * firstIndexHash % mod + mod) * alphabetSize+ text[i + patternLen]) % mod;
                }
            }

            return result;
        }
    }
}
