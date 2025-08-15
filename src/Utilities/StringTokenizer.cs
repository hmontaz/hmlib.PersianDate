using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hmlib.PersianDate.Utilities
{
	internal class StringTokenizer
	{
		internal struct Token
		{
			public string Value { get; }
			public bool IsLiteral { get; }

			public Token(string value, bool isLiteral)
			{
				Value = value;
				IsLiteral = isLiteral;
			}
		}
		internal static string[] KnownTokens = new[]
					{
						"yyyy", "yyy", "yy", "y",
						"MMMM", "MMM", "MM", "M",
						"dddd", "ddd", "dd", "d",
						"HH", "H", "hh", "h",
						"mm", "m", "ss", "s",
						"fffffff", "ffffff", "fffff", "ffff", "fff", "ff", "f",
						"tt", "t"
					};
		internal static Token[] Tokenize(string format, string[] knownTokens)
		{
			knownTokens ??= StringTokenizer.KnownTokens;
			knownTokens = knownTokens.OrderByDescending(t => t.Length).ToArray();

			var tokens = new List<Token>();
			int i = 0;

			while (i < format.Length)
			{
				char c = format[i];

				// 1) Quoted literals: '...' or "..."
				if (c == '\'' || c == '\"')
				{
					char quote = c;
					int start = i++;
					while (i < format.Length)
					{
						if (format[i] == quote)
						{
							if (i + 1 < format.Length && format[i + 1] == quote)
							{
								i += 2; // escaped quote
								continue;
							}
							i++;
							break;
						}
						i++;
					}
					tokens.Add(new Token(format.Substring(start, i - start), true));
					continue;
				}

				// 2) Backslash escapes
				if (c == '\\')
				{
					if (i + 1 < format.Length)
					{
						tokens.Add(new Token(format.Substring(i, 2), true));
						i += 2;
					}
					else
					{
						tokens.Add(new Token("\\", true));
						i++;
					}
					continue;
				}

				// 3) Try to match a known token
				string match = knownTokens.FirstOrDefault(t =>
					i + t.Length <= format.Length &&
					format.Substring(i, t.Length) == t);

				if (match != null)
				{
					tokens.Add(new Token(match, false));
					i += match.Length;
					continue;
				}

				// 4) Fallback: treat as literal
				tokens.Add(new Token(format[i].ToString(), true));
				i++;
			}

			return tokens.ToArray();
		}
	}
}
