using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define the directory and file where Aspose.Email will write its logs
            string logDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
            string logFilePath = Path.Combine(logDirectory, "AsposeEmail.log");

            // Ensure the log directory exists
            if (!Directory.Exists(logDirectory))
            {
                try
                {
                    Directory.CreateDirectory(logDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create log directory – {logDirectory}. {dirEx.Message}");
                    return;
                }
            }

            // Placeholder connection details (replace with real values for actual use)
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during sample execution
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping server connection.");
                Console.WriteLine($"Log files will be written to: {logFilePath}");
                return;
            }

            // Create the EWS client and configure logging
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    client.LogFileName = logFilePath;
                    client.UseDateInLogFileName = false; // optional: keep a single log file

                    // Perform a simple operation to generate log entries
                    try
                    {
                        ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                        Console.WriteLine($"Inbox URI: {mailboxInfo.InboxUri}");
                    }
                    catch (Exception opEx)
                    {
                        Console.Error.WriteLine($"Operation error: {opEx.Message}");
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
