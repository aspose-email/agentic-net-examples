using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.eml";
            string outputPath = "output.msg";

            // Ensure the input EML file exists; create a minimal placeholder if missing
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
                    string placeholder = "Subject: Placeholder\r\n\r\nThis is a placeholder email body.";
                    File.WriteAllText(inputPath, placeholder);
                    Console.WriteLine($"Created placeholder EML file at '{inputPath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ioEx.Message}");
                    return;
                }
            }

            // Load the EML message with options to preserve attachments and embedded messages
            try
            {
                var emlLoadOptions = new EmlLoadOptions
                {
                    PreserveTnefAttachments = true,
                    PreserveEmbeddedMessageFormat = true
                };

                using (MailMessage message = MailMessage.Load(inputPath, emlLoadOptions))
                {
                    // Save the message as MSG format using default save options
                    try
                    {
                        message.Save(outputPath, SaveOptions.DefaultMsg);
                        Console.WriteLine($"Message successfully converted and saved to '{outputPath}'.");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Error saving MSG file: {saveEx.Message}");
                    }
                }
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Error loading EML file: {loadEx.Message}");
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
