using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.IO;
using System.Threading;
class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection settings
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Local folder to store downloaded messages
            string localFolder = Path.Combine(Environment.CurrentDirectory, "DownloadedMessages");

            // Ensure the local folder exists
            if (!Directory.Exists(localFolder))
            {
                try
                {
                    Directory.CreateDirectory(localFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create local folder '{localFolder}': {ex.Message}");
                    return;
                }
            }

            // Periodically synchronize the mailbox
            while (true)
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        // List all messages in the Inbox folder
                        ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                        foreach (ExchangeMessageInfo info in messages)
                        {
                            // Use the unique URI as part of the local file name
                            string safeFileName = info.UniqueUri.Replace("/", "_").Replace("\\", "_");
                            string localPath = Path.Combine(localFolder, safeFileName + ".eml");

                            // Download only if the message does not already exist locally
                            if (!File.Exists(localPath))
                            {
                                try
                                {
                                    client.SaveMessage(info.UniqueUri, localPath);
                                    Console.WriteLine($"Downloaded message to '{localPath}'.");
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Failed to save message '{info.UniqueUri}': {ex.Message}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during mailbox synchronization: {ex.Message}");
                    }
                }

                // Wait for a minute before the next sync cycle
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
