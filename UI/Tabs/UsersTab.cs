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
   public class UsersTab : UserControl
   {
       private readonly PowerShellService _ps;
       private readonly ILogService _log;
       private readonly DataGridView _dgvUsers = new();
       private readonly TextBox _txtFilter = new();
       private readonly Label _lblStatus = new() { AutoSize = true, Text = "Status: idle" };
       private readonly Button _btnLoad = new() { Text = "Load Users" };
       private readonly Button _btnExport = new() { Text = "Export Users CSV" };
       private readonly BindingSource _bsUsers = new();
       private List<UserDto> _all = new();
       private CancellationTokenSource? _cts;
       public UsersTab(PowerShellService ps, ILogService log)
       {
           _ps = ps;
           _log = log;
           BuildLayout();
       }
       private void BuildLayout()
       {
           Dock = DockStyle.Fill;
           _btnLoad.Location = new System.Drawing.Point(10, 10);
           _btnLoad.Click += async (_, __) => { _cts?.Cancel(); _cts = new(); await LoadUsersAsync(_cts.Token); };
           _btnExport.Location = new System.Drawing.Point(110, 10);
           _btnExport.Click += (_, __) =>
           {
               if (_all.Count == 0) { MessageBox.Show("No data"); return; }
               var p = SaveCsv("users.csv"); if (p == null) return;
               CsvExporter.ExportUsers(p, _bsUsers.Cast<UserDto>().ToList());
               MessageBox.Show("Exported");
           };
           var lblFilter = new Label { Text = "Filter (DisplayName/UPN):", AutoSize = true, Location = new System.Drawing.Point(10, 45) };
           _txtFilter.Location = new System.Drawing.Point(170, 42);
           _txtFilter.Width = 250;
           _txtFilter.TextChanged += (_, __) => ApplyFilter();
           _lblStatus.Location = new System.Drawing.Point(450, 45);
           _dgvUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
           _dgvUsers.Location = new System.Drawing.Point(10, 75);
           _dgvUsers.Size = new System.Drawing.Size(960, 400);
           _dgvUsers.AutoGenerateColumns = true;
           _dgvUsers.ReadOnly = true;
           _dgvUsers.DataSource = _bsUsers;
           Controls.AddRange(new Control[] { _btnLoad, _btnExport, lblFilter, _txtFilter, _lblStatus, _dgvUsers });
       }
       private async Task LoadUsersAsync(CancellationToken ct)
       {
           try
           {
               SetStatus("Loading users...");
               var filter = string.Empty; // optionally build OData filter
               _all = (await _ps.GetUsersAsync(filter, ct)).ToList();
               ApplyFilter();
               SetStatus($"Loaded {_all.Count} users");
               _log.Log(LogLevel.Info, "Users", $"Loaded {_all.Count}");
           }
           catch (OperationCanceledException) { SetStatus("Canceled"); }
           catch (PowerShellExecutionException ex)
           {
               SetStatus("PS error");
               MessageBox.Show(ex.Message, "PowerShell", MessageBoxButtons.OK, MessageBoxIcon.Error);
               _log.Log(LogLevel.Error, "Users", ex.Message, string.Join("\n", ex.Errors.Select(er => er.Message)));
           }
           catch (Exception ex)
           {
               SetStatus("Error");
               MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               _log.Log(LogLevel.Error, "Users", ex.Message, ex.ToString());
           }
       }
       private void ApplyFilter()
       {
           var term = _txtFilter.Text.Trim();
           IEnumerable<UserDto> view = _all;
           if (!string.IsNullOrWhiteSpace(term))
           {
               view = view.Where(u =>
                   (u.DisplayName ?? "").Contains(term, StringComparison.OrdinalIgnoreCase) ||
                   (u.UserPrincipalName ?? "").Contains(term, StringComparison.OrdinalIgnoreCase));
           }
           _bsUsers.DataSource = view.ToList();
           _bsUsers.ResetBindings(false);
       }
       private void SetStatus(string msg) => _lblStatus.Text = $"Status: {msg}";
       private static string? SaveCsv(string filename)
       {
           using var dlg = new SaveFileDialog { Filter = "CSV|*.csv", FileName = filename };
           return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : null;
       }
   }
}