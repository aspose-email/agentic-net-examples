using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        // Input and output file paths
        string inputPath = "input.eml";
        string outputPath = "output.eml";

        // Guard file existence
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

        try
        {
            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Remove all custom headers (extended properties) to streamline storage
                mailMessage.Headers.Clear();

                // Save the cleaned message
                mailMessage.Save(outputPath, SaveOptions.DefaultEml);
            }

            Console.WriteLine($"Message saved without extended properties to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
