using System;
using System.Globalization;

namespace hmlib.PersianDate.Globalization
{
	/// <summary>
	/// Provides culture-specific formatting information for JalaliDateTime.
	/// </summary>
	public sealed class JalaliDateTimeFormatInfo : ICloneable, IFormatProvider
	{
		/// <summary>
		/// Gets the invariant JalaliDateTimeFormatInfo.
		/// </summary>
		//public static JalaliDateTimeFormatInfo InvariantInfo => new JalaliDateTimeFormatInfo();

		/// <summary>
		/// Gets the current culture-based JalaliDateTimeFormatInfo.
		/// </summary>
		//public static JalaliDateTimeFormatInfo CurrentInfo => new JalaliDateTimeFormatInfo(CultureInfo.CurrentCulture);

		private readonly CultureInfo _baseCulture;

		/// <summary>
		/// Initializes a new instance with the default Persian culture.
		/// </summary>
		//public JalaliDateTimeFormatInfo() : this(new CultureInfo("fa-IR", useUserOverride: false))
		public JalaliDateTimeFormatInfo() : this(CultureInfo.CurrentCulture)
		{

		}

		public JalaliDateTimeFormatInfo(IFormatProvider formatProvider)
			: this((formatProvider is CultureInfo) ? (formatProvider as CultureInfo) : null)
		{
			//if (formatProvider != null && formatProvider is CultureInfo)
			//{
			//	this._baseCulture = (CultureInfo)formatProvider;
			//}
			//else
			//	this._baseCulture = CultureInfo.CurrentCulture;
		}
		/// <summary>
		/// Initializes a new instance based on a provided base culture.
		/// </summary>
		public JalaliDateTimeFormatInfo(CultureInfo baseCulture)
		{
			_baseCulture = (CultureInfo)(baseCulture ?? CultureInfo.CurrentCulture).Clone();
			Calendar = new PersianCalendar();
			// -------- Int



			if (_baseCulture.Name.StartsWith("fa-"))
			{
				MonthNames = new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
				DayNames = new[] { "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه", "شنبه" };
				AbbreviatedDayNames = new[] { "یک", "دو", "سه", "چهار", "پنج", "جمعه", "شنبه" };
				AMDesignator = "ق.ظ";
				PMDesignator = "ب.ظ";
				UsePersianDigits = true;
				UsePersianComma = true;
			}
			else
			{
				MonthNames = new[] { "Farvardin", "Ordibehesht", "Khordad", "Tir", "Mordad", "Shahrivar", "Mehr", "Aban", "Azar", "Dey", "Bahman", "Esfand" };
				DayNames = new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
				AbbreviatedDayNames = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
				AMDesignator = "AM";
				PMDesignator = "PM";
				UsePersianDigits = false;
				UsePersianComma = false;
			}
			AbbreviatedMonthNames = MonthNames;
			MonthGenitiveNames = MonthNames;
			AbbreviatedMonthGenitiveNames = MonthNames;
			ShortestDayNames = AbbreviatedDayNames;

			DateSeparator = "/";
			TimeSeparator = ":";

			var types = _baseCulture.CultureTypes;
			if (_baseCulture.ThreeLetterWindowsLanguageName == "IVL")
			{
				ShortDatePattern = "yyyy/MM/dd";
				ShortTimePattern = "HH:mm";
				LongTimePattern = "HH:mm:ss";

				LongDatePattern = "dddd, dd MMMM yyyy";
				MonthDayPattern = "MMMM dd";
				YearMonthPattern = "yyyy MMMM";
			}
			else
			{
				ShortDatePattern = "yyyy/M/d";
				ShortTimePattern = "H:mm";
				LongTimePattern = "h:mm:ss tt";

				LongDatePattern = "dddd, dd MMMM yyyy";
				MonthDayPattern = "MMMM dd";
				YearMonthPattern = "yyyy MMMM";
			}
			FullDateTimePattern = LongDatePattern + " " + LongTimePattern;

			CalendarWeekRule = CalendarWeekRule.FirstDay;
			FirstDayOfWeek = DayOfWeek.Saturday;
		}

		public string[] MonthNames { get; set; }
		public string[] AbbreviatedMonthNames { get; set; }
		public string[] MonthGenitiveNames { get; set; }
		public string[] AbbreviatedMonthGenitiveNames { get; set; }
		public string[] DayNames { get; set; }
		public string[] AbbreviatedDayNames { get; set; }
		public string[] ShortestDayNames { get; set; }

		public string AMDesignator { get; set; }
		public string PMDesignator { get; set; }
		public bool UsePersianDigits { get; private set; }
		public bool UsePersianComma { get; private set; }
		public string DateSeparator { get; set; }
		public string TimeSeparator { get; set; }

		public string ShortDatePattern { get; set; }
		public string LongDatePattern { get; set; }
		public string ShortTimePattern { get; set; }
		public string LongTimePattern { get; set; }
		public string FullDateTimePattern { get; set; }
		public string MonthDayPattern { get; set; }
		public string YearMonthPattern { get; set; }

		public CalendarWeekRule CalendarWeekRule { get; set; }
		public DayOfWeek FirstDayOfWeek { get; set; }
		public Calendar Calendar { get; set; }

		public object Clone() => MemberwiseClone();

		public object GetFormat(Type formatType)
		{
			return formatType == typeof(JalaliDateTimeFormatInfo) || formatType == typeof(IFormatProvider)
				? this
				: null;
		}
	}
}
