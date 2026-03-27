using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace ImapMessageMover
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // IMAP server connection settings
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Source folder where the message currently resides
                string sourceFolder = "INBOX";

                // Destination folder to move the message to
                string destinationFolder = "Archive";

                // Unique identifier (UID) of the message to be moved
                string messageUid = "12345";

                // Create and configure the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Select the source folder
                    client.SelectFolder(sourceFolder);

                    // Move the message to the destination folder
                    // The method returns the new UID of the moved message
                    string newUid = client.MoveMessage(messageUid, destinationFolder);

                    Console.WriteLine("Message moved successfully. New UID: " + newUid);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}