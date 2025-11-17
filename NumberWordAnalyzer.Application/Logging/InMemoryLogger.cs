using Microsoft.Extensions.Logging;
using NumberWordAnalyzer.Application.Models;
using NumberWordAnalyzer.Application.Services;

namespace NumberWordAnalyzer.Application.Logging;

public class InMemoryLogger : ILogger
{
    private readonly string _categoryName;
    private readonly LogStorageService _logStorage;

    public InMemoryLogger(string categoryName, LogStorageService logStorage)
    {
        _categoryName = categoryName;
        _logStorage = logStorage;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);

        var logEntry = new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            Level = logLevel,
            Message = $"[{_categoryName}] {message}",
            Exception = exception,
            RequestId = GetRequestIdFromState(state)
        };

        _logStorage.AddLog(logEntry);
    }

    private static string? GetRequestIdFromState<TState>(TState state)
    {
        if (state is IEnumerable<KeyValuePair<string, object>> properties)
        {
            return properties.FirstOrDefault(p => p.Key == "RequestId").Value?.ToString();
        }
        return null;
    }
}

public class InMemoryLoggerProvider : ILoggerProvider
{
    private readonly LogStorageService _logStorage;

    public InMemoryLoggerProvider(LogStorageService logStorage)
    {
        _logStorage = logStorage;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new InMemoryLogger(categoryName, _logStorage);
    }

    public void Dispose() { }
}