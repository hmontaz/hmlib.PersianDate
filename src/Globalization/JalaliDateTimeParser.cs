using hmlib.PersianDate.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace hmlib.PersianDate.Globalization
{
	internal class JalaliDateTimeParser : IJalaliDateTimeParse
	{
		public JalaliDateTime ParseExact(string s, string format, DateTimeFormatInfo dateTimeFormatInfo, DateTimeStyles none)
		{
			if (s == null) throw new ArgumentNullException(nameof(s));
			if (format == null) throw new ArgumentNullException(nameof(format));
			if (!TryParseExact(s, format, dateTimeFormatInfo, none, strictDate: false, strictTime: false, out var result))
			{
				throw new FormatException($"The string '{s}' is not in the correct format for '{format}'.");
			}
			return result;

		}

		public bool TryParse(string s, out JalaliDateTime result)
		{
			return TryParse(s, null, DateTimeStyles.None, out result);
		}

		public bool TryParse(string s, IFormatProvider? formatProvider, DateTimeStyles styles, out JalaliDateTime result)
		{
			if (s == null) throw new ArgumentNullException(nameof(s));
			//var dateFormats = new[] { "yyyy|MM|dd", "yyyy|M|dd", "yyyy|MM|d", "yyyy|M|d", /*"yy|MM|dd", "yy|M|dd", "yy|MM|d", "yy|M|d", "" };
			var dateFormats = new[] { "yyyy|M|d", "yy|M|d", "" };
			//var timeFormats = new[] { "HH:mm:ss.fff", "HH:mm:ss.ff", "HH:mm:ss.f", "HH:mm:ss", "HH:mm", "HH:mm:ss.fff tt", "HH:mm:ss.ff tt", "HH:mm:ss.f tt", "HH:mm:ss tt", "HH:mm tt", "" };
			var timeFormats = new[] {
				"H:m:s.f tt", "H:m:s.f"
				, "H:m:s tt", "H:m:s"
				, "h:m:s tt", "h:m:s"
				, "H:m tt", "H:m"
				, "h:m tt", "h:m"
				, "" };

			foreach (var timeFormat in timeFormats)
			{
				foreach (var dateSeparator in new[] { "/", "-", ".", " " })
				{
					foreach (var dateFormat in dateFormats)
					{
						var df = dateFormat.Replace("|", dateSeparator);
						var format = $"{df} {timeFormat}".Trim();
						if (format == "") continue;
						if (TryParseExact(s, format, formatProvider, styles, strictDate: dateFormat != "", strictTime: false, out result))
						{
							return true;
						}
					}
				}
			}

			result = default;
			return false;
		}

		string normalizeDigits(string s, JalaliDateTimeFormatInfo jalaliFormatInfo)
		{
			if (jalaliFormatInfo.UsePersianDigits)
			{
				s = s.Replace("۰", "0")
					.Replace("۱", "1")
					.Replace("۲", "2")
					.Replace("۳", "3")
					.Replace("۴", "4")
					.Replace("۵", "5")
					.Replace("۶", "6")
					.Replace("۷", "7")
					.Replace("۸", "8")
					.Replace("۹", "9");
			}
			if (jalaliFormatInfo.UsePersianComma)
			{
				s = s.Replace("،", ",");
			}
			return s;
		}

		class CharReader
		{
			private readonly string _s;
			private int _pos;

			public CharReader(string s) => _s = s;

			public bool ReadInt(int digits, bool strict, out int value)
			{
				if (_pos + digits > _s.Length && strict) throw new FormatException();
				if (_pos + 1 > _s.Length && !strict) throw new FormatException();
				value = 0;
				for (int i = 0; i < digits; i++)
				{
					if (!strict && _pos >= _s.Length) break;
					char c = _s[_pos++];
					if (c < '0' || c > '9')
					{
						if (!strict && i > 0)
						{
							_pos--;
							return true;
						}
						return false;
					}
					value = value * 10 + (c - '0');
				}
				return true;
			}

			public bool ReadDigits(int digits, bool strict, out string value)
			{
				static bool failed(out string value)
				{
					value = "";
					return false;
				}
				static bool success(StringBuilder sb, out string value)
				{
					value = sb.ToString();
					return true;
				}
				if (_pos + digits > _s.Length && strict) throw new FormatException();
				if (_pos + 1 > _s.Length && !strict) throw new FormatException();
				var sb = new StringBuilder();
				for (int i = 0; i < digits; i++)
				{
					if (!strict && _pos >= _s.Length) break;
					char c = _s[_pos++];
					if (c < '0' || c > '9')
					{
						if (!strict && i > 0)
						{
							_pos--;
							return success(sb, out value);
						}
						return failed(out value);
					}
					sb.Append(c);
				}
				return success(sb, out value);
			}

			public bool ReadLiteral(out int index, params string[] literals)
			{
				var maxLen = literals.Max(l => l.Length);
				var dictionary = literals.Select(a => a.PadRight(maxLen)).ToDictionary(a => a, a => false);//literal/failed
				var end = Math.Min(_s.Length, _pos + maxLen);
				for (int i = _pos; i < end; i++)
				{
					foreach (var literal in dictionary.Keys.ToList())
					{
						//already failed
						if (dictionary[literal]) continue;
						//if literal's current character does not match, mark as failed
						if (_s[i] != literal[i - _pos])
						{
							dictionary[literal] = true;
							continue;
						}
						//if reached the end of literal, or a space in literal, it's a match
						if ((i - _pos) >= (literal.Length - 1) || literal[i - _pos] == ' ')
						{
							_pos += literal.TrimEnd().Length;
							index = Array.IndexOf(literals, literal.TrimEnd());
							return true;
						}

					}
				}
				index = -1;
				return false;
			}

			public bool Expect(string literal)
			{
				if (_s.Substring(_pos).StartsWith(literal))
				{
					_pos += literal.Length;
					return true;
				}
				return false;
			}

			public bool End => _pos >= _s.Length;

		}

		bool IsLiteral(string token) => !char.IsLetter(token[0]);

		public bool TryParseExact(string input, string format, IFormatProvider? provider, DateTimeStyles style, bool strictDate, bool strictTime, out JalaliDateTime result)
		{
			format = JalaliDateTimeFormat.GetStandardFormat(format, provider);
			// Step 1: Normalize
			var jalaliInfo = JalaliDateTimeFormatInfo.GetInstance(provider);

			input = normalizeDigits(input, jalaliInfo); // handle Persian/Arabic numerals

			// Step 2: Tokenize format (naively, or cache later)
			var tokens = StringTokenizer.Tokenize(format, StringTokenizer.KnownTokens).Select(a => a.Value);

			// Step 3: Parse input based on tokens
			var reader = new CharReader(input);


			int Year = 0, Month = 0, Day = 0;
			int Hour = 0, Minute = 0, Second = 0, Millisecond = 0;
			bool IsPM = false;
			bool IsAM = false;

			static bool failed(out JalaliDateTime result)
			{
				result = default;
				return false;
			}
			static int getMilliSecond(string f)
			{
				if (!decimal.TryParse("." + f, out var ms)) throw new FormatException($"Invalid fractional part: {f}");
				return (int)(ms * 1000);
			}

			foreach (var token in tokens)
			{
				if (IsLiteral(token))
				{
					if (!reader.Expect(token)) return failed(out result);
					continue;
				}

				switch (token)
				{
					case "yyyy":
						if (!reader.ReadInt(4, true, out var yyyy)) return failed(out result);
						Year = yyyy;
						break;
					case "yy":
						if (!reader.ReadInt(2, true, out var yy)) return failed(out result);
						if (yy < 0 || yy > 99) return failed(out result);
						Year = PivotTwoDigitYear(yy);
						break;
					case "y":
						if (!reader.ReadInt(2, false, out var y)) return failed(out result);
						if (y < 0 || y > 99) return failed(out result);
						Year = PivotTwoDigitYear(y);
						break;
					case "MMMM":
						if (!reader.ReadLiteral(out int index, jalaliInfo.MonthNames)) return failed(out result);
						Month = index + 1;
						break;
					case "MMM":
						if (!reader.ReadLiteral(out int monthIndex, jalaliInfo.AbbreviatedMonthNames)) return failed(out result);
						Month = monthIndex + 1;
						break;
					case "MM":
						if (!reader.ReadInt(2, true, out var MM)) return failed(out result);
						Month = MM;
						break;
					case "M":
						if (!reader.ReadInt(2, false, out var M)) return failed(out result);
						Month = M;
						break;
					case "dddd":
						if (!reader.ReadLiteral(out int dayIndex, jalaliInfo.DayNames)) return failed(out result);
						//do nothing, ignore day of week
						break;
					case "dd":
						if (!reader.ReadInt(2, true, out var dd)) return failed(out result);
						if (dd < 1 || dd > 31) return failed(out result);
						Day = dd;
						break;
					case "d":
						if (!reader.ReadInt(2, false, out var d)) return failed(out result);
						if (d < 1 || d > 31) return failed(out result);
						Day = d;
						break;
					case "HH":
						if (!reader.ReadInt(2, true, out var HH)) return failed(out result);
						Hour = HH;
						break;
					case "H":
						if (!reader.ReadInt(2, false, out var H)) return failed(out result);
						Hour = H;
						break;
					case "hh":
						if (!reader.ReadInt(2, false, out var hh)) return failed(out result);
						Hour = hh;
						break;
					case "h":
						if (!reader.ReadInt(2, false, out var h)) return failed(out result);
						Hour = h;
						break;
					case "mm":
						if (!reader.ReadInt(2, false, out var mm)) return failed(out result);
						Minute = mm;
						break;
					case "m":
						if (!reader.ReadInt(2, false, out var m)) return failed(out result);
						Minute = m;
						break;
					case "ss":
						if (!reader.ReadInt(2, false, out var ss)) return failed(out result);
						Second = ss;
						break;
					case "s":
						if (!reader.ReadInt(2, false, out var s)) return failed(out result);
						Second = s;
						break;
					case "fffffff":
						if (!reader.ReadDigits(7, true, out var f7)) return failed(out result);
						Millisecond = getMilliSecond(f7);
						break;
					case "ffffff":
						if (!reader.ReadDigits(6, true, out var f6)) return failed(out result);
						Millisecond = getMilliSecond(f6);
						break;
					case "fffff":
						if (!reader.ReadDigits(5, true, out var f5)) return failed(out result);
						Millisecond = getMilliSecond(f5);
						break;
					case "ffff":
						if (!reader.ReadDigits(4, true, out var f4)) return failed(out result);
						Millisecond = getMilliSecond(f4);
						break;
					case "fff":
						if (!reader.ReadDigits(3, true, out var f3)) return failed(out result);
						Millisecond = getMilliSecond(f3);
						break;
					case "ff":
						if (!reader.ReadDigits(2, true, out var f2)) return failed(out result);
						Millisecond = getMilliSecond(f2);
						break;
					case "f":
						if (!reader.ReadDigits(8, false, out var f)) return failed(out result);
						Millisecond = getMilliSecond(f);
						break;
					case "tt":
						if (!reader.ReadLiteral(out int ampmIndex, jalaliInfo.AMDesignator, jalaliInfo.PMDesignator))
							return failed(out result);
						IsAM = ampmIndex == 0;
						IsPM = ampmIndex == 1;
						break;
					case "t":
						if (!reader.ReadLiteral(out int apIndex, jalaliInfo.ADesignator, jalaliInfo.PDesignator))
							return failed(out result);
						IsAM = apIndex == 0;
						IsPM = apIndex == 1;
						break;

					default:
						return failed(out result);
				}
			}

			// Step 4: Fix up AM/PM
			if (IsPM && Hour < 12) Hour += 12;
			if (IsAM) Hour = Hour % 12;

			if (!strictDate)
			{
				var now = JalaliDateTime.Now;
				if (Year == 0) Year = now.Year;
				if (Month == 0) Month = now.Month;
				if (Day == 0) Day = now.Day;
			}

			// Step 6: Construct JalaliDateTime
			try
			{
				result = new JalaliDateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);
				return true;
			}
			catch
			{
				return failed(out result);
			}
		}

		int PivotTwoDigitYear(int yy)
		{
			var jalaliYear = DateTime.Now.Year - 621;
			int currentCentury = (jalaliYear / 100) * 100;
			int pivot = jalaliYear % 100;

			return (yy <= pivot) ? currentCentury + yy : currentCentury - 100 + yy;
		}
	}
}
