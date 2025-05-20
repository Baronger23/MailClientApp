namespace MailClient.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lvEmails = new System.Windows.Forms.ListView();
            this.chFrom = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSubject = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lvEmails
            this.lvEmails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFrom,
            this.chSubject,
            this.chDate});
            this.lvEmails.FullRowSelect = true;
            this.lvEmails.GridLines = true;
            this.lvEmails.HideSelection = false;
            this.lvEmails.Location = new System.Drawing.Point(12, 12);
            this.lvEmails.Name = "lvEmails";
            this.lvEmails.Size = new System.Drawing.Size(600, 350);
            this.lvEmails.TabIndex = 0;
            this.lvEmails.UseCompatibleStateImageBehavior = false;
            this.lvEmails.View = System.Windows.Forms.View.Details;
            this.lvEmails.DoubleClick += new System.EventHandler(this.lvEmails_DoubleClick);

            // chFrom
            this.chFrom.Text = "From";
            this.chFrom.Width = 200;

            // chSubject
            this.chSubject.Text = "Subject";
            this.chSubject.Width = 250;

            // chDate
            this.chDate.Text = "Date";
            this.chDate.Width = 120;

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(12, 370);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 30);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 411);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lvEmails);
            this.MinimumSize = new System.Drawing.Size(640, 450);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView lvEmails;
        private System.Windows.Forms.ColumnHeader chFrom;
        private System.Windows.Forms.ColumnHeader chSubject;
        private System.Windows.Forms.ColumnHeader chDate;
        private System.Windows.Forms.Button btnRefresh;
    }
}