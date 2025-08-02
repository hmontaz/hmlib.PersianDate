using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hmlib.PersianDate.Utilities
{
	internal class JalaliLeap
	{
		int[] _list = null;
		protected int[] list
		{
			get
			{
				if (_list == null)
				{
					var array = new int[46];
					array[0] = 33;// 9 x1
					array[1] = 29;// 42 x1
					array[2] = 33;// 71 x7
					array[9] = 29;// 302 x1
					array[10] = 33;// 331 x6
					array[16] = 29;// 529 x1
					array[17] = 33;// 558 x2
					array[19] = 37;// 624 x1
					array[20] = 29;// 661 x1
					array[21] = 33;// 690 x8
					array[29] = 29;// 954 x1
					array[30] = 33;// 983 x7
					array[37] = 29;// 1214 x1
					array[38] = 37;// 1243 x1
					array[39] = 33;// 1280 x2
					array[41] = 29;// 1346 x1
					array[42] = 33;// 1375 x2
					array[44] = 37;// 1441 x1
					array[45] = 33;// 1478 x1
					var last = 0;
					// Fill List
					var y = 9;
					var result = new List<int>();
					for (var i = 0; i < array.Length; i++)
					{
						y = y + last;
						result.Insert(0, y);
						if (array[i] != 0) last = array[i];
					}
					_list = result.ToArray();
				}
				return _list;
			}
		}

		internal bool IsLeapYear(int year)
		{

			foreach (var _y in list)
			{
				//	if (year == _y) return true;
				if (year == _y - 1) return false;
				if (year >= _y) return (year - _y) % 4 == 0;
			}
			return year % 4 == 0;
		}

		internal void StartDate(out int year, out int month, out int day)
		{
			year = -621;
			month = 10;
			day = 16;
		}
	}
}
