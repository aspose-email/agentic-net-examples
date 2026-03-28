using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the IMAP client with server details
            using (ImapClient client = new ImapClient("imap.example.com", "username", "password", SecurityOptions.Auto))
            {
                // Activate client-side logging
                client.EnableLogger = true;

                // Additional client operations can be performed here
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
