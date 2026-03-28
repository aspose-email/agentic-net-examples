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
            // Desired log file path
            string logFilePath = Path.Combine(Environment.CurrentDirectory, "imap_client.log");

            // Ensure the directory for the log file exists
            string logDir = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDir))
            {
                try
                {
                    Directory.CreateDirectory(logDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create log directory: {ex.Message}");
                    return;
                }
            }

            // IMAP server connection parameters (replace with real values as needed)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Initialize the IMAP client inside a using block to ensure disposal
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                // Enable logging and set the log file name
                client.EnableLogger = true;
                client.LogFileName = logFilePath;

                try
                {
                    // Connect to the server (connection is established by the constructor)
                    // Perform a simple operation to verify the connection
                    client.SelectFolder("INBOX");
                    Console.WriteLine("Connected to IMAP server and selected INBOX.");
                }
                catch (Exception connEx)
                {
                    Console.Error.WriteLine($"IMAP connection error: {connEx.Message}");
                    return;
                }

                // Additional IMAP operations can be placed here
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
