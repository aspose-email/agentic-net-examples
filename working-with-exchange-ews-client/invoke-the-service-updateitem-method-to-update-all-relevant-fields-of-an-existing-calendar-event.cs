using System;
using System.Net;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange.WebService.Models;
using Aspose.Email.Mapi;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Exchange server connection details (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and configure the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Ensure the client supports async operations
                IAsyncEwsClient asyncClient = client as IAsyncEwsClient;
                if (asyncClient == null)
                {
                    Console.Error.WriteLine("The EWS client does not support async operations.");
                    return;
                }

                // Build a MapiCalendar object with the updated fields
                MapiCalendar calendar = new MapiCalendar(
                    location: "Conference Room A",
                    summary: "Project Sync",
                    description: "Updated meeting details",
                    startDate: DateTime.Now.AddHours(1),
                    endDate: DateTime.Now.AddHours(2));

                // Update specific properties as needed
                calendar.Subject = "Updated Project Sync";
                calendar.Location = "Conference Room B";

                // Prepare the update request
                EwsUpdateItem updateItem = EwsUpdateItem.Create(calendar);

                // Invoke the UpdateItem operation
                await asyncClient.UpdateItemAsync(updateItem);

                Console.WriteLine("Calendar event updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
