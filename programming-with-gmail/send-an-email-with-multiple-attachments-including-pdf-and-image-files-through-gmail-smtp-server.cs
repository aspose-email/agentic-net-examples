using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder Gmail credentials – replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "your.email@gmail.com";

            // Guard against placeholder credentials to avoid real network calls.
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_") || defaultEmail.StartsWith("your."))
            {
                Console.Error.WriteLine("Gmail credentials are placeholders. Skipping send operation.");
                return;
            }

            // Prepare attachment file paths.
            string pdfPath = "sample.pdf";
            string imagePath = "image.jpg";

            // Ensure attachment files exist; create minimal placeholders if missing.
            try
            {
                if (!File.Exists(pdfPath))
                {
                    File.WriteAllBytes(pdfPath, new byte[] { 0x25, 0x50, 0x44, 0x46 }); // "%PDF" header
                }

                if (!File.Exists(imagePath))
                {
                    // Simple JPEG header bytes.
                    File.WriteAllBytes(imagePath, new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 });
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare attachment files: {ex.Message}");
                return;
            }

            // Create the email message.
            MailMessage mailMessage = new MailMessage
            {
                From = defaultEmail,
                To = "recipient@example.com",
                Subject = "Test Email with Attachments",
                Body = "Please find the attached PDF and image files."
            };

            // Add attachments.
            try
            {
                mailMessage.Attachments.Add(new Attachment(pdfPath));
                mailMessage.Attachments.Add(new Attachment(imagePath));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to add attachments: {ex.Message}");
                return;
            }

            // Create Gmail client instance.
            IGmailClient gmailClient = null;
            try
            {
                gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Send the message.
            try
            {
                string messageId = gmailClient.SendMessage(mailMessage);
                Console.WriteLine($"Message sent successfully. Id: {messageId}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send email: {ex.Message}");
            }
            finally
            {
                // Dispose client if it implements IDisposable.
                if (gmailClient is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
                mailMessage.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
