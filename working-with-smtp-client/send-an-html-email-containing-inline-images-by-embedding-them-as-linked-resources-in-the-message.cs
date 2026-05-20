using System;
using System.IO;
using System.Net.Mime;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Paths and resources
            string imagePath = "image.jpg";

            // Verify image file exists
            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine($"Image file not found: {imagePath}");
                return;
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "HTML Email with Inline Image";

                // Plain text view
                using (AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is the plain text version.", null, "text/plain"))
                {
                    // HTML view with CID reference
                    using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                        "Here is an embedded image: <img src=\"cid:image1\"/>", null, "text/html"))
                    {
                        // Linked resource (inline image)
                        using (LinkedResource linked = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg))
                        {
                            linked.ContentId = "image1";

                            // Add linked resource to the message
                            message.LinkedResources.Add(linked);
                        }

                        // Add alternate views to the message
                        message.AlternateViews.Add(plainView);
                        message.AlternateViews.Add(htmlView);
                    }
                }

                // SMTP client configuration (placeholder values)
                string host = "smtp.example.com";
                int port = 587;
                string username = "user@example.com";
                string password = "password";

                // Guard against placeholder credentials/hosts
                if (host.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder SMTP settings detected. Skipping send.");

                    // Optionally save the message to a file for verification
                    string outputPath = "output.eml";
                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    }

                    return;
                }

                // Send the email
                using (SmtpClient client = new SmtpClient(host, port, username, password))
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
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
