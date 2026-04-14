using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Build a MAPI message (required for AppendMessage with markAsSent)
                MapiMessage mapiMessage = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Test Subject",
                    "Hello, this is a test message sent via IEWSClient.");

                // Append the message as a sent item and capture its URI
                string sentItemUri;
                try
                {
                    sentItemUri = client.AppendMessage(mapiMessage, true);
                    Console.WriteLine("Sent item URI: " + sentItemUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to append message: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
