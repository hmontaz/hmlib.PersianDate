using hmlib.PersianDate.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace hmlib.PersianDate.Globalization
{
	internal class JalaliDateTimeFormat
	{
		public static string Format(JalaliDateTime j, string? format, IFormatProvider? formatProvider)
		{
			format = GetStandardFormat(format, formatProvider);
			var formatInfo = JalaliDateTimeFormatInfo.GetInstance(formatProvider);
			//http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx
			string yyyy;
			if (j.Year >= 0)
				yyyy = j.Year.ToString().PadLeft(4, '0');
			else
				yyyy = '-' + (-j.Year).ToString().PadLeft(3, '0');

			string yyy;
			if (j.Year >= 0)
				yyy = j.Year.ToString().PadLeft(3, '0');
			else
				yyy = '-' + (-j.Year).ToString().PadLeft(2, '0');

			var yy = (j.Year % 100).ToString("D2");

			var y = (j.Year % 100).ToString();
			var M = j.Month;
			var d = j.Day;
			var tt = j.Hour < 12 ? formatInfo.AMDesignator : formatInfo.PMDesignator;
			var H = j.Hour;
			var h = j.Hour % 12 == 0 ? 12 : j.Hour % 12;

			var m = j.Minute;
			var s = j.Second;
			var ms = j.Millisecond;

			var tokenMap = new Dictionary<string, string>
			{
				["yyyy"] = yyyy,
				["yyy"] = yyy,
				["yy"] = yy,
				["y"] = y,

				["MMMM"] = formatInfo.MonthNames[j.Month - 1],
				["MMM"] = formatInfo.AbbreviatedMonthNames[j.Month - 1],
				["MM"] = M.ToString("D2"),
				["M"] = M.ToString(),

				["dddd"] = formatInfo.DayNames[(int)j.DayOfWeek],
				["ddd"] = formatInfo.AbbreviatedDayNames[(int)j.DayOfWeek],
				["dd"] = d.ToString("D2"),
				["d"] = d.ToString(),

				["HH"] = H.ToString("D2"),
				["H"] = H.ToString(),
				["hh"] = h.ToString("D2"),
				["h"] = h.ToString(),

				["mm"] = m.ToString("D2"),
				["m"] = m.ToString(),
				["ss"] = s.ToString("D2"),
				["s"] = s.ToString(),

				["fff"] = ms.ToString("D3").Substring(0, 3),
				["ff"] = ms.ToString("D3").Substring(0, 2),
				["f"] = ms.ToString("D3").Substring(0, 1),

				["tt"] = tt,
				["t"] = tt.Substring(0, 1)
			};

			// List of known format tokens
			var knownTokens = tokenMap.Keys.ToArray();

			// Tokenize and replace
			var tokens = StringTokenizer.Tokenize(format ?? "yyyy/MM/dd h:mm:ss tt", knownTokens);
			var sb = new StringBuilder();

			foreach (var token in tokens)
			{
				sb.Append(tokenMap.TryGetValue(token, out var value) ? value : token);
			}

			if (formatInfo.UsePersianDigits)
			{
				// Convert to Persian digits
				var persianDigits = new[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
				foreach (var digit in persianDigits.Select((d, i) => new { d, i }))
				{
					sb.Replace(digit.i.ToString(), digit.d.ToString());
				}
			}
			if (formatInfo.UsePersianComma)
			{
				sb.Replace(",", "،"); // Replace comma with Persian comma
			}
			return sb.ToString();
		}

		internal static string GetStandardFormat(string? format, IFormatProvider? formatProvider)
		{
			var formatInfo = JalaliDateTimeFormatInfo.GetInstance(formatProvider);
			switch (format)
			{
				case null:
				case "G":
					// General (long time) => short date + long time
					return formatInfo.ShortDatePattern + " " + formatInfo.LongTimePattern;
				case "g":
					// General (short time) => short date + short time
					return formatInfo.ShortDatePattern + " " + formatInfo.ShortTimePattern;
				case "d":
					return formatInfo.ShortDatePattern;
				case "D":
					return formatInfo.LongDatePattern;
				case "t":
					return formatInfo.ShortTimePattern;
				case "T":
					return formatInfo.LongTimePattern;
				case "f":
					// Full date (long) + short time
					return formatInfo.LongDatePattern + " " + formatInfo.ShortTimePattern;
				case "F":
					// Full date (long) + long time
					return formatInfo.LongDatePattern + " " + formatInfo.LongTimePattern;
				case "M":
				case "m":
					// Month/day pattern
					return formatInfo.MonthDayPattern;
				case "Y":
				case "y":
					// Year/month pattern
					return formatInfo.YearMonthPattern;
				/*case "O":
				case "o":
					// Round-trip pattern (ISO 8601)
					return "yyyy-MM-ddTHH:mm:ss.fffffffK";
				case "R":
				case "r":
					// RFC1123 pattern
					return "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
				case "s":
					// Sortable (ISO 8601)
					return "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
				case "u":
					// Universal sortable (UTC)
					return "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";*/
				case "U":
					// Universal full (long time, UTC)
					// Same as "F" but caller must convert DateTime to UTC
					return formatInfo.FullDateTimePattern;
				default:
					return format;
			}

		}
	}
}
