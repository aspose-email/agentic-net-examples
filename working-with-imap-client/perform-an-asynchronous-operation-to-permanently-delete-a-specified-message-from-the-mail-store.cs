using System;
using System.Net;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Define connection parameters (replace with real values)
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";
                string messageUid = "12345"; // Unique ID of the message to delete

                // Create and configure the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Select the folder containing the message
                    await client.SelectFolderAsync("INBOX");

                    // Permanently delete the message and commit the deletion
                    await client.DeleteMessageAsync(messageUid, true);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}