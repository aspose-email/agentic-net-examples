using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "sample.msg");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a MailMessage with basic fields
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = "sender@example.com";
                mailMessage.To.Add("recipient@example.com");
                mailMessage.Subject = "Sample Message with Alternate Views";

                // Plain‑text alternate view
                string plainText = "This is the plain‑text version of the email.";
                ContentType plainContentType = new ContentType("text/plain");
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(plainText, plainContentType);
                mailMessage.AddAlternateView(plainView);

                // HTML alternate view
                string htmlText = "<html><body><h1>HTML Version</h1><p>This is the <b>HTML</b> version of the email.</p></body></html>";
                ContentType htmlContentType = new ContentType("text/html");
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlText, htmlContentType);
                mailMessage.AddAlternateView(htmlView);

                // Convert to MapiMessage and save as MSG
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    try
                    {
                        mapiMessage.Save(outputPath);
                        Console.WriteLine("MSG file saved to: " + outputPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error saving MSG file: " + ex.Message);
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}