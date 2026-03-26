using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths to the source EML file and the target PST file
            string emlPath = "sample.eml";
            string pstPath = "output.pst";

            // Verify that the EML file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            // Open existing PST or create a new one if it does not exist
            using (PersonalStorage pst = File.Exists(pstPath) ?
                PersonalStorage.FromFile(pstPath) :
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                // Load the EML message
                using (MailMessage mailMessage = MailMessage.Load(emlPath))
                {
                    // Convert MailMessage to MapiMessage
                    using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                    {
                        // Get the Inbox folder (creates it if necessary)
                        FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                        // Append the message to the PST folder
                        string entryId = inboxFolder.AddMessage(mapiMessage);

                        Console.WriteLine($"Message appended to PST. EntryId: {entryId}");
                    }
                }
                // Changes are saved when the PersonalStorage object is disposed
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}