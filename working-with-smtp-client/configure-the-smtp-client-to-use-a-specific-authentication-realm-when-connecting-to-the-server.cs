using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;

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
            string authenticationRealm = "myRealm";

            // Skip real network calls when placeholder values are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP host detected. Skipping connection.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Set the authentication realm using reflection (property may not exist in all versions)
                var realmProp = client.GetType().GetProperty("AuthenticationRealm");
                if (realmProp != null && realmProp.CanWrite)
                {
                    realmProp.SetValue(client, authenticationRealm);
                }

                client.UseAuthentication = true;

                // Validate credentials safely
                try
                {
                    bool credentialsValid = client.ValidateCredentials();
                    Console.WriteLine($"Credentials valid: {credentialsValid}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Credential validation failed: {ex.Message}");
                    return;
                }

                // Prepare a simple email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(username);
                    message.To.Add(new MailAddress("recipient@example.com"));
                    message.Subject = "Test Email";
                    message.Body = "This is a test email sent using Aspose.Email SMTP client.";

                    // Send the message
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
