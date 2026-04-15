using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the custom file to attach
            string attachmentPath = "customfile.txt";

            // Ensure the attachment file exists; create a minimal placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Placeholder content");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ioEx.Message}");
                    return;
                }
            }

            // Create and connect the EWS client
            IEWSClient client;
            try
            {
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception connEx)
            {
                Console.Error.WriteLine($"Failed to connect to Exchange: {connEx.Message}");
                return;
            }

            using (client)
            {
                // Prepare the task
                ExchangeTask task = new ExchangeTask();
                task.Subject = "Project Update";
                task.Body = "Please review the attached document.";
                task.StartDate = DateTime.Now;
                task.DueDate = DateTime.Now.AddDays(7);

                // Attach the custom file to the task
                try
                {
                    task.Attachments.Add(new Attachment(attachmentPath));
                }
                catch (Exception attEx)
                {
                    Console.Error.WriteLine($"Failed to add attachment: {attEx.Message}");
                    return;
                }

                // Create the task on the Exchange server
                try
                {
                    string taskUri = client.CreateTask(task);
                    Console.WriteLine($"Aspose.Email.Calendar.Task created with URI: {taskUri}");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create task: {createEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
