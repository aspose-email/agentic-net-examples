using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        // Top‑level exception guard
        try
        {
            const string emlPath = "sample.eml";
            const string msgPath = "output.msg";

            // Ensure source EML exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    string placeholder = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Test EML\r\n\r\nThis is a test email.";
                    File.WriteAllText(emlPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML with options preserving TNEF and embedded messages
            var emlLoadOptions = new EmlLoadOptions
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            // Guard the load/save operations
            try
            {
                using (MailMessage message = MailMessage.Load(emlPath, emlLoadOptions))
                {
                    // Convert and save as MSG using default MSG save options
                    message.Save(msgPath, SaveOptions.DefaultMsg);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
                return;
            }

            Console.WriteLine($"Conversion succeeded. MSG saved to '{msgPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
