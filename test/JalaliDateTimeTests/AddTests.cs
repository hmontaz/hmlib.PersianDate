using hmlib.PersianDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hmlib.PersianDateTests.JalaliDateTimeTests
{
	
	public class AddTests
	{
		[Fact]
		public void AddTest()
		{
			Assert.Equal(new JalaliDateTime(1370, 1, 1, 0, 0, 1), new JalaliDateTime(1370, 1, 1).AddMilliseconds(1000));
			Assert.Equal(new JalaliDateTime(1370, 1, 1, 0, 0, 2), new JalaliDateTime(1370, 1, 1).AddSeconds(2));
			Assert.Equal(new JalaliDateTime(1370, 1, 1, 0, 3, 0), new JalaliDateTime(1370, 1, 1).AddMinutes(3));
			Assert.Equal(new JalaliDateTime(1370, 1, 1, 4, 0, 0), new JalaliDateTime(1370, 1, 1).AddHours(4));
			Assert.Equal(new JalaliDateTime(1390, 1, 2), new JalaliDateTime(1390, 1, 1).Add(TimeSpan.FromDays(1)));
		}
		[Fact]
		public void AddTicksTest()
		{
			Assert.Equal(new JalaliDateTime(1390, 1, 2)
										, new JalaliDateTime(1390, 1, 1).AddTicks(TimeSpan.TicksPerDay));
		}
		[Fact]
		public void AddDaysTest()
		{
			var t = new TimeSpan(10, 22, 15).Add(TimeSpan.FromTicks(12));
			JalaliDateTime date1 = new JalaliDateTime(1390, 12, 29, t);
			JalaliDateTime date2 = new JalaliDateTime(1391, 1, 1, t);
			date2 = date2.AddDays(-1);
			Assert.Equal(date1, date2);
		}
		[Fact]
		public void AddMonthsTest()
		{
			var t = new TimeSpan(10, 22, 15).Add(TimeSpan.FromTicks(12));
			Assert.Equal(new JalaliDateTime(1378, 1, 1, t), new JalaliDateTime(1378, 2, 1, t).AddMonths(-1));
			Assert.Equal(new JalaliDateTime(1377, 12, 29, t), new JalaliDateTime(1378, 1, 31, t).AddMonths(-1));
		}
		[Fact]
		public void AddYearsTest()
		{
			var t = new TimeSpan(10, 22, 15).Add(TimeSpan.FromTicks(12));
			Assert.Equal(new JalaliDateTime(1377, 1, 1, t), new JalaliDateTime(1378, 1, 1, t).AddYears(-1));
			Assert.Equal(new JalaliDateTime(1374, 12, 29, t), new JalaliDateTime(1375, 12, 30, t).AddYears(-1));
			Assert.Equal(new JalaliDateTime(1376, 12, 29, t), new JalaliDateTime(1375, 12, 30, t).AddYears(1));
		}
	}
}
