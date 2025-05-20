using MailClient.Models;
using MailClient.Services;
using MailKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailClient.Views
{
    public partial class MainForm : Form
    {
        private readonly EmailService _emailService;
        private readonly string _emailAddress;
        private List<EmailMessage> _emails = new List<EmailMessage>();

        public MainForm(EmailService emailService, string emailAddress)
        {
            InitializeComponent();
            _emailService = emailService;
            _emailAddress = emailAddress;
            this.Text = $"Mail Client - {emailAddress}";
            LoadEmailsAsync(); // Gọi phương thức bất đồng bộ để tải email
        }

        private async Task LoadEmailsAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                lvEmails.Items.Clear();

                // Tải danh sách email
                _emails = await _emailService.GetEmailsAsync();
                foreach (var email in _emails)
                {
                    var item = new ListViewItem(new[] {
                        email.From,
                        email.Subject,
                        email.Date.ToString("g")
                    });
                    item.Tag = email.Uid;
                    lvEmails.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading emails: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void lvEmails_DoubleClick(object sender, EventArgs e)
        {
            if (lvEmails.SelectedItems.Count == 1)
            {
                var selectedUid = (UniqueId)lvEmails.SelectedItems[0].Tag;
                var email = _emails.Find(m => m.Uid == selectedUid);

                if (email != null)
                {
                    // Hiển thị nội dung email
                    var messageForm = new MessageForm(email);
                    messageForm.ShowDialog();
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadEmailsAsync(); // Tải lại danh sách email
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _emailService.Dispose();
            Application.Exit();
        }
    }
}
