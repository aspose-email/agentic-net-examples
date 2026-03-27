using System;
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
            // POP3 server configuration
            string host = "pop3.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";

            // Initialize and connect the POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                // Validate credentials
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception credEx)
                {
                    Console.Error.WriteLine("Authentication failed: " + credEx.Message);
                    return;
                }

                // Build a query to filter messages with a specific subject keyword
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.Subject.Contains("Invoice");
                MailQuery query = builder.GetQuery();

                // Retrieve messages matching the query
                Pop3MessageInfoCollection messages = client.ListMessages(query);

                foreach (Pop3MessageInfo info in messages)
                {
                    // Fetch the full message
                    using (MailMessage message = client.FetchMessage(info.SequenceNumber))
                    {
                        // Process the message (example: display subject and sender)
                        Console.WriteLine("Subject: " + message.Subject);
                        Console.WriteLine("From: " + (message.From != null ? message.From.ToString() : "Unknown"));
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
