using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        // Top‑level exception guard
        try
        {
            // Placeholder connection parameters
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping validation.");
                return;
            }

            // Create the IMAP client inside a using block for deterministic disposal
            try
            {
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Validate the credentials and evaluate the boolean result
                    bool isValid = client.ValidateCredentials();

                    // Output the evaluation result
                    Console.WriteLine(isValid ? "Credentials are valid." : "Credentials are invalid.");
                }
            }
            catch (Exception ex)
            {
                // Friendly error handling for client‑related failures
                Console.Error.WriteLine($"Error during credential validation: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            // Catch any unexpected exceptions
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
