using Aspose.Email.Tools.Search;
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
            // Initialize client
            IEWSClient client = null;
            try
            {
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                var credentials = new NetworkCredential("username", "password");
                client = EWSClient.GetEWSClient(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or connect client: {ex.Message}");
                return;
            }

            // Build AQS query to find calendar items with subject containing "Meeting"
            MailQuery aqsQuery = new ExchangeAdvancedSyntaxMailQuery("subject:\"Meeting\"");

            // Use the default calendar folder URI
            string calendarFolderUri = client.MailboxInfo.CalendarUri;

            // Retrieve item URIs matching the query
            string[] itemUris = null;
            try
            {
                itemUris = client.ListItems(calendarFolderUri, null, aqsQuery, false);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving items: {ex.Message}");
                return;
            }

            // Output results
            if (itemUris != null && itemUris.Length > 0)
            {
                Console.WriteLine("Found calendar items:");
                foreach (string uri in itemUris)
                {
                    Console.WriteLine(uri);
                }
            }
            else
            {
                Console.WriteLine("No matching calendar items found.");
            }

            // Ensure client is disposed
            client?.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
