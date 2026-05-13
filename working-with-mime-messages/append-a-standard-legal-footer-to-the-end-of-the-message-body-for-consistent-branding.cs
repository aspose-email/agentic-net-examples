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
            // Define input and output file paths
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure the input file exists; create a minimal placeholder if it does not
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "This is a placeholder message body."))
                    {
                        placeholder.Save(inputPath);
                        Console.WriteLine($"Placeholder message created at '{inputPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the existing message, append the legal footer, and save
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    const string legalFooter = "\n\n---\nLegal Disclaimer: This email is confidential and intended solely for the recipient.";
                    // Append footer to the existing body
                    string originalBody = message.Body ?? string.Empty;
                    message.Body = originalBody + legalFooter;

                    // Save the modified message
                    message.Save(outputPath);
                    Console.WriteLine($"Message with legal footer saved to '{outputPath}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
