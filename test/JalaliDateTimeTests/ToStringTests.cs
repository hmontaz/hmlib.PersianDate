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

	public class ToStringTests
	{
		
		[Fact]
		public void ToStringTest()
		{
			var j = new JalaliDateTime(1357, 1, 5, 16, 30, 10, 10);
			Assert.Equal("Farvardin 05, 1357 04:30 P", j.ToString("MMM dd, yyyy hh:mm t"));
			Assert.Equal("Farvardin 05, 1357 04:30 P", j.ToString("MMM dd, yyyy hh:mm t"));
			Assert.Equal("Farvardin 05, 1357 04:30 PM", j.ToString("MMM dd, yyyy hh:mm tt"));
			Assert.Equal("Farvardin 05, 1357 16:30", j.ToString("MMM dd, yyyy HH:mm"));

			//Assert.Equal("1357/01/05", j.ToShortDateString());
			//Assert.Equal("1357/01/05 4:30:10 PM", j.ToString());
			//Assert.Equal("57/01/05", string.Format("{0:yy/MM/dd}", j));

			Assert.Equal("1:2:3", new JalaliDateTime(1396, 1, 15, 13, 2, 3).ToString("h:m:s"));
			Assert.Equal("13:2:3", new JalaliDateTime(1396, 1, 15, 13, 2, 3).ToString("H:m:s"));
		}

		[Fact]
		public void ToStringTest_3()
		{
			Assert.Equal("10/1/2", new DateTime(2010, 1, 2).ToString("y/M/d"));
			Assert.Equal("57/1/2", new JalaliDateTime(1357, 1, 2).ToString("y/M/d"));

			Assert.Equal("10/1/2", new DateTime(2010, 1, 2).ToString("yy/M/d"));
			Assert.Equal("57/1/2", new JalaliDateTime(1357, 1, 2).ToString("yy/M/d"));

			Assert.Equal("00/1/2", new DateTime(2000, 1, 2).ToString("yy/M/d"));
			Assert.Equal("00/1/2", new JalaliDateTime(1400, 1, 2).ToString("yy/M/d"));

			Assert.Equal("2010/1/2", new DateTime(2010, 1, 2).ToString("yyyy/M/d"));
			Assert.Equal("1357/1/2", new JalaliDateTime(1357, 1, 2).ToString("yyyy/M/d"));

			Assert.Equal("January 2 10", new DateTime(2010, 1, 2).ToString("MMMM d y"));
			Assert.Equal("Farvardin 2 57", new JalaliDateTime(1357, 1, 2).ToString("MMMM d y"));

			Assert.Equal("Saturday January 2 10", new DateTime(2010, 1, 2).ToString("dddd MMMM d y"));
			Assert.Equal("Wednesday Farvardin 2 57", new JalaliDateTime(1357, 1, 2).ToString("dddd MMMM d y"));

			Assert.Equal("Sat January 2 10", new DateTime(2010, 1, 2).ToString("ddd MMMM d y"));
			Assert.Equal("Wed Farvardin 2 57", new JalaliDateTime(1357, 1, 2).ToString("ddd MMMM d y"));

			Assert.Equal("Jan 2 10", new DateTime(2010, 1, 2).ToString("MMM d y"));
			Assert.Equal("Farvardin 2 80", new JalaliDateTime(1380, 1, 2).ToString("MMM d y"));

			Assert.Equal("2000/1/2 12:5:9 PM", new DateTime(2000, 1, 2, 12, 5, 9, 0).ToString("yyyy/M/d h:m:s tt"));
			Assert.Equal("1357/1/2 12:5:9 PM", new JalaliDateTime(1357, 1, 2, 12, 5, 9, 0).ToString("yyyy/M/d h:m:s tt"));

			Assert.Equal("2000/1/2 12:5:9 AM", new DateTime(2000, 1, 2, 0, 5, 9, 0).ToString("yyyy/M/d h:m:s tt"));
			Assert.Equal("1357/1/2 12:5:9 AM", new JalaliDateTime(1357, 1, 2, 0, 5, 9, 0).ToString("yyyy/M/d h:m:s tt"));

			Assert.Equal("2000/1/2 0:5:9", new DateTime(2000, 1, 2, 0, 5, 9, 0).ToString("yyyy/M/d H:m:s"));
			Assert.Equal("1357/1/2 0:5:9", new JalaliDateTime(1357, 1, 2, 0, 5, 9, 0).ToString("yyyy/M/d H:m:s"));

			Assert.Equal("1357/1/2 12:5:9 AM", new JalaliDateTime(1357, 1, 2, 0, 5, 9, 0).ToString("yyyy/M/d h:m:s tt"));
			Assert.Equal("1357/1/2 12:5:9 A", new JalaliDateTime(1357, 1, 2, 0, 5, 9, 0).ToString("yyyy/M/d h:m:s t"));

			Assert.Equal("2000/1/2 12:5:9", new DateTime(2000, 1, 2, 12, 5, 9, 0).ToString("yyyy/M/d H:m:s"));
			Assert.Equal("1357/1/2 12:5:9", new JalaliDateTime(1357, 1, 2, 12, 5, 9, 0).ToString("yyyy/M/d H:m:s"));

			Assert.Equal("2000/1/2 23:5:9", new DateTime(2000, 1, 2, 23, 5, 9, 0).ToString("yyyy/M/d H:m:s"));
			Assert.Equal("1357/1/2 23:5:9", new JalaliDateTime(1357, 1, 2, 23, 5, 9, 0).ToString("yyyy/M/d H:m:s"));

			Assert.Equal("2000/1/2 11:5:9", new DateTime(2000, 1, 2, 23, 5, 9, 0).ToString("yyyy/M/d h:m:s"));
			Assert.Equal("1357/1/2 11:5:9", new JalaliDateTime(1357, 1, 2, 23, 5, 9, 0).ToString("yyyy/M/d h:m:s"));

			Assert.Equal("2000/1/2 23:5:9 PM", new DateTime(2000, 1, 2, 23, 5, 9, 0).ToString("yyyy/M/d H:m:s tt"));
			Assert.Equal("1357/1/2 23:5:9 PM", new JalaliDateTime(1357, 1, 2, 23, 5, 9, 0).ToString("yyyy/M/d H:m:s tt"));

			Assert.Equal("2000/1/2 23:5:9.520 PM", new DateTime(2000, 1, 2, 23, 5, 9, 520).ToString("yyyy/M/d H:m:s.fff tt"));
			Assert.Equal("1357/1/2 23:5:9.520 PM", new JalaliDateTime(1357, 1, 2, 23, 5, 9, 520).ToString("yyyy/M/d H:m:s.fff tt"));

			Assert.Equal("2000/1/2 23:5:9.52 PM", new DateTime(2000, 1, 2, 23, 5, 9, 520).ToString("yyyy/M/d H:m:s.ff tt"));
			Assert.Equal("1357/1/2 23:5:9.52 PM", new JalaliDateTime(1357, 1, 2, 23, 5, 9, 520).ToString("yyyy/M/d H:m:s.ff tt"));

			Assert.Equal("2000/1/2 23:5:9.5 PM", new DateTime(2000, 1, 2, 23, 5, 9, 520).ToString("yyyy/M/d H:m:s.f tt"));
			Assert.Equal("1357/1/2 23:5:9.5 PM", new JalaliDateTime(1357, 1, 2, 23, 5, 9, 520).ToString("yyyy/M/d H:m:s.f tt"));

			Assert.Equal("2000/1/2 23:5:9.052 PM", new DateTime(2000, 1, 2, 23, 5, 9, 52).ToString("yyyy/M/d H:m:s.fff tt"));
			Assert.Equal("1357/1/2 23:5:9.052 PM", new JalaliDateTime(1357, 1, 2, 23, 5, 9, 52).ToString("yyyy/M/d H:m:s.fff tt"));

			Assert.Equal("2000/1/2 23:5:9.05 PM", new DateTime(2000, 1, 2, 23, 5, 9, 52).ToString("yyyy/M/d H:m:s.ff tt"));
			Assert.Equal("1357/1/2 23:5:9.05 PM", new JalaliDateTime(1357, 1, 2, 23, 5, 9, 52).ToString("yyyy/M/d H:m:s.ff tt"));

			Assert.Equal("2000/1/2 23:5:9.0 PM", new DateTime(2000, 1, 2, 23, 5, 9, 52).ToString("yyyy/M/d H:m:s.f tt"));
			Assert.Equal("1357/1/2 23:5:9.0 PM", new JalaliDateTime(1357, 1, 2, 23, 5, 9, 52).ToString("yyyy/M/d H:m:s.f tt"));
		}
	}
}
