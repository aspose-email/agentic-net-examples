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
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure input file exists; create minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    MapiMessage placeholder = new MapiMessage(
                        "placeholder@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body");
                    placeholder.Save(inputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the message
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load message: {ex.Message}");
                return;
            }

            // Validate HTML body for prohibited tags
            string bodyHtml = message.BodyHtml ?? string.Empty;
            string[] prohibitedTags = new string[] { "<script", "<iframe", "<object" };
            bool containsProhibited = false;
            foreach (string tag in prohibitedTags)
            {
                if (bodyHtml.IndexOf(tag, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    containsProhibited = true;
                    break;
                }
            }

            if (containsProhibited)
            {
                Console.Error.WriteLine("The message body contains prohibited HTML tags. Save operation aborted.");
                return;
            }

            // Save the validated message
            try
            {
                message.Save(outputPath);
                Console.WriteLine($"Message saved successfully to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save message: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
