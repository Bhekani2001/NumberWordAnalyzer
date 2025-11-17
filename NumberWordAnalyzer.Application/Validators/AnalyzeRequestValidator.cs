using NumberWordAnalyzer.Application.Dtos;

namespace NumberWordAnalyzer.Application.Validators;

public class AnalyzeRequestValidator
{
    public (bool isValid, string errorMessage) Validate(AnalyzeRequestDto request)
    {
        if (request == null)
            return (false, "Request cannot be null");

        if (string.IsNullOrWhiteSpace(request.InputText))
            return (false, "InputText cannot be null or empty");

        if (request.InputText.Length > 1000000) 
            return (false, "InputText is too large");

        return (true, string.Empty);
    }
}