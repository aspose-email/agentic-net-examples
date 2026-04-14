using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string messageUri = "https://exchange.example.com/EWS/MessageId"; // replace with actual message URI
            string outputPath = "RawMessage.eml";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {dirEx.Message}");
                    return;
                }
            }

            // Create and connect the Exchange client
            NetworkCredential credentials = new NetworkCredential(username, password);
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Retrieve raw MIME content into a memory stream
                    using (MemoryStream mimeStream = new MemoryStream())
                    {
                        try
                        {
                            client.SaveMessage(messageUri, mimeStream);
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                            return;
                        }

                        mimeStream.Position = 0;
                        string rawMime;
                        using (StreamReader reader = new StreamReader(mimeStream))
                        {
                            rawMime = reader.ReadToEnd();
                        }

                        // Log raw MIME content to console
                        Console.WriteLine("=== Raw MIME Content ===");
                        Console.WriteLine(rawMime);
                        Console.WriteLine("========================");

                        // Optionally write raw MIME to a file
                        try
                        {
                            File.WriteAllText(outputPath, rawMime);
                        }
                        catch (Exception fileEx)
                        {
                            Console.Error.WriteLine($"Failed to write raw MIME to file: {fileEx.Message}");
                        }
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Exchange client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
