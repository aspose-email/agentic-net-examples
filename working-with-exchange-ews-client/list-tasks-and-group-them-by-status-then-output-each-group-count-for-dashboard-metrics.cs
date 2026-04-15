using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            IEWSClient client = null;
            try
            {
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                NetworkCredential credentials = new NetworkCredential("username", "password");
                client = EWSClient.GetEWSClient(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Retrieve all tasks
            TaskCollection tasks = null;
            try
            {
                tasks = client.ListTasks();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to list tasks: {ex.Message}");
                return;
            }

            // Group tasks by their status and output counts
            var grouped = tasks.GroupBy(t => t.Status);
            foreach (IGrouping<ExchangeTaskStatus, ExchangeTask> group in grouped)
            {
                Console.WriteLine($"Status: {group.Key}, Count: {group.Count()}");
            }

            // Dispose client
            if (client is IDisposable disposableClient)
            {
                disposableClient.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
