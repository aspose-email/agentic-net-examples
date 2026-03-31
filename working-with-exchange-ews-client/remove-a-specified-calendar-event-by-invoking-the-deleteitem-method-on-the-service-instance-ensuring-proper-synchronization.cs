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
            // Placeholder connection details
            string serviceUrl = "https://ews.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip real network calls when placeholders are used
            if (serviceUrl.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operation.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // URI of the calendar item to delete (replace with actual item URI)
                    string itemUri = "https://ews.example.com/EWS/Exchange.asmx/Calendar/ItemId";

                    // Use default deletion options (moves item to Deleted Items)
                    DeletionOptions options = CalendarDeletionOptions.Default;

                    // Delete the calendar event
                    client.DeleteItem(itemUri, options);
                    Console.WriteLine("Calendar event deleted successfully.");

                    // Optional: synchronize the calendar folder after deletion
                    string calendarFolderUri = client.CurrentCalendarFolderUri;
                    client.SyncFolder(calendarFolderUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during deletion: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
