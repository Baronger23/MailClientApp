using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailClient.Models
{
    public static class EmailProviderSettings
    {
        public static (string Host, int Port, bool Ssl) GmailImap = ("imap.gmail.com", 993, true);
        public static (string Host, int Port, bool Ssl) OutlookImap = ("outlook.office365.com", 993, true);

        public static (string Host, int Port, bool Ssl) GmailSmtp = ("smtp.gmail.com", 587, true);
        public static (string Host, int Port, bool Ssl) OutlookSmtp = ("smtp.office365.com", 587, true);

        public static (string ImapHost, int ImapPort, bool ImapSsl, string SmtpHost, int SmtpPort, bool SmtpSsl)
            GetSettings(string provider)
        {
            if (provider == "Gmail")
            {
                return (GmailImap.Host, GmailImap.Port, GmailImap.Ssl, GmailSmtp.Host, GmailSmtp.Port, GmailSmtp.Ssl);
            }
            else if (provider == "Outlook")
            {
                return (OutlookImap.Host, OutlookImap.Port, OutlookImap.Ssl, OutlookSmtp.Host, OutlookSmtp.Port, OutlookSmtp.Ssl);
            }
            else
            {
                throw new ArgumentException("Unsupported email provider");
            }
        }
    }
}