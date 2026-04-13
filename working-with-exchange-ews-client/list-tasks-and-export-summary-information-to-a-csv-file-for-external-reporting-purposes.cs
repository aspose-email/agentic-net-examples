using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string csvPath = "tasks_report.csv";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure output directory exists
            string csvDir = Path.GetDirectoryName(csvPath);
            if (!string.IsNullOrEmpty(csvDir) && !Directory.Exists(csvDir))
            {
                Directory.CreateDirectory(csvDir);
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve tasks from the default Tasks folder
                TaskCollection tasks = client.ListTasks();

                // Export task summary to CSV
                using (StreamWriter writer = new StreamWriter(csvPath, false))
                {
                    // CSV header
                    writer.WriteLine("Subject,StartDate,DueDate,Status,PercentComplete");

                    foreach (ExchangeTask task in tasks)
                    {
                        // Format dates (use empty string if default value)
                        string startDate = task.StartDate != default(DateTime) ? task.StartDate.ToString("o") : "";
                        string dueDate = task.DueDate != default(DateTime) ? task.DueDate.ToString("o") : "";

                        // Status may be null; convert safely
                        string status = task.Status != null ? task.Status.ToString() : "";

                        // PercentComplete is a double; format as invariant string
                        string percentComplete = task.PercentComplete.ToString(System.Globalization.CultureInfo.InvariantCulture);

                        // Escape subject for CSV
                        string subject = task.Subject ?? "";
                        if (subject.Contains("\""))
                            subject = subject.Replace("\"", "\"\"");
                        if (subject.Contains(",") || subject.Contains("\""))
                            subject = $"\"{subject}\"";

                        writer.WriteLine($"{subject},{startDate},{dueDate},{status},{percentComplete}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
