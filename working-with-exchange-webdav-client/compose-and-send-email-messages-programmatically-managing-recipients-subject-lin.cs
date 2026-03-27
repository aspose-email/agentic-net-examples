using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

namespace EmailSenderExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define email parameters
                string fromAddress = "sender@example.com";
                string toAddress = "recipient@example.com";
                string ccAddress = "cc@example.com";
                string bccAddress = "bcc@example.com";
                string subject = "Test Email with Attachment";
                string body = "Hello,\n\nThis is a test email sent using Aspose.Email.\n\nBest regards.";
                string attachmentPath = "sample.txt";

                // Verify attachment file exists
                if (!File.Exists(attachmentPath))
                {
                    Console.Error.WriteLine($"Error: Attachment file not found – {attachmentPath}");
                    return;
                }

                // Create the mail message
                using (MailMessage message = new MailMessage(fromAddress, toAddress, subject, body))
                {
                    // Add CC and BCC recipients
                    message.CC.Add(ccAddress);
                    message.Bcc.Add(bccAddress);

                    // Add attachment
                    using (Attachment attachment = new Attachment(attachmentPath))
                    {
                        message.Attachments.Add(attachment);

                        // Create and configure the SMTP client
                        try
                        {
                            using (SmtpClient client = new SmtpClient("smtp.example.com", 587, "username", "password", SecurityOptions.Auto))
                            {
                                // Send the email
                                client.Send(message);
                                Console.WriteLine("Email sent successfully.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"SMTP error: {ex.Message}");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}