using System.Windows.Forms;

namespace ProjectSonicWave.UI.Tabs
{
    /// <summary>
    /// Placeholder voor Users; later uitbreiden met Get-MgUser en licenties
    /// </summary>
    
    public class Userstab() : UserControl
    {
        public UsersTab()
        {
            var lbl = new Label { Text = "Users (placeholder) - hier komt UI voor Entra/M365 users en licenties.", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleCenter};
            Controls.Add(lbl);
        }
         
    }
}