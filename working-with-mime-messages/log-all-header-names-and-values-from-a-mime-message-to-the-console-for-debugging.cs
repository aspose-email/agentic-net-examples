using Aspose.Email.Mime;
using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";

            // Ensure the input file exists; create a minimal placeholder if missing.
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
                    using (StreamWriter writer = new StreamWriter(emlPath, false))
                    {
                        writer.WriteLine("Subject: Placeholder");
                        writer.WriteLine("From: placeholder@example.com");
                        writer.WriteLine("To: recipient@example.com");
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder email body.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the MIME message and log all headers.
            try
            {
                using (MailMessage mailMessage = MailMessage.Load(emlPath))
                {
                    HeaderCollection headers = mailMessage.Headers;
                    foreach (string key in headers.Keys)
                    {
                        string value = headers[key];
                        Console.WriteLine($"{key}: {value}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading or processing the email: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
