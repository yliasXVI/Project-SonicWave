using System.Windows.Forms;
namespace ProjectSonicWave.UI
{
   partial class MainForm
   {
       private System.ComponentModel.IContainer components = null;
       private TabControl tabControl;
       private TabPage tabMailboxes;
       private TabPage tabGroups;
       private TabPage tabUsers;
       private TabPage tabLog;
       protected override void Dispose(bool disposing)
       {
           if (disposing && (components != null))
           {
               components.Dispose();
           }
           base.Dispose(disposing);
       }
       private void InitializeComponent()
       {
           this.tabControl = new TabControl();
           this.tabMailboxes = new TabPage();
           this.tabGroups = new TabPage();
           this.tabUsers = new TabPage();
           this.tabLog = new TabPage();
           // tabControl
           this.tabControl.Dock = DockStyle.Fill;
           this.tabControl.TabPages.AddRange(new TabPage[]
           {
               this.tabMailboxes,
               this.tabGroups,
               this.tabUsers,
               this.tabLog
           });
           // tab pages
           this.tabMailboxes.Text = "Mailboxes";
           this.tabGroups.Text = "Groups";
           this.tabUsers.Text = "Users";
           this.tabLog.Text = "Log / Console";
           // MainForm
           this.ClientSize = new System.Drawing.Size(1100, 700);
           this.Controls.Add(this.tabControl);
           this.Name = "MainForm";
           this.Text = "Project SonicWave - Read-only M365 Admin Tool";
           this.StartPosition = FormStartPosition.CenterScreen;
       }
   }
}