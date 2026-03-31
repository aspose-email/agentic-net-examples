using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection settings (placeholders)
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.None;

            // Guard against executing with placeholder credentials
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder POP3 server settings detected. Skipping network call.");
                return;
            }

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, security))
            {
                try
                {
                    // Validate credentials; throws Pop3Exception on failure
                    client.ValidateCredentials();

                    // Retrieve list of messages
                    Pop3MessageInfoCollection messageInfos = client.ListMessages();

                    Console.WriteLine($"Total messages on server: {messageInfos.Count}");

                    if (messageInfos.Count > 0)
                    {
                        // Fetch the first message
                        int sequenceNumber = messageInfos[0].SequenceNumber;
                        using (MailMessage message = client.FetchMessage(sequenceNumber))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"Date: {message.Date}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No messages found on the server.");
                    }
                }
                catch (Pop3Exception popEx)
                {
                    // Handle POP3-specific errors
                    Console.Error.WriteLine($"POP3 error: {popEx.Message}");
                }
                catch (Exception ex)
                {
                    // Handle other errors related to client operations
                    Console.Error.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
