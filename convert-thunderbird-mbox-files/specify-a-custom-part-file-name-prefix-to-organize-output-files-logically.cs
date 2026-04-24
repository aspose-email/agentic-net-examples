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
            // Placeholder values – replace with real credentials and URIs.
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string messageUri = "/mailfolders/inbox/messages/123";

            // Guard against executing with placeholder data.
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder Exchange URI detected. Skipping network operation.");
                return;
            }

            // Ensure output directory exists.
            string outputDirectory = "Output";
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Define custom prefix for the saved file.
            string filePrefix = "CustomPrefix_";
            string outputPath = Path.Combine(outputDirectory, filePrefix + "message.eml");

            // Create and use the Exchange client.
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    try
                    {
                        client.SaveMessage(messageUri, outputPath);
                        Console.WriteLine($"Message saved to: {outputPath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Failed to connect to Exchange server: {clientEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
