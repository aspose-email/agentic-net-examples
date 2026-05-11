using System;
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
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create a synchronous EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Target folder (Inbox in this example)
                string folderUri = client.MailboxInfo.InboxUri;

                // Prepare messages to append
                List<MailMessage> messages = new List<MailMessage>();

                MailMessage msg1 = new MailMessage("sender@example.com", "recipient@example.com", "Subject 1", "Body 1");
                // Preserve original ReceivedTime by setting the Date header
                msg1.Headers["Date"] = DateTime.UtcNow.ToString("r");
                messages.Add(msg1);

                MailMessage msg2 = new MailMessage("sender2@example.com", "recipient2@example.com", "Subject 2", "Body 2");
                msg2.Headers["Date"] = DateTime.UtcNow.AddMinutes(-5).ToString("r");
                messages.Add(msg2);

                // Append each message to the specified folder
                foreach (MailMessage message in messages)
                {
                    client.AppendMessage(folderUri, message);
                }

                Console.WriteLine("Messages appended successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
