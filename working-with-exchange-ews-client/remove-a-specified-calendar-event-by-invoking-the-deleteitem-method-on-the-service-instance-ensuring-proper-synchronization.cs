using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Create the EWS client.
            IEWSClient client = null;
            try
            {
                // TODO: Replace with actual service URL and credentials.
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                var credentials = new System.Net.NetworkCredential("username", "password");
                client = EWSClient.GetEWSClient(serviceUrl, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // TODO: Replace with the unique URI of the calendar event to delete.
                string eventUri = "https://exchange.example.com/EWS/Exchange.asmx/UniqueEventUri";

                try
                {
                    // Delete the calendar event permanently.
                    client.DeleteItem(eventUri, DeletionOptions.DeletePermanently);
                    Console.WriteLine("Calendar event deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to delete calendar event: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
