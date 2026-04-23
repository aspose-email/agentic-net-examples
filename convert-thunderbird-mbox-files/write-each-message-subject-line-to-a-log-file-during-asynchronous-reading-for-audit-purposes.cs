using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Configuration (replace with real values)
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";
            string logFilePath = "audit.log";

            // Guard against placeholder credentials/host
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder host detected. Skipping execution.");
                return;
            }

            // Ensure the directory for the log file exists
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            {
                try
                {
                    Directory.CreateDirectory(logDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create log directory: {dirEx.Message}");
                    return;
                }
            }

            // Create and connect the IMAP client
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    client.SecurityOptions = SecurityOptions.Auto;
                    client.ValidateCredentials();
                }
                catch (Exception connEx)
                {
                    Console.Error.WriteLine($"IMAP connection failed: {connEx.Message}");
                    return;
                }

                // Open the log file for appending
                using (StreamWriter logWriter = new StreamWriter(logFilePath, true))
                {
                    try
                    {
                        // Asynchronously list messages in the INBOX folder
                        ImapMessageInfoCollection messageInfos = await client.ListMessagesAsync("INBOX", false);

                        // Write each subject line to the log file
                        foreach (ImapMessageInfo messageInfo in messageInfos)
                        {
                            string subject = messageInfo.Subject ?? string.Empty;
                            logWriter.WriteLine(subject);
                        }
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Error during message retrieval or logging: {ioEx.Message}");
                        return;
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
