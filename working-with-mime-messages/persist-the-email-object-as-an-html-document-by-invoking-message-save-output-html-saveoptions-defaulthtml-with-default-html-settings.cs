using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "EmailWithAttachEmbedded.eml";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                string outputPath = $"{inputPath}.html";

                // Save the email as HTML using default HTML save options
                message.Save(outputPath, SaveOptions.DefaultHtml);

                Console.WriteLine($"Message saved as HTML to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
