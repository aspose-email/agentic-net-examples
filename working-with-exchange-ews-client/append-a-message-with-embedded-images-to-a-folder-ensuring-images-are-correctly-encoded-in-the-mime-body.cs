using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Server and credentials
            string hostUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(hostUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Ensure image file exists
                string imagePath = "image.png";
                if (!File.Exists(imagePath))
                {
                    try
                    {
                        // Minimal 1x1 PNG placeholder
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

                // Build mail message with embedded image
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = "sender@example.com";
                mailMessage.To.Add("recipient@example.com");
                mailMessage.Subject = "Message with embedded image";

                string htmlBody = "<html><body><h1>Hello</h1><img src=\"cid:image1\"></body></html>";
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

                LinkedResource linkedImage = new LinkedResource(imagePath);
                linkedImage.ContentId = "image1";
                htmlView.LinkedResources.Add(linkedImage);

                mailMessage.AlternateViews.Add(htmlView);

                // Append to Drafts folder as a draft
                string draftsFolderUri = client.MailboxInfo.DraftsUri;
                try
                {
                    string messageUri = client.AppendMessage(draftsFolderUri, MapiMessage.FromMailMessage(mailMessage), false);
                    Console.WriteLine($"Message appended successfully. Uri: {messageUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to append message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
