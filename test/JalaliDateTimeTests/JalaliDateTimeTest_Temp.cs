using System;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using hmlib.PersianDate;
using System.Runtime.CompilerServices;
using Xunit;

namespace hmlib.PersianDateTests.JalaliDateTimeTests
{

	public class JalaliDateTimeTest
	{
		[Fact]
		public void KindTest_1()
		{
			//Assert.Equal("", ((JalaliDateTime)new DateTime(1999, 9, 3, 8, 0, 0)));

			Assert.Equal(DateTimeKind.Unspecified, new JalaliDateTime(0).Kind);

			Assert.Equal("1378/6/12 8:00:00 AM", new JalaliDateTime(1378, 06, 12, 8, 0, 0).ToString());
			//Assert.Equal("1378/6/12 0:30:00", new JalaliDateTime(1378, 6, 12, 8, 0, 0).ToLocalTime().ToString());
			//Assert.Equal("1378/6/12 3:30:00", new JalaliDateTime(1378, 6, 12, 8, 0, 0).ToUniversalTime().ToString());
			Assert.Equal("1378/6/12 8:00:00 AM", new JalaliDateTime(new JalaliDateTime(1378, 6, 12, 8, 0, 0).Ticks, DateTimeKind.Utc).ToString());
			Assert.Equal("1378/6/12 8:00:00 AM", new JalaliDateTime(new JalaliDateTime(1378, 6, 12, 8, 0, 0).Ticks, DateTimeKind.Local).ToString());

			//Assert.Equal("9/3/1999 12:30:00 PM", new System.DateTime(1999, 9, 3, 8, 0, 0).ToLocalTime().ToString());
			//Assert.Equal("9/3/1999 3:30:00 AM", new System.DateTime(1999, 9, 3, 8, 0, 0).ToUniversalTime().ToString());
			Assert.Equal("9/3/1999 8:00:00 AM", new DateTime(new DateTime(1999, 9, 3, 8, 0, 0).Ticks, DateTimeKind.Utc).ToString());
			Assert.Equal("9/3/1999 8:00:00 AM", new DateTime(new DateTime(1999, 9, 3, 8, 0, 0).Ticks, DateTimeKind.Local).ToString());

		}

		[Fact]
		public void ImplicitYear_Test()
		{
			Assert.Equal(new JalaliDateTime(1392, 10, 10), JalaliDateTime.Parse("92/10/10"));
			Assert.Equal(new JalaliDateTime(1402, 10, 10), JalaliDateTime.Parse("02/10/10"));
		}
		/*[Fact]
		public void Checkpoint_Tests()
		{
			_AreEqual(new DateTime(1, 1, 1), (DateTime)JalaliDateTime.MinValue);
			_AreEqual(new DateTime(0622, 03, 22), new JalaliDateTime(0001, 1, 1));
			_AreEqual(new DateTime(1978, 11, 14), new JalaliDateTime(1357, 08, 23));//known
			_AreEqual(new DateTime(1988, 04, 01), new JalaliDateTime(1367, 01, 12));//known

			Assert.Equal(new JalaliDateTime(9378, 08, 11), JalaliDateTime.MaxValue);

			_Test(Properties.Resources.Jalali_Big.Replace("\r\n", "\0").Split('\0', StringSplitOptions.RemoveEmptyEntries));
			_Test(Properties.Resources.Jalali_Contemporary.Replace("\r\n", "\0").Split('\0', StringSplitOptions.RemoveEmptyEntries));
		}*/

		void _Test(string[] items)
		{
			foreach (var item in items)
			{
				var split = item.Split('|');
				var dateTime = DateTime.Parse(split[0]);
				var jalaliDateTime = JalaliDateTime.Parse(split[1]);
				_AreEqual(dateTime, jalaliDateTime);
			}
		}

		/*[Fact]
		
		public void Generate()
		{
			TextWriter file = new StreamWriter("C:\\hmlib_temp\\jalalidatetimes.txt");
			for (var i = 0; i < 800; i++)
			{
				var j = new JalaliDateTime(1000 + i, 1, 1);//.AddDays(-1);
				var d = (System.DateTime)j;
				file.WriteLine(d.ToString("yyyy/MM/dd") + "|" + j.ToString("yyyy/MM/dd"));
			}
			file.Flush();
			file.Close();
		}*/

