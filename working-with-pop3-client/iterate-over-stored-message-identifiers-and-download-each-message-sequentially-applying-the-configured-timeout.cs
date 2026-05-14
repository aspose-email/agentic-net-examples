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
            // Configuration (replace with real values or keep placeholders for safe execution)
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";
            int timeoutMilliseconds = 30000; // 30 seconds

            // Guard against placeholder credentials/hosts
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder POP3 configuration detected. Skipping network operations.");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = "DownloadedMessages";
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Create and configure POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                client.Timeout = timeoutMilliseconds;

                // Validate credentials safely
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception credEx)
                {
                    Console.Error.WriteLine($"Credential validation failed: {credEx.Message}");
                    return;
                }

                // Retrieve list of message identifiers
                Pop3MessageInfoCollection messageInfos;
                try
                {
                    messageInfos = await client.ListMessagesAsync();
                }
                catch (Exception listEx)
                {
                    Console.Error.WriteLine($"Failed to list messages: {listEx.Message}");
                    return;
                }

                // Iterate over each message identifier and download sequentially
                foreach (Pop3MessageInfo messageInfo in messageInfos)
                {
                    // Use sequence number for fetching
                    int sequenceNumber = messageInfo.SequenceNumber;

                    // Fetch the message
                    MailMessage mailMessage;
                    try
                    {
                        mailMessage = await client.FetchMessageAsync(sequenceNumber);
                    }
                    catch (Exception fetchEx)
                    {
                        Console.Error.WriteLine($"Failed to fetch message #{sequenceNumber}: {fetchEx.Message}");
                        continue;
                    }

                    // Save the message to a file
                    string safeFileName = $"Message_{sequenceNumber}_{Guid.NewGuid():N}.eml";
                    string filePath = Path.Combine(outputDirectory, safeFileName);

                    try
                    {
                        using (mailMessage)
                        {
                            mailMessage.Save(filePath);
                        }
                        Console.WriteLine($"Message #{sequenceNumber} saved to {filePath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save message #{sequenceNumber}: {saveEx.Message}");
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
