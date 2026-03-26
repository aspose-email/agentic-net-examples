using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.eml";
            string outputPath = "sample.mhtml";

            // Ensure the input EML file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                string placeholder = "From: example@example.com\r\nSubject: Test\r\n\r\nThis is a test email.";
                File.WriteAllText(inputPath, placeholder);
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