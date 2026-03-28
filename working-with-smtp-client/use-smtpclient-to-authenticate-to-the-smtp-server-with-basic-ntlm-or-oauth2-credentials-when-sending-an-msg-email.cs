using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file to be sent
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage("sender@example.com", "recipient@example.com", "Test Subject", "Test Body"))
                    {
                        placeholder.Save(msgPath, SaveOptions.DefaultMsgUnicode);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            MailMessage message;
            try
            {
                message = MailMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // -----------------------------------------------------------------
            // 1. Basic authentication (username & password)
            // -----------------------------------------------------------------
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.example.com", "username", "password", SecurityOptions.Auto))
                {
                    client.Send(message);
                    Console.WriteLine("Message sent using Basic authentication.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Basic auth failed: {ex.Message}");
            }

            // -----------------------------------------------------------------
            // 2. NTLM authentication (use default Windows credentials)
            // -----------------------------------------------------------------
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.example.com"))
                {
                    client.UseDefaultCredentials = true;          // Enable NTLM
                    client.UseAuthentication = true;
                    client.SecurityOptions = SecurityOptions.Auto;
                    client.Send(message);
                    Console.WriteLine("Message sent using NTLM authentication.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"NTLM auth failed: {ex.Message}");
            }

            // -----------------------------------------------------------------
            // 3. OAuth2 authentication (token provider)
            // -----------------------------------------------------------------
            try
            {
                // Obtain a token provider (placeholder values)
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.GetInstance(
                    "https://login.microsoftonline.com/common/oauth2/v2.0/token",
                    "clientId",
                    "clientSecret",
                    "refreshToken");

                using (SmtpClient client = new SmtpClient("smtp.example.com", "username", tokenProvider, SecurityOptions.Auto))
                {
                    client.Send(message);
                    Console.WriteLine("Message sent using OAuth2 authentication.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"OAuth2 auth failed: {ex.Message}");
            }

            // Dispose the loaded message
            message.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
