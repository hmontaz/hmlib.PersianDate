using hmlib.PersianDate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hmlib.PersianDateTests.JalaliDateTimeTests
{

	public class ParseTests
	{
		[Theory]
		[InlineData("1357.10.10", 1357, 10, 10, 0, 0, 0, 0)]
		[InlineData("1357-10-10", 1357, 10, 10, 0, 0, 0, 0)]
		[InlineData("1391/6/25 8:5", 1391, 6, 25, 8, 5, 0, 0)]
		[InlineData("1391/6/25 17:43", 1391, 6, 25, 17, 43, 0, 0)]
		[InlineData("1391/6/25 16:43:12", 1391, 6, 25, 16, 43, 12, 0)]
		[InlineData("1391/6/25 15:43:12.25", 1391, 6, 25, 15, 43, 12, 250)]
		[InlineData("1391/6/25 15:43:12.5", 1391, 6, 25, 15, 43, 12, 500)]
		[InlineData("1391/6/25 15:43:12.6", 1391, 6, 25, 15, 43, 12, 600)]
		[InlineData("1391/6/25 15:43:12.8", 1391, 6, 25, 15, 43, 12, 800)]

		public void ParseHappyPath(string s, int year, int month, int day, int hour, int min, int sec, int ms)
		{
			var expectedDateTime = new DateTime(year, month, day, hour, min, sec, ms);
			var expectedJalaliDateTime = new JalaliDateTime(year, month, day, hour, min, sec, ms);


			Assert.Equal(expectedDateTime, DateTime.Parse(s));

			Assert.Equal(expectedJalaliDateTime, JalaliDateTime.Parse(s));
			Assert.True(JalaliDateTime.TryParse(s, out var jd) && expectedJalaliDateTime == jd);
		}

		[Theory]
		[InlineData("1391/13/16")] //invalid month number
		[InlineData("1391/10/32")] //invalid day number
		//[InlineData("1391/10/16 25:00")] //invalid hours [fix]
		//[InlineData("1391/10/16 14:60")] //invalid minutes [fix]
		//[InlineData("1391/10/16 14:43:60")] //invalid seconds [fix]
		[InlineData("1391+6/25 14:43:12.5")] //invalid formats
		//[InlineData("1391/01 14:43")] //invalid formats [fix]
		public void TryParseUnhappyPath(string s)
		{
			Assert.False(DateTime.TryParse(s, out _));
			Assert.False(JalaliDateTime.TryParse(s, out _));
		}


		[Fact]
		public void BoundryTests()
		{
			Assert.True(JalaliDateTime.TryParse("9378/08/11 00:00:00.000", out var _));
			Assert.True(JalaliDateTime.TryParse("9378/08/11 23:59:59.999", out var _));
			Assert.False(JalaliDateTime.TryParse("9379/08/12", out var _));
		}

	}
}
