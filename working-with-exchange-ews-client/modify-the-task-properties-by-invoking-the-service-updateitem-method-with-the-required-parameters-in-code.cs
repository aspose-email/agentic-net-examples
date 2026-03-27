using System;
using System.Net;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange.WebService.Models;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize credentials and service URL (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create async EWS client
            IAsyncEwsClient client = EWSClient.GetEwsClientAsync(serviceUrl, credentials, null, CancellationToken.None, null)
                                            .GetAwaiter()
                                            .GetResult();

            using (client)
            {
                // Create a task and set properties to be updated
                MapiTask task = new MapiTask
                {
                    Subject = "Updated Task Subject",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(3),
                    Status = MapiTaskStatus.NotStarted
                };

                // Prepare the update item request
                EwsUpdateItem updateItem = EwsUpdateItem.Create(task);

                // Invoke the UpdateItem operation
                client.UpdateItemAsync(updateItem)
                      .GetAwaiter()
                      .GetResult();

                Console.WriteLine("Task updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
