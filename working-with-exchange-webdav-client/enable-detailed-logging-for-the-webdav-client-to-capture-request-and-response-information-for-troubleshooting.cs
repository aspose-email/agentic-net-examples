using System;
using System.IO;
using System.Net;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare log file path and ensure its directory exists
            string logFilePath = "exchange_client.log";
            try
            {
                string logDir = Path.GetDirectoryName(logFilePath);
                if (!string.IsNullOrEmpty(logDir) && !Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare log directory: {ex.Message}");
                return;
            }

            // Connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Initialize the WebDAV client with detailed logging enabled
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    client.LogFileName = logFilePath;
                    client.UseDateInLogFileName = true; // optional: include date in log file name

                    // Perform a simple operation to generate request/response logs
                    try
                    {
                        // List messages in the Inbox folder
                        var messages = client.ListMessages("Inbox");
                        Console.WriteLine($"Retrieved {messages.Count} messages from Inbox.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Operation failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or connect client: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
