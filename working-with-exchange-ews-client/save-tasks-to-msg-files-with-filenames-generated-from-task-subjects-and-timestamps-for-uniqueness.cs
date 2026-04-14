using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare output directory
            string outputDirectory = "TasksOutput";
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Initialize EWS client
            IEWSClient client = null;
            try
            {
                // Replace with actual server URL and credentials
                string ewsUrl = "https://exchange.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (ewsUrl.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                NetworkCredential credentials = new NetworkCredential("username", "password");
                client = EWSClient.GetEWSClient(ewsUrl, credentials);
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {clientEx.Message}");
                return;
            }

            // Use the client to list tasks
            try
            {
                using (client)
                {
                    TaskCollection tasks = client.ListTasks();
                    foreach (ExchangeTask task in tasks)
                    {
                        // Generate a safe filename based on subject and timestamp
                        string rawSubject = task.Subject ?? "Untitled";
                        char[] invalidChars = Path.GetInvalidFileNameChars();
                        foreach (char c in invalidChars)
                        {
                            rawSubject = rawSubject.Replace(c, '_');
                        }
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        string fileName = $"{rawSubject}_{timestamp}.msg";
                        string filePath = Path.Combine(outputDirectory, fileName);

                        // Save the task to MSG format
                        try
                        {
                            task.Save(filePath, TaskSaveFormat.Msg);
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save task '{task.Subject}': {saveEx.Message}");
                        }
                    }
                }
            }
            catch (Exception opEx)
            {
                Console.Error.WriteLine($"Error during task processing: {opEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
