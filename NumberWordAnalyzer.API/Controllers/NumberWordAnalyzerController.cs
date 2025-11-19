using Microsoft.AspNetCore.Mvc;
using NumberWordAnalyzer.Application.Dtos;
using NumberWordAnalyzer.Application.Interfaces;
using NumberWordAnalyzer.Application.Validators;
using NumberWordAnalyzer.Domain.Constants;
using System.Diagnostics;
using System.Text;

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
                generate = "/api/NumberWordAnalyzer/generate",
                batch = "/api/NumberWordAnalyzer/batch",
                benchmark = "/api/NumberWordAnalyzer/benchmark",
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

    [HttpGet("generate")]
    public IActionResult GeneratePuzzle([FromQuery] int minLength = 30)
    {
        try
        {
            var random = new Random();
            var words = NumberWords.Words;
            var puzzle = new StringBuilder();

            while (puzzle.Length < minLength)
            {
                var word = words[random.Next(words.Length)];
                var scrambled = new string(word.OrderBy(x => random.Next()).ToArray());
                puzzle.Append(scrambled);
            }

            return Ok(new
            {
                puzzle = puzzle.ToString(),
                originalWords = words,
                generatedLength = puzzle.Length,
                minLengthRequested = minLength
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating puzzle");
            return StatusCode(500, "An error occurred while generating the puzzle");
        }
    }

    [HttpPost("batch")]
    public async Task<ActionResult<List<AnalyzeResponseDto>>> AnalyzeBatch([FromBody] List<AnalyzeRequestDto> requests)
    {
        try
        {
            _logger.LogInformation("Received batch analysis request with {RequestCount} items", requests?.Count ?? 0);

            if (requests == null || !requests.Any())
            {
                return BadRequest("Batch requests cannot be null or empty");
            }

            var results = new List<AnalyzeResponseDto>();
            foreach (var request in requests)
            {
                var result = await _analyzerService.AnalyzeTextAsync(request);
                results.Add(result);
            }

            _logger.LogInformation("Batch analysis completed for {RequestCount} items", requests.Count);
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing batch analysis");
            return StatusCode(500, "An error occurred while processing batch analysis");
        }
    }

    [HttpPost("benchmark")]
    public async Task<ActionResult<object>> BenchmarkAnalysis([FromBody] AnalyzeRequestDto request)
    {
        try
        {
            _logger.LogInformation("Starting benchmark analysis");

            if (request == null)
            {
                return BadRequest("Request body cannot be null");
            }

            var stopwatch = Stopwatch.StartNew();
            var iterations = 100;
            var results = new List<AnalyzeResponseDto>();

            for (int i = 0; i < iterations; i++)
            {
                var result = await _analyzerService.AnalyzeTextAsync(request);
                results.Add(result);
            }

            stopwatch.Stop();

            var benchmarkResult = new
            {
                iterations = iterations,
                totalTimeMs = stopwatch.ElapsedMilliseconds,
                averageTimeMs = stopwatch.ElapsedMilliseconds / (double)iterations,
                inputLength = request.InputText?.Length ?? 0,
                firstResult = results.First(),
                timestamp = DateTime.UtcNow
            };

            _logger.LogInformation("Benchmark completed: {AverageTime}ms average", benchmarkResult.averageTimeMs);
            return Ok(benchmarkResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during benchmark analysis");
            return StatusCode(500, "An error occurred during benchmark analysis");
        }
    }

    [HttpGet("ordered")]
    public IActionResult GetOrderedWords()
    {
        var words = NumberWords.Words;
        var orderedString = string.Join("", words);

        return Ok(new
        {
            orderedString = orderedString,
            words = words,
            totalLength = orderedString.Length,
            description = "Ordered concatenation of all supported number words"
        });
    }

    [HttpGet("random")]
    public IActionResult GetRandomWords()
    {
        var random = new Random();
        var words = NumberWords.Words.OrderBy(x => random.Next()).ToArray();
        var randomString = string.Join("", words);

        return Ok(new
        {
            randomString = randomString,
            words = words,
            totalLength = randomString.Length,
            description = "Randomized concatenation of all supported number words"
        });
    }
}