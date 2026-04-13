using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // URI of the task to fetch (replace with a valid task URI)
                string taskUri = "tasks/12345";

                // Fetch the task from Exchange
                ExchangeTask task = null;
                try
                {
                    task = client.FetchTask(taskUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch task: {ex.Message}");
                    return;
                }

                // Path to save the task as MSG
                string outputPath = "Task.msg";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Ensure the output directory exists
                try
                {
                    string directory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Save the task preserving metadata and attachments
                    task.Save(outputPath, TaskSaveFormat.Msg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save task: {ex.Message}");
                    return;
                }
                finally
                {
                    task?.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
