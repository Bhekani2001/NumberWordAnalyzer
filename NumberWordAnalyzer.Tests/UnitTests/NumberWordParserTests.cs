using NumberWordAnalyzer.Domain.Services;
using Xunit;

namespace NumberWordAnalyzer.Tests.UnitTests;

public class NumberWordParserTests
{
    private readonly NumberWordParser _parser;

    public NumberWordParserTests()
    {
        _parser = new NumberWordParser();
    }

    [Fact]
    public void CountNumberWords_WithEmptyString_ReturnsAllZeros()
    {
        // Arrange
        var input = "";

        // Act
        var result = _parser.CountNumberWords(input);

        // Assert
        Assert.All(result.Values, count => Assert.Equal(0, count));
    }

    [Fact]
    public void CountNumberWords_WithNullInput_ReturnsAllZeros()
    {
        string input = null;

        var result = _parser.CountNumberWords(input);

        Assert.All(result.Values, count => Assert.Equal(0, count));
    }

    [Fact]
    public void CountNumberWords_WithSingleWord_ReturnsCorrectCount()
    {
        var input = "one";

        var result = _parser.CountNumberWords(input);

        Assert.Equal(1, result["one"]);
        Assert.Equal(0, result["two"]);
    }

    [Fact]
    public void CountNumberWords_WithMultipleOccurrences_ReturnsCorrectCounts()
    {
        var input = "oneonetwo";

        var result = _parser.CountNumberWords(input);

        Assert.Equal(2, result["one"]);
        Assert.Equal(1, result["two"]);
    }

    [Fact]
    public void CountNumberWords_WithScrambledCharacters_ReturnsCorrectCounts()
    {
        var input = "enoowt";

        var result = _parser.CountNumberWords(input);

        Assert.Equal(1, result["one"]);
        Assert.Equal(1, result["two"]);
    }

    [Fact]
    public void CountNumberWords_WithMixedCase_ReturnsCorrectCounts()
    {
        var input = "OneTwO";

        var result = _parser.CountNumberWords(input);

        Assert.Equal(1, result["one"]);
        Assert.Equal(1, result["two"]);
    }

    [Fact]
    public void CountNumberWords_WithSpecialCharacters_IgnoresSpecialChars()
    {
        var input = "one!two@three123";

        var result = _parser.CountNumberWords(input);

        Assert.Equal(1, result["one"]);
        Assert.Equal(1, result["two"]);
        Assert.Equal(1, result["three"]);
    }
}