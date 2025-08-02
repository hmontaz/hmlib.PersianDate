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
		[Fact]
		public void ParseHappyPath()
		{
			Assert.Equal(new JalaliDateTime(1357, 10, 10), JalaliDateTime.Parse("1357.10.10"));
			Assert.Equal(new JalaliDateTime(1357, 10, 10), JalaliDateTime.Parse("1357-10-10"));
			Assert.Equal(new JalaliDateTime(1391, 6, 25, 18, 45, 0, 0), JalaliDateTime.Parse("1391/6/25 18:45"));
			Assert.Equal(new JalaliDateTime(1391, 6, 25, 17, 43, 0, 0), JalaliDateTime.Parse("1391/6/25 17:43"));
			Assert.Equal(new JalaliDateTime(1391, 6, 25, 16, 43, 12, 0), JalaliDateTime.Parse("1391/6/25 16:43:12"));
			Assert.Equal(new JalaliDateTime(1391, 6, 25, 15, 43, 12, 250), JalaliDateTime.Parse("1391/6/25 15:43:12.25"));
			Assert.Equal(new JalaliDateTime(1391, 6, 25, 14, 43, 12, 500), JalaliDateTime.Parse("1391/6/25 14:43:12.5"));
			Assert.Equal(new JalaliDateTime(1391, 6, 25, 14, 43, 12, 600), JalaliDateTime.Parse("1391/6/25 14:43:12.6"));
			Assert.Equal(new JalaliDateTime(1391, 6, 25, 14, 43, 12, 800), JalaliDateTime.Parse("1391/6/25 14:43:12.8"));
		}

		[Fact]
		public void TryParseHappyPath()
		{
			Assert.True(JalaliDateTime.TryParse("1391/10/16", out var j) && j == new JalaliDateTime(1391, 10, 16));
		}

		[Fact]
		public void TryParseUnhappyPath()
		{
			// invalid year
			Assert.False(JalaliDateTime.TryParse("9379-01-01", out var _));
			//invalid month number
			Assert.False(JalaliDateTime.TryParse("1391/13/16", out _));
			//invalid day number
			Assert.False(JalaliDateTime.TryParse("1391/10/32", out _));
			//invalid hours
			Assert.False(JalaliDateTime.TryParse("1391/10/16 25:00", out _));
			//invalid minutes
			Assert.False(JalaliDateTime.TryParse("1391/10/16 14:60", out _));
			//invalid seconds
			Assert.False(JalaliDateTime.TryParse("1391/10/16 14:43:60", out _));

			//invalid formats
			Assert.False(JalaliDateTime.TryParse("1391-6/25 14:43:12.5", out _));
			Assert.False(JalaliDateTime.TryParse("1391/6-25 14:43", out _));
			Assert.False(JalaliDateTime.TryParse("1391/6.25 14:43", out _));
			Assert.False(JalaliDateTime.TryParse("1391.6-25 14:43", out _));
			Assert.False(JalaliDateTime.TryParse("1391/01 14:43", out _));
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
