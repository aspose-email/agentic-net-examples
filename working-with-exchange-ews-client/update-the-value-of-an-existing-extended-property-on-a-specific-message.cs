using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define PST file path
            string pstPath = "sample.pst";

            // Ensure PST file exists; create minimal PST if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {createEx.Message}");
                    return;
                }
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Access the Inbox folder (standard predefined folder)
                FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                // Retrieve messages in the folder
                MessageInfoCollection messageInfos = inboxFolder.GetContents();

                if (messageInfos == null || messageInfos.Count == 0)
                {
                    Console.WriteLine("No messages found in the Inbox.");
                    return;
                }

                // Use the first message's entry identifier
                MessageInfo firstMessageInfo = messageInfos[0];
                string entryId = firstMessageInfo.EntryIdString;

                // Prepare updated extended property collection
                MapiPropertyCollection updatedProperties = new MapiPropertyCollection();

                // Example: update the PR_EMS_AB_SEND_EMAIL_MESSAGE property to true (value 1)
                MapiProperty updatedProperty = new MapiProperty(KnownPropertyList.EmsAbSendEmailMessage, 1);
                updatedProperties.Add(updatedProperty);

                // Apply the property update to the specific message
                try
                {
                    pst.ChangeMessage(entryId, updatedProperties);
                    Console.WriteLine("Extended property updated successfully.");
                }
                catch (Exception updateEx)
                {
                    Console.Error.WriteLine($"Failed to update extended property: {updateEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
