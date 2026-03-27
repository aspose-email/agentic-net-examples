using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters
            string host = "pop.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Initialize POP3 client
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                // Configure client behavior
                client.UseAuthentication = true;
                client.UseMultiConnection = MultiConnectionMode.Enable;
                client.UsePipelining = true;
                client.EnableLogger = true;
                client.LogFileName = "pop3_log.txt";
                client.UseDateInLogFileName = true;
                client.Timeout = 60000; // 60 seconds
                client.GreetingTimeout = 30000; // 30 seconds

                // Optional explicit settings
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.Auto;

                // Validate credentials as a simple operation
                client.ValidateCredentials();
                Console.WriteLine("POP3 client configured and credentials validated successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
