using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder host detected. Skipping credential validation.");
                return;
            }

            // Create the IMAP client
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Validate the credentials
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine(isValid ? "Credentials are valid." : "Invalid credentials.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during credential validation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
