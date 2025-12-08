using System.Windows.Forms;
using ProjectSonicWave.Services;
using ProjectSonicWave.UI.Tabs;
namespace ProjectSonicWave.UI
{
   public partial class MainForm : Form
   {
       private readonly UiLogService _uiLogService;
       private readonly PowerShellService _ps;
       public MainForm()
       {
           InitializeComponent();
           _uiLogService = new UiLogService();
           _ps = new PowerShellService(_uiLogService);
           // Instantiate tabs
           var mailboxesControl = new MailboxesTab(_ps, _uiLogService) { Dock = DockStyle.Fill };
           var groupsControl = new GroupsTab(_ps, _uiLogService) { Dock = DockStyle.Fill };
           var usersControl = new UsersTab(_ps, _uiLogService) { Dock = DockStyle.Fill };
           var logControl = new LogTab(_uiLogService) { Dock = DockStyle.Fill };
           tabMailboxes.Controls.Add(mailboxesControl);
           tabGroups.Controls.Add(groupsControl);
           tabUsers.Controls.Add(usersControl);
           tabLog.Controls.Add(logControl);
       }
       protected override void OnFormClosed(FormClosedEventArgs e)
       {
           _ps.Dispose();
           base.OnFormClosed(e);
       }
   }
}