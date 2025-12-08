using System.Windows.Forms;
namespace ProjectSonicWave.UI.Tabs
{
   partial class LogTab
   {
       private System.ComponentModel.IContainer components = null;
       private DataGridView dgvLog;
       private BindingSource logBindingSource;
       protected override void Dispose(bool disposing)
       {
           if (disposing && (components != null))
           {
               components.Dispose();
               logBindingSource?.Dispose();
           }
           base.Dispose(disposing);
       }
       private void InitializeComponent()
       {
           this.components = new System.ComponentModel.Container();
           this.dgvLog = new DataGridView();
           this.logBindingSource = new BindingSource(this.components);
           ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
           ((System.ComponentModel.ISupportInitialize)(this.logBindingSource)).BeginInit();
           this.SuspendLayout();
           //
           // dgvLog
           //
           this.dgvLog.AllowUserToAddRows = false;
           this.dgvLog.AllowUserToDeleteRows = false;
           this.dgvLog.AutoGenerateColumns = true;
           this.dgvLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
           this.dgvLog.Dock = DockStyle.Fill;
           this.dgvLog.Location = new System.Drawing.Point(0, 0);
           this.dgvLog.Name = "dgvLog";
           this.dgvLog.ReadOnly = true;
           this.dgvLog.RowHeadersVisible = false;
           this.dgvLog.RowTemplate.Height = 25;
           this.dgvLog.Size = new System.Drawing.Size(800, 450);
           this.dgvLog.TabIndex = 0;
           this.dgvLog.DataSource = this.logBindingSource;
           //
           // LogTab
           //
           this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
           this.AutoScaleMode = AutoScaleMode.Font;
           this.Controls.Add(this.dgvLog);
           this.Name = "LogTab";
           this.Size = new System.Drawing.Size(800, 450);
           ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
           ((System.ComponentModel.ISupportInitialize)(this.logBindingSource)).EndInit();
           this.ResumeLayout(false);
       }
   }
}