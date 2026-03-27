using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Define log file path
            string logFilePath = "logs/imap_log.txt";

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
                    Console.Error.WriteLine($"Error: Unable to create log directory – {dirEx.Message}");
                    return;
                }
            }

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient("imap.example.com", "username", "password", SecurityOptions.Auto))
            {
                try
                {
                    // Enable logging and set the log file name
                    client.EnableLogger = true;
                    client.LogFileName = logFilePath;

                    // Perform a simple operation to verify connection
                    client.Noop();
                    Console.WriteLine("IMAP client configured successfully. Logging to: " + logFilePath);
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"Error: IMAP client operation failed – {clientEx.Message}");
                    return;
                }
                finally
                {
                    // Disable logging after use (optional)
                    client.EnableLogger = false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}