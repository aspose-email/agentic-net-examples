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
            // Placeholder connection settings
            string host = "pop.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing real network calls with placeholder data
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
            {
                // Validate credentials asynchronously
                try
                {
                    await client.ValidateCredentialsAsync();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Credential validation failed: {ex.Message}");
                    return;
                }

                // Get total message count asynchronously
                int messageCount = await client.GetMessageCountAsync();
                Console.WriteLine($"Total messages: {messageCount}");

                // List messages asynchronously
                Pop3MessageInfoCollection messageInfos = await client.ListMessagesAsync();
                foreach (Pop3MessageInfo info in messageInfos)
                {
                    Console.WriteLine($"UID: {info.UniqueId}, Size: {info.Size} bytes");
                }

                // Fetch the first message and save to a memory stream asynchronously
                if (messageCount > 0)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await client.SaveMessageAsync(1, memoryStream);
                        Console.WriteLine($"Fetched first message, stream length: {memoryStream.Length} bytes");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
