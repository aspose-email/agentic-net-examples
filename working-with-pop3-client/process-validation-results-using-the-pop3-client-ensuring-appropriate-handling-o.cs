using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Tools.Search;

namespace Pop3ValidationSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server configuration
                string host = "pop.example.com";
                int port = 995;
                string username = "user";
                string password = "pass";

                // Initialize POP3 client with SSL
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Validate credentials
                    try
                    {
                        client.ValidateCredentials();
                    }
                    catch (Exception authEx)
                    {
                        Console.Error.WriteLine($"Authentication failed: {authEx.Message}");
                        return;
                    }

                    // Retrieve list of messages
                    Pop3MessageInfoCollection messageInfos = client.ListMessages();
                    Console.WriteLine($"Total messages: {messageInfos.Count}");

                    // Ensure output directory exists
                    string outputDir = "SavedMessages";
                    if (!Directory.Exists(outputDir))
                    {
                        try
                        {
                            Directory.CreateDirectory(outputDir);
                        }
                        catch (Exception dirEx)
                        {
                            Console.Error.WriteLine($"Failed to create directory '{outputDir}': {dirEx.Message}");
                            return;
                        }
                    }

                    // Process each message
                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        Console.WriteLine($"Message #{info.SequenceNumber} UID: {info.UniqueId}");

                        // Fetch the full message
                        MailMessage message;
                        try
                        {
                            message = client.FetchMessage(info.SequenceNumber);
                        }
                        catch (Exception fetchEx)
                        {
                            Console.Error.WriteLine($"Failed to fetch message {info.UniqueId}: {fetchEx.Message}");
                            continue;
                        }

                        Console.WriteLine($"Subject: {message.Subject}");

                        // Save the message to a file
                        string filePath = Path.Combine(outputDir, $"{info.UniqueId}.eml");
                        try
                        {
                            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                message.Save(fs, SaveOptions.DefaultEml);
                            }
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"Failed to save message {info.UniqueId}: {ioEx.Message}");
                        }

                        // Delete the message from the server after processing
                        try
                        {
                            client.DeleteMessage(info.SequenceNumber);
                        }
                        catch (Exception delEx)
                        {
                            Console.Error.WriteLine($"Failed to delete message {info.UniqueId}: {delEx.Message}");
                        }
                    }

                    // Commit deletions
                    try
                    {
                        client.CommitDeletes();
                    }
                    catch (Exception commitEx)
                    {
                        Console.Error.WriteLine($"Failed to commit deletions: {commitEx.Message}");
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
