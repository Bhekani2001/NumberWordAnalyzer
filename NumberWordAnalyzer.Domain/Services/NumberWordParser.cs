using NumberWordAnalyzer.Domain.Constants;

namespace NumberWordAnalyzer.Domain.Services;

public class NumberWordParser
{
    public Dictionary<string, int> CountNumberWords(string inputText)
    {
        if (string.IsNullOrWhiteSpace(inputText))
            return InitializeEmptyResult();

        // Convert to lowercase for case-insensitive matching
        var lowerInput = inputText.ToLowerInvariant();
        var inputCharCount = GetCharacterCount(lowerInput);
        var result = InitializeEmptyResult();

        foreach (var numberWord in NumberWords.Words)
        {
            result[numberWord] = CountPossibleOccurrences(numberWord, inputCharCount);
        }

        return result;
    }

    private Dictionary<char, int> GetCharacterCount(string text)
    {
        var charCount = new Dictionary<char, int>();
        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                if (charCount.ContainsKey(c))
                    charCount[c]++;
                else
                    charCount[c] = 1;
            }
        }
        return charCount;
    }

    private int CountPossibleOccurrences(string word, Dictionary<char, int> availableChars)
    {
        var wordCharCount = GetCharacterCount(word);
        int maxOccurrences = int.MaxValue;

        foreach (var kvp in wordCharCount)
        {
            char requiredChar = kvp.Key;
            int requiredCount = kvp.Value;

            if (!availableChars.ContainsKey(requiredChar))
                return 0;

            int availableCount = availableChars[requiredChar];
            int possibleForThisChar = availableCount / requiredCount;

            maxOccurrences = Math.Min(maxOccurrences, possibleForThisChar);
        }

        return maxOccurrences == int.MaxValue ? 0 : maxOccurrences;
    }

    private Dictionary<string, int> InitializeEmptyResult()
    {
        var result = new Dictionary<string, int>();
        foreach (var word in NumberWords.Words)
        {
            result[word] = 0;
        }
        return result;
    }
}