using Aspose.Email.Clients;
using System;
using Aspose.Email.Clients.Imap;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection parameters
            string host = "imap.example.com";
            int port = 993;
            SecurityOptions security = SecurityOptions.Auto;

            // Guard against placeholder credentials/hosts
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder host detected. Skipping actual connection.");
                return;
            }

            // Create the IMAP client and set a custom greeting timeout (3 seconds)
            using (ImapClient client = new ImapClient(host, port, security))
            {
                try
                {
                    client.GreetingTimeout = 3000; // Timeout in milliseconds

                    // Example: connect and list folders (commented out to avoid real network calls)
                    // client.Connect();
                    // var folders = client.ListFolders();
                    // foreach (var folder in folders)
                    // {
                    //     Console.WriteLine(folder.Name);
                    // }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP client error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
