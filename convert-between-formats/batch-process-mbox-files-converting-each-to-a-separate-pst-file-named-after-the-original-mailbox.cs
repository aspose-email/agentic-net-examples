using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage;

class Program
{
    static void Main(string[] args)
    {
        // Directory containing MBOX files
        string mboxDirectory = "MboxFiles";

        // Ensure the directory exists
        if (!Directory.Exists(mboxDirectory))
        {
            Console.Error.WriteLine($"Directory not found: {mboxDirectory}");
            return;
        }

        // Process each .mbox file in the directory
        foreach (string mboxPath in Directory.GetFiles(mboxDirectory, "*.mbox"))
        {
            try
            {
                // Verify the MBOX file exists
                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                    continue;
                }

                // Determine PST file name (same base name, .pst extension)
                string pstPath = Path.Combine(mboxDirectory, Path.GetFileNameWithoutExtension(mboxPath) + ".pst");

                // Create MBOX reader with required options
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                // Create PST storage
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Create a folder in PST (named after the MBOX file)
                    string pstFolderName = Path.GetFileNameWithoutExtension(mboxPath);
                    FolderInfo pstFolder = pst.RootFolder.AddSubFolder(pstFolderName);

                    // Read each message from MBOX and add to PST folder
                    MailMessage mailMessage;
                    while ((mailMessage = mboxReader.ReadNextMessage()) != null)
                    {
                        pstFolder.AddMessage(MapiMessage.FromMailMessage(mailMessage));
                    }

                    Console.WriteLine($"Converted '{mboxPath}' to '{pstPath}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing '{mboxPath}': {ex.Message}");
            }
        }
    }
}
