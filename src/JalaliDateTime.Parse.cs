using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using hmlib.PersianDate.Utilities;

namespace hmlib.PersianDate
{
	public partial struct JalaliDateTime
	{
		public static JalaliDateTime Parse(string str)
		{
			//Cleanup
			str = str.Trim();
			while (str.Contains("  "))
			{
				str.Replace("  ", " ");
			}
			string dateStr = str.Split(' ')[0];
			int dot_count = dateStr.Count(c => c == '.');
			int dash_count = dateStr.Count(c => c == '-');
			int slash_count = dateStr.Count(c => c == '/');

			if (dot_count == 2)
				if (dash_count != 0 || slash_count != 0) throw new FormatException("Invalid Format!");
			if (dash_count == 2)
				if (dot_count != 0 || slash_count != 0) throw new FormatException("Invalid Format!");
			if (slash_count == 2)
				if (dot_count != 0 || dash_count != 0) throw new FormatException("Invalid Format!");
			if (dot_count != 2 && dash_count != 2 && slash_count != 2) throw new FormatException("Invalid Format!");

			string[] datePart = dateStr.Replace("-", "/").Replace(".", "/").Split('/');
			if (datePart.Length != 3)
			{
				throw new FormatException("Invalid Format");
			}
			int year = Convert.ToInt16(datePart[0]);
			int month = Convert.ToInt16(datePart[1]);
			int day = Convert.ToInt16(datePart[2]);
			int hour = 0;
			int minute = 0;
			int second = 0;
			int msec = 0;
			if (str.Split(' ').Length == 2)
			{
				string[] timePart = str.Split(' ')[1].Split(':');
				hour = int.Parse(timePart[0]);
				minute = int.Parse(timePart[1]);
				if (timePart.Length == 3)
				{
					double sec = double.Parse(timePart[2]);
					second = (int)Math.Floor(sec);
					//msec = (int)(Math.Truncate(sec).GetFraction() * 1000);
					//msec = (int)((sec - Math.Floor(sec)) * 1000);
					msec = (int)((decimal)(sec - Math.Floor(sec)) * 1000);

				}
			}
			if (year < 100)
			{
				var currentYear = JalaliDateTime.Now.Year;
				var era = (JalaliDateTime.Today.Year / 100) * 100;
				if (era + year - currentYear > 31) era -= 100;
				year += era;
			}
			return new JalaliDateTime(year, month, day, hour, minute, second, msec);
		}

		public static bool TryParse(string str, out JalaliDateTime result)
		{
			try
			{
				result = JalaliDateTime.Parse(str);
				return true;
			}
			catch
			{
				result = new JalaliDateTime();
				return false;
			}
		}

		/*public static JalaliDateTime? TryParse(string str)
		{
			try
			{
				return JalaliDateTime.Parse(str);
			}
			catch
			{
				return null;
			}
		}

		static JalaliDateTime? TryParse(object str)
		{
			try
			{
				return JalaliDateTime.Parse((string)str);
			}
			catch
			{
				return null;
			}
		}*/

		public static bool TryParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style, out JalaliDateTime result)
		{
			foreach (var format in formats)
			{
				if (JalaliDateTime.TryParseExact(s, format, provider, style, out result))
				{
					return true;
				}
			}
			result = new JalaliDateTime();
			return false;
		}

