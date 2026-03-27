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
            string msgPath = "input.msg";
            string emlPath = "output.eml";

            // Verify input file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(emlPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating directory '{outputDir}': {ex.Message}");
                    return;
                }
            }

            // Load MSG file into MapiMessage
            using (MapiMessage mapiMsg = MapiMessage.Load(msgPath))
            {
                // Convert to MailMessage preserving properties
                MailMessage mailMsg = mapiMsg.ToMailMessage(new MailConversionOptions());

                // Save as EML with default options
                mailMsg.Save(emlPath, SaveOptions.DefaultEml);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
