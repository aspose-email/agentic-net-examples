using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – real credentials should be provided by the user.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string taskUri = "https://exchange.example.com/EWS/Tasks/12345";

            // Skip execution when placeholder values are detected to avoid unwanted network calls.
            if (serviceUrl.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected – skipping Exchange operation.");
                return;
            }

            // Create and configure the EWS client.
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Fetch the task from Exchange.
                    ExchangeTask task = client.FetchTask(taskUri);

                    // Compare the stored reminder date with the current system time.
                    DateTime reminderDate = task.ReminderDate;
                    DateTime now = DateTime.Now;

                    // Log the result.
                    if (reminderDate != now)
                    {
                        Console.WriteLine($"Discrepancy detected: ReminderDate = {reminderDate:u}, System Time = {now:u}");
                    }
                    else
                    {
                        Console.WriteLine("ReminderDate matches the current system time.");
                    }

                    // Dispose the task object.
                    task.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
