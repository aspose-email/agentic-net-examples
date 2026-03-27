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
            string outputPath = "output.html";

            // Ensure the input EML file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                string placeholder = "From: sender@example.com\r\nTo: receiver@example.com\r\nSubject: Test Email\r\nDate: Thu, 01 Jan 1970 00:00:00 +0000\r\n\r\nThis is a placeholder email.";
                File.WriteAllText(inputPath, placeholder);
            }

            // Load the EML file and convert it to HTML (MHT) while preserving the original date
            using (Aspose.Email.MailMessage mailMessage = Aspose.Email.MailMessage.Load(inputPath))
            {
                Aspose.Email.MhtSaveOptions saveOptions = new Aspose.Email.MhtSaveOptions();
                saveOptions.PreserveOriginalDate = true;

                mailMessage.Save(outputPath, saveOptions);
                Console.WriteLine("EML file has been converted to HTML with the original date preserved.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}