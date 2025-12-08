namespace ProjectSonicWave.Models
{
    public sealed class MailboxDto
    {
      public string? DisplayName { get; set; }
      public string? PrimarySmtpAddress { get; set; }
      public string? RecipientTypeDetails { get; set; }
      public string? Database { get; set; }
      public string? TotalItemSize { get; set; }
      public string? ItemCount { get; set; }
    }
}