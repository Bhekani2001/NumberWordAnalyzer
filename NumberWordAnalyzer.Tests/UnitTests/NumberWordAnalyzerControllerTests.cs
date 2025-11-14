using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NumberWordAnalyzer.API.Controllers;
using NumberWordAnalyzer.Application.Dtos;
using NumberWordAnalyzer.Application.Interfaces;
using Xunit;

namespace NumberWordAnalyzer.Tests.UnitTests;

public class NumberWordAnalyzerControllerTests
{
    private readonly Mock<INumberWordAnalyzerService> _mockService;
    private readonly Mock<ILogger<NumberWordAnalyzerController>> _mockLogger;
    private readonly NumberWordAnalyzerController _controller;

    public NumberWordAnalyzerControllerTests()
    {
        _mockService = new Mock<INumberWordAnalyzerService>();
        _mockLogger = new Mock<ILogger<NumberWordAnalyzerController>>();
        _controller = new NumberWordAnalyzerController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task AnalyzeText_WithValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = new AnalyzeRequestDto { InputText = "test" };
        var expectedResponse = new AnalyzeResponseDto { WordCounts = new Dictionary<string, int>() };

        _mockService.Setup(s => s.AnalyzeTextAsync(request))
                   .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.AnalyzeText(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<AnalyzeResponseDto>(okResult.Value);
        Assert.Equal(expectedResponse, response);
    }

    [Fact]
    public async Task AnalyzeText_WithNullRequest_ReturnsBadRequest()
    {
        AnalyzeRequestDto request = null;

        var result = await _controller.AnalyzeText(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Request body cannot be null", badRequestResult.Value);
    }

    [Fact]
    public async Task AnalyzeText_WhenServiceThrowsArgumentException_ReturnsBadRequest()
    {
        var request = new AnalyzeRequestDto { InputText = "" };
        _mockService.Setup(s => s.AnalyzeTextAsync(request))
                   .ThrowsAsync(new ArgumentException("Invalid input"));

        var result = await _controller.AnalyzeText(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Invalid input", badRequestResult.Value);
    }
}