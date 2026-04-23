using System;
using System.Net;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Skip execution when placeholders are detected to avoid network calls.
            if (accessToken.StartsWith("YOUR_") || defaultEmail.StartsWith("YOUR_"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Gmail client operations.");
                return;
            }

            // Configure HTTP proxy.
            IWebProxy proxy = new WebProxy("http://proxy.example.com:8080");

            // Create Gmail client instance.
            using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
            {
                // Assign the proxy to the client.
                gmailClient.Proxy = proxy;

                // Example operation: list messages and output their IDs.
                List<GmailMessageInfo> messages = gmailClient.ListMessages();
                foreach (GmailMessageInfo info in messages)
                {
                    Console.WriteLine($"Message Id: {info.Id}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
