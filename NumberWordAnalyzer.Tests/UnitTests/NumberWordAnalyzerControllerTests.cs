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
        // Arrange
        AnalyzeRequestDto request = null;

        // Act
        var result = await _controller.AnalyzeText(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Request body cannot be null", badRequestResult.Value);
    }

    [Fact]
    public async Task AnalyzeText_WhenServiceThrowsArgumentException_ReturnsBadRequest()
    {
        // Arrange
        var request = new AnalyzeRequestDto { InputText = "" };
        _mockService.Setup(s => s.AnalyzeTextAsync(request))
                   .ThrowsAsync(new ArgumentException("Invalid input"));

        // Act
        var result = await _controller.AnalyzeText(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Invalid input", badRequestResult.Value);
    }

    [Fact]
    public void HealthCheck_ReturnsHealthyStatus()
    {
        // Act
        var result = _controller.HealthCheck();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        Assert.NotNull(response);

        var responseType = response.GetType();
        var statusProperty = responseType.GetProperty("status");
        var statusValue = statusProperty?.GetValue(response) as string;
        Assert.Equal("Healthy", statusValue);
    }

    [Fact]
    public void GetSupportedWords_ReturnsWordsList()
    {
        // Act
        var result = _controller.GetSupportedWords();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        Assert.NotNull(response);

        var responseType = response.GetType();
        var wordsProperty = responseType.GetProperty("supportedWords");
        var wordsValue = wordsProperty?.GetValue(response) as string[];
        Assert.NotNull(wordsValue);
        Assert.Contains("one", wordsValue);
        Assert.Contains("nine", wordsValue);
    }

    [Fact]
    public void GetAlgorithmInfo_ReturnsAlgorithmDetails()
    {
        // Act
        var result = _controller.GetAlgorithmInfo();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        Assert.NotNull(response);

        var responseType = response.GetType();
        var nameProperty = responseType.GetProperty("name");
        var nameValue = nameProperty?.GetValue(response) as string;
        Assert.Equal("Character Frequency Counting Algorithm", nameValue);
    }

    [Fact]
    public void ValidateInput_WithValidRequest_ReturnsValidationResult()
    {
        // Arrange
        var request = new AnalyzeRequestDto { InputText = "valid text" };

        // Act
        var result = _controller.ValidateInput(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        Assert.NotNull(response);

        var responseType = response.GetType();
        var isValidProperty = responseType.GetProperty("isValid");
        var isValidValue = (bool)(isValidProperty?.GetValue(response) ?? false);
        Assert.True(isValidValue);
    }

    [Fact]
    public void ValidateInput_WithNullRequest_ReturnsBadRequest()
    {
        // Arrange
        AnalyzeRequestDto request = null;

        // Act
        var result = _controller.ValidateInput(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Request body cannot be null", badRequestResult.Value);
    }

    [Fact]
    public void Root_ReturnsApiInformation()
    {
        // Act
        var result = _controller.Root();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        Assert.NotNull(response);

        var responseType = response.GetType();
        var messageProperty = responseType.GetProperty("message");
        var messageValue = messageProperty?.GetValue(response) as string;
        Assert.Equal("NumberWordAnalyzer API is running", messageValue);
    }
}