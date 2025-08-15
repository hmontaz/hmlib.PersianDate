using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace hmlib.PersianDate.Utilities
{
	static class Extensions
	{
		public static string IfNullOrEmpty(this string value, string defaultValue)
		{
			if (string.IsNullOrEmpty(value)) return defaultValue;
			return value;
		}

		public static int? GetInt32(this GroupCollection groups, string groupName)
		{
			if (groups[groupName].Success &&
				int.TryParse(groups[groupName].Value, out int result))
			{
				return result;
			}
			return null;
		}

		public static string? GetValue(this GroupCollection groups, params string[] groupNames)
		{
			foreach (var groupName in groupNames)
			{
				var value = groups[groupName];
				if (groups[groupName].Success)
				{
					return groups[groupName].Value;
				}
			}
			return null;
		}

		internal static StringBuilder NormalizeSpaces(this StringBuilder sb)
		{
			// Replace non-breaking spaces and thin spaces with regular spaces
			return sb.Replace('\u202F', ' ').Replace('\u00A0', ' ');
		}

	}
}
