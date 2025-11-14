using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberWordAnalyzer.Domain.Models;

public class AnalysisResult
{
    public Dictionary<string, int> WordCounts { get; set; } = new();

    public AnalysisResult()
    {
        // Initialize with all number words set to 0
        foreach (var word in Constants.NumberWords.Words)
        {
            WordCounts[word] = 0;
        }
    }
}
