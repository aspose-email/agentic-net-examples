using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Service URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // URI of the calendar event to delete
                string eventUri = "https://exchange.example.com/EWS/Exchange.asmx/Calendar/ItemId";

                // Deletion options (default moves to Deleted Items)
                DeletionOptions options = DeletionOptions.Default;

                // Delete the calendar event
                client.DeleteItem(eventUri, options);

                Console.WriteLine("Calendar event deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
