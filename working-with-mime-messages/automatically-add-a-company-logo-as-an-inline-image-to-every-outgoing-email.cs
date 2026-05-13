using Aspose.Email.Clients.Exchange.Dav;
using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare logo file
            string logoPath = "company_logo.png";
            if (!File.Exists(logoPath))
            {
                // Create a minimal placeholder PNG (1x1 pixel)
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
                try
                {
                    File.WriteAllBytes(logoPath, pngBytes);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder logo: {ex.Message}");
                    return;
                }
            }

            // Build the email message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Welcome to Our Company";

            // HTML body with reference to the inline image
            string htmlBody = @"<html><body><h1>Welcome!</h1><p>See our logo below:</p><img src=""cid:logo"" alt=""Company Logo""/></body></html>";
            AlternateView altView = AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, "text/html");

            // Create linked resource for the logo
            LinkedResource logoResource = new LinkedResource(logoPath, "logo");
            altView.LinkedResources.Add(logoResource);
            message.AlternateViews.Add(altView);
            message.IsBodyHtml = true;

            // Exchange client configuration (placeholder values)
            string exchangeUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (exchangeUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping actual send operation.");
                return;
            }

            // Send the message using ExchangeClient
            try
            {
                using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                {
                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
