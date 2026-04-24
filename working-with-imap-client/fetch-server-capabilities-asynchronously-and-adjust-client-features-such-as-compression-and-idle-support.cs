using Aspose.Email;
using System;
using System.Threading.Tasks;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Define the capabilities we are interested in
                    string[] capabilityNames = new string[] { "IDLE", "COMPRESS" };

                    // Asynchronously fetch server capabilities
                    string[] serverCapabilities = await client.ClientCapabilitiesAsync(capabilityNames);

                    // Adjust IDLE support based on server response
                    if (Array.Exists(serverCapabilities, c => c.Equals("IDLE", StringComparison.OrdinalIgnoreCase)))
                    {
                        client.IdSupported = true;
                        Console.WriteLine("Server supports IDLE. IDLE support enabled.");
                    }
                    else
                    {
                        client.IdSupported = false;
                        Console.WriteLine("Server does not support IDLE.");
                    }

                    // Adjust compression support based on server response
                    if (Array.Exists(serverCapabilities, c => c.Equals("COMPRESS", StringComparison.OrdinalIgnoreCase)))
                    {
                        client.CompressSupported = true;
                        Console.WriteLine("Server supports COMPRESS. Compression enabled.");
                    }
                    else
                    {
                        client.CompressSupported = false;
                        Console.WriteLine("Server does not support COMPRESS.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
