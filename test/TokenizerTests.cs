using hmlib.PersianDate.Utilities;
using Xunit;

namespace hmlib.PersianDateTests
{

	public class TokenizerTests
	{
		[Fact]
		public void TokenizeFormat_ShouldReturnExpectedTokens()
		{
			// Arrange
			string format = "yyyy/MM/dd h:mm:ss tt";
			string[] knownTokens = new[]
			{
			"yyyy", "yyy", "yy", "y",
			"MMMM", "MMM", "MM", "M",
			"dddd", "ddd", "dd", "d",
			"HH", "H", "hh", "h",
			"mm", "m", "ss", "s",
			"fff", "ff", "f",
			"tt", "t"
		};

			// Act
			var result = StringTokenizer.Tokenize(format, knownTokens);

			// Assert
			var expected = new[] { "yyyy", "/", "MM", "/", "dd", " ", "h", ":", "mm", ":", "ss", " ", "tt" };

			Assert.Equal(expected, result);
		}

		[Fact]
		public void TokenizeFormat_ShouldHandleBadOrderOfTokens()
		{
			// Arrange
			string format = "A+AA+B+BB";
			string[] knownTokens = new[]
			{
				"A", "AA", "B", "BB"
			};
			// Act
			var result = StringTokenizer.Tokenize(format, knownTokens);
			// Assert
			var expected = new[] { "A", "+", "AA", "+", "B", "+", "BB" };
			Assert.Equal(expected, result);
		}
	}
}