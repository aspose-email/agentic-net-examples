using Aspose.Email.Tools.Search;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder POP3 server credentials
                string host = "pop3.example.com";
                int port = 110;
                string username = "username";
                string password = "password";

                // Guard against executing with placeholder credentials
                if (host.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder POP3 server credentials detected. Skipping connection.");
                    return;
                }

                // Initialize POP3 client
                using (Pop3Client client = new Pop3Client(host, port, username, password))
                {
                    try
                    {
                        // Validate credentials
                        client.ValidateCredentials();

                        // Build a query to retrieve all messages
                        MailQueryBuilder builder = new MailQueryBuilder();
                        MailQuery query = builder.GetQuery();

                        // List messages based on the query
                        Pop3MessageInfoCollection messages = client.ListMessages(query);
                        Console.WriteLine($"Total messages retrieved: {messages.Count}");

                        // Ensure output directory exists
                        string outputDir = "RetrievedMessages";
                        if (!Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        // Iterate through messages and save each to a file
                        foreach (Pop3MessageInfo messageInfo in messages)
                        {
                            string filePath = Path.Combine(outputDir, $"Message_{messageInfo.SequenceNumber}.eml");
                            try
                            {
                                client.SaveMessage(messageInfo.SequenceNumber, filePath);
                                Console.WriteLine($"Saved message {messageInfo.SequenceNumber} to {filePath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message {messageInfo.SequenceNumber}: {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"POP3 client operation error: {ex.Message}");
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
