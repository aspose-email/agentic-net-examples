using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "input.eml";
            string msgPath = "output.msg";

            // Verify input file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {emlPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {ex.Message}");
                    return;
                }
            }

            // Load the EML file with options and convert to MSG
            using (MailMessage message = MailMessage.Load(emlPath, new EmlLoadOptions
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            }))
            {
                message.Save(msgPath, SaveOptions.DefaultMsg);
                Console.WriteLine($"Successfully converted '{emlPath}' to '{msgPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
