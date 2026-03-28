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
    try
    {
        string placeholder = "From: placeholder@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder EML.";
        File.WriteAllText(emlPath, placeholder);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
        return;
    }
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
