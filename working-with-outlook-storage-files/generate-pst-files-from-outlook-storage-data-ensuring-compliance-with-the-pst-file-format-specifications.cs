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
            // Define PST file path
            string pstPath = "output.pst";

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory '{pstDirectory}'. {ex.Message}");
                    return;
                }
            }

            // Create the PST file (Unicode format)
            try
            {
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Create a predefined Inbox folder
                    pst.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                    FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                    // Create a simple MailMessage
                    MailMessage mail = new MailMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Sample Subject",
                        "This is a sample email body.");

                    // Convert MailMessage to MapiMessage
                    MapiMessage mapiMessage = MapiMessage.FromMailMessage(mail);

                    // Add the message to the Inbox folder
                    inboxFolder.AddMessage(mapiMessage);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Failed to create or write PST file. {ex.Message}");
                return;
            }

            Console.WriteLine("PST file created successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
