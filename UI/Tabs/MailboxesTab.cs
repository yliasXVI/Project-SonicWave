using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectSonicWave.Models;
using ProjectSonicWave.Services;
namespace ProjectSonicWave.UI.Tabs
{
   public partial class MailboxesTab : UserControl
   {
       private readonly PowerShellService _ps;
       private readonly ILogService _log;
       private List<MailboxDto> _allMailboxes = new();
       public MailboxesTab(PowerShellService ps, ILogService log)
       {
           _ps = ps ?? throw new ArgumentNullException(nameof(ps));
           _log = log ?? throw new ArgumentNullException(nameof(log));
           InitializeComponent();
           lblStatus.Text = "Status: idle";
       }
       private async void btnLoad_Click(object sender, EventArgs e)
       {
           await LoadMailboxesAsync();
       }
       private async Task LoadMailboxesAsync(CancellationToken ct = default)
       {
           try
           {
               SetStatus("Bezig met ophalen...");
               btnLoad.Enabled = false;
               var result = await _ps.GetMailboxesTypedAsync(ct);
               _allMailboxes = result.ToList();
               ApplyFilter();
               SetStatus($"Gereed. {_allMailboxes.Count} mailboxen geladen.");
               _log.Log(LogLevel.Info, "MailboxesTab", $"Get-Mailbox: {_allMailboxes.Count} items");
           }
           catch (PowerShellExecutionException ex)
           {
               SetStatus("Fout bij ophalen (PowerShell).");
               MessageBox.Show(this, ex.Message, "PowerShell error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               _log.Log(LogLevel.Error, "MailboxesTab", ex.Message,
                   string.Join("\n", ex.Errors.Select(er => er.Message)));
           }
           catch (Exception ex)
           {
               SetStatus("Fout bij ophalen.");
               MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               _log.Log(LogLevel.Error, "MailboxesTab", ex.Message, ex.ToString());
           }
           finally
           {
               btnLoad.Enabled = true;
           }
       }
       private void txtFilter_TextChanged(object sender, EventArgs e)
       {
           ApplyFilter();
       }
       private void ApplyFilter()
       {
           var term = txtFilter.Text.Trim();
           IEnumerable<MailboxDto> view = _allMailboxes;
           if (!string.IsNullOrWhiteSpace(term))
           {
               view = view.Where(m =>
                   (m.DisplayName ?? "").Contains(term, StringComparison.OrdinalIgnoreCase) ||
                   (m.PrimarySmtpAddress ?? "").Contains(term, StringComparison.OrdinalIgnoreCase));
           }
           mailboxBindingSource.DataSource = view.ToList();
       }
       private void SetStatus(string message)
       {
           lblStatus.Text = $"Status: {message}";
       }
   }
}