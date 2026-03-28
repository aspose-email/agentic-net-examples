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
            // POP3 server connection parameters
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Initialize POP3 client and ensure proper disposal
            try
            {
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    // Build a query to filter messages whose subject contains "Invoice"
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.Subject.Contains("Invoice");
                    MailQuery query = builder.GetQuery();

                    // Retrieve message infos that match the query
                    Pop3MessageInfoCollection messageInfos = client.ListMessages(query);

                    // Process each matching message
                    foreach (Pop3MessageInfo messageInfo in messageInfos)
                    {
                        // Fetch the full message using its sequence number
                        using (MailMessage message = client.FetchMessage(messageInfo.SequenceNumber))
                        {
                            // Example processing: output the subject to the console
                            Console.WriteLine($"Subject: {message.Subject}");
                            // Additional processing logic can be placed here
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
