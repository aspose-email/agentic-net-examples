using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping connection.");
                return;
            }

            // Create the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                // Subscribe to the OnConnect event to capture the server's banner (greeting)
                client.OnConnect += (object sender, EventArgs e) =>
                {
                    // The event args type provides the greeting message; cast to dynamic to access it safely
                    dynamic args = e;
                    try
                    {
                        string greeting = args.Greeting ?? args.GreetingMessage ?? string.Empty;
                        Console.WriteLine("SMTP Server Banner: " + greeting);
                    }
                    catch
                    {
                        Console.WriteLine("SMTP Server Banner: (unavailable)");
                    }
                };

                // Attempt to validate credentials which triggers the connection and the OnConnect event
                try
                {
                    client.ValidateCredentials();
                    Console.WriteLine("Credentials validated successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to validate credentials: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
