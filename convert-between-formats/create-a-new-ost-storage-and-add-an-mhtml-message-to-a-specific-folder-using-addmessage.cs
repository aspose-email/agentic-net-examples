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
            string ostPath = "sample.ost";
            string mhtmlPath = "sample.mht";

            // Ensure the MHTML file exists; create a minimal placeholder if missing
            if (!File.Exists(mhtmlPath))
            {
                try
                {
                    File.WriteAllText(mhtmlPath, "<html><body><p>Placeholder MHTML content</p></body></html>");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MHTML file: {ex.Message}");
                    return;
                }
            }

            // Create a new OST (PST) file if it does not exist
            if (!File.Exists(ostPath))
            {
                try
                {
                    PersonalStorage.Create(ostPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create OST file: {ex.Message}");
                    return;
                }
            }

            // Open the OST file
            using (PersonalStorage pst = PersonalStorage.FromFile(ostPath))
            {
                // Get the Inbox predefined folder
                FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                // Load the MHTML message into a MailMessage
                using (FileStream mhtmlStream = File.OpenRead(mhtmlPath))
                using (MailMessage mailMessage = MailMessage.Load(mhtmlStream))
                {
                    // Convert MailMessage to MapiMessage
                    using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                    {
                        // Add the MAPI message to the folder
                        string entryId = inboxFolder.AddMessage(mapiMessage);
                        Console.WriteLine($"Message added successfully. EntryId: {entryId}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
