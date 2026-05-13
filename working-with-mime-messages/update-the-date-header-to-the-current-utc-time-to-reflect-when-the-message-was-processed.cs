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
            string outputPath = "processed.eml";

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
                    string placeholder = "From: placeholder@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(inputPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            // Load the email message
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email message: {ex.Message}");
                return;
            }

            using (mailMessage)
            {
                // Update the Date header to current UTC time
                mailMessage.Date = DateTime.UtcNow;

                // Save the updated message
                try
                {
                    mailMessage.Save(outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save email message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
