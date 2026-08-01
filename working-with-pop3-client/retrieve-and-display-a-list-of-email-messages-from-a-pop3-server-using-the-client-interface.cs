using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        // POP3 server configuration – replace with actual values when testing
        string host = "pop3.example.com";
        int port = 110;
        string username = "user@example.com";
        string password = "password";


        // Skip external calls when placeholder credentials are used
        if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
            return;
        }

        try
        {
            // Pop3Client implements IDisposable, so use a using block
            using (Pop3Client pop3Client = new Pop3Client())
            {
                pop3Client.Host = host;
                pop3Client.Port = port;
                pop3Client.Username = username;
                pop3Client.Password = password;
                // Uncomment and adjust if your server requires SSL/TLS
                // pop3Client.SecurityOptions = SecurityOptions.Auto;

                // Retrieve information about all messages in the mailbox
                Pop3MessageInfoCollection messageInfos = pop3Client.ListMessages();

                Console.WriteLine($"Total messages: {messageInfos.Count}");

                // Iterate through each message info and fetch the full message
                foreach (Pop3MessageInfo messageInfo in messageInfos)
                {
                    MailMessage message = pop3Client.FetchMessage(messageInfo.SequenceNumber);
                    Console.WriteLine($"Seq:{messageInfo.SequenceNumber} Subject:{message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
