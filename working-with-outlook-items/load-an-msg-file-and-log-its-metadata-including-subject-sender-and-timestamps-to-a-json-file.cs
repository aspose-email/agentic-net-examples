using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;
using System.Text.Json;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.msg";
            string outputPath = "metadata.json";

            // Verify that the input MSG file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                var metadata = new
                {
                    Subject = message.Subject,
                    SenderEmail = message.SenderEmailAddress,
                    SenderSmtp = message.SenderSmtpAddress,
                    ClientSubmitTime = message.ClientSubmitTime,
                    DeliveryTime = message.DeliveryTime
                };

                string json = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });

                // Write metadata to JSON file
                try
                {
                    File.WriteAllText(outputPath, json);
                    Console.WriteLine($"Metadata written to {outputPath}");
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Failed to write JSON file: {writeEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
