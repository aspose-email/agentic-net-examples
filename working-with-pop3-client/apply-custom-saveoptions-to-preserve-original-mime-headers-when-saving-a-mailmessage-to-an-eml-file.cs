using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string sourcePath = "input.eml";
            string targetPath = "output.eml";

            // Ensure the source EML file exists; create a minimal placeholder if missing.
            if (!File.Exists(sourcePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(sourcePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                using (MailMessage placeholder = new MailMessage())
                {
                    placeholder.From = new MailAddress("sender@example.com");
                    placeholder.To.Add(new MailAddress("recipient@example.com"));
                    placeholder.Subject = "Placeholder";
                    placeholder.Body = "This is a placeholder email.";
                    placeholder.Save(sourcePath);
                }
            }

            // Load the existing email.
            using (MailMessage message = MailMessage.Load(sourcePath))
            {
                // Create custom save options for EML format.
                EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);
                // Save the message with the custom options, preserving original MIME headers.
                message.Save(targetPath, saveOptions);
                Console.WriteLine($"Email saved to '{targetPath}' with custom save options.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
