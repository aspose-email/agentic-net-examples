using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define EWS connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Prepare log file path and ensure its directory exists
            string logFilePath = "EwsClientLog.txt";
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Create network credentials
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize the EWS client inside a using block for proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Configure activity logging
                    client.LogFileName = logFilePath;
                    client.UseDateInLogFileName = true;

                    // Perform a lightweight operation to generate log entries
                    client.GetMailboxInfo();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