		/// <summary>
		/// Converts the specified string representation of a date and time to its System.DateTime equivalent using the specified array of formats, culture-specific format information, and style. The format of the string representation must match at least one of the specified formats exactly. The method returns a value that indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">A string containing one or more dates and times to convert.</param>
		/// <param name="format">The required format of s.</param>
		/// <param name="provider">An object that supplies culture-specific format information about s.</param>
		/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of s. A typical value to specify is System.Globalization.DateTimeStyles.None.</param>
		/// <param name="result">When this method returns, contains the System.DateTime value equivalent to the date and time contained in s, if the conversion succeeded, or System.DateTime.MinValue if the conversion failed. The conversion fails if s or formats is null, s or an element of formats is an empty string, or the format of s is not exactly as specified by at least one of the format patterns in formats. This parameter is passed uninitialized.</param>
		/// <returns>true if the s parameter was converted successfully; otherwise, false.</returns>
		/// <exception cref="System.ArgumentException">styles is not a valid System.Globalization.DateTimeStyles value.-or-styles contains an invalid combination of System.Globalization.DateTimeStyles values (for example, both System.Globalization.DateTimeStyles.AssumeLocal and System.Globalization.DateTimeStyles.AssumeUniversal).</exception>		
		public static bool TryParseExact(string s, string format, IFormatProvider formatProvider, DateTimeStyles style, out JalaliDateTime result)
		{
			var tokenMap = new Dictionary<string, string>
			{
				["yyyy"] = @"(?<yyyy>\d{4})",
				["yy"] = @"(?<yy>\d{2})",
				["MMM"] = @"(?<MMM>[^,\s]+)",
				["MM"] = @"(?<MM>\d{1,2})",
				["dd"] = @"(?<dd>\d{1,2})",
				["hh"] = @"(?<hh>\d{1,2})",
				["h"] = @"(?<h>\d{1,2})",
				["mm"] = @"(?<mm>\d{1,2})",
				["m"] = @"(?<m>\d{1,2})",
				["ss"] = @"(?<ss>\d{1,2})",
				["s"] = @"(?<s>\d{1,2})",
				["fff"] = @"(?<fff>\d{1,3})",
				["ff"] = @"(?<ff>\d{1,2})",
				["f"] = @"(?<f>\d{1})",
				["tt"] = @"(?<tt>AM|PM)",
				["t"] = @"(?<t>A|P)"
			};

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
			var month = getMonth(groups, formatProvider);
			var day = getDay(groups);
			var hour = getHours(groups);
			var minute = getMinutes(groups);
			var second = getSeconds(groups);
			var millisecond = getMilliseconds(groups);

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

		static int getYear(GroupCollection groups)
		{
			var yy = groups.GetInt32("yy");
			if (yy != null) yy = yy + 1300;

			return groups.GetInt32("yyyy")
					?? yy
					?? 1;
		}

		static int getMonth(GroupCollection groups, IFormatProvider formatProvider)
		{
			if (!string.IsNullOrEmpty(groups["MMM"].Value))
			{
				//var months = DateTools._jalali_english_month_names.Select(a => a.ToLower()).ToArray();
				var months = JalaliDateTimeFormat.GetMonthNames(formatProvider).Select(a => a.ToLower()).ToArray();
				var key = groups["MMM"].Value.Trim().ToLower();
				return Array.IndexOf(months, key) + 1;
			}
			return groups.GetInt32("MM") ?? 1;
		}

		static int getDay(GroupCollection groups)
		{
			return groups.GetInt32("dd")
					?? groups.GetInt32("d")
					?? 1;
		}

		static int getHours(GroupCollection groups)
		{
			var pm = groups["tt"].Value.IfNullOrEmpty(groups["tt"].ToString()).ToUpper();
			var offset = (pm == "PM" || pm == "P") ? 12 : 0;

			return (groups.GetInt32("hh")
					?? groups.GetInt32("h")
					?? 0) + offset;
		}

		static int getMinutes(GroupCollection groups)
		{
			return groups.GetInt32("mm")
					?? groups.GetInt32("m")
					?? 0;
		}

		static int getSeconds(GroupCollection groups)
		{
			return groups.GetInt32("ss")
					?? groups.GetInt32("s")
					?? 0;
		}

		static int getMilliseconds(GroupCollection groups)
		{
			return groups.GetInt32("fff")
					?? groups.GetInt32("ff")
					?? groups.GetInt32("f")
					?? 0;
		}

	}
}
