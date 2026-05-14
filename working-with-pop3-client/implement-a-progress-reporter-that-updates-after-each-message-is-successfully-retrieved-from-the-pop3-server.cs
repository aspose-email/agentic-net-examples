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
            // Placeholder credentials – skip actual network call in CI environments
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network operation.");
                return;
            }

            // Output directory for saved messages
            string outputDir = "RetrievedMessages";
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to validate POP3 credentials: {ex.Message}");
                    return;
                }

                // List messages
                Pop3MessageInfoCollection messageInfos;
                try
                {
                    messageInfos = await client.ListMessagesAsync();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                int total = messageInfos.Count;
                int retrieved = 0;

                foreach (Pop3MessageInfo info in messageInfos)
                {
                    try
                    {
                        // Fetch the full message
                        using (MailMessage message = await client.FetchMessageAsync(info.SequenceNumber))
                        {
                            // Save the message to a file
                            string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                            {
                                safeSubject = safeSubject.Replace(c, '_');
                            }
                            string filePath = Path.Combine(outputDir, $"{info.SequenceNumber}_{safeSubject}.eml");

                            try
                            {
                                message.Save(filePath);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message {info.SequenceNumber}: {ex.Message}");
                                continue;
                            }

                            retrieved++;
                            Console.WriteLine($"Retrieved {retrieved}/{total}: {message.Subject}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing message {info.SequenceNumber}: {ex.Message}");
                    }
                }

                Console.WriteLine($"Completed. {retrieved} of {total} messages retrieved.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
