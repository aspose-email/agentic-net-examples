using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials detection – skip network call if defaults are used
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (username == "username" && password == "password")
            {
                Console.WriteLine("Placeholder credentials detected – skipping EWS operations.");
                return;
            }

            // Create credentials
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Prepare list of item URIs to delete
                List<string> itemUris = new List<string>
                {
                    "itemUri1",
                    "itemUri2",
                    "itemUri3"
                };

                // Deletion options – move items to Deleted Items folder
                DeletionOptions deleteOptions = new DeletionOptions(DeletionType.MoveToDeletedItems);

                // Cancellation token source with 5‑second timeout
                using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    // Run the synchronous DeleteItems method on a background task
                    Task deleteTask = Task.Run(() => client.DeleteItems(itemUris, deleteOptions), cts.Token);

                    try
                    {
                        deleteTask.Wait(cts.Token);
                        Console.WriteLine("Items deleted successfully.");
                    }
                    catch (OperationCanceledException)
                    {
                        Console.Error.WriteLine("Deletion operation was cancelled after timeout.");
                    }
                    catch (AggregateException ae)
                    {
                        // Unwrap and display any errors from the DeleteItems call
                        foreach (var ex in ae.InnerExceptions)
                        {
                            Console.Error.WriteLine($"Error deleting items: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
