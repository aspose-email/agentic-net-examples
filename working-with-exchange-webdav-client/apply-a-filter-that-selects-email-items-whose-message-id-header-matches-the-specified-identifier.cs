using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string folderUri = "Inbox"; // Folder to search
            string targetMessageId = "<unique-message-id@example.com>"; // Message-ID to match

            // Initialize the Exchange client inside a try/catch to handle connection issues
            try
            {
                using (ExchangeClient client = new ExchangeClient(serviceUrl, new NetworkCredential(username, password)))
                {
                    // List messages that match the specified Message-ID header
                    ExchangeMessageInfoCollection messages = client.ListMessagesById(folderUri, targetMessageId);

                    // Process the results
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        // Fetch the full mail message
                        using (MailMessage message = client.FetchMessage(info.UniqueUri))
                        {
                            Console.WriteLine("Subject: " + message.Subject);
                            Console.WriteLine("From: " + message.From);
                            Console.WriteLine("Message-ID: " + message.Headers["Message-ID"]);
                        }
                    }

                    if (messages.Count == 0)
                    {
                        Console.WriteLine("No messages found with the specified Message-ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error connecting to Exchange server: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
