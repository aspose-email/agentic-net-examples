using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Clients.Pop3.Models;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection parameters (replace with real values)
            const string host = "pop3.example.com";
            const int port = 995;
            const string username = "user@example.com";
            const string password = "password";
            const SecurityOptions security = SecurityOptions.Auto;

            // Create and connect the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, security))
            {
                try
                {
                    // Retrieve mailbox status information
                    Pop3MailboxInfo mailboxInfo = client.GetMailboxInfo();
                    Console.WriteLine($"Message Count: {mailboxInfo.MessageCount}");
                    Console.WriteLine($"Occupied Size: {mailboxInfo.OccupiedSize} bytes");

                    // List messages (basic info only)
                    Pop3MessageInfoCollection messageInfos = client.ListMessages();
                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        Console.WriteLine($"Seq: {info.SequenceNumber}, Subject: {info.Subject}, Size: {info.Size} bytes");
                    }

                    // Example: fetch the first message if any
                    if (messageInfos.Count > 0)
                    {
                        int firstSeq = messageInfos[0].SequenceNumber;
                        using (MailMessage message = client.FetchMessage(firstSeq))
                        {
                            Console.WriteLine("\n--- Fetched Message ---");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"Body:\n{message.Body}");
                        }
                    }
                }
                catch (Pop3Exception popEx)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {popEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to initialize POP3 client: {ex.Message}");
        }
    }
}
