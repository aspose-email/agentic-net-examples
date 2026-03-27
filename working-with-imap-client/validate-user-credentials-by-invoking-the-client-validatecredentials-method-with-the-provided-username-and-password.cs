using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Connection parameters
                string host = "imap.example.com";
                string username = "user@example.com";
                string password = "password";

                // Create ImapClient and validate credentials
                using (Aspose.Email.Clients.Imap.ImapClient imapClient = new Aspose.Email.Clients.Imap.ImapClient(host, username, password))
                {
                    try
                    {
                        bool isValid = imapClient.ValidateCredentials();
                        if (isValid)
                        {
                            Console.WriteLine("Credentials are valid.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid credentials.");
                        }
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