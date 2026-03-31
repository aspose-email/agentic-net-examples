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
            // Determine log folder and ensure it exists
            string logFolder = Path.Combine(Environment.CurrentDirectory, "Logs");
            try
            {
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to prepare log directory: {ioEx.Message}");
                return;
            }

            string logPath = Path.Combine(logFolder, "ews.log");

            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder credentials
            if (mailboxUri.Contains("example.com") || username == "username")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping EWS connection.");
                return;
            }

            // Create EWS client safely
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Set log file location
                    client.LogFileName = logPath;

                    // Example operation: retrieve server version
                    try
                    {
                        string versionInfo = client.GetVersionInfo();
                        Console.WriteLine($"Exchange Server version: {versionInfo}");
                    }
                    catch (Exception opEx)
                    {
                        Console.Error.WriteLine($"EWS operation failed: {opEx.Message}");
                    }
                }
            }
            catch (Exception connEx)
            {
                Console.Error.WriteLine($"Failed to create or use EWS client: {connEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
