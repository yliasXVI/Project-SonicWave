using System.Windows.Forms;
namespace ProjectSonicWave.UI.Tabs
{
   partial class MailboxesTab
   {
       private System.ComponentModel.IContainer components = null;
       private Button btnLoad;
       private TextBox txtFilter;
       private Label lblFilter;
       private DataGridView dgvMailboxes;
       private Label lblStatus;
       private BindingSource mailboxBindingSource;
       protected override void Dispose(bool disposing)
       {
           if (disposing && (components != null))
           {
               components.Dispose();
               mailboxBindingSource?.Dispose();
           }
           base.Dispose(disposing);
       }
       private void InitializeComponent()
       {
           this.components = new System.ComponentModel.Container();
           this.btnLoad = new Button();
           this.txtFilter = new TextBox();
           this.lblFilter = new Label();
           this.dgvMailboxes = new DataGridView();
           this.lblStatus = new Label();
           this.mailboxBindingSource = new BindingSource(this.components);
           ((System.ComponentModel.ISupportInitialize)(this.dgvMailboxes)).BeginInit();
           ((System.ComponentModel.ISupportInitialize)(this.mailboxBindingSource)).BeginInit();
           this.SuspendLayout();
           //
           // btnLoad
           //
           this.btnLoad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
           this.btnLoad.Location = new System.Drawing.Point(650, 10);
           this.btnLoad.Name = "btnLoad";
           this.btnLoad.Size = new System.Drawing.Size(120, 27);
           this.btnLoad.TabIndex = 0;
           this.btnLoad.Text = "Load Mailboxes";
           this.btnLoad.UseVisualStyleBackColor = true;
           this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
           //
           // txtFilter
           //
           this.txtFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
           this.txtFilter.Location = new System.Drawing.Point(90, 12);
           this.txtFilter.Name = "txtFilter";
           this.txtFilter.Size = new System.Drawing.Size(540, 23);
           this.txtFilter.TabIndex = 1;
           this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
           //
           // lblFilter
           //
           this.lblFilter.AutoSize = true;
           this.lblFilter.Location = new System.Drawing.Point(15, 15);
           this.lblFilter.Name = "lblFilter";
           this.lblFilter.Size = new System.Drawing.Size(69, 15);
           this.lblFilter.TabIndex = 2;
           this.lblFilter.Text = "Filter (naam)";
           //
           // dgvMailboxes
           //
           this.dgvMailboxes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
           this.dgvMailboxes.AutoGenerateColumns = true;
           this.dgvMailboxes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
           this.dgvMailboxes.Location = new System.Drawing.Point(15, 50);
           this.dgvMailboxes.Name = "dgvMailboxes";
           this.dgvMailboxes.RowTemplate.Height = 25;
           this.dgvMailboxes.Size = new System.Drawing.Size(755, 360);
           this.dgvMailboxes.TabIndex = 3;
           this.dgvMailboxes.DataSource = this.mailboxBindingSource;
           //
           // lblStatus
           //
           this.lblStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
           this.lblStatus.AutoSize = true;
           this.lblStatus.Location = new System.Drawing.Point(15, 420);
           this.lblStatus.Name = "lblStatus";
           this.lblStatus.Size = new System.Drawing.Size(42, 15);
           this.lblStatus.TabIndex = 4;
           this.lblStatus.Text = "Status:";
           //
           // MailboxesTab
           //
           this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
           this.AutoScaleMode = AutoScaleMode.Font;
           this.Controls.Add(this.lblStatus);
           this.Controls.Add(this.dgvMailboxes);
           this.Controls.Add(this.lblFilter);
           this.Controls.Add(this.txtFilter);
           this.Controls.Add(this.btnLoad);
           this.Name = "MailboxesTab";
           this.Size = new System.Drawing.Size(785, 450);
           ((System.ComponentModel.ISupportInitialize)(this.dgvMailboxes)).EndInit();
           ((System.ComponentModel.ISupportInitialize)(this.mailboxBindingSource)).EndInit();
           this.ResumeLayout(false);
           this.PerformLayout();
       }
   }
}