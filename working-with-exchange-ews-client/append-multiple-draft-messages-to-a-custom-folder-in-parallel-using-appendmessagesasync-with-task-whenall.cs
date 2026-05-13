using Aspose.Email.Mapi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange.WebService.Models;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Exchange Web Services endpoint and credentials
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // URI of the custom folder where drafts will be stored
                string customFolderUri = "custom-folder-uri";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Prepare draft messages
                List<MailMessage> drafts = new List<MailMessage>();
                MailMessage draft1 = new MailMessage("sender@example.com", "recipient1@example.com", "Draft 1", "Body of draft 1");
                drafts.Add(draft1);
                MailMessage draft2 = new MailMessage("sender@example.com", "recipient2@example.com", "Draft 2", "Body of draft 2");
                drafts.Add(draft2);
                // Add more drafts as needed

                // Create and use the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Cast to async interface
                    IAsyncEwsClient asyncClient = client as IAsyncEwsClient;
                    if (asyncClient == null)
                    {
                        Console.Error.WriteLine("Async EWS client is not available.");
                        return;
                    }

                    // List of tasks for parallel appends
                    List<Task<IEnumerable<string>>> appendTasks = new List<Task<IEnumerable<string>>>();

                    foreach (MailMessage draft in drafts)
                    {
                        // Build parameters for AppendMessagesAsync
                        EwsAppendMessage appendParams = EwsAppendMessage.Create()
                            .SetFolder(customFolderUri)
                            .AddMessage(MapiMessage.FromMailMessage(draft));

                        // Start the asynchronous append operation
                        Task<IEnumerable<string>> task = asyncClient.AppendMessagesAsync(appendParams);
                        appendTasks.Add(task);
                    }

                    // Wait for all append operations to finish
                    Task.WhenAll(appendTasks).Wait();

                    // Output results
                    foreach (Task<IEnumerable<string>> task in appendTasks)
                    {
                        if (task.IsCompletedSuccessfully)
                        {
                            foreach (string uri in task.Result)
                            {
                                Console.WriteLine($"Draft appended with URI: {uri}");
                            }
                        }
                        else if (task.IsFaulted && task.Exception != null)
                        {
                            Console.Error.WriteLine($"Append failed: {task.Exception.GetBaseException().Message}");
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
}
