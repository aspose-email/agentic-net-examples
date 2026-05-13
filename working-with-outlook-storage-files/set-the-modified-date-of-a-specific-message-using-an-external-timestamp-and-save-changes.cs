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
            // Paths for PST file
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // External timestamp to set as Modified Date (UTC)
            DateTime externalTimestamp = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);

            // Open PST with write access
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
            {
                // Get the Inbox folder (or any predefined folder)
                FolderInfo inboxFolder;
                try
                {
                    inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get Inbox folder: {ex.Message}");
                    return;
                }

                // Find the first message in the folder
                MessageInfo targetMessageInfo = null;
                foreach (MessageInfo info in inboxFolder.EnumerateMessages())
                {
                    targetMessageInfo = info;
                    break;
                }

                if (targetMessageInfo == null)
                {
                    Console.Error.WriteLine("No messages found in the Inbox folder.");
                    return;
                }

                // Extract the message
                MapiMessage mapMessage;
                try
                {
                    mapMessage = pst.ExtractMessage(targetMessageInfo);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to extract message: {ex.Message}");
                    return;
                }

                // Set the Last Modification Time property
                try
                {
                    MapiProperty lastModProp = new MapiProperty(KnownPropertyList.LastModificationTime, externalTimestamp);
                    mapMessage.SetProperty(lastModProp);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to set Modified Date: {ex.Message}");
                    return;
                }

                // Update the message in the folder
                try
                {
                    inboxFolder.UpdateMessage(targetMessageInfo.EntryIdString, mapMessage);
                    Console.WriteLine("Modified Date updated successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to update message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
