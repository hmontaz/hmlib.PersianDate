//using hmlib.PersianDate;
using hmlib.PersianDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hmlib.PersianDateTests.JalaliDateTimeTests
{
	public class KnownDates
	{
		[Theory]
		[InlineData(1356, 10, 11, 1978, 1, 1, DayOfWeek.Sunday)]
		[InlineData(1323, 3, 19, 1944, 6, 9, DayOfWeek.Friday)]
		[InlineData(1404, 5, 10, 2025, 8, 1, DayOfWeek.Friday)]
		[InlineData(1332, 5, 28, 1953, 8,19, DayOfWeek.Wednesday)]


		public void TestKnownDates(int jY, int jM, int jD, int gY, int gM, int gD, DayOfWeek dayOfWeek)
		{
			var j = new JalaliDateTime(jY, jM, jD);
			var g = new DateTime(gY, gM, gD);

			Assert.Equal(j, (JalaliDateTime)g);
			Assert.Equal(g, (DateTime)j);
			Assert.Equal(j.Ticks, g.Ticks);
			Assert.Equal(dayOfWeek, g.DayOfWeek);
			Assert.Equal(dayOfWeek, j.DayOfWeek);
		}
	}
}
