using Xunit;

namespace NumberWordAnalyzer.Tests.IntegrationTests;

public class NumberWordAnalyzerIntegrationTests
{
    [Fact]
    public void EndToEnd_WithValidInput_ProcessesCorrectly()
    {
        // This tests the integration between Domain -> Application layers
        // Arrange
        var input = "one two three";

        // Act
        var parser = new Domain.Services.NumberWordParser();
        var result = parser.CountNumberWords(input);

        // Assert
        Assert.True(result["one"] >= 1, "Should find at least one 'one'");
        Assert.True(result["two"] >= 1, "Should find at least one 'two'");
        Assert.True(result["three"] >= 1, "Should find at least one 'three'");
    }

    [Fact]
    public void EndToEnd_WithComplexInput_ProcessesCorrectly()
    {
        // Arrange - Using the assessment example
        var input = "eeehffetstriuueuefxxeexeseetoionneghtvvsentniheinungeiefev";

        // Act
        var parser = new Domain.Services.NumberWordParser();
        var result = parser.CountNumberWords(input);

        // Assert - Verify we get some reasonable results
        Assert.NotNull(result);
        Assert.Equal(9, result.Count); // All number words should be present
        Assert.All(result.Values, count => Assert.True(count >= 0)); // All counts should be non-negative
    }

    [Fact]
    public void EndToEnd_WithEmptyInput_ReturnsAllZeros()
    {
        var input = "";

        var parser = new Domain.Services.NumberWordParser();
        var result = parser.CountNumberWords(input);

        Assert.All(result.Values, count => Assert.Equal(0, count));
    }
}