using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Server and credential details
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Initialize the IMAP client with SSL implicit security
            using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Validate the supplied credentials
                    bool credentialsValid = imapClient.ValidateCredentials();

                    if (credentialsValid)
                    {
                        Console.WriteLine("Credentials are valid.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid credentials.");
                    }
                }
                catch (Exception validationException)
                {
                    Console.Error.WriteLine($"Validation error: {validationException.Message}");
                }
            }
        }
        catch (Exception unexpectedException)
        {
            Console.Error.WriteLine($"Unexpected error: {unexpectedException.Message}");
        }
    }
}