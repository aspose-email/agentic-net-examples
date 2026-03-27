using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Connection parameters
                string host = "pop.example.com";
                int port = 995;
                string username = "user@example.com";
                string password = "password";
                SecurityOptions security = SecurityOptions.Auto;

                // Create POP3 client
                using (Pop3Client client = new Pop3Client(host, port, username, password, security))
                {
                    try
                    {
                        // Validate credentials (establishes connection)
                        client.ValidateCredentials();

                        // Retrieve mailbox information
                        var mailboxInfo = client.GetMailboxInfo();

                        Console.WriteLine($"Total messages: {mailboxInfo.MessageCount}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
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
