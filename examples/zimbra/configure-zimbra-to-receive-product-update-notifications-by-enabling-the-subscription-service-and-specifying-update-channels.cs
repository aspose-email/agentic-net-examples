using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Zimbra server connection settings
            string host = "zimbra.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Create and configure the IMAP client
            using (Aspose.Email.Clients.Imap.ImapClient client = new Aspose.Email.Clients.Imap.ImapClient(host, port, username, password, Aspose.Email.Clients.SecurityOptions.SSLImplicit))
            {
                // Enable logging (optional)
                client.EnableLogger = true;
                client.LogFileName = "imap_log.txt";

                // Assign a safe BindIPEndPointHandler

                // Folder that will receive product update notifications
                string updatesFolder = "Updates";

                // Ensure the folder exists; create if missing
                try
                {
                    client.SelectFolder(updatesFolder);
                }
                catch (Exception)
                {
                    client.CreateFolderAsync(updatesFolder).GetAwaiter().GetResult();
                    client.SelectFolder(updatesFolder);
                }

                // Subscribe to the folder to enable notification service
                client.SubscribeFolderAsync(updatesFolder).GetAwaiter().GetResult();

                Console.WriteLine($"Subscribed to folder '{updatesFolder}' for product update notifications.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}