using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Aspose.Email;

namespace AsposeEmailJsonExport
{
    // Author note: This example loads an email file, converts selected fields to a DTO,
    // serializes it to JSON, and saves the JSON to disk with proper error handling.
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output file paths
                string inputPath = "sample.eml";
                string outputPath = "sample.json";

                // Verify that the input email file exists
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

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the email message
                using (MailMessage mailMessage = MailMessage.Load(inputPath))
                {
                    // Map the MailMessage to a simple DTO for JSON serialization
                    MailMessageDto dto = new MailMessageDto
                    {
                        Subject = mailMessage.Subject,
                        Body = mailMessage.Body,
                        From = mailMessage.From?.ToString(),
                        To = mailMessage.To?.Select(address => address.ToString()).ToArray()
                    };

                    // Serialize the DTO to JSON with indentation
                    JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    string jsonContent = JsonSerializer.Serialize(dto, jsonOptions);

                    // Write the JSON content to the output file
                    try
                    {
                        File.WriteAllText(outputPath, jsonContent);
                        Console.WriteLine($"Message serialized to JSON and saved at: {outputPath}");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Failed to write JSON file: {ioEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    // Simple DTO representing the parts of MailMessage we want to export
    public class MailMessageDto
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string[] To { get; set; }
    }
}
