using hmlib.PersianDate.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace hmlib.PersianDate.Globalization
{
	internal static class JalaliDateTimeParse
	{
		internal static bool TryParseExact(string s, string format, IFormatProvider? formatProvider, DateTimeStyles style, bool strictDate, bool strictTime, out JalaliDateTime result)
		{
			format = Regex.Escape(JalaliDateTimeFormat.GetStandardFormat(format, formatProvider));

			var jalaliFormatInfo = JalaliDateTimeFormatInfo.GetInstance(formatProvider);
			var t = new[] { jalaliFormatInfo.ADesignator, jalaliFormatInfo.PDesignator };
			var tt = new[] { jalaliFormatInfo.ADesignator, jalaliFormatInfo.PDesignator, jalaliFormatInfo.AMDesignator, jalaliFormatInfo.PMDesignator };
			var tokenMap = new Dictionary<string, string>
			{
				["yyyyy"] = @"(?<yyyy>\d{5})",
				["yyyy"] = @"(?<yyyy>\d{4})",
				["yy"] = @"(?<yy>\d{2})",
				["MMMM"] = $@"(?<MMMM>{string.Join("|", jalaliFormatInfo.MonthNames)})",
				["MMM"] = $@"(?<MMM>{string.Join("|", jalaliFormatInfo.AbbreviatedMonthNames)})",
				["MM"] = strictDate ? @"(?<MM>\d{2})" : @"(?<MM>\d{1,2})",
				["M"] = strictDate ? @"(?<M>\d{1})" : @"(?<M>\d{1,2})",
				["dddd"] = $@"(?<dddd>{string.Join("|", jalaliFormatInfo.DayNames)})",
				["ddd"] = $@"(?<ddd>{string.Join("|", jalaliFormatInfo.AbbreviatedDayNames)})",
				["dd"] = strictTime ? @"(?<dd>\d{2})" : @"(?<dd>\d{1,2})",
				["d"] = strictTime ? @"(?<d>\d{1})" : @"(?<d>\d{1,2})",
				["hh"] = strictTime ? @"(?<hh>\d{2})" : @"(?<hh>\d{1,2})",
				["h"] = strictTime ? @"(?<h>\d{1})" : @"(?<h>\d{1,2})",
				["HH"] = strictTime ? @"(?<hh>\d{2})" : @"(?<hh>\d{1,2})",
				["H"] = strictTime ? @"(?<h>\d{1})" : @"(?<h>\d{1,2})",
				["mm"] = strictTime ? @"(?<mm>\d{2})" : @"(?<mm>\d{1,2})",
				["m"] = strictTime ? @"(?<m>\d{1})" : @"(?<m>\d{1,2})",
				["ss"] = strictTime ? @"(?<ss>\d{2})" : @"(?<ss>\d{1,2})",
				["s"] = strictTime ? @"(?<s>\d{1})" : @"(?<s>\d{1,2})",
				//["FFFFFFF"]
				//["FFFFFF"]
				//["FFFFF"]
				//["FFFF"]
				//["FFF"] = @"(?<FFF>\d{1,3})",
				//["FF"] = @"(?<FF>\d{1,2})",
				//["F"] = @"(?<F>\d{1})",
				["fffffff"] = strictTime ? @"(?<fff>\d{7})" : @"(?<fff>\d{1,7})",
				["ffffff"] = strictTime ? @"(?<fff>\d{6})" : @"(?<fff>\d{1,6})",
				["fffff"] = strictTime ? @"(?<fff>\d{5})" : @"(?<fff>\d{1,5})",
				["ffff"] = strictTime ? @"(?<fff>\d{4})" : @"(?<fff>\d{1,4})",
				["fff"] = strictTime ? @"(?<fff>\d{3})" : @"(?<fff>\d{1,3})",
				["ff"] = strictTime ? @"(?<ff>\d{2})" : @"(?<ff>\d{1,2})",
				["f"] = @"(?<f>\d{1})",
				["tt"] = $@"(?<tt>{string.Join("|", tt)})",
				["t"] = $@"(?<t>{string.Join("|", t)})",
				//["g"]
				//["gg"]
				//["K"]"K"	Time zone information.
				//"zzz"	Hours and minutes offset from UTC.
				//"zz"
				//"z"
			};
			s = normalizeDigits(s, jalaliFormatInfo);

			// Create a regex pattern that matches only format tokens (e.g., "yyyy", "dd")
			var tokenRegex = new Regex(string.Join("|", tokenMap.Keys
				.OrderByDescending(k => k.Length)
				.Select(Regex.Escape)));

			var pattern = tokenRegex.Replace(format, m => tokenMap[m.Value]);
			pattern = "^" + pattern + "$";
			var regex = new Regex(pattern);
			var match = regex.Match(s);

			if (!match.Success)
			{
				result = default;
				return false;
			}

			var groups = match.Groups;
			var year = getYear(groups);
			var month = getMonth(groups, jalaliFormatInfo);
			var day = getDay(groups);
			var hour = getHours(groups, jalaliFormatInfo);
			var minute = getMinutes(groups);
			var second = getSeconds(groups);
			var millisecond = getMilliseconds(groups);
			if (!strictDate)
			{
				var now = JalaliDateTime.Now;
				if (year == 0) year = now.Year;
				if (month == 0) month = now.Month;
				if (day == 0) day = now.Day;
			}

			try
			{
				result = new JalaliDateTime(year, month, day, hour, minute, second, millisecond);
				return true;
			}
			catch (ArgumentOutOfRangeException)
			{
				result = new JalaliDateTime();
				return false;
			}
			catch
			{
				throw;
			}
		}

		internal static JalaliDateTime ParseExact(string s, string format, DateTimeFormatInfo dateTimeFormatInfo, DateTimeStyles none)
		{
			if (s == null) throw new ArgumentNullException(nameof(s));
			if (format == null) throw new ArgumentNullException(nameof(format));
			if (!TryParseExact(s, format, dateTimeFormatInfo, none, strictDate: false, strictTime: false, out var result))
			{
				throw new FormatException($"The string '{s}' is not in the correct format for '{format}'.");
			}
			return result;

		}

		public static bool TryParse(string s, out JalaliDateTime result)
		{
			return TryParse(s, null, DateTimeStyles.None, out result);
		}

		internal static bool TryParse(string s, IFormatProvider? formatProvider, DateTimeStyles styles, out JalaliDateTime result)
		{
			if (s == null) throw new ArgumentNullException(nameof(s));
			var dateFormats = new[] { "yyyy|MM|dd", "yyyy|M|dd", "yyyy|MM|d", "yyyy|M|d", "yy|MM|dd", "yy|M|dd", "yy|MM|d", "yy|M|d", "" };
			var timeFormats = new[] { "HH:mm:ss.fff", "HH:mm:ss.ff", "HH:mm:ss.f", "HH:mm:ss", "HH:mm", "HH:mm:ss.fff tt", "HH:mm:ss.ff tt", "HH:mm:ss.f tt", "HH:mm:ss tt", "HH:mm tt", "" };

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

		static string normalizeDigits(string s, JalaliDateTimeFormatInfo jalaliFormatInfo)
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

		static int getYear(GroupCollection groups)
		{
			var yy = groups.GetInt32("yy");
			if (yy != null) yy = yy + 1300;

			return groups.GetInt32("yyyy") ?? yy ?? 0;
		}

		static int getMonth(GroupCollection groups, JalaliDateTimeFormatInfo formatProvider)
		{
			if (!string.IsNullOrEmpty(groups["MMMM"].Value))
			{
				var months = formatProvider.MonthNames.Select(a => a.ToLower()).ToArray();
				var key = groups["MMMM"].Value.Trim().ToLower();
				return Array.IndexOf(months, key) + 1;
			}
			if (!string.IsNullOrEmpty(groups["MMM"].Value))
			{
				var months = formatProvider.AbbreviatedMonthNames.Select(a => a.ToLower()).ToArray();
				var key = groups["MMM"].Value.Trim().ToLower();
				return Array.IndexOf(months, key) + 1;
			}
			return groups.GetInt32("MM") ?? groups.GetInt32("M") ?? 0;
		}

		static int getDay(GroupCollection groups)
		{
			return groups.GetInt32("dd") ?? groups.GetInt32("d") ?? 0;
		}

		static int getHours(GroupCollection groups, JalaliDateTimeFormatInfo formatProvider)
		{
			var tt = groups["tt"].Value.IfNullOrEmpty(groups["tt"].ToString()).ToUpper();
			var isPM = tt == formatProvider.PMDesignator || tt == formatProvider.PDesignator;
			var isAM = tt == formatProvider.AMDesignator || tt == formatProvider.ADesignator;

			var h = groups.GetInt32("hh") ?? groups.GetInt32("h") ?? 0;
			if (isPM) return h + 12;
			if (isAM) return h % 12;
			return h;
		}

		static int getMinutes(GroupCollection groups)
		{
			return groups.GetInt32("mm") ?? groups.GetInt32("m") ?? 0;
		}

		static int getSeconds(GroupCollection groups)
		{
			return groups.GetInt32("ss") ?? groups.GetInt32("s") ?? 0;
		}

		static int getMilliseconds(GroupCollection groups)
		{
			var s = groups.GetValue("fff", "ff", "f");
			if (s == null) return 0;
			if (decimal.TryParse("." + s, out var ms)) return (int)(ms * 1000);

			return 0;
		}

	}
}
