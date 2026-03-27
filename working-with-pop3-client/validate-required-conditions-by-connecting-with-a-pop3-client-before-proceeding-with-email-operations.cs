using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize POP3 client with server details
                using (Pop3Client client = new Pop3Client("pop.example.com", 110, "user", "password", SecurityOptions.Auto))
                {
                    try
                    {
                        // Attempt to retrieve mailbox information to validate the connection
                        client.GetMailboxInfo();
                        Console.WriteLine("POP3 client connected successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during POP3 operation: {ex.Message}");
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
