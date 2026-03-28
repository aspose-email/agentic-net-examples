using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Mail server connection settings
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient())
            {
                client.Host = host;
                client.Port = port;
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.Auto;

                // Validate the credentials against the mail server
                bool credentialsValid;
                try
                {
                    credentialsValid = client.ValidateCredentials();
                }
                catch (Exception validationEx)
                {
                    Console.Error.WriteLine($"Credential validation error: {validationEx.Message}");
                    return;
                }

                Console.WriteLine(credentialsValid ? "Credentials are valid." : "Credentials are invalid.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
