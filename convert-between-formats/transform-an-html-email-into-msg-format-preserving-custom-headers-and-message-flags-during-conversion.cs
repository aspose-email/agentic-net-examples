using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            const string inputPath = "input.eml";
            const string outputPath = "output.msg";

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

                // Create a minimal placeholder email
                var placeholderMessage = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Placeholder Subject",
                    "<html><body><p>Placeholder email body.</p></body></html>")
                {
                    IsBodyHtml = true
                };
                placeholderMessage.Save(inputPath, SaveOptions.DefaultEml);
                Console.Error.WriteLine($"Input file not found. Created placeholder at '{inputPath}'.");
            }

            // Load the HTML email (EML format)
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Preserve custom headers
                mailMessage.Headers.Add("X-Custom-Header", "CustomValue");

                // Convert to MAPI message
                MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                // Preserve message flags (e.g., mark as unsent)
                mapiMessage.SetMessageFlags(MapiMessageFlags.MSGFLAG_UNSENT);

                // Save as MSG format
                mapiMessage.Save(outputPath);
            }

            Console.WriteLine($"Conversion completed successfully. MSG saved to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
