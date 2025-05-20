using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailClient.Services;
using Microsoft.Identity.Client;

namespace MailClient.Views
{
    public partial class LoginForm : Form
    {
        private readonly EmailService _emailService;

        public string UserEmail { get; private set; }
        public string AppPassword { get; private set; }
        public string SelectedProvider { get; private set; }
        public string AccessToken { get; private set; }

        // Thay bằng ClientId ứng dụng của bạn trên Azure
        private const string ClientId = "a8df82ed-c914-423e-a16e-43af387e3ddc";
        private const string TenantId = "common";
        private const string RedirectUri = "http://localhost";
        private static readonly string[] Scopes = { "https://outlook.office.com/IMAP.AccessAsUser.All" };

        public LoginForm(EmailService emailService)
        {
            InitializeComponent();
            _emailService = emailService;

            cmbProvider.Items.Add("Gmail");
            cmbProvider.Items.Add("Outlook");
            cmbProvider.Items.Add("Yahoo");
            cmbProvider.SelectedIndex = 0;

            cmbProvider.SelectedIndexChanged += cmbProvider_SelectedIndexChanged;
            btnMicrosoftLogin.Visible = false;
        }

        private void cmbProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProvider.SelectedItem.ToString() == "Outlook")
            {
                btnMicrosoftLogin.Visible = true;
                btnLogin.Visible = false;
                txtPassword.Enabled = false;
            }
            else
            {
                btnMicrosoftLogin.Visible = false;
                btnLogin.Visible = true;
                txtPassword.Enabled = true;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            UserEmail = txtEmail.Text;
            AppPassword = txtPassword.Text;
            SelectedProvider = cmbProvider.SelectedItem.ToString();

            if (string.IsNullOrWhiteSpace(UserEmail) || string.IsNullOrWhiteSpace(AppPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ email và mật khẩu ứng dụng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                btnCancel.Enabled = false;
                lblStatus.Text = "Đang đăng nhập...";
                lblStatus.Visible = true;

                string host = GetHostForProvider(SelectedProvider);
                int port = 993;

                await _emailService.ConnectAsync(host, port, true);
                await _emailService.AuthenticateAsync(UserEmail, AppPassword);

                MessageBox.Show("Đăng nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đăng nhập thất bại: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnCancel.Enabled = true;
                lblStatus.Visible = false;
            }
        }

        private async void btnMicrosoftLogin_Click(object sender, EventArgs e)
        {
            UserEmail = txtEmail.Text;
            SelectedProvider = "Outlook";

            if (string.IsNullOrWhiteSpace(UserEmail))
            {
                MessageBox.Show("Vui lòng nhập email.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                btnMicrosoftLogin.Enabled = false;
                btnCancel.Enabled = false;
                lblStatus.Text = "Đang đăng nhập Microsoft...";
                lblStatus.Visible = true;

                var app = PublicClientApplicationBuilder
                    .Create(ClientId)
                    .WithRedirectUri(RedirectUri)
                    .WithTenantId(TenantId)
                    .Build();

                var result = await app.AcquireTokenInteractive(Scopes)
                    .WithLoginHint(UserEmail)
                    .ExecuteAsync();

                AccessToken = result.AccessToken;

                string host = GetHostForProvider(SelectedProvider);
                int port = 993;

                await _emailService.ConnectAsync(host, port, true);
                await _emailService.AuthenticateAsync(UserEmail, AccessToken, true);

                MessageBox.Show("Đăng nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đăng nhập Microsoft thất bại: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnMicrosoftLogin.Enabled = true;
                btnCancel.Enabled = true;
                lblStatus.Visible = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private string GetHostForProvider(string provider)
        {
            switch (provider.ToLower())
            {
                case "gmail":
                    return "imap.gmail.com";
                case "outlook":
                    return "imap-mail.outlook.com";
                case "yahoo":
                    return "imap.mail.yahoo.com";
                default:
                    throw new NotSupportedException($"Nhà cung cấp '{provider}' chưa được hỗ trợ.");
            }
        }
    }
}
