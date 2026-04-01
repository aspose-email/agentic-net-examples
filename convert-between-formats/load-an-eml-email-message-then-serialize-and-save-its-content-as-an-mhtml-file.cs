using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.mhtml";

            // Ensure the input EML file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                string placeholderContent = "Subject: Placeholder\r\n\r\nThis is a placeholder email body.";
                File.WriteAllText(inputPath, placeholderContent);
            }

            // Ensure the output directory exists.
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the EML message and save it as MHTML.
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                mailMessage.Save(outputPath, SaveOptions.DefaultMhtml);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
