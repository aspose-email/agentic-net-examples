using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Guard against executing with placeholder credentials
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network operation.");
                return;
            }

            // Ensure output directory exists
            string outputDir = "Output";
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Create and connect POP3 client
            try
            {
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    // List messages on the server
                    Pop3MessageInfoCollection messageInfos;
                    try
                    {
                        messageInfos = client.ListMessages();
                    }
                    catch (Exception listEx)
                    {
                        Console.Error.WriteLine($"Failed to list messages: {listEx.Message}");
                        return;
                    }

                    int index = 1;
                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        // Fetch the full message
                        MailMessage message;
                        try
                        {
                            message = client.FetchMessage(info.SequenceNumber);
                        }
                        catch (Exception fetchEx)
                        {
                            Console.Error.WriteLine($"Failed to fetch message #{index}: {fetchEx.Message}");
                            continue;
                        }

                        // Save the message to a file
                        string filePath = Path.Combine(outputDir, $"Message_{index}.eml");
                        try
                        {
                            using (message)
                            {
                                message.Save(filePath);
                            }
                            Console.WriteLine($"Saved message #{index} to {filePath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message #{index}: {saveEx.Message}");
                        }

                        index++;
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
