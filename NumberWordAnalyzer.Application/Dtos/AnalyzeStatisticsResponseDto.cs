namespace NumberWordAnalyzer.Application.Dtos;

public class AnalyzeStatisticsResponseDto
{
    public int TotalWords { get; set; }
    public string MostFrequentWord { get; set; } = string.Empty;
    public Dictionary<string, int> WordCounts { get; set; } = new();
}