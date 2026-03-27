using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace EmailServerVerification
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Server connection parameters
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Create an IMAP client (inherits EmailClient) with SSL/TLS security
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        // Validate the credentials against the server
                        bool credentialsValid = client.ValidateCredentials();

                        if (credentialsValid)
                        {
                            Console.WriteLine("Credentials are valid. Connection successful.");
                        }
                        else
                        {
                            Console.WriteLine("Credentials are invalid. Connection failed.");
                        }
                    }
                    catch (Exception validationEx)
                    {
                        // Handle any errors that occur during validation
                        Console.Error.WriteLine($"Error during credential validation: {validationEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
