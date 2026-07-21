using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

public class Program
{
    public static void Main(string[] args)
    {
        // Define connection parameters
        string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
        string username = "user@example.com";
        string password = "password";


        // Skip external calls when placeholder credentials are used
        if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
            return;
        }

        try
        {
            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve messages from the default Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages();

                // Iterate through each message and output selected properties
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Received: {info.InternalDate}");
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            // Gracefully handle any errors during client operations
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
