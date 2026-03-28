using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Compose a new email message
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Test Email";
                message.Body = "This is a test email sent via Aspose.Email.";

                string smtpHost = "smtp.example.com";
                string imapHost = "imap.example.com";
                string username = "username";
                string password = "password";

                if (smtpHost.Contains("example.com") || imapHost.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.WriteLine("Placeholder SMTP/IMAP settings detected. Skipping external operations.");
                    return;
                }

                // Add an attachment if the file exists
                string attachmentPath = "attachment.txt";
                if (File.Exists(attachmentPath))
                {
                    try
                    {
                        using (FileStream fs = File.OpenRead(attachmentPath))
                        {
                            Attachment attachment = new Attachment(fs, "attachment.txt");
                            message.Attachments.Add(attachment);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add attachment: {ex.Message}");
                    }
                }

                // Send the message using SMTP
                try
                {
                    using (SmtpClient smtpClient = new SmtpClient(smtpHost, 587, username, password))
                    {
                        smtpClient.SecurityOptions = SecurityOptions.Auto;
                        smtpClient.Send(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP send failed: {ex.Message}");
                    return;
                }
            }

            // Retrieve messages using IMAP
            try
            {
                using (ImapClient imapClient = new ImapClient(imapHost, 993, username, password))
                {
                    imapClient.SecurityOptions = SecurityOptions.Auto;

                    // List messages in the INBOX folder
                    var messageInfos = imapClient.ListMessages("INBOX");
                    foreach (var info in messageInfos)
                    {
                        try
                        {
                            // Fetch the full message by its unique identifier
                            MailMessage fetched = imapClient.FetchMessage(info.UniqueId);
                            Console.WriteLine($"Subject: {fetched.Subject}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to fetch message {info.UniqueId}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
