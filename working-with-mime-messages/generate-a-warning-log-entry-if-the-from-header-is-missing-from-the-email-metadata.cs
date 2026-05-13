using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string emlPath = "sample.eml";

            // Ensure the EML file exists; create a minimal placeholder if missing
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
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder email body.");
                    }
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ioEx.Message}");
                    return;
                }
            }

            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Check for missing From header
                if (mailMessage.From == null)
                {
                    Console.Error.WriteLine("Warning: From header is missing from the email metadata.");
                }
                else
                {
                    Console.WriteLine($"From: {mailMessage.From}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
