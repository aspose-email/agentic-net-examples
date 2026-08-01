using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using System;

class Program
{
    static void Main()
    {
        // Placeholder values – replace with real credentials for actual execution.
        string host = "imap.example.com";
        int port = 993;
        string username = "user@example.com";
        string password = "password";

        // Guard: skip external calls when placeholders are detected.
        bool placeholdersDetected = host.Contains("example.com") ||
                                    username.Contains("example.com") ||
                                    password.Contains("password");

        if (placeholdersDetected)
        {
            Console.WriteLine("Placeholder credentials detected. Skipping credential validation.");
            return;
        }

        try
        {
            using (ImapClient client = new ImapClient(host, port, username, password))
            {
                client.SecurityOptions = SecurityOptions.Auto;

                bool isValid;
                try
                {
                    isValid = client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Credential validation error: {ex.Message}");
                    return;
                }

                Console.WriteLine(isValid ? "Credentials are valid." : "Credentials are invalid.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
