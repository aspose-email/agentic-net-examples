using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    // Author: Aspose.Email example for asynchronous POP3 message deletion
    static async Task Main(string[] args)
    {
        try
        {
            // POP3 server connection settings
            string host = "pop3.example.com";
            int port = 995;
            string username = "user@example.com";
            string password = "password";

            // Unique identifier of the message to delete
            string messageId = "12345";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and configure the POP3 client
            using (Pop3Client client = new Pop3Client())
            {
                client.Host = host;
                client.Port = port;
                client.SecurityOptions = SecurityOptions.SSLImplicit;
                client.Username = username;
                client.Password = password;

                // Asynchronously delete the specified message
                await client.DeleteMessageAsync(messageId);
                Console.WriteLine($"Message '{messageId}' deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
