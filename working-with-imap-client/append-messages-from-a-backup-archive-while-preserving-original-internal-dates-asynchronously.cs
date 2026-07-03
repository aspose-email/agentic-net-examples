using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        // Placeholder IMAP connection settings
        string host = "imap.example.com";
        int port = 993;
        string username = "user@example.com";
        string password = "password";

        // Skip execution if placeholder credentials are detected
        if (host.Contains("example.com") || username.Contains("example.com"))
        {
            Console.Error.WriteLine("Placeholder IMAP credentials detected. Skipping execution.");
            return;
        }

        // Path to the PST backup file
        string backupPath = "backup.pst";

        // Ensure a PST file exists (create a minimal placeholder if missing)
        if (!File.Exists(backupPath))
        {
            try
            {
                using (PersonalStorage.Create(backupPath, FileFormatVersion.Unicode)) { }
                Console.WriteLine($"Created placeholder PST file at '{backupPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create placeholder PST file: {ex.Message}");
                return;
            }
        }

        // Load the PST file
        PersonalStorage pst;
        try
        {
            pst = PersonalStorage.FromFile(backupPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load PST file: {ex.Message}");
            return;
        }

        // Open IMAP client and connect
        using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
        {
            client.Port = port;

            // Target IMAP folder (INBOX by default)
            string targetFolder = "INBOX";

            // Ensure the target folder exists
            try
            {
                if (!client.ExistFolder(targetFolder))
                {
                    client.CreateFolder(targetFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to verify/create folder '{targetFolder}': {ex.Message}");
                return;
            }

            // Iterate over all messages in the PST root folder
            foreach (MessageInfo info in pst.RootFolder.GetContents())
            {
                // Extract the MAPI message from the PST
                MapiMessage mapiMsg;
                try
                {
                    mapiMsg = pst.ExtractMessage(info);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to extract message (EntryId: {info.EntryIdString}): {ex.Message}");
                    continue;
                }

                // Convert to MailMessage
                MailMessage mailMsg;
                try
                {
                    mailMsg = mapiMsg.ToMailMessage(new MailConversionOptions());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Conversion to MailMessage failed: {ex.Message}");
                    continue;
                }

                // Append the message to the IMAP folder
                try
                {
                    client.AppendMessage(targetFolder, mailMsg);
                    Console.WriteLine($"Appended message: {mailMsg.Subject}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to append message '{mailMsg.Subject}': {ex.Message}");
                }
            }

            // Disconnect gracefully
            try
            {
                client.Dispose();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during IMAP disconnect: {ex.Message}");
            }
        }
    }
}
