using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailPstSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "sample.pst";

                // Ensure the PST file exists; create a minimal one if missing
                if (!File.Exists(pstPath))
                {
                    // Create a new Unicode PST file
                    using (PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // No additional setup required
                    }
                }

                // Open the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Subscribe to the ItemMoved event
                    pst.ItemMoved += (sender, e) =>
                    {
                        Console.WriteLine("ItemMoved event triggered.");
                        Console.WriteLine($"Destination folder: {e.DestinationFolder?.DisplayName}");
                        Console.WriteLine($"EntryId: {e.EntryId}");
                        Console.WriteLine($"IsMessage: {e.IsMessage}");
                        Console.WriteLine($"IsFolder: {e.IsFolder}");
                    };

                    // Get the root folder of the PST
                    FolderInfo rootFolder = pst.RootFolder;

                    // Create source and destination subfolders
                    FolderInfo sourceFolder = rootFolder.AddSubFolder("Source");
                    FolderInfo destinationFolder = rootFolder.AddSubFolder("Destination");

                    // Create a simple MAPI message
                    using (MapiMessage message = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Test Subject",
                        "Test body"))
                    {
                        // Add the message to the source folder
                        sourceFolder.AddMessage(message);
                    }

                    // Retrieve the MessageInfo of the newly added message
                    MessageInfo movedMessageInfo = null;
                    foreach (MessageInfo info in sourceFolder.EnumerateMessages())
                    {
                        movedMessageInfo = info;
                        break; // Only need the first message
                    }

                    // Move the message to the destination folder, triggering the event
                    if (movedMessageInfo != null)
                    {
                        pst.MoveItem(movedMessageInfo, destinationFolder);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
