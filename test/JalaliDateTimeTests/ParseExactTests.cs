using hmlib.PersianDate;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace hmlib.PersianDateTests.JalaliDateTimeTests
{
	public class ParseExactTests
	{
		#region Year Tests
		[Theory]
		[InlineData("1960/01/02", "1360/01/02", "yyyy/MM/dd", 1960, 1, 2, 1360)]
		[InlineData("60/01/02", "60/01/02", "yy/MM/dd", 1960, 1, 2, 1360)]
		[InlineData("60/01/02", "60/01/02", "y/MM/dd", 1960, 1, 2, 1360)]
		[InlineData("4/01/02", "4/01/02", "y/MM/dd", 2004, 1, 2, 1404)]
		public void YearHappyPath(string dtString, string jdString, string format, int dtYear, int month, int day, int jd)
		{
			//DateTime as a reference
			HappyDateTime_Date(dtString, format, dtYear, month, day);
			HappyJalaliDateTime_Date(jdString, format, jd, month, day);
		}
		#endregion Year Tests

		#region Month Tests
		[Theory]
		[InlineData("1960/01/02", "1360/01/02", "yyyy/MM/dd", 1960, 1, 2, 1360)]
		[InlineData("1960/01/02", "1360/01/02", "yyyy/M/d", 1960, 1, 2, 1360)]
		[InlineData("1960/1/02", "1360/1/02", "yyyy/M/d", 1960, 1, 2, 1360)]
		[InlineData("1960/February/03", "1360/Ordibehesht/03", "yyyy/MMMM/dd", 1960, 2, 3, 1360)]
		[InlineData("1960/Feb/03", "1360/Ord/03", "yyyy/MMM/dd", 1960, 2, 3, 1360)]
		public void MonthHappyPath(string dtString, string jdString, string format, int dtYear, int month, int day, int jd)
		{
			HappyDateTime_Date(dtString, format, dtYear, month, day);
			HappyJalaliDateTime_Date(jdString, format, jd, month, day);
		}

		[Fact]
		public void ParseExactUnHappyPath_Month()
		{
			//DateTime as a reference			
			Assert.False(DateTime.TryParseExact("1960/1/02", "yyyy/MM/dd", null, DateTimeStyles.None, out _));
			Assert.False(DateTime.TryParseExact("1960/Jan/02", "yyyy/MMMM/dd", null, DateTimeStyles.None, out _));
			Assert.False(DateTime.TryParseExact("1960/January/02", "yyyy/MMM/dd", null, DateTimeStyles.None, out _));

			Assert.False(JalaliDateTime.TryParseExact("1360/1/02", "yyyy/MM/dd", null, DateTimeStyles.None, out _));
			Assert.False(JalaliDateTime.TryParseExact("1360/Ordibehesht/2", "yyyy/MMM/dd", null, DateTimeStyles.None, out _));
			//Assert.False(JalaliDateTime.TryParseExact("Khordad 02, 1357", "MMMM dd, yyyy", null, DateTimeStyles.None, out _));
		}

		#endregion Month Tests

		#region Day Tests
		[Theory]
		[InlineData("1960/01/02", "1360/01/02", "yyyy/MM/dd", 1960, 1, 2, 1360)]
		[InlineData("1960/01/2", "1360/01/2", "yyyy/MM/d", 1960, 1, 2, 1360)]
		[InlineData("1960/1/02", "1360/1/02", "yyyy/M/d", 1960, 1, 2, 1360)]
		public void DayHappyPath(string dtString, string jdString, string format, int dtYear, int month, int day, int jd)
		{
			HappyDateTime_Date(dtString, format, dtYear, month, day);
			HappyJalaliDateTime_Date(jdString, format, jd, month, day);
		}

		#endregion Day Tests

		#region HourTests
		[Theory]
		[InlineData("13:20", "HH:mm", 13, 20, 0, 0)]
		[InlineData("13:20", "H:mm", 13, 20, 0, 0)]
		[InlineData("01:20", "hh:mm", 1, 20, 0, 0)]
		[InlineData("1:20", "h:mm", 1, 20, 0, 0)]
		public void HourHappyPath(string time, string format, int hour, int minur, int second, int ms)
		{
			HappyDateTime_Time(time, format, hour, minur, second, ms);
			HappyJalaliDateTime_Time(time, format, hour, minur, second, ms);
		}

		#endregion HourTests

		#region Minute Tests
		[Theory]
		[InlineData("00:01", "HH:mm", 0, 1, 0, 0)]
		[InlineData("00:01", "HH:m", 0, 1, 0, 0)]
		[InlineData("00:1", "HH:m", 0, 1, 0, 0)]
		public void MinuteHappyPath(string time, string format, int hour, int minur, int second, int ms)
		{
			HappyDateTime_Time(time, format, hour, minur, second, ms);
			HappyJalaliDateTime_Time(time, format, hour, minur, second, ms);
		}
		#endregion Minute Tests

		#region Second Tests
		[Theory]
		[InlineData("13:20:30", "HH:mm:ss", 13, 20, 30, 0)]
		[InlineData("13:20:30", "HH:mm:s", 13, 20, 30, 0)]
		[InlineData("13:20:3", "HH:mm:s", 13, 20, 3, 0)]
		public void SecondHappyPath(string time, string format, int hour, int minur, int second, int ms)
		{
			HappyDateTime_Time(time, format, hour, minur, second, ms);
			HappyJalaliDateTime_Time(time, format, hour, minur, second, ms);
		}

		#endregion Second Tests

		#region Millisecond Tests
		[Theory]
		[InlineData("00:00:00.0", "hh:mm:ss.f", 0, 0, 0, 0)]
		[InlineData("00:00:00.01", "hh:mm:ss.ff", 0, 0, 0, 10)]
		[InlineData("00:00:00.10", "hh:mm:ss.ff", 0, 0, 0, 100)]
		[InlineData("00:00:00.001", "hh:mm:ss.fff", 0, 0, 0, 1)]
		[InlineData("00:00:00.010", "hh:mm:ss.fff", 0, 0, 0, 10)]
		[InlineData("00:00:00.100", "hh:mm:ss.fff", 0, 0, 0, 100)]
		[InlineData("00:00:00.0010", "hh:mm:ss.ffff", 0, 0, 0, 1)]
		[InlineData("00:00:00.00100", "hh:mm:ss.fffff", 0, 0, 0, 1)]
		[InlineData("00:00:00.001000", "hh:mm:ss.ffffff", 0, 0, 0, 1)]
		[InlineData("00:00:00.0010000", "hh:mm:ss.fffffff", 0, 0, 0, 1)]
		public void MilliSecondHappyPath(string time, string format, int hour, int minur, int second, int ms)
		{
			HappyDateTime_Time(time, format, hour, minur, second, ms);
			HappyJalaliDateTime_Time(time, format, hour, minur, second, ms);
		}
		#endregion Millisecond Tests


		#region AM/PM Tests
		[Theory]
		[InlineData("12:00:00 PM", "H:mm:ss tt", 12, 0, 0, 0)]
		[InlineData("1:00:00 AM", "h:mm:ss tt", 1, 0, 0, 0)]
		[InlineData("1:00:00 PM", "h:mm:ss tt", 13, 0, 0, 0)]
		public void AMPMHappyPath(string time, string format, int hour, int minur, int second, int ms)
		{
			HappyDateTime_Time(time, format, hour, minur, second, ms);
			HappyJalaliDateTime_Time(time, format, hour, minur, second, ms);
		}
		#endregion AM/PM Tests

		[StackTraceHidden]
		void HappyDateTime_Time(string time, string format, int hour, int minur, int second, int ms)
		{
			DateTime dt;
			var dtYear = DateTime.Now.Year;
			var dtMonth = DateTime.Now.Month;
			var dtDay = DateTime.Now.Day;
			var result = DateTime.TryParseExact(time, format, null, DateTimeStyles.None, out dt);
			Assert.True(result && dt == new DateTime(dtYear, dtMonth, dtDay, hour, minur, second, ms));
		}
		[StackTraceHidden]
		void HappyJalaliDateTime_Time(string time, string format, int hour, int minur, int second, int ms)
		{
			JalaliDateTime jd;
			var jdYear = JalaliDateTime.Now.Year;
			var jdMonth = JalaliDateTime.Now.Month;
			var jdDay = JalaliDateTime.Now.Day;
			Assert.True(JalaliDateTime.TryParseExact(time, format, null, DateTimeStyles.None, out jd) && jd == new JalaliDateTime(jdYear, jdMonth, jdDay, hour, minur, second, ms));
		}
		[StackTraceHidden]
		void HappyDateTime_Date(string date, string format, int year, int month, int day)
		{
			DateTime dt;
			Assert.True(DateTime.TryParseExact(date, format, null, DateTimeStyles.None, out dt));
			Assert.Equal(year, dt.Year);
			Assert.Equal(month, dt.Month);
			Assert.Equal(day, dt.Day);
		}
		[StackTraceHidden]
		void HappyJalaliDateTime_Date(string date, string format, int year, int month, int day)
		{
			JalaliDateTime jd;
			Assert.True(JalaliDateTime.TryParseExact(date, format, null, DateTimeStyles.None, out jd));
			Assert.Equal(year, jd.Year);
			Assert.Equal(month, jd.Month);
			Assert.Equal(day, jd.Day);
		}
	}
}
