using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.eml";
            string outputPath = "sample.html";

            // Ensure the input EML file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    // Create a minimal placeholder EML file
                    string placeholder = "From: placeholder@example.com\r\nTo: placeholder@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(inputPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML message and save as HTML using default HTML options (UTF‑8)
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultHtml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save HTML file: {ex.Message}");
                    return;
                }
            }

            Console.WriteLine($"EML file '{inputPath}' successfully converted to HTML at '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}