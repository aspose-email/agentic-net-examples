using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExamples
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            // Top‑level exception guard
            try
            {
                // Placeholder EWS endpoint and credentials
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create the EWS client inside a using block to ensure disposal
                using (Aspose.Email.Clients.Exchange.WebService.IEWSClient client =
                    Aspose.Email.Clients.Exchange.WebService.EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // List messages from the Inbox folder
                    Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection messages =
                        client.ListMessages("Inbox");

                    // Iterate over each message info and display basic metadata
                    foreach (Aspose.Email.Clients.Exchange.ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine("Subject: " + info.Subject);
                        Console.WriteLine("From   : " + info.From);
                        Console.WriteLine("Date   : " + info.Date);
                        Console.WriteLine("URI    : " + info.UniqueUri);

                        // Fetch the full message using the UniqueUri (not Id)
                        Aspose.Email.MailMessage fullMessage = client.FetchMessage(info.UniqueUri);
                        Console.WriteLine("Full Subject: " + fullMessage.Subject);
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                // Write any errors to the error stream and exit gracefully
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}