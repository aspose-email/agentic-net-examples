using System;
using System.IO;
using Aspose.Email;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                const string inputPath = "TestEml.eml";
                const string outputPath = "output.msg";

                // Ensure the input EML file exists; create a minimal placeholder if missing.
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
                        File.WriteAllText(inputPath, "Subject: Test\r\n\r\nThis is a test email.");
                        Console.WriteLine($"Created placeholder EML file: {inputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
                        return;
                    }
                }

                // Initialize load options to preserve attachments and embedded messages.
                EmlLoadOptions emlLoadOptions = new EmlLoadOptions()
                {
                    PreserveTnefAttachments = true,
                    PreserveEmbeddedMessageFormat = true
                };

                // Load the EML message with the specified options.
                using (MailMessage message = MailMessage.Load(inputPath, emlLoadOptions))
                {
                    try
                    {
                        // Save the message as MSG using the default MSG save options.
                        message.Save(outputPath, SaveOptions.DefaultMsg);
                        Console.WriteLine($"Converted '{inputPath}' to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save MSG: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
