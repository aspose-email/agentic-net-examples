using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        // Top-level exception guard
        try
        {
            // IMAP server connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Client creation and connection safety
            try
            {
                // Create the ImapClient with secure connection options
                using (Aspose.Email.Clients.Imap.ImapClient imapClient = new Aspose.Email.Clients.Imap.ImapClient(host, port, username, password, Aspose.Email.Clients.SecurityOptions.Auto))
                {
                    // Verify the connection by sending a NOOP command
                    try
                    {
                        imapClient.Noop();
                        Console.WriteLine("IMAP connection established successfully.");
                    }
                    catch (Exception connectionEx)
                    {
                        Console.Error.WriteLine($"Failed to verify IMAP connection: {connectionEx.Message}");
                        return;
                    }

                    // Additional IMAP operations can be performed here
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Failed to create or connect IMAP client: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}