		void _AreEqual(DateTime dateTime, JalaliDateTime jalaliDateTime)
		{
			var msg = "Expected " + dateTime + " Actual: " + (DateTime)jalaliDateTime;
			if (dateTime.Ticks != jalaliDateTime.Ticks) Assert.Fail(msg);
			if (dateTime != (DateTime)jalaliDateTime) Assert.Fail(msg);
			if (dateTime.DayOfWeek != jalaliDateTime.DayOfWeek) Assert.Fail(msg);
		}

		[Fact]
		public void LoopTest1()
		{
			Random rand = new Random();
			DateTime min = new DateTime(1, 1, 1);
			DateTime max = DateTime.Today;
			DateTime d = min;
			int prev_d = 0;
			while (d <= max)
			{
				JalaliDateTime j1 = new JalaliDateTime(d.Ticks);
				JalaliDateTime j2 = new JalaliDateTime(j1.Year, j1.Month, j1.Day, j1.TimeOfDay);
				Assert.Equal(d.DayOfWeek, j2.DayOfWeek);
				Assert.Equal(j1.Year, j2.Year);
				Assert.Equal(j1.Month, j2.Month);
				Assert.Equal(j1.Day, j2.Day);
				Assert.Equal(j1.Ticks, j2.Ticks);

				Assert.Equal(d, (DateTime)j1);
				Assert.Equal(j1, j1.Clone());
				Assert.Equal(d.DayOfWeek, j1.DayOfWeek);
				Assert.Equal(d.Ticks, j1.Ticks);
				Assert.Equal(d.Ticks, j1.Ticks);
				Assert.Equal(d.TimeOfDay, j1.TimeOfDay);
				Assert.NotEqual(prev_d, j1.Day);

				prev_d = j1.Day;
				d = d.Date.AddDays(1).AddTicks(rand.NextInt64(0, TimeSpan.TicksPerDay));
			}
		}
		[Fact]
		public void PerformanceTest1()
		{
			var rand = new Random();
			DateTime dt0 = DateTime.Now;
			for (int i = 0; i < 100000; i++)
			{
				var j = new JalaliDateTime(rand.Next(1, 1392), 1, 1);
			}
			var dt = DateTime.Now - dt0;
			Assert.True(dt < TimeSpan.FromSeconds(2));
		}
		[Fact]
		public void PerformanceTest2()
		{
			var rand = new Random();
			DateTime dt0 = DateTime.Now;
			DateTime min = new DateTime(1700, 1, 1);
			DateTime max = DateTime.Now.AddYears(100);
			for (int i = 0; i < 1000000; i++)
			{
				var ticks = rand.NextInt64(min.Ticks, max.Ticks);
				//new DateTime(rand.NextInt64(min.Ticks, max.Ticks));
				var j = new JalaliDateTime(ticks);
				var t = j.Year;
				//new JalaliDateTime(min.AddDays(i).Ticks);
			}
			var dt = DateTime.Now - dt0;
			Assert.True(dt < TimeSpan.FromSeconds(4));
		}

		[Fact]
		public void BackAndForth()
		{
			DateTime date = new DateTime(1000, 1, 1);
			while (date <= new DateTime(2100, 1, 1))
			{
				date = date.AddDays(1);
				JalaliDateTime j1 = JalaliDateTime.FromDateTime(date);
				JalaliDateTime j2 = new JalaliDateTime(j1.Year, j1.Month, j1.Day);
				Assert.Equal(date.DayOfWeek, j2.DayOfWeek);
				Assert.Equal(j1.Year, j2.Year);
				Assert.Equal(j1.Month, j2.Month);
				Assert.Equal(j1.Day, j2.Day);
				Assert.Equal(j1.Ticks, j2.Ticks);
				Assert.Equal(j1.DayOfWeek, j2.DayOfWeek);
				JalaliDateTime j3 = JalaliDateTime.FromTicks(j1.Ticks);
				_AreEqual(j1, j3);
			}
		}

