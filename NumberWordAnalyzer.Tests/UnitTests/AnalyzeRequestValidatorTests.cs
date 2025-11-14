using NumberWordAnalyzer.Application.Dtos;
using NumberWordAnalyzer.Application.Validators;
using Xunit;

namespace NumberWordAnalyzer.Tests.UnitTests;

public class AnalyzeRequestValidatorTests
{
    private readonly AnalyzeRequestValidator _validator;

    public AnalyzeRequestValidatorTests()
    {
        _validator = new AnalyzeRequestValidator();
    }

    [Fact]
    public void Validate_WithValidRequest_ReturnsValid()
    {
        // Arrange
        var request = new AnalyzeRequestDto { InputText = "test" };

        // Act
        var (isValid, errorMessage) = _validator.Validate(request);

        // Assert
        Assert.True(isValid);
        Assert.Empty(errorMessage);
    }

    [Fact]
    public void Validate_WithNullRequest_ReturnsInvalid()
    {
        AnalyzeRequestDto request = null;

        var (isValid, errorMessage) = _validator.Validate(request);

        Assert.False(isValid);
        Assert.NotEmpty(errorMessage);
    }

    [Fact]
    public void Validate_WithEmptyInputText_ReturnsInvalid()
    {
        var request = new AnalyzeRequestDto { InputText = "" };

        var (isValid, errorMessage) = _validator.Validate(request);

        Assert.False(isValid);
        Assert.NotEmpty(errorMessage);
    }

    [Fact]
    public void Validate_WithWhitespaceInputText_ReturnsInvalid()
    {
        var request = new AnalyzeRequestDto { InputText = "   " };

        var (isValid, errorMessage) = _validator.Validate(request);

        Assert.False(isValid);
        Assert.NotEmpty(errorMessage);
    }
}