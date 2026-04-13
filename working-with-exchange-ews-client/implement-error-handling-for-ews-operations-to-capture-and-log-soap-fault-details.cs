using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

namespace EwsErrorHandlingSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Mailbox URI and credentials
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create EWS client with proper disposal
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Attempt to get inbox folder information
                    try
                    {
                        ExchangeFolderInfo inboxInfo = client.GetFolderInfo("inbox");
                        Console.WriteLine($"Inbox folder URI: {inboxInfo.Uri}");
                    }
                    catch (ExchangeException ex)
                    {
                        Console.Error.WriteLine("Failed to retrieve folder info.");
                        Console.Error.WriteLine($"Message: {ex.Message}");
                        Console.Error.WriteLine($"SOAP Fault Details: {ex.ErrorDetails}");
                    }

                    // Attempt to list messages in the inbox folder
                    try
                    {
                        ExchangeMessageInfoCollection messages = client.ListMessages("inbox");
                        Console.WriteLine($"Number of messages in inbox: {messages.Count}");
                    }
                    catch (ExchangeException ex)
                    {
                        Console.Error.WriteLine("Failed to list messages.");
                        Console.Error.WriteLine($"Message: {ex.Message}");
                        Console.Error.WriteLine($"SOAP Fault Details: {ex.ErrorDetails}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
