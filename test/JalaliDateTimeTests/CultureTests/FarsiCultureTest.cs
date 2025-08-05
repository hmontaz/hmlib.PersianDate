using hmlib.PersianDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hmlib.PersianDateTests.JalaliDateTimeTests.CultureTests
{
	public class FarsiCultureTest
	{
		public FarsiCultureTest()
		{
			var culture = System.Globalization.CultureInfo.GetCultureInfo("fa-IR");
			Thread.CurrentThread.CurrentCulture = culture;
			//Thread.CurrentThread.CurrentUICulture = culture;
		}
		[Fact]
		public void ToStringTest()
		{
			Assert.Equal("۱۳۵۷/۱/۵ ۱۲:۰۰:۰۰ ق.ظ", new JalaliDateTime(1357, 1, 5).ToString());
			Assert.Equal("۱۳۵۷/۱/۵ ۴:۳۰:۰۰ ب.ظ", new JalaliDateTime(1357, 1, 5, 16, 30, 0, 0).ToString());
			Assert.Equal("۱۳۰۰/۱/۱", new JalaliDateTime(1300, 1, 1).ToShortDateString());
			Assert.Equal("دوشنبه، ۰۱ فروردین ۱۳۰۰", new JalaliDateTime(1300, 1, 1).ToLongDateString());
			Assert.Equal("۰:۰۰", new JalaliDateTime(1300, 1, 1).ToShortTimeString());
			Assert.Equal("۱۲:۰۰:۰۰ ق.ظ", new JalaliDateTime(1300, 1, 1).ToLongTimeString());
		}
	}
}
