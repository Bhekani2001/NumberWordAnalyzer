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

    [HttpPost("statistics")]
    public async Task<ActionResult<AnalyzeStatisticsResponseDto>> AnalyzeTextStatistics([FromBody] AnalyzeRequestDto request)
    {
        try
        {
            _logger.LogInformation("Received request to analyze text for statistics");

            if (request == null)
            {
                return BadRequest("Request body cannot be null");
            }

            var result = await _analyzerService.AnalyzeTextAsync(request);

            // Compute statistics
            var totalWords = result.WordCounts.Values.Sum();
            var mostFrequentWord = result.WordCounts.OrderByDescending(kv => kv.Value).First().Key;

            var statisticsResult = new AnalyzeStatisticsResponseDto
            {
                TotalWords = totalWords,
                MostFrequentWord = mostFrequentWord,
                WordCounts = result.WordCounts
            };

            return Ok(statisticsResult);
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

    [HttpGet("words")]
    public ActionResult<IEnumerable<string>> GetNumberWords()
    {
        var words = Domain.Constants.NumberWords.Words;
        return Ok(words);
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
            totalCount = NumberWords.Words.Length
        });
    }

    [HttpPost("batch")]
    public async Task<ActionResult<List<AnalyzeResponseDto>>> AnalyzeBatch([FromBody] List<AnalyzeRequestDto> requests)
    {
        var results = new List<AnalyzeResponseDto>();
        foreach (var request in requests)
        {
            var result = await _analyzerService.AnalyzeTextAsync(request);
            results.Add(result);
        }
        return Ok(results);
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

    [HttpPost("benchmark")]
    public async Task<ActionResult<BenchmarkResult>> BenchmarkAnalysis([FromBody] BenchmarkRequest request)
    {
        var stopwatch = Stopwatch.StartNew();
        var results = new List<AnalyzeResponseDto>();

        for (int i = 0; i < request.Iterations; i++)
        {
            var result = await _analyzerService.AnalyzeTextAsync(request.TestRequest);
            results.Add(result);
        }

        stopwatch.Stop();

        return Ok(new BenchmarkResult
        {
            Iterations = request.Iterations,
            TotalTime = stopwatch.Elapsed,
            AverageTime = stopwatch.Elapsed / request.Iterations,
            Results = results
        });
    }
}