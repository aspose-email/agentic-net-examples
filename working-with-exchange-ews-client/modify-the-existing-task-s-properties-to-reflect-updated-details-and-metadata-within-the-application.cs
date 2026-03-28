using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            // Initialize the EWS client using the factory method.
            // Wrap client creation in a try/catch to handle connection/authentication errors.
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(
                    "https://example.com/EWS/Exchange.asmx",
                    new NetworkCredential("username", "password"));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to create EWS client: " + ex.Message);
                return;
            }

            // Ensure the client is disposed after use.
            using (client)
            {
                try
                {
                    // List messages in the Inbox folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    if (messages != null && messages.Count > 0)
                    {
                        // Fetch the first message using its UniqueUri.
                        ExchangeMessageInfo firstInfo = messages[0];
                        using (MailMessage mail = client.FetchMessage(firstInfo.UniqueUri))
                        {
                            Console.WriteLine("Subject: " + mail.Subject);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No messages found in the Inbox.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error while accessing mailbox: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard.
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
