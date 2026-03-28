using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            SecurityOptions security = SecurityOptions.SSLImplicit;
            string username = "user@example.com";
            string password = "password";

            using (ImapClient client = new ImapClient(host, port, security))
            {
                client.Username = username;
                client.Password = password;

                try
                {
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine(isValid ? "Credentials are valid." : "Invalid credentials.");
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
