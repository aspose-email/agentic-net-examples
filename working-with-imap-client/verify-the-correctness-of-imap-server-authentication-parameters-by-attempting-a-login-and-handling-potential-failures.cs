using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // IMAP server connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and authenticate the IMAP client
            try
            {
                using (Aspose.Email.Clients.Imap.ImapClient client = new Aspose.Email.Clients.Imap.ImapClient(host, port, username, password, Aspose.Email.Clients.SecurityOptions.Auto))
                {
                    try
                    {
                        client.ValidateCredentials();
                        Console.WriteLine("IMAP authentication succeeded.");
                    }
                    catch (Aspose.Email.ImapException imapEx)
                    {
                        Console.Error.WriteLine($"IMAP authentication failed: {imapEx.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or connect IMAP client: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}