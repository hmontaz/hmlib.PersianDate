using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using hmlib.PersianDate.Utilities;
using hmlib.PersianDate.Globalization;

namespace hmlib.PersianDate
{
	public partial struct JalaliDateTime
	{
		readonly static IJalaliDateTimeParse _parser = new JalaliDateTimeParser();
		public static JalaliDateTime Parse(string s)
		{
			if (s == null) throw new ArgumentNullException(nameof(s));
			if (_parser.TryParse(s, out var result))
			{
				return result;
			}
			throw new FormatException("Input string was not in a correct format.");
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
		public static bool TryParseExact(string s, string format, IFormatProvider? formatProvider, DateTimeStyles style, out JalaliDateTime result)
		{
			return _parser.TryParseExact(s, format, formatProvider, style, strictDate: false, strictTime: false, out result);
		}
		public static JalaliDateTime ParseExact(string s, string format, IFormatProvider? provider)
		{
			if (s == null) throw new ArgumentNullException(nameof(s));
			if (format == null) throw new ArgumentNullException(nameof(format));
			return _parser.ParseExact(s, format, DateTimeFormatInfo.GetInstance(provider), DateTimeStyles.None);
		}
	}
}
