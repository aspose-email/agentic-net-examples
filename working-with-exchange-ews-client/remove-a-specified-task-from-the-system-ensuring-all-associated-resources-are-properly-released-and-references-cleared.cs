using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

namespace AsposeEmailTaskRemoval
{
    class Program
    {
        static void Main()
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            // Top‑level exception guard
            try
            {
                // Define connection parameters (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // The unique URI of the task to be removed
                string taskUri = "https://exchange.example.com/EWS/Tasks/UniqueTaskId";

                // Create a synchronous EWS client via the factory
                using (IEWSClient syncClient = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
                {
                    // Attempt to obtain an asynchronous client interface
                    IAsyncEwsClient asyncClient = syncClient as IAsyncEwsClient;

                    if (asyncClient != null)
                    {
                        // Asynchronous deletion using the verified DeleteItemAsync method
                        // DeletionOptions.DeletePermanently removes the item permanently
                        var deleteTask = asyncClient.DeleteItemAsync(taskUri, DeletionOptions.DeletePermanently);
                        deleteTask.Wait(); // Wait synchronously inside the non‑async Main
                        Console.WriteLine("Task deleted asynchronously.");
                    }
                    else
                    {
                        // Fallback to synchronous deletion if async interface is unavailable
                        syncClient.DeleteItem(taskUri, DeletionOptions.DeletePermanently);
                        Console.WriteLine("Task deleted synchronously.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Write any errors to the error stream without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}