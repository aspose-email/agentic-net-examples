using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange Web Services connection details
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messageInfos = client.ListMessages("Inbox");

                // Iterate through each message info
                foreach (ExchangeMessageInfo messageInfo in messageInfos)
                {
                    // Fetch the full message to access headers
                    MailMessage mailMessage = client.FetchMessage(messageInfo.UniqueUri);

                    // Retrieve the Message-ID header
                    string messageId = mailMessage.Headers["Message-ID"];

                    // Output the Message-ID
                    Console.WriteLine(messageId);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
