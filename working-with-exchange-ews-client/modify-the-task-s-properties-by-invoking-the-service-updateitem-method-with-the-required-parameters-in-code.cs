using Aspose.Email.Mapi;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange.WebService.Models;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials and mailbox URI
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing real network calls with placeholder data
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder mailbox URI detected. Skipping EWS operation.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Create a task to be updated
                MapiTask task = new MapiTask
                {
                    Subject = "Updated Task",
                    Body = "This task has been updated via EWS.",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(2)
                };

                // Prepare the update parameters
                EwsUpdateItem updateItem = EwsUpdateItem.Create(task);

                // Cast to async interface to use UpdateItemAsync
                IAsyncEwsClient asyncClient = client as IAsyncEwsClient;
                if (asyncClient == null)
                {
                    Console.Error.WriteLine("Failed to obtain async EWS client.");
                    return;
                }

                // Perform the update
                try
                {
                    await asyncClient.UpdateItemAsync(updateItem).ConfigureAwait(false);
                    Console.WriteLine("Task updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error updating task: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
