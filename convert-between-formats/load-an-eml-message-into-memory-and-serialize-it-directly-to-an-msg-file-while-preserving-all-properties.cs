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
            string emlPath = "input.eml";
            string msgPath = "output.msg";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    string placeholder = "From: placeholder@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder EML.";
                    File.WriteAllText(emlPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            try
            {
                string outputDirectory = Path.GetDirectoryName(msgPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Load the EML message with options to preserve embedded formats
            try
            {
                EmlLoadOptions loadOptions = new EmlLoadOptions
                {
                    PreserveEmbeddedMessageFormat = true,
                    PreserveTnefAttachments = true
                };

                using (MailMessage mailMessage = MailMessage.Load(emlPath, loadOptions))
                {
                    // Convert to MAPI message preserving all properties
                    using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                    {
                        // Save as MSG file
                        mapiMessage.Save(msgPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing EML to MSG: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
