using hmlib.PersianDate.Globalization;
using hmlib.PersianDate.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace hmlib.PersianDate
{
	internal class JalaliDateTimeFormat
	{
		public static string Format(JalaliDateTime j, string? format, IFormatProvider? formatProvider)
		{
			format = getFormat(format, formatProvider);
			var formatInfo = new JalaliDateTimeFormatInfo(formatProvider);
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
			var h = (j.Hour % 12) == 0 ? 12 : (j.Hour % 12);

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

		internal static string[] GetMonthNames(IFormatProvider? formatProvider)
		{
			var formatInfo = getFormatInfo(formatProvider);
			return formatInfo.MonthNames.ToArray();
		}

		private static string getFormat(string? format, IFormatProvider? formatProvider)
		{
			switch (format)
			{
				case null:
				case "G":
					var formatInfo = getFormatInfo(formatProvider);
					return formatInfo.ShortDatePattern + " " + formatInfo.LongTimePattern;
				case "d":
					return getFormatInfo(formatProvider).ShortDatePattern;
				case "D":
					return getFormatInfo(formatProvider).LongDatePattern;
				case "t":
					return getFormatInfo(formatProvider).ShortTimePattern;
				case "T":
					return getFormatInfo(formatProvider).LongTimePattern;
				default:
					return format;
			}

		}

		private static JalaliDateTimeFormatInfo getFormatInfo(IFormatProvider? formatProvider)
		{
			return new JalaliDateTimeFormatInfo(formatProvider);
		}
	}
}
