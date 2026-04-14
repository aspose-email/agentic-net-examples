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
            // Paths for the PST file and the folder where extracted messages will be saved
            string pstPath = "input.pst";
            string outputDir = "ExtractedMessages";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // If the PST file does not exist, create a minimal placeholder PST (Unicode format)
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Inbox folder (or any other predefined folder)
                FolderInfo inbox = pst.RootFolder.GetSubFolder("Inbox");
                if (inbox == null)
                {
                    // Fallback to the default Inbox predefined folder if custom folder not found
                    inbox = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                }

                // Enumerate all messages in the folder
                foreach (MessageInfo msgInfo in inbox.EnumerateMessages())
                {
                    try
                    {
                        // Extract the full MAPI message
                        using (MapiMessage mapiMsg = pst.ExtractMessage(msgInfo))
                        {
                            // Build a safe file name from the subject
                            string safeSubject = string.IsNullOrWhiteSpace(mapiMsg.Subject) ? "NoSubject" : mapiMsg.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                                safeSubject = safeSubject.Replace(c, '_');

                            string msgPath = Path.Combine(outputDir, $"{safeSubject}.msg");

                            // Save the message as an MSG file
                            mapiMsg.Save(msgPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing message ID {msgInfo.EntryIdString}: {ex.Message}");
                    }
                }

                // No Files collection exists on FolderInfo; therefore, nothing to clear.
                // If any temporary files were added via AddFile, they could be removed using DeleteChildItem,
                // but this example does not add such files.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
