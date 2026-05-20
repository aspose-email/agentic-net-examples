using Aspose.Email.Clients.Exchange.Dav;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Paths and credentials (replace with real values)
            string emlPath = "message.eml";
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder values are detected
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Verify the email file exists
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
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{emlPath}' not found.");
                return;
            }

            // Load the message safely
            MailMessage message;
            try
            {
                message = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load message: {ex.Message}");
                return;
            }

            // Determine message size (25 MB limit)
            const long MaxSizeBytes = 25L * 1024 * 1024;
            long messageSize;
            try
            {
                using (var ms = new MemoryStream())
                {
                    message.Save(ms, SaveOptions.DefaultEml);
                    messageSize = ms.Length;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unable to determine message size: {ex.Message}");
                return;
            }

            if (messageSize > MaxSizeBytes)
            {
                Console.Error.WriteLine($"Message size {messageSize} bytes exceeds the 25 MB limit. Sending aborted.");
                return;
            }

            // Send the message using ExchangeClient
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
