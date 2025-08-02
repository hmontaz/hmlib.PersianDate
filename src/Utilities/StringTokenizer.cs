using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hmlib.PersianDate.Utilities
{
	internal class StringTokenizer
	{
		internal static string[] Tokenize(string s, string[] knownTokens)
		{
			knownTokens = knownTokens.OrderByDescending(t => t.Length).ToArray();
			var result = new List<string>();
			int i = 0;

			while (i < s.Length)
			{
				// Try to match the longest known token at this position
				string match = knownTokens
					.OrderByDescending(t => t.Length)
					.FirstOrDefault(t => i + t.Length <= s.Length && s.Substring(i, t.Length) == t);

				if (match != null)
				{
					result.Add(match);
					i += match.Length;
				}
				else
				{
					// It's a literal (e.g., '/', ':', ' ')
					result.Add(s[i].ToString());
					i++;
				}
			}

			return result.ToArray();
		}
	}
}
