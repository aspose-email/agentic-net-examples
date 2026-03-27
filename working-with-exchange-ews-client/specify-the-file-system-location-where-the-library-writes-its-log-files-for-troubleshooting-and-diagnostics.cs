using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

public class Program
{
    public static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define the log file location.
            string logFilePath = @"C:\Logs\EwsClient.log";

            // Ensure the directory for the log file exists.
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
                Console.Error.WriteLine("Error creating log directory: " + dirEx.Message);
                return;
            }

            // Set up credentials and mailbox URI.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client using the factory method.
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Configure logging.
                    client.LogFileName = logFilePath;
                    client.UseDateInLogFileName = true;

                    Console.WriteLine("EWS client configured to write logs to: " + client.LogFileName);
                    // Additional EWS operations can be performed here.
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine("Error initializing EWS client: " + clientEx.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}