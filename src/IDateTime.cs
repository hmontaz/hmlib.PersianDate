using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hmlib.PersianDate
{
	public interface IDateTime : IComparable
	{
		int Year { get; }
		int Month { get; }
		int Day { get; }
		int Hour { get; }
		int Minute { get; }
		int Second { get; }
		int Millisecond { get; }
		int DayOfYear { get; }
		DayOfWeek DayOfWeek { get; }
		TimeSpan TimeOfDay { get; }
		long Ticks { get; }

		//IDateTime Add(TimeSpan value);
		//IDateTime AddDays(double value);
		//IDateTime AddMonths(int months);
		//IDateTime AddYears(int years);
		//IDateTime AddHours(double value);
		//IDateTime AddMinutes(double value);
		//IDateTime AddSeconds(double value);
		//IDateTime AddMillisecond(double value);
		//IDateTime AddTicks(Int64 value);
	}
}
