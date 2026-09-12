using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Author note: This sample extracts the email body, strips HTML tags, and saves it as a text file.
            string inputPath = "sample.eml";
            string outputPath = "output.txt";

            // Verify input file exists
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Convert HTML body to plain text without URLs
                string plainBody = mailMessage.GetHtmlBodyText(false);

                // Fallback to the Body property if GetHtmlBodyText returns empty
                if (string.IsNullOrEmpty(plainBody))
                {
                    plainBody = mailMessage.Body;
                }

                // Write the plain text body to a .txt file
                File.WriteAllText(outputPath, plainBody);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
