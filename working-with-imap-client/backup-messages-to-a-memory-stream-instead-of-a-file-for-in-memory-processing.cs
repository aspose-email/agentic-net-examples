using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder hosts
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder IMAP host detected. Skipping backup.");
                return;
            }

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.Auto))
            {
                client.Username = username;
                client.Password = password;

                // Validate credentials by selecting a folder (lightweight operation)
                try
                {
                    client.SelectFolder("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect or authenticate: {ex.Message}");
                    return;
                }

                // Retrieve all folders to back up
                ImapFolderInfoCollection folders = client.ListFolders();

                // Backup to an in‑memory stream
                using (MemoryStream backupStream = new MemoryStream())
                {
                    try
                    {
                        BackupSettings settings = new BackupSettings();
                        client.Backup(folders, backupStream, settings);

                        // Reset stream position for further processing
                        backupStream.Position = 0;
                        Console.WriteLine($"Backup completed. Stream length: {backupStream.Length} bytes.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Backup failed: {ex.Message}");
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
