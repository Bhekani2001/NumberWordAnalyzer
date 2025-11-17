using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberWordAnalyzer.Application.Dtos
{
    public class LogsResponseDto
    {
        public List<LogEntryDto> Logs { get; set; } = new();
        public int TotalCount { get; set; }
        public DateTime RetrievedAt { get; set; }
        public string Environment { get; set; }
    }
}
