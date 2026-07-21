using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: sample demonstrates establishing a secure IMAP connection.
            string host = "imap.example.com";
            int port = 993; // Standard IMAPS port
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the ImapClient with SSL/TLS security.
            using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                // Verify the connection by listing available folders.
                ImapFolderInfoCollection folders = imapClient.ListFolders();
                Console.WriteLine("Connected successfully. Folders:");
                foreach (ImapFolderInfo folder in folders)
                {
                    Console.WriteLine("- " + folder.Name);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
