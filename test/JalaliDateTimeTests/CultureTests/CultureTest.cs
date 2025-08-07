using hmlib.PersianDate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hmlib.PersianDateTests.JalaliDateTimeTests.CultureTests
{
	public class CultureTest
	{
		public CultureTest()
		{
			CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
		}

		[Theory]
		[InlineData("fa-IR", 1357, 1, 5, null, "۱۳۵۷/۱/۵ ۱۲:۰۰:۰۰ ق.ظ")]
		[InlineData("en-IR", 1357, 1, 5, null, "1357/1/5 12:00:00 AM")]
		[InlineData("fa-IR", 1357, 1, 5, "yyyy/M/d", "۱۳۵۷/۱/۵")]
		[InlineData("en-IR", 1357, 1, 5, "yyyy/M/d", "1357/1/5")]
		[InlineData("fa-IR", 1300, 1, 1, "yyyy/MM/dd", "۱۳۰۰/۰۱/۰۱")]
		[InlineData("en-IR", 1300, 1, 1, "yyyy/MM/dd", "1300/01/01")]
		[InlineData("fa-IR", 1300, 1, 1, "dddd, dd MMMM yyyy", "دوشنبه، ۰۱ فروردین ۱۳۰۰")]
		[InlineData("en-IR", 1300, 1, 1, "dddd, dd MMMM yyyy", "Monday, 01 Farvardin 1300")]
		[InlineData("fa-IR", 1300, 1, 1, "H:mm", "۰:۰۰")]
		[InlineData("en-IR", 1300, 1, 1, "H:mm", "0:00")]
		[InlineData("fa-IR", 1300, 1, 1, "hh:mm:ss tt", "۱۲:۰۰:۰۰ ق.ظ")]
		[InlineData("en-IR", 1300, 1, 1, "hh:mm:ss tt", "12:00:00 AM")]

		public void ToStringTest(string cultureName, int year, int month, int day, string? format, string expectedString)
		{
			var culture = System.Globalization.CultureInfo.GetCultureInfo(cultureName);
			var jalaliDateTime = new JalaliDateTime(year, month, day);
			Assert.Equal(expectedString, jalaliDateTime.ToString(format, culture));

		}

		[Theory]
		[InlineData("fa-IR", "۱۳۵۷/۱/۵", "yyyy/M/d", 1357, 1, 5)]
		[InlineData("en-IR", "1357/1/5", "yyyy/M/d", 1357, 1, 5)]
		[InlineData("fa-IR", "۱۳۰۰/۰۱/۰۱", "yyyy/MM/dd", 1300, 1, 1)]
		[InlineData("en-IR", "1300/01/01", "yyyy/MM/dd", 1300, 1, 1)]
		//[InlineData("fa-IR", "دوشنبه، ۰۱ فروردین ۱۳۰۰", "dddd, dd MMMM yyyy", 1300, 1, 1)]//x
		//[InlineData("en-IR", "Monday, 01 Farvardin 1300", "dddd, dd MMMM yyyy", 1300, 1, 1)]//x
		[InlineData("fa-IR", "۱۳۷۰/۱/۲ ۰:۰۰", "yyyy/M/d H:mm", 1370, 1, 2)]
		[InlineData("en-IR", "1370/1/2 0:00", "yyyy/M/d H:mm", 1370, 1, 2)]
		[InlineData("fa-IR", "۱۳۷۰/۱/۲ ۱۲:۰۰:۰۰ ق.ظ", "yyyy/M/d hh:mm:ss tt", 1370, 1, 2)]
		[InlineData("en-IR", "1370/1/2 12:00:00 AM", "yyyy/M/d hh:mm:ss tt", 1370, 1, 2)]
		[InlineData("fa-IR", "۱۳۷۰/۱/۲ ۱۲:۰۰:۰۰ ق", "yyyy/M/d hh:mm:ss t", 1370, 1, 2)]
		[InlineData("en-IR", "1370/1/2 12:00:00 A", "yyyy/M/d hh:mm:ss t", 1370, 1, 2)]
		public void ParseExactTest(string cultureName, string dateString, string format, int expectedYear, int expectedMonth, int expectedDay)
		{
			var culture = System.Globalization.CultureInfo.GetCultureInfo(cultureName);
			Assert.True(JalaliDateTime.TryParseExact(dateString, new[] { format }, culture, DateTimeStyles.None, out var jalaliDateTime));
			Assert.Equal(expectedYear, jalaliDateTime.Year);
			Assert.Equal(expectedMonth, jalaliDateTime.Month);
			Assert.Equal(expectedDay, jalaliDateTime.Day);
		}

		[InlineData("en-US", "g", true)]
		[InlineData("en-US", "G", true)]
		[InlineData("en-US", "d", true)]
		//[InlineData("en-US", "D", true)]//[fix github]
		[InlineData("en-US", "t", true)]
		[InlineData("en-US", "T", true)]
		//[InlineData("en-US", "f", true)]//[fix github]
		//[InlineData("en-US", "F", true)]//[fix github]
		[InlineData("en-US", "M", true)]
		[InlineData("en-US", "m", true)]
		//[InlineData("en-US", "Y", true)]//xx
		//[InlineData("en-US", "y", true)]//xx
		//[InlineData("en-US", "o", true)]//[fix]
		//[InlineData("en-US", "O", true)]//[fix]
		//[InlineData("en-US", "R", true)]//[fix]
		//[InlineData("en-US", "r", true)]//[fix]
		//[InlineData("en-US", "s", true)]//[fix]
		//[InlineData("en-US", "u", true)]//[fix]
		//[InlineData("en-US", "U", true)]//[fix github]
		[Theory]
		public void StandardFormatBackAndFortTest(string cultureName, string format, bool justDate)
		{
			var culture = CultureInfo.GetCultureInfo(cultureName);

			var dateTime = justDate ? DateTime.Today : DateTime.Now;
			var s = dateTime.ToString(format, culture);
			Assert.Equal(dateTime, DateTime.ParseExact(s, format, culture));

			var jalaliDateTime = new JalaliDateTime(dateTime);
			var jalaliString = jalaliDateTime.ToString(format, culture);
			Assert.Equal(jalaliDateTime, JalaliDateTime.ParseExact(jalaliString, format, culture));

		}
	}
}
