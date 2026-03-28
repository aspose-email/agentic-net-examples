using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Paths and server credentials (replace with real values as needed)
            const string pstPath = "backup.pst";
            const string imapHost = "imap.example.com";
            const int imapPort = 993;
            const string imapUser = "user@example.com";
            const string imapPassword = "password";

            // Verify that the PST file exists before proceeding
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Load the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Initialize the IMAP client
                using (ImapClient client = new ImapClient(imapHost, imapPort, imapUser, imapPassword, SecurityOptions.Auto))
                {
                    try
                    {
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to connect to IMAP server: {ex.Message}");
                        return;
                    }

                    // Prepare restore settings
                    RestoreSettings restoreSettings = new RestoreSettings();
                    restoreSettings.Options = RestoreOptions.Recursive |
                                              RestoreOptions.RemoveNonexistentFolders |
                                              RestoreOptions.RemoveNonexistentItems;

                    // Perform the restore operation
                    try
                    {
                        client.Restore(pst, restoreSettings);
                        Console.WriteLine("Mailbox restored successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Restore operation failed: {ex.Message}");
                    }

                    // Optional: list top‑level folders after restore
                    try
                    {
                        ImapFolderInfoCollection folders = client.ListFolders();
                        Console.WriteLine("Folders on the server:");
                        foreach (ImapFolderInfo folder in folders)
                        {
                            Console.WriteLine($"- {folder.Name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list folders: {ex.Message}");
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
