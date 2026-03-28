using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "Message.eml";
            string outputPath = "Message.msg";

            // Ensure the input EML file exists; create a minimal placeholder if missing.
            try
            {
                if (!File.Exists(inputPath))
                {
                    string placeholder = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Test\r\n\r\nBody of the email.";
                    File.WriteAllText(inputPath, placeholder);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to prepare input file: {ioEx.Message}");
                return;
            }

            // Ensure the output directory exists.
            try
            {
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Load the email message and save it as MSG using default options.
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                message.Save(outputPath, SaveOptions.DefaultMsg);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
