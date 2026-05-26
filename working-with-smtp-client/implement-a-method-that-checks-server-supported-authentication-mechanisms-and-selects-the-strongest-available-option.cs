using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder values – replace with real server details.
            string host = "smtp.example.com";
            int port = 587;
            string username = "username";
            string password = "password";

            // Skip real network calls when placeholders are used.
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping connection.");
                return;
            }

            using (SmtpClient client = new SmtpClient(host, port, SecurityOptions.Auto))
            {
                client.Username = username;
                client.Password = password;

                // Determine the strongest authentication mechanism supported by the server.
                SmtpKnownAuthenticationType strongest = GetStrongestSupported(client.SupportedAuthentication);
                if (strongest == SmtpKnownAuthenticationType.None)
                {
                    Console.Error.WriteLine("No supported authentication mechanisms were found.");
                    return;
                }

                // Restrict the client to use only the selected mechanism.
                client.AllowedAuthentication = strongest;

                // Attempt to validate credentials using the chosen mechanism.
                try
                {
                    bool isValid = client.ValidateCredentials();
                    if (isValid)
                    {
                        Console.WriteLine($"Authentication succeeded using {strongest}.");
                    }
                    else
                    {
                        Console.Error.WriteLine("Authentication failed.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Authentication error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Returns the strongest authentication type supported by the server.
    private static SmtpKnownAuthenticationType GetStrongestSupported(SmtpKnownAuthenticationType supported)
    {
        // Define strength order from strongest to weakest.
        SmtpKnownAuthenticationType[] strengthOrder = new[]
        {
            SmtpKnownAuthenticationType.NTLM,
            SmtpKnownAuthenticationType.GSSAPI,
            SmtpKnownAuthenticationType.CramMD5,
            SmtpKnownAuthenticationType.Login,
            SmtpKnownAuthenticationType.Plain,
            SmtpKnownAuthenticationType.OAUTH2,
            SmtpKnownAuthenticationType.Anonymous
        };

        foreach (SmtpKnownAuthenticationType type in strengthOrder)
        {
            if ((supported & type) == type)
                return type;
        }

        return SmtpKnownAuthenticationType.None;
    }
}
