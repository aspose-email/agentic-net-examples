using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Calendar;
using EmailTask = Aspose.Email.Calendar.Task;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and server URI
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials
            if (mailboxUri.Contains("example.com") ||
                username.Contains("example.com") ||
                password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Output CSV path
            string outputPath = "contacts_tasks.csv";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Connect to Exchange server
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Retrieve contacts from the default Contacts folder
                Contact[] contacts = client.GetContacts("contacts");

                // Retrieve tasks – using an empty array if the client does not expose a task method
                EmailTask[] tasks = Array.Empty<EmailTask>();

                // Write CSV
                using (StreamWriter writer = new StreamWriter(outputPath, false))
                {
                    // Header
                    writer.WriteLine("ContactName,ContactEmail,TaskSubject,TaskStatus,TaskDueDate");

                    // Write contacts (task columns left empty)
                    if (contacts != null)
                    {
                        foreach (Contact contact in contacts)
                        {
                            string contactName = contact.DisplayName ?? string.Empty;
                            string contactEmail = string.Empty;
                            if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                            {
                                contactEmail = contact.EmailAddresses[0].Address ?? string.Empty;
                            }

                            writer.WriteLine($"{Escape(contactName)},{Escape(contactEmail)},,,");
                        }
                    }

                    // Write tasks (contact columns left empty)
                    if (tasks != null)
                    {
                        foreach (EmailTask task in tasks)
                        {
                            string taskSubject = task.Subject ?? string.Empty;
                            string taskStatus = task.Status != null ? task.Status.ToString() : string.Empty;
                            string taskDueDate = task.DueDate != DateTime.MinValue ? task.DueDate.ToString("o") : string.Empty;

                            writer.WriteLine($",,{Escape(taskSubject)},{Escape(taskStatus)},{Escape(taskDueDate)}");
                        }
                    }
                }

                Console.WriteLine($"Export completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper to escape CSV fields
    private static string Escape(string field)
    {
        if (string.IsNullOrEmpty(field)) return string.Empty;
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            field = field.Replace("\"", "\"\"");
            return $"\"{field}\"";
        }
        return field;
    }
}
