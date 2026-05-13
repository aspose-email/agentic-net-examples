using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Please provide the path to the MIME file as a command‑line argument.");
                return;
            }

            string messageFilePath = args[0];

            if (!File.Exists(messageFilePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(messageFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                // Ensure the directory exists before attempting to save a placeholder file
                string? directory = Path.GetDirectoryName(messageFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                try
                {
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {messageFilePath}");
                return;
            }

            using (MailMessage mailMessage = MailMessage.Load(messageFilePath))
            {
                Console.WriteLine($"Subject: {mailMessage.Subject}");
                Console.WriteLine($"From: {mailMessage.From}");
                Console.WriteLine($"To: {mailMessage.To}");
                Console.WriteLine($"Date: {mailMessage.Date}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
