using Microsoft.Extensions.Logging;
using NumberWordAnalyzer.Application.Dtos;
using NumberWordAnalyzer.Application.Interfaces;
using NumberWordAnalyzer.Application.Models;
using System.Collections.Concurrent;

namespace NumberWordAnalyzer.Application.Services;

public class LogStorageService : ILogStorageService
{
    private readonly ConcurrentQueue<LogEntryDto> _logs = new();
    private readonly int _maxLogEntries = 1000;

    public void AddLog(LogEntry logEntry)
    {
        var dto = new LogEntryDto
        {
            Timestamp = logEntry.Timestamp,
            Level = logEntry.Level.ToString(),
            Message = logEntry.Message,
            Exception = logEntry.Exception?.ToString(),
            RequestId = logEntry.RequestId
        };

        _logs.Enqueue(dto);

        while (_logs.Count > _maxLogEntries)
        {
            _logs.TryDequeue(out _);
        }
    }

    public List<LogEntryDto> GetLogs(LogsQueryDto query)
    {
        var logs = _logs.ToList();

        if (!string.IsNullOrEmpty(query.Level))
        {
            logs = logs.Where(l => l.Level.Equals(query.Level, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (query.Since.HasValue)
        {
            logs = logs.Where(l => l.Timestamp >= query.Since.Value).ToList();
        }

        if (!string.IsNullOrEmpty(query.Contains))
        {
            logs = logs.Where(l => l.Message.Contains(query.Contains, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (query.MaxEntries.HasValue)
        {
            logs = logs.Take(query.MaxEntries.Value).ToList();
        }

        return logs.OrderByDescending(l => l.Timestamp).ToList();
    }

    public void ClearLogs()
    {
        while (_logs.Count > 0)
        {
            _logs.TryDequeue(out _);
        }
    }

    public int GetLogCount()
    {
        return _logs.Count;
    }
}