using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Author note: Example demonstrates loading an EML file and saving it as MSG while preserving all properties.
            string inputPath = "input.eml";
            string outputPath = "output.msg";

            // Verify input file exists
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load EML with options to preserve TNEF attachments and embedded message format
            EmlLoadOptions emlLoadOptions = new EmlLoadOptions
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            using (MailMessage message = MailMessage.Load(inputPath, emlLoadOptions))
            {
                // Save as MSG preserving all properties
                message.Save(outputPath, SaveOptions.DefaultMsg);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
