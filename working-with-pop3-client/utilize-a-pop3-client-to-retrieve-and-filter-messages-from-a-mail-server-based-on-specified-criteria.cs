using Aspose.Email.Tools.Search;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3MessageRetriever
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // POP3 server configuration
                string host = "pop.example.com";
                int port = 110;
                string username = "user@example.com";
                string password = "password";

                // Directory to save downloaded messages
                string outputDir = "DownloadedMessages";

                // Ensure the output directory exists
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Initialize POP3 client
                using (Pop3Client client = new Pop3Client(host, username, password, SecurityOptions.Auto))
                {
                    client.Port = port;

                    // Build a query to filter messages (e.g., subject contains "Invoice")
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains("Invoice");
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages that match the query
                    Pop3MessageInfoCollection messages = client.ListMessages(query);

                    foreach (Pop3MessageInfo info in messages)
                    {
                        // Fetch the full message
                        MailMessage message = client.FetchMessage(info.SequenceNumber);

                        // Define the file path for saving
                        string filePath = Path.Combine(outputDir, $"Message_{info.SequenceNumber}.eml");

                        // Save the message to a file
                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            message.Save(fs, SaveOptions.DefaultEml);
                        }

                        Console.WriteLine($"Saved message {info.SequenceNumber} with subject: {info.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
