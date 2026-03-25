using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Zimbra IMAP server configuration
            string host = "imap.zimbra.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Connect to the Zimbra IMAP server
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
            {
                client.Username = username;
                client.Password = password;

                // Validate credentials; any authentication failure is caught and reported
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception authEx)
                {
                    Console.Error.WriteLine("Authentication failed: " + authEx.Message);
                    return;
                }

                // Define the folder that will receive product update notifications
                string updatesFolder = "Updates";

                // Ensure the folder exists; create it if it does not
                if (!client.ExistFolder(updatesFolder))
                {
                    client.CreateFolder(updatesFolder);
                }

                // Subscribe to the folder so that the client receives notifications
                client.SubscribeFolder(updatesFolder);
                Console.WriteLine($"Successfully subscribed to folder '{updatesFolder}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}