using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberWordAnalyzer.Application.Dtos;

public class AnalyzeResponseDto
{
    public Dictionary<string, int> WordCounts { get; set; } = new();
}
