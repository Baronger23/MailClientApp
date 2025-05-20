using MailKit;
using MimeKit;
using System;
using System.Linq;

namespace MailClient.Models
{
    public class EmailMessage
    {
        public UniqueId Uid { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
        public bool HasAttachments { get; set; }

        public static EmailMessage FromMimeMessage(IMessageSummary summary, MimeMessage message)
        {
            return new EmailMessage
            {
                Uid = summary.UniqueId,
                From = message.From.ToString(),
                To = message.To.ToString(),
                Subject = message.Subject,
                Date = message.Date.DateTime,
                Body = message.TextBody ?? string.Empty,
                IsRead = summary.Flags?.HasFlag(MessageFlags.Seen) ?? false, // Safely check if the message is read
                HasAttachments = message.Attachments.Any()
            };
        }
    }
}
