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
            // Placeholder mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Placeholder task URI to be deleted
                string taskUri = "https://exchange.example.com/EWS/Tasks/UniqueTaskId";

                // Delete the task permanently
                client.DeleteItem(taskUri, DeletionOptions.DeletePermanently);
                Console.WriteLine("Task deleted permanently.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
