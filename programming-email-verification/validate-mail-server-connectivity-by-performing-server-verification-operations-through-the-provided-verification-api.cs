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
            // Initialize the IMAP client with server details
            using (ImapClient client = new ImapClient("imap.example.com", 993, SecurityOptions.SSLImplicit))
            {
                client.Username = "user@example.com";
                client.Password = "password";

                try
                {
                    bool isValid = client.ValidateCredentials();
                    Console.WriteLine(isValid ? "Connection successful." : "Invalid credentials.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Validation failed: {ex.Message}");
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
