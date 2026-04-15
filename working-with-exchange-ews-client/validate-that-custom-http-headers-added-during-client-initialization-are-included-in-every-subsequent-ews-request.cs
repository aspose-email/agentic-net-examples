using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client with safety guard
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Add custom HTTP headers
                    client.AddHeader("X-Custom-Header", "Value1");
                    client.AddHeader("X-Another-Header", "Value2");

                    // Perform a simple request to ensure headers are sent with the request
                    ExchangeFolderInfo inboxInfo = client.GetFolderInfo("Inbox");

                    // Verify that the custom headers are present in the client configuration
                    foreach (KeyValuePair<string, string> header in client.Headers)
                    {
                        Console.WriteLine($"{header.Key}: {header.Value}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
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
