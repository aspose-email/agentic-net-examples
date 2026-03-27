using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection details
            string host = "pop.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";

            // Ensure the output directory exists
            string outputDir = "Messages";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                // Build a query to filter messages (e.g., subject contains "Report")
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.Subject.Contains("Report");
                MailQuery query = builder.GetQuery();

                // Retrieve the list of messages matching the query
                Pop3MessageInfoCollection messages = client.ListMessages(query);

                foreach (Pop3MessageInfo info in messages)
                {
                    // Fetch the full message using its unique identifier
                    MailMessage message = client.FetchMessage(info.UniqueId);

                    // Save the message to a file
                    string filePath = Path.Combine(outputDir, $"{info.UniqueId}.eml");
                    try
                    {
                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            message.Save(fs, SaveOptions.DefaultEml);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving message {info.UniqueId}: {ex.Message}");
                        continue;
                    }

                    Console.WriteLine($"Saved message {info.UniqueId}: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
