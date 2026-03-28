using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output file paths
            string inputPath = "sample.eml";
            string outputPath = "sample.html";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                string placeholder = "From: sender@example.com\r\nTo: receiver@example.com\r\nSubject: Test\r\n\r\nThis is a test email.";
                File.WriteAllText(inputPath, placeholder, Encoding.UTF8);
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the EML message with UTF‑8 preferred encoding
            EmlLoadOptions loadOptions = new EmlLoadOptions
            {
                PreferredTextEncoding = Encoding.UTF8
            };

            using (MailMessage message = MailMessage.Load(inputPath, loadOptions))
            {
                // Save the message as HTML; DefaultHtml uses UTF‑8 encoding internally
                message.Save(outputPath, SaveOptions.DefaultHtml);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return;
        }
    }
}
