using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // POP3 server connection settings
            string host = "pop.example.com";
            string username = "user@example.com";
            string password = "password";
            SecurityOptions security = SecurityOptions.Auto;

            // Directory to save fetched messages
            string outputDirectory = "SavedMessages";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {dirEx.Message}");
                    return;
                }
            }

            // Connect to the POP3 server and perform operations
            try
            {
                using (Pop3Client client = new Pop3Client(host, username, password, security))
                {
                    // Get total message count
                    int totalMessages = client.GetMessageCount();
                    Console.WriteLine($"Total messages on server: {totalMessages}");

                    // List messages
                    Pop3MessageInfoCollection messageInfos = client.ListMessages();

                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        Console.WriteLine($"Message #{info.SequenceNumber} - Subject: {info.Subject}");

                        // Fetch the full message
                        using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                        {
                            string filePath = Path.Combine(outputDirectory, $"Message_{info.SequenceNumber}.eml");

                            // Save the message to a file
                            try
                            {
                                message.Save(filePath);
                                Console.WriteLine($"Saved message to: {filePath}");
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save message #{info.SequenceNumber}: {saveEx.Message}");
                            }
                        }

                        // Delete the message from the server
                        try
                        {
                            client.DeleteMessage(info.SequenceNumber);
                            Console.WriteLine($"Marked message #{info.SequenceNumber} for deletion.");
                        }
                        catch (Exception delEx)
                        {
                            Console.Error.WriteLine($"Failed to delete message #{info.SequenceNumber}: {delEx.Message}");
                        }
                    }

                    // Commit deletions
                    try
                    {
                        client.CommitDeletes();
                        Console.WriteLine("Committed deletions on the server.");
                    }
                    catch (Exception commitEx)
                    {
                        Console.Error.WriteLine($"Failed to commit deletions: {commitEx.Message}");
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"POP3 client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
