using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholders are detected.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Prepare JSON payload.
            string jsonContent = "{\"key\":\"value\"}";
            string attachmentPath = Path.Combine(Path.GetTempPath(), "payload.json");

            // Ensure the directory exists and write the JSON file.
            try
            {
                string dir = Path.GetDirectoryName(attachmentPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(attachmentPath, jsonContent);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create attachment file: {ex.Message}");
                return;
            }

            // Build the email message with a multipart/mixed body.
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "API Integration Request";
                message.Body = "Please find the JSON payload attached.";

                // Add the JSON file as an attachment.
                using (Attachment attachment = new Attachment(attachmentPath, "application/json"))
                {
                    message.Attachments.Add(attachment);

                    // Send the message via Exchange client.
                    using (ExchangeClient client = new ExchangeClient(mailboxUri, new NetworkCredential(username, password)))
                    {
                        try
                        {
                            client.Send(message);
                            Console.WriteLine("Email sent successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                        }
                    }
                }
            }

            // Clean up the temporary JSON file.
            try
            {
                if (File.Exists(attachmentPath))
                {
                    File.Delete(attachmentPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to delete temporary file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
