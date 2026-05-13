using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";

            // Ensure the file exists; create a minimal placeholder if missing.
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    string placeholder = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(emlPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the MIME message.
            MailMessage message;
            try
            {
                message = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            // Perform basic RFC 5322 validation.
            List<string> errors = new List<string>();

            if (message.From == null || string.IsNullOrWhiteSpace(message.From.Address))
                errors.Add("Missing or empty From address.");

            if (message.To == null || message.To.Count == 0)
                errors.Add("Missing To address.");

            if (string.IsNullOrWhiteSpace(message.Subject))
                errors.Add("Missing Subject header.");

            // Additional simple checks can be added here as needed.

            if (errors.Count == 0)
            {
                Console.WriteLine("The message complies with basic RFC 5322 requirements.");
            }
            else
            {
                Console.WriteLine("Validation errors found:");
                foreach (var error in errors)
                {
                    Console.WriteLine($"- {error}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
