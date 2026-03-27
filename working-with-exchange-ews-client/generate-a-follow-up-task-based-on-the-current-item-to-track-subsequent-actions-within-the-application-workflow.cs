using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client with mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Create a follow‑up task to track the calendar sharing invitation
                ExchangeTask followUpTask = new ExchangeTask
                {
                    Subject = "Follow up on calendar sharing invitation",
                    StartDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(2),
                    Body = "Check the status of the calendar sharing invitation sent earlier."
                };

                // Add the task to the default Tasks folder
                client.CreateTask(followUpTask);

                Console.WriteLine("Follow‑up task created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
