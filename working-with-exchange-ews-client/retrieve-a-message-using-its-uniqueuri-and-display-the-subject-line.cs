using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.Net;
using Aspose.Email;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Mailbox connection parameters
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Initialize the Exchange client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Unique URI of the message to retrieve
                string messageUri = "https://exchange.example.com/ews/MessageUniqueUri";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || messageUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Fetch the message
                MailMessage message = client.FetchMessage(messageUri);

                // Display the subject line
                Console.WriteLine("Subject: " + message.Subject);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
