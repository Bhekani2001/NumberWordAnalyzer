using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberWordAnalyzer.Domain.Constants;

public static class NumberWords
{
    public static readonly string[] Words = {
        "one", "two", "three", "four", "five",
        "six", "seven", "eight", "nine"
    };

    public static readonly Dictionary<string, int> WordToNumber = new()
    {
        { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 },
        { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 }
    };
}
