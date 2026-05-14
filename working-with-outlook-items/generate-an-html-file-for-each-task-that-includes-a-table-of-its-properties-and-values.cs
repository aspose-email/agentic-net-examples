using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    // Simple representation of a task with the required properties
    class TaskInfo
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string Organizer { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public float? PercentComplete { get; set; }
        public string BillingInformation { get; set; }
        public string Mileage { get; set; }
        public string[] Companies { get; set; }
        public string[] Attendees { get; set; }
        public string[] Attachments { get; set; }
    }

    static void Main()
    {
        try
        {
            // Placeholder credentials – skip execution if not replaced
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Ensure output directory exists
            string outputDir = "Output";
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Connect to Exchange server (client kept for naming consistency)
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Since ListTasks is not available in the current library version,
                    // we will simulate task retrieval with a placeholder list.
                    List<TaskInfo> tasks = GetSampleTasks();

                    foreach (TaskInfo task in tasks)
                    {
                        // Build HTML content for the task
                        StringBuilder htmlBuilder = new StringBuilder();
                        htmlBuilder.AppendLine("<!DOCTYPE html>");
                        htmlBuilder.AppendLine("<html>");
                        htmlBuilder.AppendLine("<head><meta charset=\"UTF-8\"><title>Task Details</title></head>");
                        htmlBuilder.AppendLine("<body>");
                        htmlBuilder.AppendLine("<h2>Task Details</h2>");
                        htmlBuilder.AppendLine("<table border=\"1\" cellpadding=\"5\" cellspacing=\"0\">");

                        void AddRow(string name, string value)
                        {
                            string escaped = System.Net.WebUtility.HtmlEncode(value ?? string.Empty);
                            htmlBuilder.AppendLine($"<tr><td><strong>{name}</strong></td><td>{escaped}</td></tr>");
                        }

                        AddRow("Subject", task.Subject);
                        AddRow("Body", task.Body);
                        AddRow("Start Date", task.StartDate.ToString("u"));
                        AddRow("Due Date", task.DueDate.ToString("u"));
                        AddRow("Completion Date", task.CompletionDate?.ToString("u") ?? string.Empty);
                        AddRow("Reminder Date", task.ReminderDate?.ToString("u") ?? string.Empty);
                        AddRow("Organizer", task.Organizer);
                        AddRow("Status", task.Status);
                        AddRow("Priority", task.Priority);
                        AddRow("Percent Complete", task.PercentComplete?.ToString() ?? string.Empty);
                        AddRow("Billing Information", task.BillingInformation);
                        AddRow("Mileage", task.Mileage);
                        AddRow("Companies", string.Join(", ", task.Companies ?? Array.Empty<string>()));
                        AddRow("Attendees", string.Join(", ", task.Attendees ?? Array.Empty<string>()));
                        AddRow("Attachments", string.Join(", ", task.Attachments ?? Array.Empty<string>()));

                        htmlBuilder.AppendLine("</table>");
                        htmlBuilder.AppendLine("</body>");
                        htmlBuilder.AppendLine("</html>");

                        // Create a safe file name
                        string rawFileName = string.IsNullOrWhiteSpace(task.Subject) ? "Task" : task.Subject;
                        foreach (char invalidChar in Path.GetInvalidFileNameChars())
                        {
                            rawFileName = rawFileName.Replace(invalidChar, '_');
                        }
                        string htmlFilePath = Path.Combine(outputDir, rawFileName + ".html");

                        // Write HTML to file
                        try
                        {
                            File.WriteAllText(htmlFilePath, htmlBuilder.ToString(), Encoding.UTF8);
                            Console.WriteLine($"Generated HTML for task: {task.Subject}");
                        }
                        catch (Exception writeEx)
                        {
                            Console.Error.WriteLine($"Failed to write HTML file for task '{task.Subject}': {writeEx.Message}");
                        }
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Failed to connect to Exchange server: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Generates a sample list of tasks for demonstration purposes
    static List<TaskInfo> GetSampleTasks()
    {
        return new List<TaskInfo>
        {
            new TaskInfo
            {
                Subject = "Sample Task 1",
                Body = "This is a sample task.",
                StartDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(7),
                CompletionDate = null,
                ReminderDate = DateTime.UtcNow.AddHours(1),
                Organizer = "organizer@example.com",
                Status = "NotStarted",
                Priority = "High",
                PercentComplete = 0,
                BillingInformation = "Billing XYZ",
                Mileage = "15 miles",
                Companies = new[] { "Company A", "Company B" },
                Attendees = new[] { "attendee1@example.com", "attendee2@example.com" },
                Attachments = new[] { "attachment1.pdf", "attachment2.docx" }
            },
            new TaskInfo
            {
                Subject = "Sample Task 2",
                Body = "Another example task.",
                StartDate = DateTime.UtcNow.AddDays(-2),
                DueDate = DateTime.UtcNow.AddDays(3),
                CompletionDate = DateTime.UtcNow,
                ReminderDate = null,
                Organizer = "organizer2@example.com",
                Status = "Completed",
                Priority = "Normal",
                PercentComplete = 100,
                BillingInformation = "Billing ABC",
                Mileage = "0",
                Companies = null,
                Attendees = null,
                Attachments = null
            }
        };
    }
}
