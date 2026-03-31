using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the folder that contains .eml files to be inserted
            string sourceFolderPath = "Emails";

            // Path to the existing PST file
            string pstFilePath = "output.pst";

            // Verify the source folder exists
            if (!Directory.Exists(sourceFolderPath))
            {
                Console.Error.WriteLine($"Error: Source folder not found – {sourceFolderPath}");
                return;
            }

            // Ensure the PST file exists; create a minimal Unicode PST if it does not
            if (!File.Exists(pstFilePath))
            {
                try
                {
                    PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created new PST file at {pstFilePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                if (!pst.CanWrite)
                {
                    Console.Error.WriteLine("Error: PST file is read‑only.");
                    return;
                }

                // Get the Inbox folder; create it if it does not exist
                FolderInfo inboxFolder;
                try
                {
                    inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                }
                catch (Exception)
                {
                    inboxFolder = pst.RootFolder.AddSubFolder("Inbox");
                }

                // Process each .eml file in the source folder
                string[] emlFiles = Directory.GetFiles(sourceFolderPath, "*.eml");
                foreach (string emlFilePath in emlFiles)
                {
                    try
                    {
                        // Load the email message from the .eml file
                        using (MailMessage mailMessage = MailMessage.Load(emlFilePath))
                        {
                            // Convert to a MAPI message suitable for PST storage
                            using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                            {
                                // Add the message to the Inbox folder
                                string entryId = inboxFolder.AddMessage(mapiMessage);
                                Console.WriteLine($"Added message '{mailMessage.Subject}' with EntryId {entryId}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add '{emlFilePath}': {ex.Message}");
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
