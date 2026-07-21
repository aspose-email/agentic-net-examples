using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace ExchangeAqsSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define EWS connection parameters
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the EWS client (implements IDisposable)
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Build an Advanced Query Syntax (AQS) query
                    // Example: messages from john@example.com with "Report" in the subject
                    var query = new ExchangeAdvancedSyntaxMailQuery(
                        "('From' Contains 'john@example.com' AND 'Subject' Contains 'Report')");

                    // Retrieve messages from the Inbox that match the query
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query);

                    // Output basic information about each matching message
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"Received: {info.InternalDate}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
