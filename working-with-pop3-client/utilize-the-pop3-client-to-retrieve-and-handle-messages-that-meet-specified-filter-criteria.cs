using Aspose.Email.Tools.Search;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3FilterExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // POP3 server connection parameters
                string host = "pop.example.com";
                int port = 110;
                string username = "user@example.com";
                string password = "password";

                // Initialize POP3 client (client variable name preserved)
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Build a query to filter messages (e.g., subject contains "Invoice")
                        MailQueryBuilder builder = new MailQueryBuilder();
                        builder.Subject.Contains("Invoice");
                        MailQuery query = builder.GetQuery();

                        // Retrieve messages that match the query
                        Pop3MessageInfoCollection messages = client.ListMessages(query);

                        if (messages != null && messages.Count > 0)
                        {
                            // Ensure the output directory exists
                            string saveFolder = "SavedMessages";
                            if (!Directory.Exists(saveFolder))
                            {
                                Directory.CreateDirectory(saveFolder);
                            }

                            // Process each matching message
                            foreach (Pop3MessageInfo info in messages)
                            {
                                // Fetch the full message
                                using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                                {
                                    Console.WriteLine($"Subject: {message.Subject}");

                                    // Save the message to a file
                                    string filePath = Path.Combine(saveFolder, $"{info.UniqueId}.eml");
                                    try
                                    {
                                        client.SaveMessage(info.SequenceNumber, filePath);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Error.WriteLine($"Failed to save message {info.UniqueId}: {ex.Message}");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("No messages matching the criteria were found.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during POP3 operations: {ex.Message}");
                        return;
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
