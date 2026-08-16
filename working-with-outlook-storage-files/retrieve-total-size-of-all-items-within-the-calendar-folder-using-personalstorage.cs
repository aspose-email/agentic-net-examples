using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            const string inputPath = "TestEml.eml";
            const string outputPath = "output.msg";

            // Ensure input EML exists; create a minimal placeholder if missing
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

                try
                {
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = "placeholder@example.com";
                        placeholder.To = "recipient@example.com";
                        placeholder.Subject = "Placeholder Email";
                        placeholder.Body = "This is a placeholder email generated because the source file was missing.";
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML with options preserving TNEF attachments and embedded message format
            EmlLoadOptions emlLoadOptions = new EmlLoadOptions
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            try
            {
                using (MailMessage message = MailMessage.Load(inputPath, emlLoadOptions))
                {
                    // Save as MSG using the default MSG save options
                    try
                    {
                        message.Save(outputPath, SaveOptions.DefaultMsg);
                        Console.WriteLine($"Conversion successful: '{inputPath}' -> '{outputPath}'");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
