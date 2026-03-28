using Aspose.Email.Clients.Exchange;
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
            // Initialize the EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Get the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // List the most recent message in the Inbox
                ExchangeMessageInfoCollection messageInfos = client.ListMessages(inboxUri, 1);
                if (messageInfos == null || messageInfos.Count == 0)
                {
                    Console.WriteLine("No messages found in the Inbox.");
                    return;
                }

                // Fetch the full message
                MailMessage message = client.FetchMessage(messageInfos[0].UniqueUri);

                // Create a follow‑up task based on the fetched message
                ExchangeTask task = new ExchangeTask();
                task.Subject = "Follow‑up: " + message.Subject;
                task.Body = "Please follow up on the email received from " + message.From + ".\n\nOriginal Message:\n" + message.Body;
                task.StartDate = DateTime.Now;
                task.DueDate = DateTime.Now.AddDays(2);

                // Add the task to the default Tasks folder
                client.CreateTask(task);

                Console.WriteLine("Follow‑up task created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
