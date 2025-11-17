using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberWordAnalyzer.Application.Dtos
{
    public class LogsQueryDto
    {
        public int? MaxEntries { get; set; } = 100;
        public string? Level { get; set; }
        public DateTime? Since { get; set; }
        public string? Contains { get; set; }
    }

}
