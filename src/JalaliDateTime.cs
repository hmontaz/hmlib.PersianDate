using hmlib.PersianDate.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace hmlib.PersianDate
{
	//https://fa.m.wikipedia.org/wiki/%DA%AF%D8%A7%D9%87%E2%80%8C%D8%B4%D9%85%D8%A7%D8%B1%DB%8C_%D8%B1%D8%B3%D9%85%DB%8C_%D8%A7%DB%8C%D8%B1%D8%A7%D9%86
	[Serializable]
	public partial struct JalaliDateTime : ICloneable, IFormattable, IComparable, IComparable<JalaliDateTime>, IEquatable<JalaliDateTime>, IDateTime
	{
		DateTimeKind _kind;
		static JalaliHelper helper = new JalaliHelper(true);
		long _ticks;
		[NonSerialized]
		object _calculate_lock;
		[NonSerialized]
		int _year;
		[NonSerialized]
		int _month;
		[NonSerialized]
		int _day;
		[NonSerialized]
		DayOfWeek _dayOfWeek;
		[NonSerialized]
		TimeSpan _timeOfDay;
		//---------
		public static readonly JalaliDateTime MinValue = System.DateTime.MinValue.Date;
		public static readonly JalaliDateTime MaxValue = System.DateTime.MaxValue.Date;
		//---------
		public object Clone()
		{
			return new JalaliDateTime(this.Ticks, this.Kind);
		}
		public JalaliDateTime Add(TimeSpan value)
		{
			long ticks = this.Ticks + value.Ticks;
			return new JalaliDateTime(ticks, this.Kind);
		}
		public JalaliDateTime AddDays(double value)
		{
			long ticks = this.Ticks + (long)(TimeSpan.TicksPerDay * value);
			return new JalaliDateTime(ticks, this.Kind);
		}
		public JalaliDateTime AddMonths(int months)
		{
			int year = this.Year;
			int month = this.Month + months;
			while (month < 1)
			{
				year--;
				month += 12;
			}
			while (month > 12)
			{
				year++;
				month -= 12;
			}
			int day = Math.Min(this.Day, DaysInMonth(year, month));
			return new JalaliDateTime(year, month, day, this.TimeOfDay, this.Kind);
		}
		public JalaliDateTime AddYears(int years)
		{
			int year = this.Year + years;
			int day = Math.Min(this.Day, DaysInMonth(year, this.Month));
			int month = this.Month;
			return new JalaliDateTime(year, month, day, this.TimeOfDay, this.Kind);
		}
		public JalaliDateTime AddHours(double value)
		{
			var ticks = TimeSpan.TicksPerHour * value;
			return new JalaliDateTime(this.Ticks + (Int64)ticks, this.Kind);
		}
		public JalaliDateTime AddMinutes(double value)
		{
			var ticks = TimeSpan.TicksPerMinute * value;
			return new JalaliDateTime(this.Ticks + (Int64)ticks, this.Kind);
		}
		public JalaliDateTime AddSeconds(double value)
		{
			var ticks = TimeSpan.TicksPerSecond * value;
			return new JalaliDateTime(this.Ticks + (Int64)ticks, this.Kind);
		}
		public JalaliDateTime AddMilliseconds(double value)
		{
			var ticks = TimeSpan.TicksPerMillisecond * value;
			return new JalaliDateTime(this.Ticks + (Int64)ticks, this.Kind);
		}
		public JalaliDateTime AddTicks(Int64 value)
		{
			return new JalaliDateTime(this.Ticks + value, this.Kind);
		}
		public static JalaliDateTime FromTicks(Int64 ticks)
		{
			return new JalaliDateTime(ticks);
		}
		public long Ticks { get { _calculate(); return _ticks; } }
		public int Year
		{
			get { _calculate(); return _year; }
		}
		public int Month { get { _calculate(); return _month; } }
		public int Day { get { _calculate(); return _day; } }
		public DateTimeKind Kind { get { return _kind; } }
		public DayOfWeek DayOfWeek
		{
			get
			{
				_calculate();
				return _dayOfWeek;
			}
		}
		public int DayOfYear
		{
			get
			{
				var result = this.Day;
				for (var i = 1; i < this.Month; i++)
				{
					result += DaysInMonth(Year, i);
				}
				return result;
			}
		}
		public TimeSpan TimeOfDay { get { _calculate(); return _timeOfDay; } }
		public int Hour { get { _calculate(); return TimeOfDay.Hours; } }
		public int Minute { get { _calculate(); return TimeOfDay.Minutes; } }
		public int Second { get { _calculate(); return TimeOfDay.Seconds; } }
		public int Millisecond { get { _calculate(); return TimeOfDay.Milliseconds; } }
		/*public string MonthNameFa
		{
			get
			{
				_calculate();
				return JalaliDateTime.FarsiLongMonthName(this.Month);
			}
		}
		public string DayNameFa
		{
			get
			{
				_calculate();
				return DateTools.GetFarsiDayName(this.DayOfWeek);
			}
		}*/

		//-----------		
		void CheckRange()
		{
			var t = this.Ticks;
			if (t > System.DateTime.MaxValue.Ticks) throw new ArgumentOutOfRangeException("");
			if (t < System.DateTime.MinValue.Ticks) throw new ArgumentOutOfRangeException("");
		}
		static void Check(int year, int month, int day)
		{
			//if (JalaliDateTime.MinValue != null && JalaliDateTime.MaxValue != null)
			//	if (year < JalaliDateTime.MinValue.Year || year > JalaliDateTime.MaxValue.Year)
			//		throw new ArgumentOutOfRangeException("Year out of range");
			if (month > 12 || month < 1) throw new ArgumentOutOfRangeException("Month out of range");
			if (day > DaysInMonth(year, month) || day < 1) throw new ArgumentOutOfRangeException("Day out of range");
		}
		static void CheckTime(int hour, int minute, int second, int millisecond)
		{
			if (hour < 0 || hour > 23) throw new ArgumentOutOfRangeException("Hour out of range");
			if (minute < 0 || minute > 59) throw new ArgumentOutOfRangeException("Minute out of range");
			if (second < 0 || second > 59) throw new ArgumentOutOfRangeException("Second out of range");
			if (millisecond < 0 || millisecond > 999) throw new ArgumentOutOfRangeException("Millisecond out of range");
		}
		void _calculate()
		{
			if (_calculate_lock == null)
			{
				_calculate_lock = new object();
				_year = Int32.MinValue;
			}
			lock (_calculate_lock)
			{
				if (_year == Int32.MinValue)
				{
					int y;
					int m;
					int d;
					DayOfWeek dayOfWeek;
					TimeSpan timeOfDay;
					helper.FromTicks(_ticks, out y, out m, out d, out timeOfDay, out dayOfWeek);
					Check(y, m, d);
					_year = y;
					_month = m;
					_day = d;
					_dayOfWeek = dayOfWeek;
					_timeOfDay = timeOfDay;
				}
				if (_ticks == Int64.MinValue)
				{
					_ticks = helper.GetTicks(_year, _month, _day, _timeOfDay);
					_dayOfWeek = helper.GetDayOfWeek(_ticks);
				}
			}
		}
		public override string ToString()
		{
			return ToString("G", null);
		}

		public string ToShortDateString()
		{
			return JalaliDateTimeFormat.Format(this, "d", null);
		}

		public string ToLongDateString()
		{
			return JalaliDateTimeFormat.Format(this, "D", null);
		}

		public string ToShortTimeString()
		{
			return JalaliDateTimeFormat.Format(this, "t", null);
		}
		public string ToLongTimeString()
		{
			return JalaliDateTimeFormat.Format(this, "T", null);
		}

		public string ToString(string format)
		{
			return ToString(format, null);
		}
		public string ToString(IFormatProvider formatProvider)
		{
			return JalaliDateTimeFormat.Format(this, null, formatProvider);
		}

		public string ToString(string format, IFormatProvider? formatProvider)
		{
			return JalaliDateTimeFormat.Format(this, format, formatProvider);
		}
		/*
		public string ToShortDateString()
		{
			return String.Format("{0:0000}/{1:00}/{2:00}", this.Year, this.Month, this.Day);
		}
		/// <summary>
		/// 1393/05/10   01:27:20 PM
		/// </summary>
		/// <returns></returns>
		override public string ToString()
		{
			//int h = Hour <= 12 ? Hour : (this.Hour - 12);
			//if (h == 0) h = 12;
			//int m = this.Minute;
			//int s = this.Second;
			//string t = Hour < 12 ? "AM" : "PM";
			//return String.Format("{0:0000}/{1:00}/{2:00} {3}:{4:00}:{5:00} {6}", this.Year, this.Month, this.Day, h, m, s, t);
			return this.ToString(null, null);
		}
		/// <summary>
		/// Converts the value of the current <see cref="JalaliDateTime"/> object to its equivalent string representation using the specified format.
		/// 1393/05/10   01:27:20 PM
		/// yyyy: 1393
		/// yy:		93
		/// y:		
		/// MMMM:	
		/// MM:		
		/// M:		
		/// dddd:
		/// dd:
		/// d:
		/// HH:		13		Two digit hout between 00 and 23
		/// H:		13		One digit hour between 0 and 23
		/// hh:		01		Two digit hour between 00 and 12
		/// h:		1			One digit hour between 0 and 12
		/// mm:
		/// m:
		/// ss:
		/// s:
		/// fff:
		/// ff:
		/// f:
		/// tt:
		/// t:
		/// </summary>
		/// <param name="format">A standard or custom date and time format string (see Remarks).</param>
		/// <returns>A string representation of value of the current System.DateTime object as
		/// specified by format.
		/// </returns>
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}
		public string ToString(string format, IFormatProvider formatProvider)
		{
			//http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx
			string yyyy;
			if (this.Year >= 0)
				yyyy = this.Year.ToString().PadLeft(4, '0');
			else
				yyyy = '-' + (-this.Year).ToString().PadLeft(3, '0');

			string yyy;
			if (this.Year >= 0)
				yyy = this.Year.ToString().PadLeft(3, '0');
			else
				yyy = '-' + (-this.Year).ToString().PadLeft(2, '0');

			var yy = (this.Year % 100).ToString("D2");

			var y = (this.Year % 100).ToString();
			var M = this.Month;
			var d = this.Day;
			var tt = this.Hour < 12 ? "AM" : "PM";
			var H = this.Hour;
			var h = (this.Hour % 12) == 0 ? 12 : (this.Hour % 12);

			var m = this.Minute;
			var s = this.Second;
			var ms = this.Millisecond;

			//if (h > 12) h = h % 12;
			//if (h == 0) h = 12;

			var tokenMap = new Dictionary<string, string>
			{
				["yyyy"] = yyyy,
				["yyy"] = yyy,
				["yy"] = yy,
				["y"] = y,

				//["MMMM"] = DateTools.GetJalaliMonthName(this.Month),
				["MMMM"] = DateTools.GetEnglishJalaliMonthName(this.Month),
				["MMM"] = DateTools.GetEnglishJalaliMonthName(this.Month),
				["MM"] = M.ToString("D2"),
				["M"] = M.ToString(),

				["dddd"] = DateTools.GetFarsiDayName(this.DayOfWeek),
				["ddd"] = DateTools.GetFarsiAbbreviatedDayName(this.DayOfWeek),
				["dd"] = d.ToString("D2"),
				["d"] = d.ToString(),

				["HH"] = H.ToString("D2"),
				["H"] = H.ToString(),
				["hh"] = h.ToString("D2"),
				["h"] = h.ToString(),

				["mm"] = m.ToString("D2"),
				["m"] = m.ToString(),
				["ss"] = s.ToString("D2"),
				["s"] = s.ToString(),

				["fff"] = ms.ToString("D3").Substring(0, 3),
				["ff"] = ms.ToString("D3").Substring(0, 2),
				["f"] = ms.ToString("D3").Substring(0, 1),

				["tt"] = tt,
				["t"] = tt.Substring(0, 1)
			};

			// List of known format tokens
			var knownTokens = tokenMap.Keys.ToArray();

			// Tokenize and replace
			format ??= ((DateTimeFormatInfo)formatProvider?.GetFormat(typeof(DateTimeFormatInfo))
		   ?? Thread.CurrentThread.CurrentCulture.DateTimeFormat).FullDateTimePattern;
			var tokens = StringTokenizer.Tokenize(format ?? "yyyy/MM/dd h:mm:ss tt", knownTokens);
			var sb = new StringBuilder();

			foreach (var token in tokens)
			{
				sb.Append(tokenMap.TryGetValue(token, out var value) ? value : token);
			}

			return sb.ToString();
		}*/
		public JalaliDateTime Date
		{
			get { return new JalaliDateTime(this.Year, this.Month, this.Day, TimeSpan.Zero, this.Kind); }
		}
		public static JalaliDateTime Now
		{
			get { return (JalaliDateTime)System.DateTime.Now; }
		}
		public static JalaliDateTime Today
		{
			get { return (JalaliDateTime)System.DateTime.Today; }
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			JalaliDateTime that = (JalaliDateTime)obj;
			//if (this.Year != that.Year) return false;
			//if (this.Month != that.Month) return false;
			//if (this.Day != that.Day) return false;
			//if (this.Hour != that.Hour) return false;
			//if (this.Minute != that.Minute) return false;
			//if (this.Second != that.Second) return false;
			//if (this.Millisecond != that.Millisecond) return false;
			if (this.Ticks != that.Ticks) return false;
			return true;
		}
		public override int GetHashCode()
		{
			return this.ToString().GetHashCode();
		}
		//------------------------- Static Methods -----------------------

		public static bool IsLeapYear(int year)
		{
			return helper.IsLeapYear(year);
		}
		public static int DaysInMonth(int year, int month)
		{
			return helper.DaysInMonth(year, month);
		}

		public static JalaliDateTime FromDateTime(System.DateTime date)
		{
			return new JalaliDateTime(date.Ticks, date.Kind);
		}
		// Interfaces
		int IComparable.CompareTo(object o)
		{
			JalaliDateTime? that = null;
			if (o is JalaliDateTime) that = (JalaliDateTime)o;
			if (o is System.DateTime) that = (JalaliDateTime)o;
			if (!that.HasValue) throw new Exception("Invalid type");
			return this.Ticks.CompareTo(that.Value.Ticks);
		}
		int IComparable<JalaliDateTime>.CompareTo(JalaliDateTime that)
		{
			return this.Ticks.CompareTo(that.Ticks);
		}
		bool IEquatable<JalaliDateTime>.Equals(JalaliDateTime that)
		{
			return this.Ticks.Equals(that.Ticks);
		}

		public JalaliDateTime ToLocalTime()
		{
			return ((System.DateTime)this).ToLocalTime();
		}

		public JalaliDateTime ToUniversalTime()
		{
			return ((System.DateTime)this).ToUniversalTime();
		}

		/// <summary>
		/// Equivalent of new Date().getTime() for javascript support
		/// </summary>
		/// <returns></returns>
		public Int64 getTime()
		{
			var d0 = (JalaliDateTime)(new System.DateTime(1970, 1, 1));
			var time = this.ToUniversalTime() - d0.ToUniversalTime();
			//return (Int64)(time.TotalMilliseconds + 0.5);
			return (Int64)Math.Ceiling(time.TotalMilliseconds);
		}
	}
}