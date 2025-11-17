using Microsoft.Extensions.Logging;
using NumberWordAnalyzer.Application.Dtos;
using NumberWordAnalyzer.Application.Models;

namespace NumberWordAnalyzer.Application.Interfaces;

public interface ILogStorageService
{
    /// <summary>
    /// Adds a log entry to the storage
    /// </summary>
    /// <param name="logEntry">The log entry to add</param>
    void AddLog(LogEntry logEntry);

    /// <summary>
    /// Retrieves logs based on the specified query parameters
    /// </summary>
    /// <param name="query">The query parameters for filtering logs</param>
    /// <returns>List of log entries matching the query</returns>
    List<LogEntryDto> GetLogs(LogsQueryDto query);

    /// <summary>
    /// Clears all stored logs
    /// </summary>
    void ClearLogs();

    /// <summary>
    /// Gets the total number of logs currently stored
    /// </summary>
    int GetLogCount();
}