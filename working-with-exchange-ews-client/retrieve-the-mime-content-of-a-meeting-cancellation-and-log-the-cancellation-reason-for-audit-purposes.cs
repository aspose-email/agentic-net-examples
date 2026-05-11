using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string cancellationId = "unique-cancellation-id"; // Unique identifier of the cancellation message
            string outputFilePath = "cancellation.eml";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputFilePath));
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Save the MIME content of the cancellation message to a file
                try
                {
                    client.SaveMessage(cancellationId, outputFilePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MIME content: {ex.Message}");
                    return;
                }

                // Fetch the message to extract the cancellation reason (body)
                MailMessage cancellationMessage = null;
                try
                {
                    cancellationMessage = client.FetchMessage(cancellationId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch cancellation message: {ex.Message}");
                    return;
                }

                // Log the cancellation reason for audit
                try
                {
                    string reason = cancellationMessage.Body;
                    Console.WriteLine("Cancellation Reason:");
                    Console.WriteLine(reason);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to read cancellation reason: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
