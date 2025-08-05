using hmlib.PersianDate;
using System;
using System.Collections.Generic;
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
		[InlineData("1391/6/25 18:45", 1391, 6, 25, 18, 45, 0, 0)]
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
		[InlineData("1391/10/16 25:00")] //invalid hours
		[InlineData("1391/10/16 14:60")] //invalid minutes
		[InlineData("1391/10/16 14:43:60")] //invalid seconds
		[InlineData("1391+6/25 14:43:12.5")] //invalid formats
		[InlineData("1391/01 14:43")] //invalid formats
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

		[Theory]
		[InlineData("00:00", 0, 0, 0, 0)]
		[InlineData("00:00:00", 0, 0, 0, 0)]
		[InlineData("00:00:00.0", 0, 0, 0, 0)]
		[InlineData("00:00:00.001", 0, 0, 0, 1)]
		[InlineData("00:00:00.01", 0, 0, 0, 10)]
		[InlineData("00:00:00.1", 0, 0, 0, 100)]
		[InlineData("00:00:00.11", 0, 0, 0, 110)]
		[InlineData("00:00:00.111", 0, 0, 0, 111)]
		[InlineData("12:00:00", 12, 0, 0, 0)]
		[InlineData("13:00:00", 13, 0, 0, 0)]
		[InlineData("1:00:00", 1, 0, 0, 0)]
		[InlineData("1:00:00 AM", 1, 0, 0, 0)]
		[InlineData("1:00:00 PM", 13, 0, 0, 0)]
		public void ParseTimeTests(string s, int hour, int minute, int second, int ms)
		{
			var dateTime = DateTime.Parse(s);
			Assert.Equal(hour, dateTime.Hour);
			Assert.Equal(minute, dateTime.Minute);
			Assert.Equal(second, dateTime.Second);
			Assert.Equal(ms, dateTime.Millisecond);

			var jalaliDateTime = JalaliDateTime.Parse(s);
			Assert.Equal(hour, jalaliDateTime.Hour);
			Assert.Equal(minute, jalaliDateTime.Minute);
			Assert.Equal(second, jalaliDateTime.Second);
			Assert.Equal(ms, jalaliDateTime.Millisecond);
		}

		[Fact]
		public void ParseExactHappyPath()
		{
			Assert.True(JalaliDateTime.TryParseExact("1360/1/2", "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out var d1) && d1 == new JalaliDateTime(1360, 1, 2));
			Assert.True(JalaliDateTime.TryParseExact("1360/Ordibehesht/2", "yyyy/MMM/dd", null, System.Globalization.DateTimeStyles.None, out var d2) && d2 == new JalaliDateTime(1360, 2, 2));
			Assert.True(JalaliDateTime.TryParseExact("Khordad 02, 1357", "MMM dd, yyyy", null, System.Globalization.DateTimeStyles.None, out var d3) && d3 == new JalaliDateTime(1357, 3, 2));
			Assert.True(JalaliDateTime.TryParseExact("1360/3/2 10:20", "yyyy/MM/dd hh:mm", null, System.Globalization.DateTimeStyles.None, out var d4) && d4 == new JalaliDateTime(1360, 3, 2, 10, 20, 0));
			Assert.True(JalaliDateTime.TryParseExact("1360/3/2 10:20 PM", "yyyy/MM/dd hh:mm tt", null, System.Globalization.DateTimeStyles.None, out var d5) && d5 == new JalaliDateTime(1360, 3, 2, 22, 20, 0));
		}
		[Fact]
		public void ParseExactUnhappyPath()
		{
			// time missing
			Assert.False(JalaliDateTime.TryParseExact("1360/1/2", "yyyy/MM/dd HH:mm", null, System.Globalization.DateTimeStyles.None, out var _));
			// expects month number but got month name
			Assert.False(JalaliDateTime.TryParseExact("1360/Ordibehesht/2", "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out var _));
			// expects month name but got month number
			Assert.False(JalaliDateTime.TryParseExact("1360/3/2", "MMM dd, yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out var _));
			// expects 12-hour format but got 24-hour format
			Assert.False(JalaliDateTime.TryParseExact("1360/3/2 10:20 PM", "yyyy/MM/dd hh:mm", null, System.Globalization.DateTimeStyles.None, out var _));
			//expects 24-hour format but got 12-hour format
			Assert.False(JalaliDateTime.TryParseExact("1360/3/2 10:20", "yyyy/MMM/dd hh:mm tt", null, System.Globalization.DateTimeStyles.None, out var _));
			// invalid year
			Assert.False(JalaliDateTime.TryParseExact("9379/1/2", "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out var _));
			// invalid month number
			Assert.False(JalaliDateTime.TryParseExact("1360/13/2", "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out var _));
			// invalid day number
			Assert.False(JalaliDateTime.TryParseExact("1360/1/32", "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out var _));
		}

	}
}
