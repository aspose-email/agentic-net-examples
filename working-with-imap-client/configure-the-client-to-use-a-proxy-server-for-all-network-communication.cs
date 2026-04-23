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
            // Placeholder server details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Detect placeholder host and skip real network call
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder host detected. Skipping network operations.");
                return;
            }

            // Configure HTTP proxy (no authentication)
            HttpProxy proxy = new HttpProxy("proxy.mycompany.com", 8080);
            // If proxy requires authentication, use the overload:
            // HttpProxy proxy = new HttpProxy("proxy.mycompany.com", 8080, "proxyUser", "proxyPass");

            // Initialize IMAP client and assign the proxy
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                client.Proxy = proxy;

                // Example operation: list folders (wrapped in try/catch for safety)
                try
                {
                    var folders = client.ListFolders();
                    Console.WriteLine("Folders retrieved successfully:");
                    foreach (var folder in folders)
                    {
                        Console.WriteLine($"- {folder.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during IMAP operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
