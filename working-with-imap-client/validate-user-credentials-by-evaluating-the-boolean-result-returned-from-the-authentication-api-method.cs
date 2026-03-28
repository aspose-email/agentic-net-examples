using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Initialize the IMAP client
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate the credentials
                    bool isValid = client.ValidateCredentials();

                    // Output the result
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
