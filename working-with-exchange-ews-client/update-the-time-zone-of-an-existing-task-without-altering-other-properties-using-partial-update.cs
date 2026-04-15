using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;

namespace UpdateTaskTimeZoneSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // EWS connection parameters
                string serviceUrl = "https://example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Unique identifier (URI) of the task to be updated
                string taskUri = "https://example.com/EWS/Tasks/UniqueTaskId";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password" || taskUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create and connect the EWS client
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                    {
                        // Fetch the existing task
                        ExchangeTask task = client.FetchTask(taskUri);
                        if (task == null)
                        {
                            Console.Error.WriteLine("Task not found.");
                            return;
                        }

                        // Update only the start date (time zone handling can be done via DateTimeKind)
                        // Here we set the start date to 9:00 AM UTC on 1 Oct 2023
                        task.StartDate = new DateTime(2023, 10, 1, 9, 0, 0, DateTimeKind.Utc);

                        // Perform the partial update (only the modified property is sent)
                        client.UpdateTask(task);
                        Console.WriteLine("Task time zone (start date) updated successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
