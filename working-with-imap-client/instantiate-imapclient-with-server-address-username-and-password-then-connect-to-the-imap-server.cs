using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ImapConnectSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                string host = "imap.example.com";
                string username = "username";
                string password = "password";

                // Skip real connection when placeholder values are used
                if (host.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping connection.");
                    return;
                }

                using (ImapClient client = new ImapClient(host, username, password))
                {
                    try
                    {
                        client.Noop();
                        Console.WriteLine("Connected to IMAP server successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to connect: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
