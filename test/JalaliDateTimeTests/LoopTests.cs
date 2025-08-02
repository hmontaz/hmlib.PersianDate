using hmlib.PersianDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hmlib.PersianDateTests.JalaliDateTimeTests
{
	public class LoopTests
	{
		[Fact]
		public void CountDaysOfWeek()
		{
			var from = new JalaliDateTime(1350, 1, 1);
			var to = new JalaliDateTime(from.Year + 1, 1, 1).AddDays(-3);
			var counts = new Dictionary<DayOfWeek, int>();
			for (var date = from; date <= to; date = date.AddDays(1))
			{
				if (!counts.ContainsKey(date.DayOfWeek))
				{
					counts[date.DayOfWeek] = 0;
				}
				counts[date.DayOfWeek]++;
			}
			foreach (var kvp in counts)
			{
				Assert.Equal(52, kvp.Value); // Each day of the week should appear 52 times in a year
			}
		}
	}
}
