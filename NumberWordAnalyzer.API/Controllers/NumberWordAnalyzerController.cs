using Microsoft.AspNetCore.Mvc;
using NumberWordAnalyzer.Application.Dtos;
using NumberWordAnalyzer.Application.Interfaces;

namespace NumberWordAnalyzer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NumberWordAnalyzerController : ControllerBase
{
    private readonly INumberWordAnalyzerService _analyzerService;
    private readonly ILogger<NumberWordAnalyzerController> _logger;

    public NumberWordAnalyzerController(
        INumberWordAnalyzerService analyzerService,
        ILogger<NumberWordAnalyzerController> logger)
    {
        _analyzerService = analyzerService;
        _logger = logger;
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
}