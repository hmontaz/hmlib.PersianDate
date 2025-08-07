using hmlib.PersianDate.Utilities;
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

		private JalaliDateTimeFormatInfo(IFormatProvider? formatProvider)
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
		private JalaliDateTimeFormatInfo(CultureInfo? baseCulture)
		{
			_baseCulture = (CultureInfo)(baseCulture ?? CultureInfo.CurrentCulture).Clone();
			Calendar = new PersianCalendar();
			// -------- Int



			if (_baseCulture.Name.StartsWith("fa-"))
			{
				MonthNames = new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
				AbbreviatedMonthNames = MonthNames;
				//AbbreviatedMonthNames = new[] { "فر", "ارد", "خرد", "تیر", "مرد", "شهری", "مهر", "آبا", "آذر", "دی", "بهمن", "اسف" };
				DayNames = new[] { "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه", "شنبه" };
				AbbreviatedDayNames = new[] { "یک", "دو", "سه", "چهار", "پنج", "جمعه", "شنبه" };
				ADesignator = "ق";
				AMDesignator = "ق.ظ";
				PDesignator = "ب";
				PMDesignator = "ب.ظ";
				UsePersianDigits = true;
				UsePersianComma = true;
			}
			else
			{
				MonthNames = new[] { "Farvardin", "Ordibehesht", "Khordad", "Tir", "Mordad", "Shahrivar", "Mehr", "Aban", "Azar", "Dey", "Bahman", "Esfand" };
				AbbreviatedMonthNames = new[] { "Far", "Ord", "Kho", "Tir", "Mor", "Sha", "Meh", "Aba", "Aza", "Dey", "Bah", "Esf" };
				DayNames = new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
				AbbreviatedDayNames = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
				ADesignator = "A";
				AMDesignator = "AM";
				PDesignator = "P";
				PMDesignator = "PM";
				UsePersianDigits = false;
				UsePersianComma = false;
			}
			//AbbreviatedMonthNames = MonthNames;
			MonthGenitiveNames = MonthNames;
			AbbreviatedMonthGenitiveNames = AbbreviatedMonthNames;
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

		public static JalaliDateTimeFormatInfo GetInstance(IFormatProvider? formatProvider = null)
		{
			if (formatProvider is JalaliDateTimeFormatInfo)
			{
				return (JalaliDateTimeFormatInfo)formatProvider;
			}
			return new JalaliDateTimeFormatInfo(formatProvider);
		}

		public string[] MonthNames { get; set; }
		public string[] AbbreviatedMonthNames { get; set; }
		public string[] MonthGenitiveNames { get; set; }
		public string[] AbbreviatedMonthGenitiveNames { get; set; }
		public string[] DayNames { get; set; }
		public string[] AbbreviatedDayNames { get; set; }
		public string[] ShortestDayNames { get; set; }

		public string ADesignator { get; set; }
		public string AMDesignator { get; set; }
		public string PDesignator { get; set; }
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

		public object? GetFormat(Type formatType)
		{
			return formatType == typeof(JalaliDateTimeFormatInfo) || formatType == typeof(IFormatProvider)
				? this
				: null;
		}

		/*public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;

				hash = hash * 31 + MonthNames.HashArray();
				hash = hash * 31 + AbbreviatedMonthNames.HashArray();
				hash = hash * 31 + MonthGenitiveNames.HashArray();
				hash = hash * 31 + AbbreviatedMonthGenitiveNames.HashArray();
				hash = hash * 31 + DayNames.HashArray();
				hash = hash * 31 + AbbreviatedDayNames.HashArray();
				hash = hash * 31 + ShortestDayNames.HashArray();

				hash = hash * 31 + (ADesignator?.GetHashCode() ?? 0);
				hash = hash * 31 + (AMDesignator?.GetHashCode() ?? 0);
				hash = hash * 31 + (PDesignator?.GetHashCode() ?? 0);
				hash = hash * 31 + (PMDesignator?.GetHashCode() ?? 0);

				hash = hash * 31 + UsePersianDigits.GetHashCode();
				hash = hash * 31 + UsePersianComma.GetHashCode();

				hash = hash * 31 + (DateSeparator?.GetHashCode() ?? 0);
				hash = hash * 31 + (TimeSeparator?.GetHashCode() ?? 0);

				hash = hash * 31 + (ShortDatePattern?.GetHashCode() ?? 0);
				hash = hash * 31 + (LongDatePattern?.GetHashCode() ?? 0);
				hash = hash * 31 + (ShortTimePattern?.GetHashCode() ?? 0);
				hash = hash * 31 + (LongTimePattern?.GetHashCode() ?? 0);
				hash = hash * 31 + (FullDateTimePattern?.GetHashCode() ?? 0);
				hash = hash * 31 + (MonthDayPattern?.GetHashCode() ?? 0);
				hash = hash * 31 + (YearMonthPattern?.GetHashCode() ?? 0);

				hash = hash * 31 + CalendarWeekRule.GetHashCode();
				hash = hash * 31 + FirstDayOfWeek.GetHashCode();

				hash = hash * 31 + (_baseCulture?.Name?.GetHashCode() ?? 0);
				hash = hash * 31 + (_baseCulture?.EnglishName?.GetHashCode() ?? 0);

				hash = hash * 31 + (Calendar?.GetType().FullName?.GetHashCode() ?? 0); // safer than Calendar.GetHashCode()

				return hash;
			}
		}
		*/
	}
}
