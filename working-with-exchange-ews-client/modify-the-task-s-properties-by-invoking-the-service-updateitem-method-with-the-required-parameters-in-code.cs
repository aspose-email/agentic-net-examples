using System;
using System.Net;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange.WebService.Models;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create an asynchronous EWS client
            IAsyncEwsClient asyncClient = Aspose.Email.Clients.Exchange.WebService.EWSClient
                .GetEwsClientAsync(mailboxUri, credentials, null, CancellationToken.None, null)
                .GetAwaiter()
                .GetResult();

            // Ensure the client is disposed properly
            using (asyncClient)
            {
                // Create a MapiTask with updated properties
                MapiTask updatedTask = new MapiTask();
                updatedTask.Subject = "Updated Task Subject";
                updatedTask.StartDate = DateTime.Now;
                updatedTask.DueDate = DateTime.Now.AddDays(3);
                updatedTask.Status = MapiTaskStatus.NotStarted;

                // Prepare the update item request
                EwsUpdateItem updateItem = EwsUpdateItem.Create(updatedTask);

                // Invoke the UpdateItem operation
                asyncClient.UpdateItemAsync(updateItem)
                    .GetAwaiter()
                    .GetResult();

                Console.WriteLine("Task properties updated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}