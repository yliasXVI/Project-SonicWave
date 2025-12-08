namespace ProjectSonicWave.Models
{
    public sealed class PsErrorInfo
    {
        public string Message { get; }
        public string? Category { get; }
        public string? StackTrace { get; }
        public PsErrorInfo(string message, string? category, string? stackTrace)
        {
            Message = message;
            Category = category;
            StackTrace = stackTrace;
        }

    }
}