using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    public static void Main()
    {
        try
        {
            // Zimbra IMAP server connection settings
            string host = "zimbra.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and connect the IMAP client with SSL/TLS
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Enable logging for troubleshooting
                    client.EnableLogger = true;
                    client.LogFileName = "imap_log.txt";

                    // Subscribe to the "Updates" folder to receive product update notifications
                    client.SubscribeFolderAsync("Updates").GetAwaiter().GetResult();

                    Console.WriteLine("Successfully subscribed to the 'Updates' folder on Zimbra.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("IMAP operation failed: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}