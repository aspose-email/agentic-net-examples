using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailLoggingExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Ensure the log directory exists
                string logDirectory = "logs";
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Define the log file path
                string logFilePath = Path.Combine(logDirectory, "ews.log");

                // Placeholder credentials and mailbox URI
                NetworkCredential credentials = new NetworkCredential("username", "password");
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

                // Create the EWS client with logging enabled
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    client.LogFileName = logFilePath;
                    client.UseDateInLogFileName = false; // Disable date suffix for simplicity

                    // Perform a simple operation to generate log entries
                    client.ListMessages("Inbox");

                    Console.WriteLine("Comprehensive logging activated. Log file located at: " + logFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}