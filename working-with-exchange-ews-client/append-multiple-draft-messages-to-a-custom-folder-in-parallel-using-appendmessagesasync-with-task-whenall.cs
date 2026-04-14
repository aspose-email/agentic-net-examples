using Aspose.Email.Mapi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange.WebService.Models;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // EWS connection parameters
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create synchronous EWS client and obtain async interface
            using (IEWSClient syncClient = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                IAsyncEwsClient client = syncClient as IAsyncEwsClient;
                if (client == null)
                {
                    Console.Error.WriteLine("Async EWS client is not available.");
                    return;
                }

                // Define the custom folder URI where drafts will be stored
                string customFolderUri = "customFolder";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Prepare draft messages
                List<MailMessage> draftMessages = new List<MailMessage>();
                for (int i = 1; i <= 3; i++)
                {
                    MailMessage draft = new MailMessage();
                    draft.From = username;
                    draft.To.Add(username);
                    draft.Subject = $"Draft {i}";
                    draft.Body = $"This is the body of draft message {i}.";
                    draftMessages.Add(draft);
                }

                // Create a list of tasks that append each draft in parallel
                List<Task<IEnumerable<string>>> appendTasks = new List<Task<IEnumerable<string>>>();
                foreach (MailMessage draft in draftMessages)
                {
                    EwsAppendMessage appendParams = EwsAppendMessage.Create()
                        .SetFolder(customFolderUri)
                        .AddMessage(MapiMessage.FromMailMessage(draft));

                    Task<IEnumerable<string>> task = client.AppendMessagesAsync(appendParams);
                    appendTasks.Add(task);
                }

                // Wait for all append operations to complete
                Task.WhenAll(appendTasks).ContinueWith(allTask =>
                {
                    if (allTask.IsFaulted)
                    {
                        Console.Error.WriteLine("One or more append operations failed.");
                        if (allTask.Exception != null)
                        {
                            Console.Error.WriteLine(allTask.Exception.Flatten().Message);
                        }
                        return;
                    }

                    // Output the URIs of the created draft messages
                    for (int i = 0; i < appendTasks.Count; i++)
                    {
                        IEnumerable<string> resultUris = appendTasks[i].Result;
                        foreach (string uri in resultUris)
                        {
                            Console.WriteLine($"Draft {i + 1} uploaded. URI: {uri}");
                        }
                    }
                }).Wait();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
