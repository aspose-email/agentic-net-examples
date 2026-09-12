using System;
using System.IO;
using System.IO.Compression;
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
            // Define mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Retrieve all tasks from the default task folder
                TaskCollection tasks = client.ListTasks();

                if (tasks == null || tasks.Count == 0)
                {
                    Console.WriteLine("No tasks found.");
                    return;
                }

                // Path for the output ZIP archive
                string zipPath = "ExportedTasks.zip";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Ensure the directory for the ZIP file exists
                string zipDirectory = Path.GetDirectoryName(Path.GetFullPath(zipPath));
                if (!Directory.Exists(zipDirectory))
                {
                    Directory.CreateDirectory(zipDirectory);
                }

                // Create the ZIP archive and add tasks with their attachments
                using (FileStream zipFile = new FileStream(zipPath, FileMode.Create))
                using (ZipArchive archive = new ZipArchive(zipFile, ZipArchiveMode.Update))
                {
                    foreach (Aspose.Email.Calendar.Task task in tasks)
                    {
                        // Save the task as a MSG file inside the ZIP
                        string taskEntryName = $"Task_{task.UniqueId}.msg";
                        using (MemoryStream taskStream = new MemoryStream())
                        {
                            task.Save(taskStream, TaskSaveFormat.Msg);
                            taskStream.Position = 0;
                            ZipArchiveEntry taskEntry = archive.CreateEntry(taskEntryName);
                            using (Stream entryStream = taskEntry.Open())
                            {
                                taskStream.CopyTo(entryStream);
                            }
                        }

                        // Export each attachment belonging to the task
                        foreach (Attachment attachment in task.Attachments)
                        {
                            string attachmentEntryName = $"Task_{task.UniqueId}_{attachment.Name}";
                            ZipArchiveEntry attachmentEntry = archive.CreateEntry(attachmentEntryName);
                            using (Stream entryStream = attachmentEntry.Open())
                            {
                                using (Stream attachmentStream = attachment.ContentStream)
                                {
                                    attachmentStream.CopyTo(entryStream);
                                }
                            }
                        }
                    }
                }

                Console.WriteLine($"Export completed: {zipPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
