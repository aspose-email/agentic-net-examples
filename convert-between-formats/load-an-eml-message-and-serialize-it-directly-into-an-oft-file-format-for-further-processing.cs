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
            string oftPath = "output.oft";

            // Verify input file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Input file not found: {emlPath}");
                return;
            }

            // Ensure output directory exists
            string? outputDir = Path.GetDirectoryName(oftPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the EML message, convert to MAPI, and save as OFT
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    mapiMessage.Save(oftPath, SaveOptions.DefaultOft);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
