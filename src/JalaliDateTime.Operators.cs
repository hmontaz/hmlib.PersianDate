using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;

namespace hmlib.PersianDate
{
	public partial struct JalaliDateTime
	{
		public static implicit operator JalaliDateTime(DateTime value)
		{
			return new JalaliDateTime(value.Ticks, value.Kind);
		}
		public static implicit operator System.DateTime(JalaliDateTime value)
		{
			return new System.DateTime(value.Ticks, value.Kind);
		}
		public static implicit operator System.DateTime?(JalaliDateTime value)
		{
			return new System.DateTime(value.Ticks, value.Kind);
		}
		/// <summary>
		/// Subtracts a specified date and time from another specified date and time and returns a time interval.
		/// </summary>
		/// <param name="d1">A JalaliDateTime (the minuend).</param>
		/// <param name="d2">A JalaliDateTime (the subtrahend).</param>
		/// <returns>A TimeSpan that is the time interval between d1 and d2; that is, d1 minus d2.</returns>
		[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
		public static TimeSpan operator -(JalaliDateTime d1, JalaliDateTime d2)
		{
			return TimeSpan.FromTicks(d1.Ticks - d2.Ticks);
		}

		/// <summary>
		/// Subtracts a specified time interval from a specified date and time and returns a new date and time.
		/// </summary>
		/// <param name="d">A JalaliDateTime.</param>
		/// <param name="t">A TimeSpan.</param>
		/// <returns>A JalaliDateTime whose value is the value of d minus the value of t.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// The resulting JalaliDateTime is less than MinValue or greater than MaxValue.
		/// </exception>
		[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
		public static JalaliDateTime operator -(JalaliDateTime d, TimeSpan t)
		{
			return d.AddTicks(-t.Ticks);
		}

		/// <summary>
		/// Determines whether two specified instances of JalaliDateTime are not equal.
		/// </summary>
		/// <param name="d1">A JalaliDateTime.</param>
		/// <param name="d2">A JalaliDateTime.</param>
		/// <returns>true if d1 and d2 do not represent the same date and time; otherwise, false.</returns>
		[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
		public static bool operator !=(JalaliDateTime d1, JalaliDateTime d2)
		{
			return !(d1 == d2);
		}

		/// <summary>
		/// Adds a specified time interval to a specified date and time, yielding a new date and time.
		/// </summary>
		/// <param name="d">A JalaliDateTime.</param>
		/// <param name="t">A TimeSpan.</param>
		/// <returns>A JalaliDateTime that is the sum of the values of d and t.</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// The resulting JalaliDateTime is less than MinValue or greater than MaxValue.
		/// </exception>
		public static JalaliDateTime operator +(JalaliDateTime d, TimeSpan t)
		{
			return d.AddTicks(t.Ticks);
		}

		/// <summary>
		/// Determines whether one specified JalaliDateTime is less than another specified JalaliDateTime.
		/// </summary>
		/// <param name="d1">A JalaliDateTime.</param>
		/// <param name="d2">A JalaliDateTime.</param>
		/// <returns>true if d1 is less than d2; otherwise, false.</returns>
		[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
		public static bool operator <(JalaliDateTime d1, JalaliDateTime d2)
		{
			return d1.Ticks < d2.Ticks;
		}

		/// <summary>
		/// Determines whether one specified JalaliDateTime is less than or equal to another specified JalaliDateTime.
		/// </summary>
		/// <param name="d1">A JalaliDateTime.</param>
		/// <param name="d2">A JalaliDateTime.</param>
		/// <returns>true if d1 is less than or equal to d2; otherwise, false.</returns>
		[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
		public static bool operator <=(JalaliDateTime d1, JalaliDateTime d2)
		{
			return d1.Ticks <= d2.Ticks;
		}

		/// <summary>
		/// Determines whether two specified instances of JalaliDateTime are equal.
		/// </summary>
		/// <param name="d1">A JalaliDateTime.</param>
		/// <param name="d2">A JalaliDateTime.</param>
		/// <returns>true if d1 and d2 represent the same Jalali date and time; otherwise, false.</returns>
		[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
		public static bool operator ==(JalaliDateTime d1, JalaliDateTime d2)
		{
			if ((object)d1 == null)
				return ((object)d2 == null);
			return d1.Equals(d2);
		}

		/// <summary>
		/// Determines whether one specified JalaliDateTime is greater than another specified JalaliDateTime.
		/// </summary>
		/// <param name="d1">A JalaliDateTime.</param>
		/// <param name="d2">A JalaliDateTime.</param>
		/// <returns>true if d1 is greater than d2; otherwise, false.</returns>
		[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
		public static bool operator >(JalaliDateTime d1, JalaliDateTime d2)
		{
			return d1.Ticks > d2.Ticks;
		}

		/// <summary>
		/// Determines whether one specified JalaliDateTime is greater than or equal to another specified JalaliDateTime.
		/// </summary>
		/// <param name="d1">A JalaliDateTime.</param>
		/// <param name="d2">A JalaliDateTime.</param>
		/// <returns>true if d1 is greater than or equal to d2; otherwise, false.</returns>
		[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
		public static bool operator >=(JalaliDateTime d1, JalaliDateTime d2)
		{
			return d1.Ticks >= d2.Ticks;
		}
	}
}
