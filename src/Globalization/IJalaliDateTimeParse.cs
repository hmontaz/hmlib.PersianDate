using System;
using System.Globalization;

namespace hmlib.PersianDate.Globalization
{
	internal interface IJalaliDateTimeParse
	{
		JalaliDateTime ParseExact(string s, string format, DateTimeFormatInfo dateTimeFormatInfo, DateTimeStyles none);
		bool TryParse(string s, IFormatProvider? formatProvider, DateTimeStyles styles, out JalaliDateTime result);
		bool TryParse(string s, out JalaliDateTime result);
		bool TryParseExact(string input, string format, IFormatProvider? provider, DateTimeStyles style, bool strictDate, bool strictTime, out JalaliDateTime result);
	}
}