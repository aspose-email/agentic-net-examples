using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "message.eml";
            string outputPath = "message.json";

            // Ensure input file exists; create a minimal placeholder if missing
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

                try
                {
                    using (MailMessage placeholder = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder Subject", "Placeholder body"))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Prepare a serializable object
                var jsonObject = new
                {
                    Subject = message.Subject,
                    From = message.From?.ToString(),
                    To = string.Join(";", message.To.Select(address => address.ToString())),
                    CC = string.Join(";", message.CC.Select(address => address.ToString())),
                    Bcc = string.Join(";", message.Bcc.Select(address => address.ToString())),
                    Date = message.Date,
                    Body = message.Body
                };

                // Serialize to JSON
                string json = JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions { WriteIndented = true });

                // Write JSON to output file
                try
                {
                    File.WriteAllText(outputPath, json);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write JSON file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
