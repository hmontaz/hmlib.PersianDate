using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hmlib.PersianDate.Utilities
{
	internal class JalaliHelper : DateTimeHelper
	{
		static JalaliLeap _leap;
		static JalaliHelper()
		{
			_leap = new JalaliLeap();
		}
		public JalaliHelper(bool highPerformance)
			: base(highPerformance)
		{
		}
		public override void StartDate(out int year, out int month, out int day)
		{
			_leap.StartDate(out year, out month, out day);
			//year = -621;
			//month = 5;
			//day = 13;
			//year = -621;
			//month = 10;
			//day = 16;
		}
		public override int DaysInYear(int year)
		{
			if (IsLeapYear(year)) return 366;
			return 365;
		}
		public override int DaysInMonth(int year, int month)
		{
			switch (month)
			{
				case 1:		// فروردین
				case 2:		// اردیبهشت
				case 3:		// خرداد
				case 4:		// تیر
				case 5:		// مرداد
				case 6:		// شهریور
					return 31;
				case 7:		// مهر
				case 8:		// آبان
				case 9:		// آذر
				case 10:	// دی
				case 11:	// بهمن
					return 30;
				case 12:	// اسفند
					return IsLeapYear(year) ? 30 : 29;
			}
			throw new Exception();
		}
		public bool IsLeapYear(int year)
		{
			return _leap.IsLeapYear(year);
		}
	}
}
