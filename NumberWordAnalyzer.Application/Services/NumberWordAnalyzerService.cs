using NumberWordAnalyzer.Application.Dtos;
using NumberWordAnalyzer.Application.Interfaces;
using NumberWordAnalyzer.Application.Validators;
using NumberWordAnalyzer.Domain.Services;

namespace NumberWordAnalyzer.Application.Services;

public class NumberWordAnalyzerService : INumberWordAnalyzerService
{
    private readonly NumberWordParser _parser;
    private readonly AnalyzeRequestValidator _validator;

    public NumberWordAnalyzerService()
    {
        _parser = new NumberWordParser();
        _validator = new AnalyzeRequestValidator();
    }

    public Task<AnalyzeResponseDto> AnalyzeTextAsync(AnalyzeRequestDto request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request), "Request cannot be null");

        var (isValid, errorMessage) = _validator.Validate(request);
        if (!isValid)
            throw new ArgumentException(errorMessage);

        var wordCounts = _parser.CountNumberWords(request.InputText);

        var result = new AnalyzeResponseDto
        {
            WordCounts = wordCounts
        };

        return Task.FromResult(result);
    }
}