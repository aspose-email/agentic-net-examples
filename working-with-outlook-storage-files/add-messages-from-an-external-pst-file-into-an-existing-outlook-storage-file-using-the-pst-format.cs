using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Paths to the source PST (external) and the destination PST (existing Outlook storage)
            string sourcePstPath = "source.pst";
            string destinationPstPath = "destination.pst";

            // Verify that the source PST file exists
            if (!File.Exists(sourcePstPath))
            {
                Console.Error.WriteLine($"Error: Source PST file not found – {sourcePstPath}");
                return;
            }

            // Ensure the destination PST file exists; create a new one if it does not
            PersonalStorage destinationPst;
            if (File.Exists(destinationPstPath))
            {
                // Open existing PST with write access
                destinationPst = PersonalStorage.FromFile(destinationPstPath, true);
            }
            else
            {
                // Create a new Unicode PST file
                destinationPst = PersonalStorage.Create(destinationPstPath, FileFormatVersion.Unicode);
            }

            // Open the source PST for reading
            using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePstPath))
            using (destinationPst)
            {
                // Create (or get) a folder in the destination PST where messages will be added
                FolderInfo importedFolder = destinationPst.RootFolder.AddSubFolder("Imported");

                // Iterate over all messages in the root folder of the source PST
                foreach (MessageInfo sourceMessageInfo in sourcePst.RootFolder.EnumerateMessages())
                {
                    // Extract the full MAPI message from the source PST
                    using (MapiMessage sourceMessage = sourcePst.ExtractMessage(sourceMessageInfo))
                    {
                        // Add the extracted message to the destination PST folder
                        importedFolder.AddMessage(sourceMessage);
                    }
                }

                Console.WriteLine("Messages have been successfully added to the destination PST.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
