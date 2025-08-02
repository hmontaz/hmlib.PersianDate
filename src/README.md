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
