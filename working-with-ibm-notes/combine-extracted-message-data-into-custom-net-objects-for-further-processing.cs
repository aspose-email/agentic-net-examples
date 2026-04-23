using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailSample
{
    // Custom data class to hold extracted email information
    public class EmailInfo
    {
        public string Subject { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Body { get; set; }
        public List<string> ToRecipients { get; set; }
        public List<string> CcRecipients { get; set; }
        public List<string> BccRecipients { get; set; }

        public EmailInfo()
        {
            ToRecipients = new List<string>();
            CcRecipients = new List<string>();
            BccRecipients = new List<string>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file to be processed
                string msgFilePath = "sample.msg";

                // Ensure the file exists; if not, create a minimal placeholder MSG
                if (!File.Exists(msgFilePath))
                {
                    try
                    {
                        using (MapiMessage placeholder = new MapiMessage(
                            "sender@example.com",
                            "recipient@example.com",
                            "Placeholder Subject",
                            "This is a placeholder message body."))
                        {
                            placeholder.Save(msgFilePath);
                            Console.WriteLine($"Placeholder MSG created at '{msgFilePath}'.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                        return;
                    }
                }

                // List to hold extracted email information
                List<EmailInfo> extractedEmails = new List<EmailInfo>();

                // Load the MSG file and extract data
                try
                {
                    using (MapiMessage msg = MapiMessage.Load(msgFilePath))
                    {
                        EmailInfo info = new EmailInfo
                        {
                            Subject = msg.Subject,
                            SenderName = msg.SenderName,
                            SenderEmail = msg.SenderEmailAddress,
                            Body = msg.Body
                        };

                        // Parse To recipients (semicolon‑separated)
                        if (!string.IsNullOrEmpty(msg.DisplayTo))
                        {
                            string[] toParts = msg.DisplayTo.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string to in toParts)
                            {
                                info.ToRecipients.Add(to.Trim());
                            }
                        }

                        // Parse Cc recipients
                        if (!string.IsNullOrEmpty(msg.DisplayCc))
                        {
                            string[] ccParts = msg.DisplayCc.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string cc in ccParts)
                            {
                                info.CcRecipients.Add(cc.Trim());
                            }
                        }

                        // Parse Bcc recipients
                        if (!string.IsNullOrEmpty(msg.DisplayBcc))
                        {
                            string[] bccParts = msg.DisplayBcc.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string bcc in bccParts)
                            {
                                info.BccRecipients.Add(bcc.Trim());
                            }
                        }

                        extractedEmails.Add(info);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                    return;
                }

                // Process the extracted email information (example: output to console)
                foreach (EmailInfo email in extractedEmails)
                {
                    Console.WriteLine("=== Extracted Email ===");
                    Console.WriteLine($"Subject: {email.Subject}");
                    Console.WriteLine($"From: {email.SenderName} <{email.SenderEmail}>");
                    Console.WriteLine($"To: {string.Join(", ", email.ToRecipients)}");
                    Console.WriteLine($"Cc: {string.Join(", ", email.CcRecipients)}");
                    Console.WriteLine($"Bcc: {string.Join(", ", email.BccRecipients)}");
                    Console.WriteLine("Body:");
                    Console.WriteLine(email.Body);
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
