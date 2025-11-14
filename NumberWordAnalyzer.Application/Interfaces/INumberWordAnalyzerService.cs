using NumberWordAnalyzer.Application.Dtos;

namespace NumberWordAnalyzer.Application.Interfaces;

public interface INumberWordAnalyzerService
{
    Task<AnalyzeResponseDto> AnalyzeTextAsync(AnalyzeRequestDto request);
}