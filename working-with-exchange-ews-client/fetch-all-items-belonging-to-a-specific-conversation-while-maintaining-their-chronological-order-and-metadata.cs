using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credential = new NetworkCredential("username", "password");

            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // Specify the conversation identifier
                string conversationId = "YOUR_CONVERSATION_ID";

                // Fetch all messages belonging to the conversation
                MailMessageCollection messages = client.FetchConversationMessages(conversationId);

                // Order messages chronologically (oldest first)
                List<MailMessage> ordered = messages.OrderBy(m => m.Date).ToList();

                // Display metadata for each message
                foreach (MailMessage msg in ordered)
                {
                    Console.WriteLine("Subject: {0}", msg.Subject);
                    Console.WriteLine("From: {0}", msg.From);
                    Console.WriteLine("Date: {0}", msg.Date);
                    Console.WriteLine("Size: {0} bytes", msg.Body?.Length ?? 0);
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
