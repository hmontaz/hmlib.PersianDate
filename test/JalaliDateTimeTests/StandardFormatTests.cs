using hmlib.PersianDate.Globalization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using hmlib.PersianDate.Utilities;

namespace hmlib.PersianDateTests.JalaliDateTimeTests
{
	public class StandardFormatTests
	{
		[Theory]
		[InlineData("en-US", new[] { "g" }, "M/d/yyyy h:mm tt")]
		[InlineData("en-US", new[] { "G" }, "M/d/yyyy h:mm:ss tt")]
		[InlineData("en-US", new[] { "d" }, "M/d/yyyy")]
		[InlineData("en-US", new[] { "D" }, "dddd, MMMM d, yyyy")]
		[InlineData("en-US", new[] { "t" }, "h:mm tt")]
		[InlineData("en-US", new[] { "T" }, "h:mm:ss tt")]
		[InlineData("en-US", new[] { "f" }, "dddd, MMMM d, yyyy h:mm tt")]
		[InlineData("en-US", new[] { "F" }, "dddd, MMMM d, yyyy h:mm:ss tt")]
		[InlineData("en-US", new[] { "M", "m" }, "MMMM d")]
		[InlineData("en-US", new[] { "Y", "y" }, "MMMM yyyy")]
		[InlineData("en-US", new[] { "o", "O" }, "yyyy-MM-ddTHH:mm:ss.fffffffK")]
		[InlineData("en-US", new[] { "R", "r" }, "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'")]
		[InlineData("en-US", new[] { "s" }, "yyyy'-'MM'-'dd'T'HH':'mm':'ss")]
		[InlineData("en-US", new[] { "u" }, "yyyy'-'MM'-'dd HH':'mm':'ss'Z'")]
		//[InlineData("en-US", new[] { "U" }, "dddd, MMMM d, yyyy h:mm:ss tt")]
		public void StandardFormatTest(string cultureName, string[] standards, string expectedFormat)
		{
			foreach (var standard in standards)
			{
				var culture = CultureInfo.GetCultureInfo(cultureName);
				//------------
				var dtNow = DateTime.Now;
				var dtStr = dtNow.ToString(standard, culture);
				// Get the expected custom format for the standard format
				Assert.Equal(expectedFormat, GetCustomPatternFromStandard(standard, culture));
				// Make sure formatting with standard and expectedFormat yield the same result
				Assert.Equal(normalize(dtNow.ToString(standard, culture)), dtNow.ToString(expectedFormat, culture));
				// Make sure parsing with standard and expectedFormat yield the same result
				Assert.Equal(DateTime.ParseExact(dtStr, standard, culture), DateTime.ParseExact(dtStr, expectedFormat, culture));

				//------------
				Assert.Equal(expectedFormat, JalaliDateTimeFormat.GetStandardFormat(standard, culture));
			}
		}

		static string GetCustomPatternFromStandard(string standard, CultureInfo culture)
		{
			var dtf = culture.DateTimeFormat;

			var result = standard switch
			{
				"d" => dtf.ShortDatePattern,
				"D" => dtf.LongDatePattern,
				"f" => $"{dtf.LongDatePattern} {dtf.ShortTimePattern}",
				"F" => dtf.FullDateTimePattern,
				"g" => $"{dtf.ShortDatePattern} {dtf.ShortTimePattern}",
				"G" => $"{dtf.ShortDatePattern} {dtf.LongTimePattern}",
				"t" => dtf.ShortTimePattern,
				"T" => dtf.LongTimePattern,
				"M" or "m" => dtf.MonthDayPattern,
				"Y" or "y" => dtf.YearMonthPattern,
				"O" or "o" => "yyyy-MM-ddTHH:mm:ss.fffffffK",
				"R" or "r" => "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
				"s" => "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
				"u" => "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
				"U" => dtf.FullDateTimePattern,
				_ => throw new FormatException($"Unknown standard format: {standard}"),
			};
			return normalize(result);
		}

		private static string normalize(string result)
		{
			return new StringBuilder(result).NormalizeSpaces().ToString();
		}
	}
}
