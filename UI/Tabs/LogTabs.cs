using System.Collections.Generic;
using System.Windows.Forms;
using ProjectSonicWave.Models;
using ProjectSonicWave.Services;
namespace ProjectSonicWave.UI.Tabs
{
   public partial class LogTab : UserControl
   {
       private readonly UiLogService _logService;
       private readonly List<LogEntry> _entries = new();
       public LogTab(UiLogService logService)
       {
           _logService = logService;
           InitializeComponent();
           _logService.EntryAdded += OnEntryAdded;
           logBindingSource.DataSource = _entries;
       }
       private void OnEntryAdded(LogEntry entry)
       {
           if (InvokeRequired)
           {
               BeginInvoke(new MethodInvoker(() => AddEntry(entry)));
           }
           else
           {
               AddEntry(entry);
           }
       }
       private void AddEntry(LogEntry entry)
       {
           _entries.Add(entry);
           logBindingSource.ResetBindings(false);
           if (dgvLog.Rows.Count > 0)
           {
               dgvLog.FirstDisplayedScrollingRowIndex = dgvLog.Rows.Count - 1;
           }
       }
   }
}