using System;
using System.Net;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credential = new NetworkCredential("username", "password");

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // Identifier of the conversation to retrieve
                string conversationId = "YOUR_CONVERSATION_ID";

                // Fetch all messages belonging to the conversation
                MailMessageCollection messages = client.FetchConversationMessages(conversationId);

                // Copy to a list for sorting
                List<MailMessage> sortedMessages = new List<MailMessage>(messages);
                sortedMessages.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

                // Output metadata in chronological order
                foreach (MailMessage message in sortedMessages)
                {
                    using (message)
                    {
                        Console.WriteLine("Subject: " + message.Subject);
                        Console.WriteLine("From: " + message.From);
                        Console.WriteLine("Date: " + message.Date);
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
