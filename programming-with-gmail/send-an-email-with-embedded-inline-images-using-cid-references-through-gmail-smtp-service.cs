using System;
using System.IO;
using System.Net.Mime;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "your.email@example.com";

            if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("your."))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail send operation.");
                return;
            }

            // Prepare inline image.
            string imagePath = "barcode.png";
            if (!File.Exists(imagePath))
            {
                try
                {
                    // Minimal 1x1 PNG byte array.
                    byte[] pngBytes = new byte[]
                    {
                        0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,0x0A,
                        0x00,0x00,0x00,0x0D,0x49,0x48,0x44,0x52,
                        0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x01,
                        0x08,0x06,0x00,0x00,0x00,0x1F,0x15,0xC4,
                        0x89,0x00,0x00,0x00,0x0A,0x49,0x44,0x41,
                        0x54,0x78,0x9C,0x63,0x60,0x00,0x00,0x00,
                        0x02,0x00,0x01,0xE2,0x21,0xBC,0x33,0x00,
                        0x00,0x00,0x00,0x49,0x45,0x4E,0x44,0xAE,
                        0x42,0x60,0x82
                    };
                    File.WriteAllBytes(imagePath, pngBytes);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder image: {ex.Message}");
                    return;
                }
            }

            // Build the email message.
            using (MailMessage message = new MailMessage())
            {
                message.From = defaultEmail;
                message.To.Add("recipient@example.com");
                message.Subject = "Email with Inline Image";
                message.IsBodyHtml = true;
                message.HtmlBody = "<html><body><h3>Embedded Image:</h3><img src=\"cid:barcode\"></body></html>";

                // Attach the image as a linked resource.
                LinkedResource linkedImage = new LinkedResource(imagePath, MediaTypeNames.Image.Png)
                {
                    ContentId = "barcode"
                };
                message.LinkedResources.Add(linkedImage);

                // Create Gmail client and send the message.
                try
                {
                    IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
                    string sentMessageId = gmailClient.SendMessage(message);
                    Console.WriteLine($"Message sent successfully. Id: {sentMessageId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email via Gmail: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
