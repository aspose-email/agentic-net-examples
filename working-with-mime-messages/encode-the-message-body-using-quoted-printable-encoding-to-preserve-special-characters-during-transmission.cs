using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = "quoted_printable_message.eml";

            // Ensure the directory for the output file exists
            try
            {
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                // Set basic properties
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Sample Message with Quoted-Printable Encoding";
                message.Body = "This is a sample body with special characters: äöü ß € ©.";
                message.BodyEncoding = Encoding.UTF8;

                // Add the Content-Transfer-Encoding header with quoted-printable value
                // Header collection is case‑insensitive; use the standard header name.
                message.Headers.Add("Content-Transfer-Encoding", "quoted-printable");

                // Save the message to an EML file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to '{outputPath}'.");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
