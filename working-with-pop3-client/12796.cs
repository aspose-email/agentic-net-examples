using System;
using System.IO;
using System.Threading;
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
            // Placeholder POP3 server credentials.
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.Auto;

            // Skip real network calls when placeholder values are used.
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Skipping POP3 operations due to placeholder credentials.");
                return;
            }

            // Directory where fetched messages will be saved.
            string outputDir = Path.Combine(Environment.CurrentDirectory, "FetchedMessages");

            // Ensure the output directory exists.
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ioEx.Message}");
                return;
            }

            // Create and use the POP3 client.
            using (Pop3Client client = new Pop3Client(host, port, username, password, security))
            {
                try
                {
                    // Retrieve mailbox information (forces connection).
                    Pop3MailboxInfo mailboxInfo = await client.GetMailboxInfoAsync();
                    Console.WriteLine($"Mailbox contains {mailboxInfo.MessageCount} messages.");

                    // List all messages.
                    Pop3MessageInfoCollection messages = await client.ListMessagesAsync();

                    foreach (Pop3MessageInfo info in messages)
                    {
                        // Fetch the full message.
                        MailMessage message = await client.FetchMessageAsync(info.SequenceNumber);

                        // Simple processing: display subject.
                        Console.WriteLine($"Processing message: {info.Subject}");

                        // Save the message to a file.
                        string filePath = Path.Combine(outputDir, $"{info.UniqueId}.eml");
                        try
                        {
                            await client.SaveMessageAsync(info.SequenceNumber, filePath);
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message {info.UniqueId}: {saveEx.Message}");
                            continue;
                        }

                        // Delete the message from the server.
                        await client.DeleteMessageAsync(info.SequenceNumber);
                    }

                    // Commit deletions on the server.
                    await client.CommitDeletesAsync();
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {clientEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
