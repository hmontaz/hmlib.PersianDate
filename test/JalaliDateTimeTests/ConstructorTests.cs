using hmlib.PersianDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hmlib.PersianDateTests.JalaliDateTimeTests
{

	public class ConstructorTests
	{
		[Fact]
		public void ConvertToDateTime()
		{
			var j = new JalaliDateTime(1357, 10, 11);
			Assert.Equal(1357, j.Year);
			Assert.Equal(10, j.Month);
			Assert.Equal(11, j.Day);
		}

		[Fact]
		public void ConvertToDateTime2()
		{
			var j = new JalaliDateTime(1357, 10, 11, 16, 30, 1, 2);
			Assert.Equal(1357, j.Year);
			Assert.Equal(10, j.Month);
			Assert.Equal(11, j.Day);
			Assert.Equal(16, j.Hour);
			Assert.Equal(30, j.Minute);
			Assert.Equal(1, j.Second);
			Assert.Equal(2, j.Millisecond);
		}

		[Fact]
		public void ConstructorsTest()
		{
			var j = new JalaliDateTime();
			Assert.Equal(-621, j.Year);
			Assert.Equal(10, j.Month);
			Assert.Equal(16, j.Day);
			Assert.Equal(0, j.Hour);
			Assert.Equal(0, j.Minute);
			Assert.Equal(0, j.Second);
			Assert.Equal(0, j.Millisecond);
		}
	}
}
