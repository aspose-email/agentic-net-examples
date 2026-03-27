using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Smtp;

namespace ThunderbirdIntegrationSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Prepare attachment file
                string attachmentPath = "sample.txt";
                if (!File.Exists(attachmentPath))
                {
                    try
                    {
                        File.WriteAllText(attachmentPath, "Sample attachment content.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create attachment file: {ex.Message}");
                        return;
                    }
                }

                // Compose email
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress("sender@example.com");
                    message.To.Add(new MailAddress("recipient@example.com"));
                    message.Subject = "Test Email via Aspose.Email";
                    message.Body = "This is a test email sent from a sample integration with Thunderbird-like workflow.";

                    using (Attachment attachment = new Attachment(attachmentPath))
                    {
                        message.Attachments.Add(attachment);

                        // Send email via SMTP
                        try
                        {
                            using (SmtpClient smtpClient = new SmtpClient("smtp.example.com", 587, "sender@example.com", "password", SecurityOptions.SSLExplicit))
                            {
                                smtpClient.Send(message);
                                Console.WriteLine("Email sent successfully.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"SMTP error: {ex.Message}");
                            // Continue to retrieval attempt even if sending fails
                        }
                    }
                }

                // Retrieve email via IMAP
                try
                {
                    using (ImapClient imapClient = new ImapClient("imap.example.com", 993, "recipient@example.com", "password", SecurityOptions.SSLExplicit))
                    {
                        imapClient.SelectFolder("INBOX");
                        ImapMessageInfoCollection messages = imapClient.ListMessages();

                        if (messages.Count > 0)
                        {
                            ImapMessageInfo firstInfo = messages[0];
                            using (MailMessage fetchedMessage = imapClient.FetchMessage(firstInfo.UniqueId))
                            {
                                Console.WriteLine($"Fetched email subject: {fetchedMessage.Subject}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No messages found in INBOX.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
