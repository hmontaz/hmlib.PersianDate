using hmlib.PersianDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hmlib.PersianDateTests.JalaliDateTimeTests
{
	public class CultureTest
	{
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
	}
}
