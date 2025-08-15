using hmlib.PersianDate.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace hmlib.PersianDateTests
{
	public class StringTokenizerTests
	{

		[Theory]
		[InlineData("yyyy-MM t", new[] { "yyyy", "-", "MM", " ", "t" }, "01010")]
		[InlineData("yyyy/MM/dd h:mm:ss tt", new[] { "yyyy", "/", "MM", "/", "dd", " ", "h", ":", "mm", ":", "ss", " ", "tt" }, "0101010101010")]
		public void TokenizeFormat_ShouldReturnExpectedTokens(string format, string[] expectedTokens, string s)
		{
			// Arrange
			var expectedIsLiteral = s.Select(c => c == '1' ? true : false).ToArray();
			// Act
			var result = StringTokenizer.Tokenize(format, StringTokenizer.KnownTokens);
			// Assert
			Assert.Equal(expectedTokens, result.Select(a => a.Value));
			Assert.Equal(expectedIsLiteral, result.Select(a => a.IsLiteral));
		}
		[Fact]
		public void TokenizeFormat_ShouldHandleBadOrderOfTokens()
		{
			// Arrange
			string format = "A+AA+B+BB";
			string[] knownTokens = ["A", "AA", "B", "BB"];
			// Act
			var result = StringTokenizer.Tokenize(format, knownTokens);
			// Assert
			var expected = new[] { "A", "+", "AA", "+", "B", "+", "BB" };
			Assert.Equal(expected, result.Select(a => a.Value));
		}
	}
}
