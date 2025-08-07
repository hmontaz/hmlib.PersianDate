using hmlib.PersianDate.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hmlib.PersianDateTests.JalaliDateTimeTests
{
	public class JalaliDateTimeParseTests
	{
		[Fact]
		public void TokenizeFormatTest()
		{
			var parser = new JalaliDateTimeParser();
			var format = "yyyy/MM/dd HH:mm:ss";
			var tokens = parser.TokenizeFormat(format);
			var expectedTokens = new List<string> { "yyyy", "/", "MM", "/", "dd", " ", "HH", ":", "mm", ":", "ss" };

			Assert.Equal(expectedTokens, tokens);
		}
	}
}
