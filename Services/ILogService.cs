namespace ProjectSonicWave.Services
{
   public enum LogLevel { Info, Warning, Error }
     public interface ILogService {
       void Log(LogLevel level, string source, string message, string? details = null);
  }
}



