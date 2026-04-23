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
            string outputPath = "output.txt";

            // Verify input file exists; create minimal placeholder if missing
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

                using (MailMessage placeholder = new MailMessage("placeholder@example.com", "placeholder@example.com", "Placeholder", "This is a placeholder body."))
                {
                    placeholder.Save(inputPath);
                }
                Console.Error.WriteLine($"Input file not found. Created placeholder at {inputPath}.");
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
                string plainBody;
                if (mailMessage.IsBodyHtml)
                {
                    // Convert HTML body to plain text (strips HTML tags)
                    plainBody = mailMessage.GetHtmlBodyText(true);
                }
                else
                {
                    plainBody = mailMessage.Body;
                }

                // Write the plain text body to a .txt file
                File.WriteAllText(outputPath, plainBody);
                Console.WriteLine($"Plain text body written to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
