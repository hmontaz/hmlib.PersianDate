using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hmlib.PersianDate.Utilities
{
	internal abstract class DateTimeHelper
	{
		public readonly bool HighPerformance;
		public DateTimeHelper()
		{
			HighPerformance = true;
		}
		internal DateTimeHelper(bool highPerformance)
		{
			HighPerformance = highPerformance;
		}
		Dictionary<int, long> dic = new Dictionary<int, long>();
		public abstract void StartDate(out int year, out int month, out int day);
		public abstract int DaysInYear(int year);
		public abstract int DaysInMonth(int year, int month);
		public void FromTicks(long ticks, out int year, out int month, out int day, out TimeSpan timeOfDay, out DayOfWeek dayOfWeek)
		{
			dayOfWeek = GetDayOfWeek(ticks);
			timeOfDay = GetTimeOfDay(ref ticks);
			StartDate(out year, out month, out day);
			if (HighPerformance)
			{
				double dy = ticks / (TimeSpan.TicksPerDay * 365.24);
				if (dy > 1/* && IsInRange(year + (int)dy, 1, 1)*/)
				{
					month = 1;
					day = 1;
					year += (int)dy;
					ticks -= GetTicks(year, 1, 1, TimeSpan.Zero);
				}
				while (ticks > TimeSpan.TicksPerDay * DaysInMonth(year, month))
				{
					ticks -= AddMonth(ref year, ref month, ref day);
				}
				while (ticks > 0)
				{
					ticks -= AddDay(ref year, ref month, ref day);
				}
				while (ticks < 0)
				{
					ticks += SubtractDay(ref year, ref month, ref day);
				}
			}
			else
			{
				while (ticks >= TimeSpan.TicksPerDay && month != 1 && day != 1)
				{
					ticks -= AddDay(ref year, ref month, ref day);
				}
				while (ticks >= DaysInYear(year) * TimeSpan.TicksPerDay)
				{
					ticks -= AddYear(ref year);
				}
				while (ticks >= TimeSpan.TicksPerDay)
				{
					ticks -= AddDay(ref year, ref month, ref day);
				}
			}
		}
		public long GetTicks(int year, int month, int day, TimeSpan time)
		{
			int y;
			int m;
			int d;
			StartDate(out y, out m, out d);
			if (!IsInRange(year, month, day)) throw new ArgumentOutOfRangeException();
			long ticks = time.Ticks;
			while (year > y && (month > 1 || day > 1))
			{
				ticks += SubtractDay(ref year, ref month, ref day);
			}
			int key = 0;
			if (HighPerformance)
			{
				key = GetKey(year, month, day);
				if (dic.ContainsKey(key))
				{
					ticks += dic[key];
					return ticks;
				}
			}
			long t = 0;
			while (year > y && m == 1 && d == 1)
			{
				t += SubtractYear(ref year);
			}
			while (year > y + 1)
			{
				t += SubtractYear(ref year);
			}
			while (year > y || month > m || day > d)
			{
				t += SubtractDay(ref year, ref month, ref day);
			}
			if (HighPerformance) dic[key] = t;
			ticks += t;
			return ticks;
		}
		bool IsInRange(int year, int month, int day)
		{
			int y;
			int m;
			int d;
			StartDate(out y, out m, out d);
			if (year > y) return true;
			if (year < y) return false;
			if (month > m) return true;
			if (month < m) return false;
			if (day > d) return true;
			if (day < d) return false;
			return true;
		}
		int GetKey(int year, int month, int day)
		{
			int sign = year >= 0 ? 1 : -1;
			year = Math.Abs(year);
			//return sign * int.Parse(String.Format("{0:0000}{1:00}{2:00}", year, month, day));
			return sign * (year * 100000 + month * 100 + day);
		}
		long AddMonth(ref int year, ref int month, ref int day)
		{
			var max = DaysInMonth(year, month);
			if (day > max)
			{
				day = max - day;
			}
			month++;
			if (month > 12)
			{
				year++;
				month = 1;
			}
			return max * TimeSpan.TicksPerDay;
		}
		long AddDay(ref int year, ref int month, ref int day)
		{
			day++;
			if (day > DaysInMonth(year, month))
			{
				month++;
				if (month > 12)
				{
					year++;
					month = 1;
				}
				day = 1;
			}
			return TimeSpan.TicksPerDay;
		}
		long AddYear(ref int year)
		{
			var ticks = DaysInYear(year) * TimeSpan.TicksPerDay;
			year++;
			return ticks;
		}
		long SubtractDay(ref int year, ref int month, ref int day)
		{
			day--;
			if (day < 1)
			{
				month--;
				if (month < 1)
				{
					year--;
					month = 12;
				}
				day = DaysInMonth(year, month);
			}
			return TimeSpan.TicksPerDay;
		}
		long SubtractYear(ref int year)
		{
			year--;
			return DaysInYear(year) * TimeSpan.TicksPerDay;
		}
		TimeSpan GetTimeOfDay(ref long ticks)
		{
			var t = ticks % TimeSpan.TicksPerDay;
			ticks = ticks - t;
			return TimeSpan.FromTicks(t);
		}
		public DayOfWeek GetDayOfWeek(long ticks)
		{
			long offset = (int)DayOfWeek.Monday + ticks / TimeSpan.TicksPerDay;
			return (DayOfWeek)(offset % 7);
		}
	}
}
