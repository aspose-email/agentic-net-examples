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
            // Define connection parameters
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Define the log file location
            string logFilePath = @"C:\Logs\AsposeEmail.log";

            // Ensure the directory for the log file exists
            try
            {
                string logDirectory = Path.GetDirectoryName(logFilePath);
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Error creating log directory: {dirEx.Message}");
                return;
            }

            // Create the EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Set the log file name (full path) and optionally include date in the name
                client.LogFileName = logFilePath;
                client.UseDateInLogFileName = false; // set true if you want date appended automatically

                // Example operation to trigger logging (e.g., fetch mailbox info)
                try
                {
                    var mailboxInfo = client.GetMailboxInfo();
                    Console.WriteLine($"Mailbox retrieved: {mailboxInfo.MailboxUri}");
                }
                catch (Exception opEx)
                {
                    Console.Error.WriteLine($"Error during client operation: {opEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
