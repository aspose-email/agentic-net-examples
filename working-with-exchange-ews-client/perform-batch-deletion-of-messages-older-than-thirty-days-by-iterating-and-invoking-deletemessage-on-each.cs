using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

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


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and connect the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
                {
                    // Retrieve messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                    DateTime cutoffDate = DateTime.Now.AddDays(-30);

                    foreach (ExchangeMessageInfo msg in messages)
                    {
                        // Use InternalDate for the received time
                        if (msg.InternalDate < cutoffDate)
                        {
                            // Delete the message by moving it to Deleted Items
                            client.DeleteItem(msg.UniqueUri, new DeletionOptions(DeletionType.MoveToDeletedItems));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
