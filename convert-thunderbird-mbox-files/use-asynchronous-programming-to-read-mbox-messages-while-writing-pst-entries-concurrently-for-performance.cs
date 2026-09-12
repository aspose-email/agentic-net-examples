using Aspose.Email.Mapi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;

namespace MboxToPstAsync
{
    class Program
    {
        // Author note: This sample demonstrates asynchronous reading of MBOX messages
        // and concurrent writing to a PST file using Aspose.Email for .NET.
        static async Task Main(string[] args)
        {
            try
            {
                // Input and output file paths.
                string mboxFilePath = "input.mbox";
                string pstFilePath = "output.pst";

                // Guard file system access.
                if (!File.Exists(mboxFilePath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxFilePath}");
                    return;
                }

                // Ensure the directory for the PST file exists.
                string pstDirectory = Path.GetDirectoryName(pstFilePath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    Directory.CreateDirectory(pstDirectory);
                }

                // Create PST storage.
                using (PersonalStorage pstStorage = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                {
                    // Create (or get) a folder inside the PST where messages will be stored.
                    FolderInfo pstFolder = pstStorage.RootFolder.AddSubFolder("Imported");

                    // Open the MBOX reader.
                    using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
                    {
                        List<Task> writeTasks = new List<Task>();

                        // Read messages sequentially; each read message is handed off to a background task.
                        while (true)
                        {
                            MailMessage message = mboxReader.ReadNextMessage();
                            if (message == null)
                                break;

                            // Capture the current message for the task closure.
                            MailMessage capturedMessage = message;

                            // Queue a task that writes the message into the PST folder.
                            Task writeTask = Task.Run(() =>
                            {
                                // PST folder operations are not thread‑safe; protect with a lock.
                                lock (pstFolder)
                                {
                                    pstFolder.AddMessage(MapiMessage.FromMailMessage(capturedMessage));
                                }
                            });

                            writeTasks.Add(writeTask);
                        }

                        // Await completion of all write operations.
                        await Task.WhenAll(writeTasks);
                    }
                }

                Console.WriteLine("MBOX to PST conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