		[Fact]
		public void NowTest()
		{
			Assert.Equal(JalaliDateTime.Today, (JalaliDateTime)DateTime.Today);
			var now = DateTime.Now;
			Assert.Equal(((JalaliDateTime)now).Date, (JalaliDateTime)now.Date);
			DateTime actual_now = JalaliDateTime.FromDateTime(now);
			Assert.Equal(now, actual_now);
		}

		/*[Fact]
		
		public void Continuity_Test()
		{
			var date = new DateTime(1900, 1, 1);
			var to = new DateTime(2050, 1, 1);
			var _prev = (JalaliDateTime)date;
			while (date < to)
			{
				date = date.AddDays(1);
				var _new = (JalaliDateTime)date;
				var diff = _new.Ticks - _prev.Ticks;
				if (diff != TimeSpan.TicksPerDay) throw new Exception();
				if (_new.ToString("yyyyMMdd").ParseAsInt32() <= _prev.ToString("yyyyMMdd").ParseAsInt32()) throw new Exception();

				_prev = _new;
			}
		}*/

		[Fact]
		public void Many_Years_Test()
		{
			var y0 = 1961;
			var days = new[] {
				21, 21, 22, 21, 21, 21, 21, 21, 21, 21, //1961[1340] ~ 1970[1349]
				21, 21, 21, 21, 21, 21, 21, 21, 21, 21, //1971[1350] ~ 1980[1359]
				21, 21, 21, 21, 21, 21, 21, 21, 21, 21, //1981[1360] ~ 1990[1369]
				21, 21, 21, 21, 21, 20, 21, 21, 21, 20, //1991[1370] ~ 2000[1379]
				21, 21, 21, 20, 21, 21, 21, 20, 21, 21, //2001[1380] ~ 2010[1389]
				21, 20, 21, 21, 21, 20, 21, 21, 21, 20, //2011[1390] ~ 2020[1399]
				21, 21, 21, 20, 21, 21, 21, 20, 20, 21, //2021[1400] ~ 2030[1409]
				21, 20, 20, 21, 21, 20, 20, 21, 21, 20, //2031[1410] ~ 2040[1419]
				20, 21, 21, 20, 20, 21, 21, 20, 20, 21, //2041[1420] ~ 2050[1429]
				21, 20, 20, 21, 21, 20, 20, 21, 21, 20, //2051[1430] ~ 2060[1439]
				20, 20, 21, 20, 20, 20, 21, 20, 20, 20, //2061[1440] ~ 2070[1449]
				 };

			for (var i = 0; i < days.Length; i++)
			{
				Assert.Equal(new DateTime(y0 + i, 3, days[i]), (DateTime)new JalaliDateTime(y0 - 621 + i, 1, 1));

			}

		}

		//[Fact]
		//
		//public void ConvertMany()
		//{
		//	var lines = System.IO.File.ReadLines(@"dates-source.txt").ToArray();
		//	var result = lines.Select(a => (DateTime)JalaliDateTime.Parse("13" + a)).Select(a => a.ToString()).ToArray();
		//	System.IO.File.WriteAllLines(@"C:\hmlib_temp\dates-dest.txt", result);
		//}

		[Fact]
		public void DayOfYearTest()
		{
			Assert.Equal(20, new DateTime(2010, 1, 20).DayOfYear);
			Assert.Equal(51, new DateTime(2010, 2, 20).DayOfYear);

			Assert.Equal(20, new JalaliDateTime(1390, 1, 20).DayOfYear);
			Assert.Equal(51, new JalaliDateTime(1390, 2, 20).DayOfYear);
		}

		/*[Fact]
		
		public void getTime_Test()
		{
			//1348/10/11
			var jd = (JalaliDateTime)new SimpleDateTime(1970, 1, 1);
			Assert.Equal(0, jd.getTime());

			Assert.Equal(0, new JalaliDateTime(1348, 10, 11).getTime());
		}*/
	}
}
