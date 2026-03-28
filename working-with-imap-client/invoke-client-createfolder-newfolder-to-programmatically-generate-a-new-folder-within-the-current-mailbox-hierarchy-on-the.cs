using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

namespace AsposeEmailImapCreateFolder
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
                SecurityOptions security = SecurityOptions.Auto;

                try
                {
                    // Initialize and connect the IMAP client
                    using (ImapClient client = new ImapClient(host, port, username, password, security))
                    {
                        // Create a new folder named "NewFolder" in the mailbox
                        client.CreateFolder("NewFolder");
                        Console.WriteLine("Folder 'NewFolder' created successfully.");
                    }
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
