using MailClient.Models;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailClient.Services
{
    public class EmailService : IDisposable
    {
        private ImapClient _imapClient;
        private string _currentMailbox = "INBOX";

        public EmailService()
        {
            _imapClient = new ImapClient();
        }

        public async Task ConnectAsync(string host, int port, bool useSsl)
        {
            if (_imapClient.IsConnected)
                throw new InvalidOperationException("Already connected to the server.");

            await _imapClient.ConnectAsync(host, port, useSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None);
        }

        // Đăng nhập truyền thống (Gmail/Yahoo)
        public async Task AuthenticateAsync(string email, string password)
        {
            if (!_imapClient.IsConnected)
                throw new InvalidOperationException("Client is not connected.");

            await _imapClient.AuthenticateAsync(email, password);
            _currentMailbox = "INBOX";
        }

        // Đăng nhập Outlook bằng OAuth2
        public async Task AuthenticateAsync(string email, string accessToken, bool isOAuth2)
        {
            if (!_imapClient.IsConnected)
                throw new InvalidOperationException("Client is not connected.");

            if (isOAuth2)
            {
                var sasl = new SaslMechanismOAuth2(email, accessToken);

                try
                {
                    await _imapClient.AuthenticateAsync(sasl);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(
                        $"Lỗi xác thực IMAP OAuth2: {ex.Message}\n{ex.StackTrace}",
                        "IMAP OAuth2 Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    throw;
                }

                // Kiểm tra trạng thái xác thực
                if (!_imapClient.IsAuthenticated)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "IMAP OAuth2 authentication failed. Token có thể không hợp lệ, scope sai, hoặc IMAP chưa bật trên tài khoản.",
                        "IMAP Not Authenticated", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    throw new InvalidOperationException("IMAP OAuth2 authentication failed.");
                }

                // Lấy danh sách mailbox thực tế để debug
                var mailboxes = await GetMailboxesAsync();
                System.Windows.Forms.MessageBox.Show(
                    "Mailboxes thực tế:\n" + string.Join("\n", mailboxes), "Mailbox List");

                // Ưu tiên "INBOX", "Inbox", "Hộp thư đến", hoặc mailbox đầu tiên
                var inbox = mailboxes.FirstOrDefault(mb =>
                    string.Equals(mb, "Inbox", StringComparison.OrdinalIgnoreCase)
                );
                if (!string.IsNullOrEmpty(inbox))
                    _currentMailbox = inbox;
                else
                    _currentMailbox = mailboxes.FirstOrDefault() ?? "Inbox";
            }
            else
            {
                await _imapClient.AuthenticateAsync(email, accessToken);

                if (!_imapClient.IsAuthenticated)
                {
                    System.Windows.Forms.MessageBox.Show(
                        "IMAP authentication failed (không phải OAuth2).",
                        "IMAP Not Authenticated", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    throw new InvalidOperationException("IMAP authentication failed.");
                }

                _currentMailbox = "INBOX";
            }
        }


        public async Task<List<string>> GetMailboxesAsync()
        {
            if (!_imapClient.IsAuthenticated)
                throw new InvalidOperationException("Client is not authenticated.");

            var folders = await _imapClient.GetFoldersAsync(_imapClient.PersonalNamespaces[0]);
            return folders.Select(f => f.Name).ToList();
        }

        public async Task SelectMailboxAsync(string mailbox)
        {
            if (!_imapClient.IsAuthenticated)
                throw new InvalidOperationException("Client is not authenticated.");

            var folder = await _imapClient.GetFolderAsync(mailbox);
            await folder.OpenAsync(FolderAccess.ReadOnly);
            _currentMailbox = mailbox;
        }

        public async Task<List<EmailMessage>> GetEmailsAsync(int count = 50)
        {
            if (!_imapClient.IsAuthenticated)
                throw new InvalidOperationException("Client is not authenticated.");

            var messages = new List<EmailMessage>();

            var folder = await _imapClient.GetFolderAsync(_currentMailbox);
            await folder.OpenAsync(FolderAccess.ReadOnly);

            var summaries = await folder.FetchAsync(0, -1, MessageSummaryItems.Envelope | MessageSummaryItems.Flags | MessageSummaryItems.UniqueId);
            var lastSummaries = summaries.Skip(Math.Max(0, summaries.Count - count)).Reverse();

            foreach (var summary in lastSummaries)
            {
                var message = await folder.GetMessageAsync(summary.UniqueId);
                messages.Add(EmailMessage.FromMimeMessage(summary, message));
            }

            return messages;
        }

        public async Task MarkAsReadAsync(UniqueId uid)
        {
            if (!_imapClient.IsAuthenticated)
                throw new InvalidOperationException("Client is not authenticated.");

            var folder = await _imapClient.GetFolderAsync(_currentMailbox);
            await folder.OpenAsync(FolderAccess.ReadWrite);
            await folder.AddFlagsAsync(uid, MessageFlags.Seen, true);
        }

        public void Dispose()
        {
            if (_imapClient != null)
            {
                if (_imapClient.IsConnected)
                    _imapClient.Disconnect(true);
                _imapClient.Dispose();
            }
        }
    }
}
