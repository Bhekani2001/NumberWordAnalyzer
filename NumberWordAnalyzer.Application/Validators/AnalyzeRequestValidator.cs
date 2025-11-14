using NumberWordAnalyzer.Application.Dtos;

namespace NumberWordAnalyzer.Application.Validators;

public class AnalyzeRequestValidator
{
    public (bool isValid, string errorMessage) Validate(AnalyzeRequestDto request)
    {
        // First check if request is null
        if (request == null)
            return (false, "Request cannot be null");

        // Then check the InputText property
        if (string.IsNullOrWhiteSpace(request.InputText))
            return (false, "InputText cannot be null or empty");

        if (request.InputText.Length > 1000000) // 1MB limit
            return (false, "InputText is too large");

        return (true, string.Empty);
    }
}