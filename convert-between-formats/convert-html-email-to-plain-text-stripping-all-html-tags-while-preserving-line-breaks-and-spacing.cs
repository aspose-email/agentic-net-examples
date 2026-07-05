using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        const string inputPath = "HtmlEmail.eml";

        try
        {
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                // Create a minimal placeholder EML file
                try
                {
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                // Create a simple email with an HTML body and save it
                MailMessage htmlMessage = new MailMessage();
                htmlMessage.From = "sender@example.com";
                htmlMessage.To = "recipient@example.com";
                htmlMessage.Subject = "Sample HTML Email";
                htmlMessage.HtmlBody = "<html><body><p>Hello <b>World</b>! Visit <a href=\"https://example.com\">example</a></p></body></html>";

                try
                {
                    htmlMessage.Save(inputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving HTML email: {ex.Message}");
                    return;
                }
            }

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                string plainWithUrl = message.GetHtmlBodyText(true);
                string plainWithoutUrl = message.GetHtmlBodyText(false);

                Console.WriteLine("Plain text with URLs:");
                Console.WriteLine(plainWithUrl);
                Console.WriteLine();
                Console.WriteLine("Plain text without URLs:");
                Console.WriteLine(plainWithoutUrl);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing email: {ex.Message}");
        }
    }
}
