using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Mapi;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Guard input file
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Create PST file (Unicode version)
            PersonalStorage pst = await PersonalStorage.CreateAsync(pstPath, FileFormatVersion.Unicode);
            using (pst)
            {
                // Create standard Inbox folder
                pst.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);

                // Open MBOX reader
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    List<Task> processingTasks = new List<Task>();

                    while (true)
                    {
                        MailMessage mailMessage = mboxReader.ReadNextMessage();
                        if (mailMessage == null)
                            break;

                        // Capture current message for the task
                        MailMessage currentMessage = mailMessage;

                        Task task = Task.Run(() =>
                        {
                            using (currentMessage)
                            {
                                // Convert to MAPI message
                                MapiMessage mapiMessage = MapiMessage.FromMailMessage(currentMessage);
                                using (mapiMessage)
                                {
                                    // Add to Inbox folder
                                    FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                                    inboxFolder.AddMessage(mapiMessage);
                                }
                            }
                        });

                        processingTasks.Add(task);
                    }

                    await Task.WhenAll(processingTasks);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
