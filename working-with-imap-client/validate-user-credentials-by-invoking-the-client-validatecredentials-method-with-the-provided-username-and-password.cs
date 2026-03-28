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
            try
            {
                // Define server and user credentials
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Create the IMAP client and ensure it is disposed properly
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Validate the provided credentials
                        bool isValid = client.ValidateCredentials();
                        Console.WriteLine(isValid ? "Credentials are valid." : "Credentials are invalid.");
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
