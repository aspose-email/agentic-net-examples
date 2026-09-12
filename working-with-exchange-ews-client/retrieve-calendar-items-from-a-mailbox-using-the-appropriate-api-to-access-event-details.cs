using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Replace with your actual EWS endpoint and credentials
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client (IEWSClient) and ensure it is disposed properly
            using (IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Obtain mailbox information to locate the Calendar folder URI
                ExchangeMailboxInfo mailboxInfo = ewsClient.GetMailboxInfo();
                string calendarUri = mailboxInfo.CalendarUri;

                // Retrieve calendar items (appointments) from the Calendar folder
                ExchangeMessageInfoCollection calendarItems = ewsClient.ListMessages(calendarUri);

                Console.WriteLine($"Found {calendarItems.Count} calendar item(s):");
                foreach (ExchangeMessageInfo itemInfo in calendarItems)
                {
                    // For demonstration, output the subject of each calendar item
                    Console.WriteLine($"- Subject: {itemInfo.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            // Gracefully report any errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
