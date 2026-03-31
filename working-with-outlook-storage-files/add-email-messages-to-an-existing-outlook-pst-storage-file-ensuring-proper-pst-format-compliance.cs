using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal placeholder if missing
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

            // Open the PST file for read/write
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                if (!pst.CanWrite)
                {
                    Console.Error.WriteLine("PST file is read‑only. Cannot add messages.");
                    return;
                }

                // Get or create the Inbox folder
                FolderInfo inbox;
                try
                {
                    inbox = pst.RootFolder.GetSubFolder("Inbox");
                }
                catch
                {
                    inbox = null;
                }

                if (inbox == null)
                {
                    try
                    {
                        inbox = pst.RootFolder.AddSubFolder("Inbox");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating Inbox folder: {ex.Message}");
                        return;
                    }
                }

                // Create a simple email message in memory
                MailMessage mail = new MailMessage(
                    new MailAddress("sender@example.com", "Sender"),
                    new MailAddress("receiver@example.com", "Receiver"));
                mail.Subject = "Test Subject";
                mail.Body = "This is a test email added to PST.";

                // Convert MailMessage to MapiMessage
                MapiMessage mapiMessage = MapiMessage.FromMailMessage(mail);

                // Add the message to the Inbox folder
                string entryId;
                try
                {
                    entryId = inbox.AddMessage(mapiMessage);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error adding message to PST: {ex.Message}");
                    return;
                }

                Console.WriteLine($"Message added successfully. EntryId: {entryId}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
