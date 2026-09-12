using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths – adjust as needed
            string sourceFolderPath = "SourceEmails";
            string pstFilePath = "Archive.pst";
            string pstTargetFolderName = "Imported";

            // Verify source folder exists
            if (!Directory.Exists(sourceFolderPath))
            {
                Console.Error.WriteLine($"Source folder not found: {sourceFolderPath}");
                return;
            }

            // Ensure PST file exists; create if missing
            if (!File.Exists(pstFilePath))
            {
                try
                {
                    // Create a new PST with Unicode format
                    PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST storage
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Get or create the target folder inside the PST
                FolderInfo targetFolder = pst.RootFolder.GetSubFolder(pstTargetFolderName);
                if (targetFolder == null)
                {
                    targetFolder = pst.RootFolder.AddSubFolder(pstTargetFolderName);
                }

                // Enumerate email files in the source folder (e.g., .eml files)
                string[] emailFiles = Directory.GetFiles(sourceFolderPath, "*.eml");
                foreach (string emailFilePath in emailFiles)
                {
                    try
                    {
                        // Load the email message from file
                        MailMessage message = MailMessage.Load(emailFilePath);

                        // Add the message to the PST folder
                        targetFolder.AddMessage(MapiMessage.FromMailMessage(message));
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process '{emailFilePath}': {ex.Message}");
                        // Continue with next file
                    }
                }

                Console.WriteLine($"Inserted {emailFiles.Length} messages into PST folder '{pstTargetFolderName}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
