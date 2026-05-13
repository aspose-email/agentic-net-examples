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
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open PST with write access
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
            {
                // Get the Inbox folder (standard predefined folder)
                FolderInfo inbox;
                try
                {
                    inbox = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve Inbox folder: {ex.Message}");
                    return;
                }

                // Iterate through messages and update read/unread flag based on custom criteria
                foreach (MessageInfo msgInfo in inbox.EnumerateMessages())
                {
                    // Example criterion: subject contains the word "Important"
                    if (!string.IsNullOrEmpty(msgInfo.Subject) && msgInfo.Subject.Contains("Important"))
                    {
                        MapiMessage message;
                        try
                        {
                            message = pst.ExtractMessage(msgInfo);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to extract message (EntryId: {msgInfo.EntryIdString}): {ex.Message}");
                            continue;
                        }

                        // Mark as read (set MSGFLAG_READ) – modify as needed for unread
                        MapiMessageFlags newFlags = message.Flags | MapiMessageFlags.MSGFLAG_READ;
                        message.SetMessageFlags(newFlags);

                        // Update the message back into the folder
                        try
                        {
                            inbox.UpdateMessage(msgInfo.EntryIdString, message);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to update message (EntryId: {msgInfo.EntryIdString}): {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
