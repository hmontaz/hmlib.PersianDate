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
	public class InvariantCultureTest
	{
		public InvariantCultureTest()
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			//Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
		}

		[Fact]
		public void DefaultFormat()
		{
			Assert.Equal("01/05/1978 00:00:00", new DateTime(1978, 1, 5).ToString());
			Assert.Equal("01/05/1978 16:30:10", new DateTime(1978, 1, 5, 16, 30, 10).AddMilliseconds(10).ToString());
			Assert.Equal("01/01/0001", new DateTime().ToShortDateString());
			Assert.Equal("Monday, 01 January 0001", new DateTime().ToLongDateString());
			Assert.Equal("00:00", new DateTime().ToShortTimeString());
			Assert.Equal("00:00:00", new DateTime().ToLongTimeString());
		}
		[Fact]
		public void ToStringTest()
		{
			Assert.Equal("1357/01/05 00:00:00", new JalaliDateTime(1357, 1, 5).ToString());
			Assert.Equal("1357/01/05 16:30:00", new JalaliDateTime(1357, 1, 5, 16, 30, 0, 0).ToString());
			Assert.Equal("1300/01/01", new JalaliDateTime(1300, 1, 1).ToShortDateString());
			Assert.Equal("Monday, 01 Farvardin 1300", new JalaliDateTime(1300, 1, 1).ToLongDateString());
			Assert.Equal("00:00", new JalaliDateTime(1300, 1, 1).ToShortTimeString());
			Assert.Equal("00:00:00", new JalaliDateTime(1300, 1, 1).ToLongTimeString());
		}
	}
}
