using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProjectSonicWave.Models;

namespace ProjectSonicWave.Services
{
    public static class CsvExporter
    {
    public static void ExportMailboxes(string path, IList<MailboxDto> items)
        {
            Write(path, new[]
        { "DisplayName", "PrimarySmtpAddress", "RecipientTypeDetails", "Database", "TotalItemSize", "ItemCount"}, items.Select(m => new[]

        {
            m.DisplayName,
            m.PrimarySmtpAddress,
            m.RecipientTypeDetails,
            m.Database,
            m.TotalItemSize,
            m.ItemCount }));
        }
    public static void  ExportGroups(string path, IList<GroupDto> items)
        {
            Write(path, new[] { "DisplayName", "PrimarySmtpAddress", "Identity", "GroupType"}, items.Select(g => new[] 
        {
            g.DisplayName,
            g.PrimarySmtpAddress,
            g.Identity,
            g.GroupType
        }));
    }

    public static void ExportGroupMembers(string path, IList<GroupMemberDto> items)
        {
            Write(path, new[] { "Displayname", "PrimarySmtpAddress", "RecipientType"}, items.Select (m => new[]
            {
                m.DisplayName,
                m.PrimarySmtpAddress,
                m.RecipientType
            }));
        }
    public static void ExportUsers(string path, IList<UserDto> items)
        {
            Write(path, new[] { "DisplayName", "UserPrincipalName", "Mail", "JobTitle", "Department", "Licenses" }, items.Select (u => new[]
            {
                u.DisplayName,
                u.UserPrincipalName,
                u.Mail,
                u.JobTitle,
                u.Department,
                u.Licenses
            }));
        }

    private static void Write(string path, IEnumerable<string> headers, IEnumerable<IEnumerable<string?>> rows)
        {
            using var sw = new StreamWriter(path, false, Encoding.UTF8);
             sw.WriteLine(string.Join(",", headers.Select(Escape)));
              foreach (var r in rows)
               sw.WriteLine(string.Join(",", r.Select(Escape)));
        }
    private static string Escape(string? s)
        {
            if (s == null) return "";
            var needs = s.Contains(',') || s.Contains('"') || s.Contains('\n');
            s = s.Replace("\"", "\"\"");
            return needs ? $"\"{s}\"" : s;
        }

    }
}