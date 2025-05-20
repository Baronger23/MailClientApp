using MailClient.Models;
using System;
using System.Windows.Forms;

namespace MailClient.Views
{
    public partial class MessageForm : Form
    {
        public MessageForm(EmailMessage email)
        {
            InitializeComponent();
            DisplayEmail(email);
        }

        private void DisplayEmail(EmailMessage email)
        {
            txtFrom.Text = email.From;
            txtTo.Text = email.To;
            txtSubject.Text = email.Subject;
            txtDate.Text = email.Date.ToString("f");
            txtBody.Text = email.Body;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}