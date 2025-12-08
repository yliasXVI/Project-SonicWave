namespace ProjectSonicWave.Models
{
    public class GroupMemberDto
    {
        public string DisplayName { get; set; } = string.Empty;
        public string PrimarySmtpAddress { get; set; } = string.Empty;
        public string RecipientType { get; set; } = string.Empty;
    }
}
