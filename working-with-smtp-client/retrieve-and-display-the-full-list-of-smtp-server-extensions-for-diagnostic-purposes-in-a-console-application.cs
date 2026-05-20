using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server connection details (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder SMTP credentials detected. Skipping server connection.");
                return;
            }

            // Create and use the SmtpClient inside a using block to ensure disposal
            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Retrieve the list of server extensions (capabilities)
                    IList<string> capabilities = client.GetCapabilities();

                    Console.WriteLine("SMTP Server Extensions:");
                    foreach (string capability in capabilities)
                    {
                        Console.WriteLine("- " + capability);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error retrieving capabilities: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
