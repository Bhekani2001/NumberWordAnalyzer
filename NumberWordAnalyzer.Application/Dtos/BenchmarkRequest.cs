using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberWordAnalyzer.Application.Dtos
{
    public class BenchmarkRequest
    {
        public AnalyzeRequestDto TestRequest { get; set; }
        public int Iterations { get; set; } = 10;
    }

    public class BenchmarkResult
    {
        public int Iterations { get; set; }
        public TimeSpan TotalTime { get; set; }
        public TimeSpan AverageTime { get; set; }
        public List<AnalyzeResponseDto> Results { get; set; }
    }

    public class CustomAnalyzeRequest
    {
        public string InputText { get; set; }
        public string[] CustomWords { get; set; }
    }
}
