using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hmlib.PersianDate
{
	public partial struct JalaliDateTime
	{
		public JalaliDateTime(long ticks)
			: this(ticks, DateTimeKind.Unspecified)
		{
		}
		public JalaliDateTime(DateTime dateTime)
			: this(dateTime.Ticks, dateTime.Kind)
		{
		}
		public JalaliDateTime(long ticks, DateTimeKind kind)
		{
			_ticks = ticks;
			_year = Int32.MinValue;
			_month = Int32.MinValue;
			_day = Int32.MinValue;
			_timeOfDay = TimeSpan.MinValue;
			_dayOfWeek = (DayOfWeek)(Int32.MinValue);
			_calculate_lock = new object();
			_kind = kind;
			CheckRange();
		}

		public JalaliDateTime(int year, int month, int day)
			: this(year, month, day, 0, 0, 0, 0)
		{
		}

		public JalaliDateTime(int year, int month, int day, TimeSpan timeOfDay)
			: this(year, month, day, timeOfDay, DateTimeKind.Unspecified)
		{

		}

		public JalaliDateTime(int year, int month, int day, TimeSpan timeOfDay, DateTimeKind kind)
		{
			Check(year, month, day);
			if (timeOfDay < TimeSpan.Zero || timeOfDay.Ticks >= TimeSpan.TicksPerDay) throw new ArgumentOutOfRangeException("timeOfDay");
			_year = year;
			_month = month;
			_day = day;
			_timeOfDay = timeOfDay;
			_ticks = Int64.MinValue;
			_kind = kind;
			_dayOfWeek = (DayOfWeek)(Int32.MinValue);
			_calculate_lock = new object();
			CheckRange();
		}


		public JalaliDateTime(int year, int month, int day, int hour, int minute, int second)
			: this(year, month, day, hour, minute, second, 0, DateTimeKind.Unspecified)
		{
		}

		public JalaliDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
	: this(year, month, day, hour, minute, second, millisecond, DateTimeKind.Unspecified)
		{
		}

		public JalaliDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
		{
			Check(year, month, day);
			CheckTime(hour, minute, second, millisecond);
			_year = year;
			_month = month;
			_day = day;
			_ticks = Int64.MinValue;
			_kind = kind;
			_timeOfDay = new TimeSpan(0, hour, minute, second, millisecond);
			_dayOfWeek = (DayOfWeek)(Int32.MinValue);
			_calculate_lock = new object();
			CheckRange();
		}
	}
}
