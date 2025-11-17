using Microsoft.AspNetCore.Mvc;
using NumberWordAnalyzer.Application.Dtos;
using NumberWordAnalyzer.Application.Interfaces;
using NumberWordAnalyzer.Application.Validators;
using NumberWordAnalyzer.Domain.Constants;
using System.Diagnostics;

namespace NumberWordAnalyzer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NumberWordAnalyzerController : ControllerBase
{
    private readonly INumberWordAnalyzerService _analyzerService;
    private readonly ILogger<NumberWordAnalyzerController> _logger;

    public NumberWordAnalyzerController(INumberWordAnalyzerService analyzerService, ILogger<NumberWordAnalyzerController> logger)
    {
        _analyzerService = analyzerService;
        _logger = logger;
    }

    [HttpGet]
    [Route("/")]
    public IActionResult Root()
    {
        return Ok(new
        {
            message = "NumberWordAnalyzer API is running",
            version = "1.0.0",
            timestamp = DateTime.UtcNow,
            endpoints = new
            {
                api = "/api/NumberWordAnalyzer",
                health = "/api/NumberWordAnalyzer/health",
                words = "/api/NumberWordAnalyzer/words",
                swagger = "/swagger"
            }
        });
    }

    [HttpPost]
    public async Task<ActionResult<AnalyzeResponseDto>> AnalyzeText([FromBody] AnalyzeRequestDto request)
    {
        try
        {
            _logger.LogInformation("Received request to analyze text");

            if (request == null)
            {
                return BadRequest("Request body cannot be null");
            }

            var result = await _analyzerService.AnalyzeTextAsync(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid request received");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request");
            return StatusCode(500, "An internal server error occurred");
        }
    }

    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            uptime = DateTime.UtcNow - Process.GetCurrentProcess().StartTime
        });
    }

    [HttpGet("words")]
    public IActionResult GetSupportedWords()
    {
        return Ok(new
        {
            supportedWords = NumberWords.Words,
            totalCount = NumberWords.Words.Length,
            description = "English number words from one to nine"
        });
    }

    [HttpPost("validate")]
    public IActionResult ValidateInput([FromBody] AnalyzeRequestDto request)
    {
        var validator = new AnalyzeRequestValidator();
        var (isValid, errorMessage) = validator.Validate(request);

        return Ok(new
        {
            isValid,
            errorMessage,
            inputLength = request.InputText?.Length ?? 0,
            characterCount = request.InputText?.Distinct().Count() ?? 0
        });
    }

    [HttpGet("algorithm")]
    public IActionResult GetAlgorithmInfo()
    {
        return Ok(new
        {
            name = "Character Frequency Counting Algorithm",
            description = "Efficiently detects number words in scrambled text by analyzing character distributions",
            approach = "Counts character frequencies and calculates maximum possible word formations",
            complexity = "O(n) - Linear time complexity",
            example = new
            {
                input = "ooonnneee",
                explanation = "Can form 'one' 3 times (3 o's, 3 n's, 3 e's)"
            }
        });
    }
}