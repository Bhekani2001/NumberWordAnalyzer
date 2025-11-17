using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberWordAnalyzer.Application.Dtos
{
    public class ValidationResultDto
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public int InputLength { get; set; }
        public int CharacterCount { get; set; }
        public int WordCount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
