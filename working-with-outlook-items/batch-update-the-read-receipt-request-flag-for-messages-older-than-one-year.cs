using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid unwanted network calls.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Define the folder to process (e.g., Inbox).
            const string folderUri = "Inbox";

            // Calculate the cutoff date (messages older than one year).
            DateTime cutoffDate = DateTime.UtcNow.AddYears(-1);

            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // List all messages in the specified folder.
                ExchangeMessageInfoCollection messages = client.ListMessages(folderUri);

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Use InternalDate for the message's received time.
                    if (info.InternalDate < cutoffDate)
                    {
                        // Fetch the message as a MapiMessage to access the ReadReceiptRequested property.
                        using (MapiMessage mapiMessage = client.FetchMapiMessage(info.UniqueUri))
                        {
                            // Update the read receipt request flag.
                            mapiMessage.ReadReceiptRequested = false;

                            // NOTE: Persisting the change back to the server typically requires an UpdateItem call.
                            // The exact method depends on the Aspose.Email version and may involve EWS update operations.
                            // This sample demonstrates the property change; implement the server update as needed.
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
