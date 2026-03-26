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
            string outputDir = Path.GetDirectoryName(oftPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Convert to MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    // Save as Outlook File Template (OFT)
                    mapiMessage.SaveAsTemplate(oftPath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}