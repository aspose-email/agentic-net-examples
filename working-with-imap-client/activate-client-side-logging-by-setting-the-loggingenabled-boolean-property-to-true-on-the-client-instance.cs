using Aspose.Email.Clients;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection settings
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Skip real network calls when using placeholder credentials
                if (host.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping connection.");
                    return;
                }

                // Create and configure the IMAP client
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Activate client-side logging
                    client.EnableLogger = true; // Enables logging for the client

                    // Example operation: list folders (wrapped in its own try/catch)
                    try
                    {
                        var folders = client.ListFolders();
                        Console.WriteLine("Folders on the server:");
                        foreach (var folder in folders)
                        {
                            Console.WriteLine("- " + folder);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error while listing folders: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
            }
        }
    }
}
