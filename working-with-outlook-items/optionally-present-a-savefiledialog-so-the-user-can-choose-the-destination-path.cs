using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder Exchange connection details
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string messageUri = "/mailfolders/inbox/messages/123";

            // Skip execution if placeholders are detected
            if (mailboxUri.Contains("example") || username.Contains("example") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operation.");
                return;
            }

            Console.Write("Enter destination file path to save the message: ");
            string destPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(destPath))
            {
                Console.Error.WriteLine("No destination path provided.");
                return;
            }

            string directory = Path.GetDirectoryName(destPath);
            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
            {
                Console.Error.WriteLine($"Directory does not exist: {directory}");
                return;
            }

            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    try
                    {
                        client.SaveMessage(messageUri, destPath);
                        Console.WriteLine($"Message saved to {destPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving message: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error connecting to Exchange server: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
