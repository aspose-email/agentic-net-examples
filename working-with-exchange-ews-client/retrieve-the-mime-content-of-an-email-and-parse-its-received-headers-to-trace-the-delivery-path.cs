using System;
using System.Collections.Generic;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters (replace with real values)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define the message URI to fetch (replace with a real URI)
                string messageUri = "/ews/items/AAAkAD...";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Fetch the mail message from the server
                MailMessage mailMessage = client.FetchMessage(messageUri);

                // Convert MailMessage to MapiMessage to access raw MIME headers
                MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                // Get the raw transport headers (includes all Received headers)
                string rawHeaders = mapiMessage.TransportMessageHeaders;

                // Parse Received headers
                List<string> receivedHeaders = new List<string>();
                if (!string.IsNullOrEmpty(rawHeaders))
                {
                    string[] headerLines = rawHeaders.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                    foreach (string line in headerLines)
                    {
                        if (line.StartsWith("Received:", StringComparison.OrdinalIgnoreCase))
                        {
                            receivedHeaders.Add(line.Trim());
                        }
                    }
                }

                // Output the delivery path
                Console.WriteLine("Received headers (delivery path):");
                foreach (string received in receivedHeaders)
                {
                    Console.WriteLine(received);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
