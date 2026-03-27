using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the IMAP client with placeholder credentials
            using (ImapClient client = new ImapClient(
                "imap.example.com",
                993,
                "username",
                "password",
                SecurityOptions.SSLImplicit))
            {
                // Activate client-side logging
                client.EnableLogger = true;
                client.LogFileName = "imap_log.txt";

                // Perform a simple operation to ensure the client is connected
                client.Noop();

                Console.WriteLine("Client-side logging has been enabled.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}