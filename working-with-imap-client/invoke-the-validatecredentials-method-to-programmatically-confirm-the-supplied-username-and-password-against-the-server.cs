using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace ValidateCredentialsSample
{
    class Program
    {
        static void Main()
        {
            // Top‑level exception guard
            try
            {
                // Server connection parameters
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Create the IMAP client inside a using block for deterministic disposal
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Wrap the validation call to handle any runtime errors gracefully
                    try
                    {
                        bool credentialsValid = client.ValidateCredentials();
                        Console.WriteLine(credentialsValid
                            ? "Credentials are valid."
                            : "Credentials are invalid.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during credential validation: {ex.Message}");
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
}
