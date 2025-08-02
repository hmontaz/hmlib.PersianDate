using hmlib.PersianDate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hmlib.PersianDateTests.JalaliDateTimeTests
{
	public class CastingAndOperators
	{
		[Fact]
		public void OperatorsTest()
		{
			Assert.Equal(TimeSpan.FromDays(1), new JalaliDateTime(1391, 1, 2) - new JalaliDateTime(1391, 1, 1));
			Assert.Equal(new JalaliDateTime(1391, 1, 1), new JalaliDateTime(1391, 1, 2) - TimeSpan.FromDays(1));
			Assert.Equal(new JalaliDateTime(1391, 1, 2), new JalaliDateTime(1391, 1, 1) + TimeSpan.FromDays(1));
			Assert.True(new JalaliDateTime(1391, 1, 2) > new JalaliDateTime(1391, 1, 1));
			Assert.False(new JalaliDateTime(1391, 1, 1) > new JalaliDateTime(1391, 1, 1));
			Assert.True(new JalaliDateTime(1391, 1, 3) >= new JalaliDateTime(1391, 1, 2));
			Assert.True(new JalaliDateTime(1391, 1, 2) >= new JalaliDateTime(1391, 1, 2));
			Assert.False(new JalaliDateTime(1391, 1, 1) >= new JalaliDateTime(1391, 1, 2));
			Assert.True(new JalaliDateTime(1391, 1, 1) < new JalaliDateTime(1391, 1, 2));
			Assert.False(new JalaliDateTime(1391, 1, 2) < new JalaliDateTime(1391, 1, 2));
			Assert.True(new JalaliDateTime(1391, 1, 1) <= new JalaliDateTime(1391, 1, 2));
			Assert.True(new JalaliDateTime(1391, 1, 2) <= new JalaliDateTime(1391, 1, 2));
			Assert.False(new JalaliDateTime(1391, 1, 3) <= new JalaliDateTime(1391, 1, 2));
			Assert.True(new JalaliDateTime(1391, 1, 1) == new JalaliDateTime(1391, 1, 1));
			Assert.False(new JalaliDateTime(1391, 1, 1) == new JalaliDateTime(1391, 1, 2));
			Assert.True(new JalaliDateTime(1391, 1, 1) != new JalaliDateTime(1391, 1, 2));
			Assert.False(new JalaliDateTime(1391, 1, 2) != new JalaliDateTime(1391, 1, 2));
		}
	}
}
