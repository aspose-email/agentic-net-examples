using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExample
{
    public class Program
    {
        public static void Main()
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define the EWS service URL and user credentials
                string mailboxUri = "https://example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create the EWS client using the factory method
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    try
                    {
                        // Retrieve the collection of messages from the Inbox folder
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                        // Iterate through the messages and display basic information
                        foreach (ExchangeMessageInfo info in messages)
                        {
                            Console.WriteLine("Subject: " + info.Subject);
                            Console.WriteLine("From: " + (info.From != null ? info.From.ToString() : "Unknown"));
                            Console.WriteLine("Date: " + info.Date);
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle errors that occur during message retrieval
                        Console.Error.WriteLine("Error listing messages: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors that occur during client creation or other unexpected failures
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}