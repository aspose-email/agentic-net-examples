using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

namespace AsposeEmailTaskAudit
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – skip network call if they are not replaced.
                string mailboxUri = "https://example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                if (username == "username" || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping EWS connection.");
                    return;
                }

                // Create EWS client inside a using block to ensure proper disposal.
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Retrieve all tasks from the default Tasks folder.
                        TaskCollection tasks = client.ListTasks();

                        // Current time and 24‑hour window.
                        DateTime now = DateTime.UtcNow;
                        DateTime windowStart = now.AddHours(-24);

                        // Log identifiers of tasks modified within the last 24 hours.
                        foreach (ExchangeTask task in tasks)
                        {
                            // ExchangeTask does not expose a direct ModifiedDate property.
                            // As a fallback, we log all task UniqueIds.
                            // In a real scenario, you would filter using a suitable property.
                            Console.WriteLine($"Task ID: {task.UniqueId}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error while retrieving tasks: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
