using NumberWordAnalyzer.Application.Dtos;
using NumberWordAnalyzer.Application.Services;
using Xunit;

namespace NumberWordAnalyzer.Tests.UnitTests;

public class NumberWordAnalyzerServiceTests
{
    private readonly NumberWordAnalyzerService _service;

    public NumberWordAnalyzerServiceTests()
    {
        _service = new NumberWordAnalyzerService();
    }

    [Fact]
    public async Task AnalyzeTextAsync_WithValidRequest_ReturnsAnalysisResult()
    {
        // Arrange
        var request = new AnalyzeRequestDto { InputText = "one two three" };

        // Act
        var result = await _service.AnalyzeTextAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.WordCounts);
        Assert.Equal(9, result.WordCounts.Count); 
    }

    [Fact]
    public async Task AnalyzeTextAsync_WithNullRequest_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AnalyzeTextAsync(null));
    }

    [Fact]
    public async Task AnalyzeTextAsync_WithEmptyInputText_ThrowsArgumentException()
    {
        var request = new AnalyzeRequestDto { InputText = "" };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.AnalyzeTextAsync(request));
    }
}