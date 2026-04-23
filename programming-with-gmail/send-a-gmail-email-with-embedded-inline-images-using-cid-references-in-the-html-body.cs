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
            // Placeholder credentials – replace with real values.
            const string accessToken = "YOUR_ACCESS_TOKEN";
            const string defaultEmail = "your.email@example.com";

            // Guard against placeholder credentials to avoid real network calls.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Skipping Gmail send – placeholder credentials detected.");
                return;
            }

            // Prepare the image to embed.
            const string imagePath = "inlineImage.jpg";
            if (!File.Exists(imagePath))
            {
                try
                {
                    // Create a minimal placeholder image file (a single white pixel JPEG).
                    byte[] placeholder = new byte[]
                    {
                        0xFF,0xD8,0xFF,0xE0,0x00,0x10,0x4A,0x46,0x49,0x46,0x00,0x01,
                        0x01,0x00,0x00,0x01,0x00,0x01,0x00,0x00,0xFF,0xDB,0x00,0x43,
                        0x00,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                        0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                        0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                        0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                        0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                        0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                        0xFF,0xD9
                    };
                    File.WriteAllBytes(imagePath, placeholder);
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
                message.To.Add(defaultEmail); // send to self for demo
                message.Subject = "Email with Inline Image";
                message.IsBodyHtml = true;

                // HTML body referencing the CID.
                const string cid = "inlineImg";
                message.HtmlBody = $"<html><body><h3>Embedded Image</h3><img src=\"cid:{cid}\" alt=\"inline\"/></body></html>";

                // Create the linked resource for the image.
                using (LinkedResource linkedImage = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg))
                {
                    linkedImage.ContentId = cid;
                    message.LinkedResources.Add(linkedImage);
                }

                // Send the message via Gmail client.
                IGmailClient gmailClient = null;
                try
                {
                    gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
                    string sentMessageId = gmailClient.SendMessage(message);
                    Console.WriteLine($"Message sent successfully. Id: {sentMessageId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Gmail send failed: {ex.Message}");
                }
                finally
                {
                    if (gmailClient is IDisposable disposableClient)
                    {
                        disposableClient.Dispose();
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
