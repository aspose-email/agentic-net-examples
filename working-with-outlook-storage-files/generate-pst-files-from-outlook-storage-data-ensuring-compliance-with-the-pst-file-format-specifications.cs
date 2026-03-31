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
            string outputPstPath = "output.pst";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPstPath));
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Delete existing PST file if present
            if (File.Exists(outputPstPath))
            {
                try
                {
                    File.Delete(outputPstPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting existing PST: {ex.Message}");
                    return;
                }
            }

            // Create a new Unicode PST file
            using (PersonalStorage pst = PersonalStorage.Create(outputPstPath, FileFormatVersion.Unicode))
            {
                // Create a simple email message
                using (MailMessage mail = new MailMessage("sender@example.com", "recipient@example.com", "Sample Subject", "This is a sample email body."))
                {
                    // Convert MailMessage to MapiMessage
                    using (MapiMessage mapiMsg = MapiMessage.FromMailMessage(mail))
                    {
                        // Add the message to the root folder
                        string entryId = pst.RootFolder.AddMessage(mapiMsg);
                        Console.WriteLine($"Message added with EntryId: {entryId}");
                    }
                }

                // Display total items count in the PST
                int totalItems = pst.Store.GetTotalItemsCount();
                Console.WriteLine($"Total items in PST: {totalItems}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
