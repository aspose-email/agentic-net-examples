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
            // Placeholder connection details
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string attachmentUri = "/Attachments/12345";
            string outputPath = "attachment.bin";

            // Skip real network calls when placeholders are used
            if (exchangeUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
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

            // Connect to Exchange server
            try
            {
                using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                {
                    // Fetch the attachment metadata
                    Attachment attachment;
                    try
                    {
                        attachment = client.FetchAttachment(attachmentUri);
                    }
                    catch (Exception fetchEx)
                    {
                        Console.Error.WriteLine($"Failed to fetch attachment: {fetchEx.Message}");
                        return;
                    }

                    // Stream the attachment to a file without loading it entirely into memory
                    try
                    {
                        using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            attachment.Save(fileStream);
                        }
                        Console.WriteLine($"Attachment saved to '{outputPath}'.");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Error saving attachment to file: {ioEx.Message}");
                    }
                }
            }
            catch (Exception connEx)
            {
                Console.Error.WriteLine($"Connection error: {connEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
