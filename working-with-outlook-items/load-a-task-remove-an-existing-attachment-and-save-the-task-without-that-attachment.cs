using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip actual network call in CI
            string ewsUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";
            string domain = "";

            if (ewsUrl.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, username, password, domain))
            {
                // Task URI to load – replace with a real URI when not using placeholders
                string taskUri = "taskUriPlaceholder";

                if (string.IsNullOrEmpty(taskUri))
                {
                    Console.Error.WriteLine("Task URI is not provided.");
                    return;
                }

                // Load the task from Exchange
                ExchangeTask task = client.FetchTask(taskUri);
                if (task == null)
                {
                    Console.Error.WriteLine("Failed to fetch the task.");
                    return;
                }

                // Remove the first attachment if any exist
                if (task.Attachments != null && task.Attachments.Count > 0)
                {
                    // Assuming Attachments implements IList-like collection
                    task.Attachments.RemoveAt(0);
                }

                // Define output file path
                string outputPath = "outputTask.msg";

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified task without the removed attachment
                task.Save(outputPath, TaskSaveFormat.Msg);
                Console.WriteLine($"Task saved without attachment to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
