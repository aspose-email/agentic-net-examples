using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // IMAP server credentials (replace with real values)
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP connection.");
                return;
            }

            // Path for the detailed protocol log
            string logFilePath = "imap_log.txt";

            // Ensure the directory for the log file exists
            try
            {
                string logDirectory = Path.GetDirectoryName(logFilePath);
                if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare log directory: {ex.Message}");
                return;
            }

            // Create and use the ImapClient
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Enable detailed logging
                    client.EnableLogger = true;
                    client.LogFileName = logFilePath;

                    // Perform a simple operation to generate log entries
                    client.SelectFolder("INBOX");
                    ImapFolderInfoCollection folders = client.ListFolders();
                    Console.WriteLine($"Retrieved {folders.Count} folders.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
                finally
                {
                    // Clean up logging settings
                    client.EnableLogger = false;
                    client.ResetLogSettings();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
