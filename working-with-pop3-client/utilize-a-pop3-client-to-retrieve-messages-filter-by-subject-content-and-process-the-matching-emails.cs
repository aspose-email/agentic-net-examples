using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip actual network call if they are not replaced.
            string host = "pop3.example.com";
            int port = 110;
            string username = "username";
            string password = "password";

            if (host.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            // Create POP3 client and ensure it is disposed properly.
            using (Pop3Client client = new Pop3Client(host, port, username, password))
            {
                try
                {
                    // Build a query to find messages whose subject contains the keyword "Invoice".
                    MailQueryBuilder queryBuilder = new MailQueryBuilder();
                    queryBuilder.Subject.Contains("Invoice");
                    MailQuery query = queryBuilder.GetQuery();

                    // Retrieve information about matching messages.
                    Pop3MessageInfoCollection messageInfos = client.ListMessages(query);

                    foreach (Pop3MessageInfo info in messageInfos)
                    {
                        // Fetch the full message for each matching entry.
                        using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            // Additional processing of the message can be performed here.
                        }
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
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
