using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailLogExample
{
    // Author: Aspose.Email example author
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define log directory and ensure it exists
                string logDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Full path to the log file
                string logFilePath = Path.Combine(logDirectory, "ews.log");

                // Initialize EWS client (replace with actual mailbox URI and credentials)
                string mailboxUri = "https://example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password);

                // Set the log file name for the client
                client.LogFileName = logFilePath;

                // Example usage: output the configured log path
                Console.WriteLine("EWS client log will be written to: " + client.LogFileName);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
