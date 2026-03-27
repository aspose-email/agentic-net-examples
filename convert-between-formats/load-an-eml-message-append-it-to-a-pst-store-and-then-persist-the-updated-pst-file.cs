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
            string emlPath = "message.eml";
            string pstPath = "output.pst";

            // Verify the EML file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            // Ensure the PST file exists; create a new one if it does not
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Convert the MailMessage to a MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    // Open the PST file
                    using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                    {
                        // Get the Inbox folder (creates it if necessary)
                        FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                        // Append the message to the Inbox folder
                        inboxFolder.AddMessage(mapiMessage);
                    }
                }
            }

            // PST changes are persisted automatically; no further action required
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
