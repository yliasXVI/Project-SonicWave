using System;
using ProjectSonicWave.Services;

namespace ProjectSonicWave.Models
{
    public record LogEntry
    (
        DateTime Timestamp,
        LogLevel Level,
        string Source, 
        string Message,
        string? Details
    ); 
}