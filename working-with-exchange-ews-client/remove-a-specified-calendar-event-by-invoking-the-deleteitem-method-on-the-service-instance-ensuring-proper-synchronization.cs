using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Placeholder credentials – replace with real values for actual execution
        string mailboxUri = "https://example.com/EWS/Exchange.asmx";
        string username = "username";
        string password = "password";

        // Guard: skip network call when placeholders are detected
        if (username == "username" || password == "password")
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operation.");
            return;
        }

        // URI of the calendar item to be removed (replace with actual item URI)
        string itemUri = "https://example.com/EWS/Exchange.asmx/Calendar/ItemId";

        try
        {
            // Create and dispose the EWS client safely
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Delete the calendar item permanently
                client.DeleteItem(itemUri, DeletionOptions.DeletePermanently);
                Console.WriteLine("Calendar item deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error deleting calendar item: {ex.Message}");
        }
    }
}
