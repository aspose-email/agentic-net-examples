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
            // Placeholder connection details – replace with real values.
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string messageUri = "/mail/inbox/12345"; // The message ID (URI) to fetch.
            string outputPath = "message.eml";

            // Skip execution when placeholder credentials are detected.
            if (exchangeUri.Contains("example.com") || username.Contains("example") || password.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Ensure the output directory exists.
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDir}': {dirEx.Message}");
                    return;
                }
            }

            // Create and use the Exchange client.
            using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
            {
                try
                {
                    // Save the message directly to an .eml file.
                    client.SaveMessage(messageUri, outputPath);
                    Console.WriteLine($"Message saved to '{outputPath}'.");
                }
                catch (Exception opEx)
                {
                    Console.Error.WriteLine($"Error fetching or saving the message: {opEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
