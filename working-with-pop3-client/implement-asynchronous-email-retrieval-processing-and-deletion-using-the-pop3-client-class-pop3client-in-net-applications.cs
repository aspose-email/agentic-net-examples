using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace AsposeEmailPop3Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // POP3 server configuration
                string host = "pop3.example.com";
                int port = 995;
                string username = "user@example.com";
                string password = "password";
                SecurityOptions security = SecurityOptions.Auto;

                // Directory to save downloaded messages
                string outputDir = "DownloadedEmails";

                // Ensure the output directory exists
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

                // Connect to POP3 server
                using (Pop3Client client = new Pop3Client(host, port, username, password, security))
                {
                    try
                    {
                        // Optional ping to verify connection
                        client.Noop();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"POP3 connection failed: {ex.Message}");
                        return;
                    }

                    // Retrieve list of messages
                    Pop3MessageInfoCollection messageInfos;
                    try
                    {
                        messageInfos = client.ListMessages();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                        return;
                    }

                    // Process each message
                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        try
                        {
                            // Fetch the full message
                            using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                            {
                                // Save the message to a file
                                string filePath = Path.Combine(outputDir, $"{info.UniqueId}.eml");
                                try
                                {
                                    message.Save(filePath, SaveOptions.DefaultEml);
                                    Console.WriteLine($"Saved message '{info.Subject}' to '{filePath}'.");
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Failed to save message '{info.Subject}': {ex.Message}");
                                }
                            }

                            // Delete the message from the server
                            await client.DeleteMessageAsync(info.SequenceNumber);
                            Console.WriteLine($"Deleted message '{info.Subject}' from server.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error processing message '{info.Subject}': {ex.Message}");
                        }
                    }

                    // Commit deletions
                    try
                    {
                        await client.CommitDeletesAsync();
                    }
                    catch (Exception ex)
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
}
