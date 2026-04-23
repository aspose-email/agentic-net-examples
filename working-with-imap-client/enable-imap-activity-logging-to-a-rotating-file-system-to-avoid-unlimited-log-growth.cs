using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email.Clients.Imap;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // IMAP server configuration (placeholder values)
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder IMAP configuration detected. Skipping connection.");
                return;
            }

            // Prepare a directory for rotating log files
            string logDirectory = Path.Combine(Environment.CurrentDirectory, "ImapLogs");
            try
            {
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to prepare log directory: {ioEx.Message}");
                return;
            }

            // Log file path (date will be appended automatically)
            string logFilePath = Path.Combine(logDirectory, "imap.log");

            // Create and configure the IMAP client
            try
            {
                using (ImapClient client = new ImapClient(host, username, password))
                {
                    // Enable activity logging with daily rotation
                    client.EnableLogger = true;
                    client.LogFileName = logFilePath;
                    client.UseDateInLogFileName = true;

                    // Simple operation to verify the connection
                    try
                    {
                        client.SelectFolder("INBOX");
                        Console.WriteLine("Connected to IMAP server and selected INBOX.");
                    }
                    catch (ImapException imapEx)
                    {
                        Console.Error.WriteLine($"IMAP operation failed: {imapEx.Message}");
                        return;
                    }

                    // List all folders as a demonstration
                    ImapFolderInfoCollection folderInfos = client.ListFolders();
                    foreach (ImapFolderInfo folderInfo in folderInfos)
                    {
                        Console.WriteLine($"Folder: {folderInfo.Name}");
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Failed to create or use ImapClient: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
