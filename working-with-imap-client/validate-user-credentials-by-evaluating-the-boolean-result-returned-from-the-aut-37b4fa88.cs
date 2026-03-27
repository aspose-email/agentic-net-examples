using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Connection parameters
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Create ImapClient and ensure it is disposed
                using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Validate credentials
                        bool isAuthenticated = imapClient.ValidateCredentials();
                        Console.WriteLine("Authentication successful: " + isAuthenticated);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Authentication failed: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}