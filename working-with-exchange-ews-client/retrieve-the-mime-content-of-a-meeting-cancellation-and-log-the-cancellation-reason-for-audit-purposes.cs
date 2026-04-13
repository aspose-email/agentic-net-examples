using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Replace with your Exchange server details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client inside a using block to ensure proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Filter for meeting cancellation messages
                    if (info.MessageClass != null && info.MessageClass.Equals("IPM.Schedule.Meeting.Canceled", StringComparison.OrdinalIgnoreCase))
                    {
                        // Fetch the full MailMessage
                        MailMessage mailMessage = client.FetchMessage(info.UniqueUri);

                        // Retrieve the raw MIME content into a memory stream
                        using (MemoryStream mimeStream = new MemoryStream())
                        {
                            client.SaveMessage(info.UniqueUri, mimeStream);
                            string mimeContent = Encoding.UTF8.GetString(mimeStream.ToArray());

                            // Log the MIME content (for demonstration, write to console)
                            Console.WriteLine("=== Meeting Cancellation MIME Content ===");
                            Console.WriteLine(mimeContent);
                            Console.WriteLine("=== End of MIME Content ===");

                            // Extract and log the cancellation reason (using the message body as a simple example)
                            string cancellationReason = mailMessage.Body ?? string.Empty;
                            Console.WriteLine("Cancellation Reason:");
                            Console.WriteLine(cancellationReason);
                            Console.WriteLine("----------------------------------------");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Friendly error handling
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
