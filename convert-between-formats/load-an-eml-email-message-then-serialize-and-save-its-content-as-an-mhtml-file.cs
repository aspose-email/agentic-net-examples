using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "sample.eml";
            string outputPath = "sample.mhtml";

            // Ensure the input EML file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    const string placeholder = "Subject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(inputPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML message and save it as MHTML.
            try
            {
                using (MailMessage mailMessage = MailMessage.Load(inputPath))
                {
                    mailMessage.Save(outputPath, SaveOptions.DefaultMhtml);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing email files: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
