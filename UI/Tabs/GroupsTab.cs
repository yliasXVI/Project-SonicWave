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
   public class GroupsTab : UserControl
   {
       private readonly PowerShellService _ps;
       private readonly ILogService _log;
       private readonly DataGridView _dgvGroups = new();
       private readonly DataGridView _dgvMembers = new();
       private readonly TextBox _txtFilter = new();
       private readonly Label _lblStatus = new() { AutoSize = true, Text = "Status: idle" };
       private readonly Button _btnLoad = new() { Text = "Load Groups" };
       private readonly Button _btnMembers = new() { Text = "Load Members" };
       private readonly Button _btnExport = new() { Text = "Export Groups CSV" };
       private readonly Button _btnExportMembers = new() { Text = "Export Members CSV" };
       private readonly BindingSource _bsGroups = new();
       private readonly BindingSource _bsMembers = new();
       private List<GroupDto> _all = new();
       private List<GroupMemberDto> _members = new();
       private CancellationTokenSource? _cts;
       public GroupsTab(PowerShellService ps, ILogService log)
       {
           _ps = ps;
           _log = log;
           BuildLayout();
       }
       private void BuildLayout()
       {
           Dock = DockStyle.Fill;
           _btnLoad.Location = new System.Drawing.Point(10, 10);
           _btnLoad.Click += async (_, __) => { _cts?.Cancel(); _cts = new(); await LoadGroupsAsync(_cts.Token); };
           _btnMembers.Location = new System.Drawing.Point(110, 10);
           _btnMembers.Click += async (_, __) =>
           {
               var sel = _bsGroups.Current as GroupDto;
               if (sel == null) { MessageBox.Show("Select a group"); return; }
               _cts?.Cancel(); _cts = new(); await LoadMembersAsync(sel.Identity ?? sel.DisplayName ?? "", _cts.Token);
           };
           _btnExport.Location = new System.Drawing.Point(220, 10);
           _btnExport.Click += (_, __) =>
           {
               if (_all.Count == 0) { MessageBox.Show("No data"); return; }
               var p = SaveCsv("groups.csv"); if (p == null) return;
               CsvExporter.ExportGroups(p, _bsGroups.Cast<GroupDto>().ToList());
               MessageBox.Show("Exported");
           };
           _btnExportMembers.Location = new System.Drawing.Point(340, 10);
           _btnExportMembers.Click += (_, __) =>
           {
               if (_members.Count == 0) { MessageBox.Show("No member data"); return; }
               var p = SaveCsv("group-members.csv"); if (p == null) return;
               CsvExporter.ExportGroupMembers(p, _bsMembers.Cast<GroupMemberDto>().ToList());
               MessageBox.Show("Exported");
           };
           var lblFilter = new Label { Text = "Filter:", AutoSize = true, Location = new System.Drawing.Point(10, 45) };
           _txtFilter.Location = new System.Drawing.Point(60, 42);
           _txtFilter.Width = 250;
           _txtFilter.TextChanged += (_, __) => ApplyFilter();
           _lblStatus.Location = new System.Drawing.Point(330, 45);
           _dgvGroups.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
           _dgvGroups.Location = new System.Drawing.Point(10, 75);
           _dgvGroups.Size = new System.Drawing.Size(500, 400);
           _dgvGroups.AutoGenerateColumns = true;
           _dgvGroups.ReadOnly = true;
           _dgvGroups.DataSource = _bsGroups;
           _dgvMembers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
           _dgvMembers.Location = new System.Drawing.Point(520, 75);
           _dgvMembers.Size = new System.Drawing.Size(450, 400);
           _dgvMembers.AutoGenerateColumns = true;
           _dgvMembers.ReadOnly = true;
           _dgvMembers.DataSource = _bsMembers;
           Controls.AddRange(new Control[] {
               _btnLoad, _btnMembers, _btnExport, _btnExportMembers,
               lblFilter, _txtFilter, _lblStatus, _dgvGroups, _dgvMembers
           });
       }
       private async Task LoadGroupsAsync(CancellationToken ct)
       {
           try
           {
               SetStatus("Loading groups...");
               _all = (await _ps.GetGroupsAsync(ct)).ToList();
               ApplyFilter();
               SetStatus($"Loaded {_all.Count} groups");
               _log.Log(LogLevel.Info, "Groups", $"Loaded {_all.Count}");
           }
           catch (OperationCanceledException) { SetStatus("Canceled"); }
           catch (PowerShellExecutionException ex)
           {
               SetStatus("PS error");
               MessageBox.Show(ex.Message, "PowerShell", MessageBoxButtons.OK, MessageBoxIcon.Error);
               _log.Log(LogLevel.Error, "Groups", ex.Message, string.Join("\n", ex.Errors.Select(er => er.Message)));
           }
           catch (Exception ex)
           {
               SetStatus("Error");
               MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               _log.Log(LogLevel.Error, "Groups", ex.Message, ex.ToString());
           }
       }
       private async Task LoadMembersAsync(string identity, CancellationToken ct)
       {
           try
           {
               SetStatus($"Loading members for {identity}...");
               _members = (await _ps.GetGroupMembersAsync(identity, ct)).ToList();
               _bsMembers.DataSource = _members;
               _bsMembers.ResetBindings(false);
               SetStatus($"Loaded {_members.Count} members");
               _log.Log(LogLevel.Info, "Groups", $"Loaded members for {identity}: {_members.Count}");
           }
           catch (OperationCanceledException) { SetStatus("Canceled"); }
           catch (PowerShellExecutionException ex)
           {
               SetStatus("PS error");
               MessageBox.Show(ex.Message, "PowerShell", MessageBoxButtons.OK, MessageBoxIcon.Error);
               _log.Log(LogLevel.Error, "GroupMembers", ex.Message, string.Join("\n", ex.Errors.Select(er => er.Message)));
           }
           catch (Exception ex)
           {
               SetStatus("Error");
               MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               _log.Log(LogLevel.Error, "GroupMembers", ex.Message, ex.ToString());
           }
       }
       private void ApplyFilter()
       {
           var term = _txtFilter.Text.Trim();
           IEnumerable<GroupDto> view = _all;
           if (!string.IsNullOrWhiteSpace(term))
           {
               view = view.Where(g =>
                   (g.DisplayName ?? "").Contains(term, StringComparison.OrdinalIgnoreCase) ||
                   (g.PrimarySmtpAddress ?? "").Contains(term, StringComparison.OrdinalIgnoreCase));
           }
           _bsGroups.DataSource = view.ToList();
           _bsGroups.ResetBindings(false);
       }
       private void SetStatus(string msg) => _lblStatus.Text = $"Status: {msg}";
       private static string? SaveCsv(string filename)
       {
           using var dlg = new SaveFileDialog { Filter = "CSV|*.csv", FileName = filename };
           return dlg.ShowDialog() == DialogResult.OK ? dlg.FileName : null;
       }
   }
}