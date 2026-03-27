using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

namespace RemoveCalendarEvents
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Exchange Web Services endpoint and credentials (replace with real values)
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";
                NetworkCredential credentials = new NetworkCredential(username, password);

                // Create the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
                {
                    // Build a query to find calendar events to delete (e.g., subject contains "Obsolete")
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.Subject.Contains("Obsolete");
                    MailQuery query = builder.GetQuery();

                    // Get the calendar folder URI
                    string calendarFolderUri = client.MailboxInfo.CalendarUri;

                    // List calendar items that match the query
                    ExchangeMessageInfoCollection messages = client.ListMessages(calendarFolderUri, query);

                    // Delete each matching calendar event
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        client.DeleteItem(info.UniqueUri, DeletionOptions.DeletePermanently);
                        Console.WriteLine("Deleted event: " + info.Subject);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}