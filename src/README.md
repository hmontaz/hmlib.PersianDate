# hmlib.PersianDate

A lightweight .NET library for working with Jalali (Persian) dates. Provides a familiar `DateTime`-like API for parsing, formatting, and converting between Jalali and Gregorian calendars.

## ✨ Features

- Fully managed implementation — no native dependencies
- Supports formatting (e.g., `"yyyy/MM/dd"`, `"dddd, dd MMMM yyyy"`)
- Leap year handling based on official Jalali rules
- Conversion between Jalali and Gregorian
- JalaliDateTime and DateTime are fully castable (implicit/explicit conversion supported)
- Compatible with `.NET Standard 2.0` and above

## 📦 Installation

```bash
dotnet add package hmlib.PersianDate
```

```C#
JalaliDateTime jDate = new DateTime(2024, 01, 01);
var s1 = jDate.ToString();// "1402/10/11 12:00:00 AM"
var s2 = jDate.ToString(CultureInfo.GetCultureInfo("en-IR"));// "1402/10/11 12:00:00 AM"
var s3 = jDate.ToString(CultureInfo.GetCultureInfo("fa-IR"));// "۱۴۰۲/۱۰/۱۱ ۱۲:۰۰:۰۰ ق.ظ"
var s4 = jDate.ToString("dddd dd MMMM yyyy", CultureInfo.GetCultureInfo("fa-IR"));// "دوشنبه ۱۱ دی ۱۴۰۲"
```
