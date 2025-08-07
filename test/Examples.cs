using hmlib.PersianDate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hmlib.PersianDateTests
{
	public class Examples
	{

		public Examples()
		{
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
		}

		[Fact]
		public void ToStringExamples()
		{
			JalaliDateTime j = new DateTime(2024, 01, 01);

			var s1 = j.ToString();// "1402/10/11 12:00:00 AM"
			var s2 = j.ToString(CultureInfo.GetCultureInfo("en-IR"));// "1402/10/11 12:00:00 AM"
			var s3 = j.ToString(CultureInfo.GetCultureInfo("fa-IR"));// "۱۴۰۲/۱۰/۱۱ ۱۲:۰۰:۰۰ ق.ظ"
			var s4 = j.ToString("dddd dd MMMM yyyy", CultureInfo.GetCultureInfo("fa-IR"));// "دوشنبه ۱۱ دی ۱۴۰۲"

			Assert.Equal("1402/10/11 00:00:00", s1);
			Assert.Equal("1402/10/11 12:00:00 AM", s2);
			Assert.Equal("۱۴۰۲/۱۰/۱۱ ۱۲:۰۰:۰۰ ق.ظ", s3);
			Assert.Equal("دوشنبه ۱۱ دی ۱۴۰۲", s4);
		}
	}
}
