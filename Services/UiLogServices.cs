using System;
using ProjectSonicWave.Models;

namespace ProjectSonicWave.Services
{
    /// <summary>
    /// Eenvoudige UI-logservice met event voor updates.
    /// </summary>
    public sealed class UiLogService : ILogService
    {
        public event Action<LogEntry>? EntryAdded;
        
        public void Log(LogLevel level, string source, string message, string? details = null)
        {
            var entry = new LogEntry(DateTime.Now, level, source, message, details);
            EntryAdded?.Invoke(entry);
        }
    }
}

