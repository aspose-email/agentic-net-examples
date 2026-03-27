using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define log file location
            string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AsposeLogs");
            string logFilePath = Path.Combine(logDirectory, "ews_log.txt");

            // Ensure the log directory exists
            try
            {
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to create log directory: " + ex.Message);
                return;
            }

            // Prepare credentials
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Initialize EWS client with logging
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credentials))
                {
                    client.LogFileName = logFilePath;
                    client.UseDateInLogFileName = false;

                    // Sample operation: list messages in the Inbox
                    try
                    {
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                        Console.WriteLine("Inbox message count: " + messages.Count);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error listing messages: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to initialize EWS client: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}