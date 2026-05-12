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
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {dirEx.Message}");
                    return;
                }
            }

            // Create a mail message with French characters
            using (MailMessage message = new MailMessage("sender@example.com", "recipient@example.com", "Bonjour – Café", "Ceci est un test avec des caractères français comme é, è, à, ç."))
            {
                // Set the preferred text encoding to ISO-8859-1
                message.PreferredTextEncoding = Encoding.GetEncoding("ISO-8859-1");

                // Save the message to a file
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultMsg);
                    Console.WriteLine($"Message saved successfully to '{outputPath}'.");
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
