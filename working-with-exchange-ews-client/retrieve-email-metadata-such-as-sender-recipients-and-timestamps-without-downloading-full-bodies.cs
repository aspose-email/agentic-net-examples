using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailEwsMetadataExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create EWS client safely
                IEWSClient client;
                try
                {
                    client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create EWS client: " + ex.Message);
                    return;
                }

                // Use the client within a using block to ensure disposal
                using (client as IDisposable)
                {
                    // List messages in the Inbox folder (metadata only)
                    ExchangeMessageInfoCollection messages;
                    try
                    {
                        messages = client.ListMessages("Inbox");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Failed to list messages: " + ex.Message);
                        return;
                    }

                    // Iterate through each message and display metadata
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine("Subject: " + messageInfo.Subject);
                        Console.WriteLine("From: " + (messageInfo.Sender != null ? messageInfo.Sender.ToString() : "N/A"));
                        Console.WriteLine("To: " + (messageInfo.To != null ? string.Join(", ", messageInfo.To) : "N/A"));
                        Console.WriteLine("CC: " + (messageInfo.CC != null ? string.Join(", ", messageInfo.CC) : "N/A"));
                        Console.WriteLine("BCC: " + (messageInfo.Bcc != null ? string.Join(", ", messageInfo.Bcc) : "N/A"));
                        Console.WriteLine("Date: " + messageInfo.InternalDate);
                        Console.WriteLine("Message ID: " + messageInfo.MessageId);
                        Console.WriteLine("Size (bytes): " + messageInfo.Size);
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
