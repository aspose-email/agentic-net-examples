using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // EWS client configuration (replace with real credentials)
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and dispose the EWS client safely
            using (IEWSClient ewsClient = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // ------------------------------------------------------------
                // Example: Direct AQS string
                // ------------------------------------------------------------
                // AQS query string can combine multiple criteria using logical operators.
                // Supported operators: AND, OR, NOT, parentheses for grouping, and property filters.
                string aqsString = "From:'john@example.com' AND Subject:'Report' AND Received>='2023-01-01'";

                // Build the AQS query
                ExchangeAdvancedSyntaxMailQuery aqsQuery = new ExchangeAdvancedSyntaxMailQuery(aqsString);

                // Retrieve the Inbox folder URI
                string inboxUri = ewsClient.GetMailboxInfo().InboxUri;

                // Execute the search using the AQS query
                ExchangeMessageInfoCollection aqsMessages = ewsClient.ListMessages(inboxUri, aqsQuery);
                Console.WriteLine($"Found {aqsMessages.Count} messages using direct AQS string.");
            }
        }
        catch (Exception ex)
        {
            // Graceful error handling
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
