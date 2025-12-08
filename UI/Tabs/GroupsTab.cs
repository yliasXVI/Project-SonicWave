using System.Windows.Forms;
namespace ProjectSonicWave.UI.Tabs
{
   /// <summary>
   /// Placeholder voor Groups; later uitbreiden met Get-DistributionGroup, Get-DistributionGroupMember.
   /// </summary>
   public class GroupsTab : UserControl
   {
       public GroupsTab()
       {
           var lbl = new Label
           {
               Text = "Groups (placeholder) - hier komt UI voor groepen en groepsleden.",
               Dock = DockStyle.Fill,
               TextAlign = System.Drawing.ContentAlignment.MiddleCenter
           };
           Controls.Add(lbl);
       }
   }
}