using Aspose.Email;
using System;
using System.IO;
using System.Text;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // PST created; no further action needed
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Get the Inbox folder (creates it if it does not exist)
                    FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                    if (inboxFolder == null)
                    {
                        Console.Error.WriteLine("Inbox folder not found in PST.");
                        return;
                    }

                    // Iterate over each message in the folder
                    foreach (MessageInfo messageInfo in inboxFolder.EnumerateMessages())
                    {
                        try
                        {
                            using (MapiMessage message = pst.ExtractMessage(messageInfo))
                            {
                                // Add custom MAPI property "ProcessingStatus" with value "Processed"
                                byte[] valueBytes = Encoding.Unicode.GetBytes("Processed");
                                message.AddCustomProperty(MapiPropertyType.PT_UNICODE, valueBytes, "ProcessingStatus");

                                // Update the message back into the folder
                                inboxFolder.UpdateMessage(messageInfo.EntryIdString, message);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error processing message '{messageInfo.Subject}': {ex.Message}");
                            // Continue with next message
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to open or process PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
