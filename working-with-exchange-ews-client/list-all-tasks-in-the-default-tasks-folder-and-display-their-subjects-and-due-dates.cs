using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define EWS service URL and credentials (replace with actual values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string userName = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || userName == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(userName, password);

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Retrieve tasks from the default Tasks folder
                TaskCollection tasks = client.ListTasks();

                // Display subject and due date for each task
                foreach (Aspose.Email.Calendar.Task task in tasks)
                {
                    Console.WriteLine("Subject: " + task.Subject);
                    string dueDateText = task.DueDate != DateTime.MinValue
                        ? task.DueDate.ToString("yyyy-MM-dd")
                        : "N/A";
                    Console.WriteLine("Due Date: " + dueDateText);
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
