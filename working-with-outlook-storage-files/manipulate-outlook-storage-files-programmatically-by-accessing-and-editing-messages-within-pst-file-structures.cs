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

            // Ensure the PST file exists; create a minimal one if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Error creating PST file: {createEx.Message}");
                    return;
                }
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Inbox folder (creates it if it does not exist).
                FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                if (inboxFolder == null)
                {
                    Console.Error.WriteLine("Inbox folder not found.");
                    return;
                }

                // Enumerate messages in the Inbox.
                foreach (MessageInfo messageInfo in inboxFolder.EnumerateMessages())
                {
                    // Extract the full MAPI message.
                    using (MapiMessage message = pst.ExtractMessage(messageInfo))
                    {
                        // Modify the subject of the message.
                        string originalSubject = message.Subject ?? string.Empty;
                        message.Subject = "Modified: " + originalSubject;

                        // Save the changes back to the PST.
                        try
                        {
                            pst.ChangeMessage(messageInfo.EntryIdString, message.Properties);
                            Console.WriteLine($"Message subject changed from '{originalSubject}' to '{message.Subject}'.");
                        }
                        catch (Exception changeEx)
                        {
                            Console.Error.WriteLine($"Error updating message: {changeEx.Message}");
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
