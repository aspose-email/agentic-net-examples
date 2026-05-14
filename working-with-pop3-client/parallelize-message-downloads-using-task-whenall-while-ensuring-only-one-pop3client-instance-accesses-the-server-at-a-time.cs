using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                string host = "pop3.example.com";
                int port = 110;
                string username = "username";
                string password = "password";
                string outputDirectory = "DownloadedMessages";

                // Skip execution when placeholder credentials are detected
                if (host.Contains("example.com") || username == "username")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                    return;
                }

                // Ensure the output directory exists
                try
                {
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }

                // Create and use a single POP3 client instance
                using (Pop3Client client = new Pop3Client(host, port, username, password))
                {
                    // Validate credentials
                    try
                    {
                        await client.ValidateCredentialsAsync();
                    }
                    catch (Exception credEx)
                    {
                        Console.Error.WriteLine($"Failed to validate POP3 credentials: {credEx.Message}");
                        return;
                    }

                    // List messages on the server
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

                    // Semaphore to ensure only one operation uses the client at a time
                    SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
                    List<Task> downloadTasks = new List<Task>();

                    foreach (Pop3MessageInfo messageInfo in messageInfos)
                    {
                        Task downloadTask = Task.Run(async () =>
                        {
                            await semaphore.WaitAsync();
                            try
                            {
                                // Fetch the message
                                using (MailMessage message = await client.FetchMessageAsync(messageInfo.SequenceNumber))
                                {
                                    string filePath = Path.Combine(outputDirectory, $"Message_{messageInfo.SequenceNumber}.eml");
                                    try
                                    {
                                        message.Save(filePath);
                                    }
                                    catch (Exception saveEx)
                                    {
                                        Console.Error.WriteLine($"Failed to save message {messageInfo.SequenceNumber}: {saveEx.Message}");
                                    }
                                }
                            }
                            catch (Exception fetchEx)
                            {
                                Console.Error.WriteLine($"Failed to fetch message {messageInfo.SequenceNumber}: {fetchEx.Message}");
                            }
                            finally
                            {
                                semaphore.Release();
                            }
                        });

                        downloadTasks.Add(downloadTask);
                    }

                    // Wait for all download tasks to complete
                    await Task.WhenAll(downloadTasks);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
