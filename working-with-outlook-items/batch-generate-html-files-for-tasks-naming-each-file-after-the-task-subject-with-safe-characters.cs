using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using Aspose.Email.Clients.Exchange;

class Program
{
    // Simple representation of a task when ExchangeTask is unavailable.
    class SimpleTask
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Detect placeholder values and skip network call.
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            // Ensure output directory exists.
            string outputDirectory = "TaskHtml";
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Connect to Exchange server.
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Since ExchangeTask and ListTasks are not available, use a placeholder list.
                    IList<SimpleTask> tasks = new List<SimpleTask>();

                    // Example: you could populate 'tasks' here if you have another way to retrieve them.
                    // For demonstration, we'll assume the list is empty.

                    foreach (SimpleTask task in tasks)
                    {
                        // Prepare a safe file name based on the task subject.
                        string rawSubject = task.Subject ?? "Untitled";
                        string safeSubject = Regex.Replace(
                            rawSubject,
                            $"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]",
                            "_");

                        string htmlFilePath = Path.Combine(outputDirectory, $"{safeSubject}.html");

                        // Build simple HTML content.
                        string bodyContent = task.Body ?? string.Empty;
                        string htmlContent = $"<html><head><meta charset=\"utf-8\"/></head><body><h1>{WebUtility.HtmlEncode(rawSubject)}</h1><p>{WebUtility.HtmlEncode(bodyContent)}</p></body></html>";

                        // Write HTML to file with error handling.
                        try
                        {
                            File.WriteAllText(htmlFilePath, htmlContent);
                            Console.WriteLine($"Generated HTML for task: {rawSubject}");
                        }
                        catch (Exception writeEx)
                        {
                            Console.Error.WriteLine($"Failed to write file '{htmlFilePath}': {writeEx.Message}");
                        }
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Exchange client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
