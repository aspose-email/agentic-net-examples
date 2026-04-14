using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal placeholder if missing
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

            // Open PST with write access
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
            {
                // Get the Inbox folder (creates if not present)
                FolderInfo inboxFolder;
                try
                {
                    inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to obtain Inbox folder: {ex.Message}");
                    return;
                }

                // Iterate through each message in the folder
                foreach (MessageInfo messageInfo in inboxFolder.EnumerateMessages())
                {
                    using (MapiMessage message = pst.ExtractMessage(messageInfo))
                    {
                        // Add custom MAPI property to track processing status
                        const string propertyName = "ProcessingStatus";
                        const string propertyValue = "Processed";
                        byte[] valueBytes = Encoding.Unicode.GetBytes(propertyValue);
                        message.AddCustomProperty(MapiPropertyType.PT_UNICODE, valueBytes, propertyName);

                        // Persist the updated properties back to the PST
                        try
                        {
                            pst.ChangeMessage(messageInfo.EntryIdString, message.Properties);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to update message '{messageInfo.Subject}': {ex.Message}");
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
