using System;
using System.Net;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // POP3 server connection parameters (replace with real values)
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Create and configure the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                // Assign a BindIPEndPoint handler to control local endpoint binding

                // Subscribe to the OnConnect event
                client.OnConnect += (sender, e) =>
                {
                    Console.WriteLine("POP3 client connected to the server.");
                };

                // Validate credentials (asynchronous)
                try
                {
                    await client.ValidateCredentialsAsync();
                    Console.WriteLine("Credentials validated successfully.");
                }
                catch (Pop3Exception ex)
                {
                    Console.Error.WriteLine($"Credential validation failed: {ex.Message}");
                    return;
                }

                // List messages asynchronously
                Pop3MessageInfoCollection messages;
                try
                {
                    messages = await client.ListMessagesAsync();
                    Console.WriteLine($"Total messages on server: {messages.Count}");
                }
                catch (Pop3Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                // Process each message (fetch subject asynchronously)
                foreach (Pop3MessageInfo info in messages)
                {
                    try
                    {
                        using (MailMessage message = await client.FetchMessageAsync(info.UniqueId))
                        {
                            Console.WriteLine($"Message ID: {info.UniqueId}, Subject: {message.Subject}");
                        }
                    }
                    catch (Pop3Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message {info.UniqueId}: {ex.Message}");
                    }
                }

                // Commit any deletions (if any were performed)
                try
                {
                    await client.CommitDeletesAsync();
                }
                catch (Pop3Exception ex)
                {
                    Console.Error.WriteLine($"Failed to commit deletions: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
