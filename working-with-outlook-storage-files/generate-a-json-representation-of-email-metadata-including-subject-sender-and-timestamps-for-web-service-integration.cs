using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure a PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                CreateSamplePst(pstPath);
            }

            // List to hold metadata objects
            List<object> messagesMetadata = new List<object>();

            // Open PST file with proper disposal
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate through all subfolders recursively
                Queue<FolderInfo> foldersQueue = new Queue<FolderInfo>();
                foldersQueue.Enqueue(pst.RootFolder);

                while (foldersQueue.Count > 0)
                {
                    FolderInfo currentFolder = foldersQueue.Dequeue();

                    // Enqueue subfolders
                    foreach (FolderInfo subFolder in currentFolder.GetSubFolders())
                    {
                        foldersQueue.Enqueue(subFolder);
                    }

                    // Process messages in the current folder
                    foreach (MessageInfo messageInfo in currentFolder.EnumerateMessages())
                    {
                        try
                        {
                            MapiMessage mapiMessage = pst.ExtractMessage(messageInfo);

                            var metadata = new
                            {
                                Subject = mapiMessage.Subject,
                                Sender = mapiMessage.SenderEmailAddress,
                                DeliveryTime = mapiMessage.DeliveryTime,
                                ClientSubmitTime = mapiMessage.ClientSubmitTime
                            };

                            messagesMetadata.Add(metadata);
                        }
                        catch (Exception exMessage)
                        {
                            Console.Error.WriteLine($"Failed to extract message: {exMessage.Message}");
                        }
                    }
                }
            }

            // Serialize metadata list to JSON
            JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            string jsonOutput = JsonSerializer.Serialize(messagesMetadata, jsonOptions);
            Console.WriteLine(jsonOutput);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void CreateSamplePst(string path)
    {
        // Create a new PST file
        using (PersonalStorage pst = PersonalStorage.Create(path, FileFormatVersion.Unicode))
        {
            // Add a folder
            FolderInfo inbox = pst.RootFolder.AddSubFolder("Inbox");

            // Create a simple MAPI message
            MapiMessage message = new MapiMessage("sender@example.com", "recipient@example.com", "Sample Subject", "This is a sample email body.");
            message.DeliveryTime = DateTime.Now;
            message.ClientSubmitTime = DateTime.Now;

            // Add the message to the folder
            inbox.AddMessage(message);
        }
    }
}
