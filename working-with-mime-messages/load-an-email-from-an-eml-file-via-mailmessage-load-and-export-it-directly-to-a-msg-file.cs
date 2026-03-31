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
            string outputPath = "output.msg";

            // Ensure the input EML file exists; create a minimal placeholder if missing.
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
                    string placeholderEml = "From: placeholder@example.com\r\nTo: placeholder@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(inputPath, placeholderEml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML file and save it as MSG.
            try
            {
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    message.Save(outputPath, SaveOptions.DefaultMsg);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing email files: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
