using System;
using System.Windows.Forms;
using MailClient.Services;
using MailClient.Views;

namespace MailClientApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var emailService = new EmailService(); // Tạo EmailService ở đây

            using (var loginForm = new LoginForm(emailService))
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm(emailService, loginForm.UserEmail));
                }
            }
        }
    }
}
