using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Example usage: retrieve messages sent to recipients at "example.com"
            RetrieveMessagesByDomain("example.com");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static void RetrieveMessagesByDomain(string domain)
    {
        // Placeholder connection settings – replace with real values if needed.
        string host = "pop3.example.com";
        int port = 110;
        string username = "user@example.com";
        string password = "password";

        // Guard against executing real network calls with placeholder data.
        if (host.Contains("example.com"))
        {
            Console.WriteLine("Placeholder POP3 host detected. Skipping network operation.");
            return;
        }

        // Create and connect the POP3 client.
        try
        {
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                // List message summaries from the server.
                Pop3MessageInfoCollection messagesInfo = client.ListMessages();

                List<MailMessage> matchingMessages = new List<MailMessage>();

                foreach (Pop3MessageInfo info in messagesInfo)
                {
                    // Fetch the full message.
                    using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                    {
                        // Check each recipient address.
                        foreach (MailAddress address in message.To)
                        {
                            if (!string.IsNullOrEmpty(address.Address) &&
                                address.Address.EndsWith("@" + domain, StringComparison.OrdinalIgnoreCase))
                            {
                                // Store or process the matching message.
                                matchingMessages.Add(message.Clone() as MailMessage);
                                Console.WriteLine($"Matched: Subject = {message.Subject}, To = {address.Address}");
                                break;
                            }
                        }
                    }
                }

                // Example: further processing of matchingMessages can be done here.
                Console.WriteLine($"Total matched messages: {matchingMessages.Count}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
        }
    }
}
