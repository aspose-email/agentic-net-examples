using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ImapAuthenticationCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Initialize and connect the IMAP client
                try
                {
                    using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                    {
                        // Verify credentials
                        bool isAuthenticated = false;
                        try
                        {
                            isAuthenticated = client.ValidateCredentials();
                        }
                        catch (Exception credEx)
                        {
                            Console.Error.WriteLine($"Credential validation error: {credEx.Message}");
                            return;
                        }

                        if (isAuthenticated)
                        {
                            Console.WriteLine("IMAP authentication succeeded.");
                        }
                        else
                        {
                            Console.WriteLine("IMAP authentication failed.");
                        }
                    }
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP connection error: {imapEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Fatal error: {ex.Message}");
            }
        }
    }
}
