using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

namespace AsposeEmailEwsCreateTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create and use the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
                {
                    // Loop to create multiple tasks
                    for (int i = 1; i <= 5; i++)
                    {
                        ExchangeTask task = new ExchangeTask();
                        task.Subject = $"Task {i}";
                        task.Body = $"This is task number {i}.";
                        // Cast integer to MailPriority enum (Low=0, Normal=1, High=2)
                        task.Priority = (MailPriority)i;

                        // Create the task in the default task folder
                        string taskUri = client.CreateTask(task);
                        Console.WriteLine($"Created task {i}: {taskUri}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
